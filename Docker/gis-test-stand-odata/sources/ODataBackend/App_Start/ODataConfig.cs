namespace IIS.FlexberryGisTestStand
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;
    using ICSSoft.Services;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using IIS.Caseberry.Logging.Objects;
    using NewPlatform.Flexberry.AspNet.WebApi.Cors;
    using NewPlatform.Flexberry.ORM.ODataService.Extensions;
    using NewPlatform.Flexberry.ORM.ODataService.Functions;
    using NewPlatform.Flexberry.ORM.ODataService.Model;
    using NewPlatform.Flexberry.ORM.ODataService.WebApi.Extensions;
    using NewPlatform.Flexberry.Services;
    using Unity;
    using Unity.AspNet.WebApi;


    /// <summary>
    /// Configure OData Service.
    /// </summary>
    internal static class ODataConfig
    {
        /// <summary>
        /// Configure OData by DataObjects assembly.
        /// </summary>
        /// <param name="config">Http configuration object.</param>
        /// <param name="container">Unity container.</param>
        public static void Configure(HttpConfiguration config, IUnityContainer container, HttpServer httpServer)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            config.EnableCors(new DynamicCorsPolicyProvider(true));

            // Use Unity as WebAPI dependency resolver
            config.DependencyResolver = new UnityDependencyResolver(container);

            // Create EDM model builder
            var assemblies = new[]
            {
                Assembly.Load("FlexberryGisTestStand.Objects"),
                Assembly.Load("NewPlatform.Flexberry.GIS.Objects"),
                typeof(ApplicationLog).Assembly,
                typeof(UserSetting).Assembly,
                typeof(Lock).Assembly
            };
            var builder = new DefaultDataObjectEdmModelBuilder(assemblies);

            // Map OData Service
            var token = config.MapODataServiceDataObjectRoute(builder, httpServer);

            // Register OData event handlers.
            token.Events.CallbackAfterCreate = CallbackAfterCreate;
        }

        private static void CallbackAfterCreate(DataObject dataObject)
        {
            // TODO: implement handler
        }

        private static string Test(QueryParameters queryParameters)
        {
            return "Hello world!";
        }

    }
}