namespace IIS.FlexberryGisTestStand
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using ICSSoft.Services;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using IIS.Caseberry.Logging.Objects;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NewPlatform.Flexberry.GIS;
    using NewPlatform.Flexberry.ORM;
    using NewPlatform.Flexberry.ORM.ODataService.Extensions;
    using NewPlatform.Flexberry.ORM.ODataService.Files;
    using NewPlatform.Flexberry.ORM.ODataService.Model;
    using NewPlatform.Flexberry.ORM.ODataService.WebApi.Extensions;
    using NewPlatform.Flexberry.ORM.ODataServiceCore.Common.Exceptions;
    using NewPlatform.Flexberry.Services;
    using Npgsql;
    using Unity;

    /// <summary>
    /// Класс настройки запуска приложения.
    /// </summary>
    public class Startup
    {
        private readonly List<Dictionary<string, string>> backgroundLayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">An application configuration properties.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            try
            {
                string layersPath = Path.Combine(Directory.GetCurrentDirectory(), "shared", "backgroundLayers.xml");
                if (!File.Exists(layersPath))
                {
                    throw new FileNotFoundException("Файл shared/backgroundLayers.xml не найден.");
                }

                var xml = new XmlDocument();
                xml.Load(layersPath);

                this.backgroundLayers = new List<Dictionary<string, string>>();

                foreach (XmlNode layerNode in xml.DocumentElement.ChildNodes)
                {
                    this.backgroundLayers.Add(new Dictionary<string, string>()
                    {
                        { "name", layerNode.Attributes["name"].Value },
                        { "crs", layerNode.Attributes["crs"].Value },
                        { "settings", layerNode.InnerText },
                        { "visibility", layerNode.Attributes["visibility"].Value },
                    });
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
            }
        }

        /// <summary>
        /// An application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configurate application services.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        /// <param name="services">An collection of application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connStr = Configuration["DefConnStr"];

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connStr);
            dataSourceBuilder.UseNetTopologySuite();
            var dataSource = dataSourceBuilder.Build();
            dataSource.OpenConnectionAsync();

            services.AddMvcCore(
                    options =>
                    {
                        options.Filters.Add<CustomExceptionFilter>();
                        options.EnableEndpointRouting = false;
                    })
                .AddFormatterMappings()
                .AddControllersAsServices();

            services.AddOData();

            services.AddCors();
            services
                .AddHealthChecks()
                .AddNpgSql(connStr);
        }

        /// <summary>
        /// Configurate the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
        /// <param name="app">An application configurator.</param>
        /// <param name="env">Information about web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            LogService.LogInfo("Инициирован запуск приложения.");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseODataService(builder =>
            {
                builder.MapFileRoute();

                // Create EDM model builder.
                var assemblies = new[]
                {
                    typeof(Address).Assembly,
                    typeof(NewPlatform.Flexberry.GIS.MapObjectSetting).Assembly,
                    typeof(ApplicationLog).Assembly,
                    typeof(UserSetting).Assembly,
                    typeof(Lock).Assembly,
                };

                var modelBuilder = new DefaultDataObjectEdmModelBuilder(assemblies, true);

                // Map OData Service.
                var token = builder.MapDataObjectRoute(modelBuilder);
                token.Events.CallbackBeforeCreate = this.BeforeCreate;
            });
        }

        /// <summary>
        /// Configurate application container.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        public void ConfigureContainer(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // FYI: сервисы, в т.ч. контроллеры, создаются из дочернего контейнера.
            while (container.Parent != null)
            {
                container = container.Parent;
            }

            // FYI: сервис данных ходит в контейнер UnityFactory.
            container.RegisterInstance(Configuration);

            RegisterDataObjectFileAccessor(container);
            RegisterORM(container);
        }

        private bool BeforeCreate(DataObject obj)
        {
            if (obj == null)
            {
                return true;
            }

            if (obj.GetType() == typeof(FavoriteFeature))
            {
                var feature = (FavoriteFeature)obj;
                feature.UserKey = "user";
            }

            if (obj.GetType() == typeof(Map))
            {
                var map = (Map)obj;
                map.EditTimeMapLayers = DateTime.Now;

                // При создании карты сразу со слоем данный метод вызывается несколько раз.
                if (map.MapLayer.Count == 0)
                {
                    foreach (var layer in this.backgroundLayers)
                    {
                        map.MapLayer.Add(new MapLayer()
                        {
                            Name = layer["name"],
                            Index = 1,
                            Type = "tile",
                            Settings = layer["settings"],
                            CoordinateReferenceSystem = layer["crs"],
                            Public = true,
                            Visibility = layer["visibility"] == "1",
                        });
                    }
                }
            }

            if (obj.GetType() == typeof(MapLayer))
            {
                var mapLayer = (MapLayer)obj;
                var map = mapLayer.Map;
                map.EditTimeMapLayers = DateTime.Now;
            }

            return true;
        }

        /// <summary>
        /// Register implementation of <see cref="IDataObjectFileAccessor"/>.
        /// </summary>
        /// <param name="container">Container to register at.</param>
        private void RegisterDataObjectFileAccessor(IUnityContainer container)
        {
            const string fileControllerPath = "api/file";
            string baseUriRaw = Configuration["BackendRoot"];
            if (string.IsNullOrEmpty(baseUriRaw))
            {
                throw new System.Configuration.ConfigurationErrorsException("BackendRoot is not specified in Configuration or enviromnent variables.");
            }

            Console.WriteLine($"baseUriRaw is {baseUriRaw}");
            var baseUri = new Uri(baseUriRaw);
            string uploadPath = Configuration["UploadUrl"];
            container.RegisterSingleton<IDataObjectFileAccessor, DefaultDataObjectFileAccessor>(
                Invoke.Constructor(
                    baseUri,
                    fileControllerPath,
                    uploadPath,
                    null));
        }

        /// <summary>
        /// Register ORM implementations.
        /// </summary>
        /// <param name="container">Container to register at.</param>
        private void RegisterORM(IUnityContainer container)
        {
            string connStr = Configuration["DefConnStr"];
            if (string.IsNullOrEmpty(connStr))
            {
                throw new System.Configuration.ConfigurationErrorsException("DefConnStr is not specified in Configuration or enviromnent variables.");
            }

            container.RegisterSingleton<ISecurityManager, EmptySecurityManager>();

            container.RegisterSingleton<IDataService, GisPostgresDataService>(
                Inject.Property(nameof(GisPostgresDataService.CustomizationString), connStr));
        }
    }
}
