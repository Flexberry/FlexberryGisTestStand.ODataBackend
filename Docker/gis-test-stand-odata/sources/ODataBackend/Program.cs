﻿// This application entry point is based on ASP.NET Core new project templates and is included
// as a starting point for app host configuration.
// This file may need updated according to the specific scenario of the application being upgraded.
// For more information on ASP.NET Core hosting, see https://docs.microsoft.com/aspnet/core/fundamentals/host/web-host
namespace IIS.FlexberryGisTestStand
{
    using ICSSoft.Services;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Unity;
    using Unity.Microsoft.DependencyInjection;

    /// <summary>
    /// Основной класс приложения.
    /// </summary>
    public class Program
    {
        private static readonly IUnityContainer Container = UnityFactory.GetContainer();

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы запуска.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Создать инициализатор приложения.
        /// </summary>
        /// <param name="args">Аргументы запуска.</param>
        /// <returns>Инициализатор приложения.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseUnityServiceProvider(Container)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
