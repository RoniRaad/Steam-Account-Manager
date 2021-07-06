using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SteamAccount;
using SteamManager.Application.Controllers;
using SteamManager.Application.ViewModels;
using SteamManager.Infrastructure;
using SteamManager.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SteamManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }


        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IIOService, IOService>();
            services.AddTransient<IStringEncryptionService, StringEncryptionService>();
            services.AddTransient<IAccountManagerController, AccountManagerController>();
            services.AddTransient<ILoginController, LoginController>();
            services.AddTransient<IAccountManagerViewModel, AccountManagerViewModel>();
            services.AddTransient<ISteamService, SteamService>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<AccountManager>();
        }

        protected override async void OnStartup(StartupEventArgs e) 
        {
            await _host.StartAsync();

            MainWindow window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }


    }
}
