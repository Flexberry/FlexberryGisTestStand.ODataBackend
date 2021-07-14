namespace IIS.FlexberryGisTestStand
{
    using System;
    using System.Web;
    using System.Web.Http;

    using ICSSoft.STORMNET.Business;

    using Microsoft.Practices.Unity.Configuration;
    using Unity;

    /// <summary>
    /// Global class which is used at application startup.
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            container.LoadConfiguration();
            container.RegisterInstance(DataServiceProvider.DataService);
            GlobalConfiguration.Configure(configuration => ODataConfig.Configure(configuration, container, GlobalConfiguration.DefaultServer));
        }
    }
}
