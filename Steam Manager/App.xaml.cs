using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SteamAccount;
using SteamManager.Infrastructure;
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
    public partial class App : Application
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
            services.AddSingleton<IIOService, IOService>();
            services.AddSingleton<IStringEncryptionService, StringEncryptionService>();
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
