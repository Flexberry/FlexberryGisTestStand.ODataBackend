namespace IIS.FlexberryGisTestStand
{
    using System;
    using System.Reflection;

    using System.Web.Http;
    using System.Web.Http.Cors;

   
    using ICSSoft.STORMNET;
    using ICSSoft.Services;

    using IIS.Caseberry.Logging.Objects;

    using Microsoft.Practices.Unity;

    using NewPlatform.Flexberry;
    using NewPlatform.Flexberry.ORM.ODataService;
    using NewPlatform.Flexberry.ORM.ODataService.Extensions;
    using NewPlatform.Flexberry.ORM.ODataService.Functions;
    using NewPlatform.Flexberry.ORM.ODataService.Model;
    using NewPlatform.Flexberry.Services;


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
        public static void Configure(HttpConfiguration config, IUnityContainer container)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Use Unity as WebAPI dependency resolver
            config.DependencyResolver = new UnityDependencyResolver(container);

            // Create EDM model builder
            var assemblies = new[]
            {
                Assembly.Load("FlexberryGisTestStand.Objects"),
                Assembly.Load("NewPlatform.Flexberry.GIS.Objects"),
                typeof(ApplicationLog).Assembly,
                typeof(UserSetting).Assembly,
                typeof(FlexberryUserSetting).Assembly,
                typeof(Lock).Assembly
            };
            var builder = new DefaultDataObjectEdmModelBuilder(assemblies);

            // Map OData Service
            var token = config.MapODataServiceDataObjectRoute(builder);

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