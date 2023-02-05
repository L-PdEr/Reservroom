using MaterialDesignThemes.Wpf.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reservroom.DbContexts;
using Reservroom.Exceptions;
using Reservroom.HostBuilders;
using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Services.ReservationConflictValidators;
using Reservroom.Services.ReservationCreators;
using Reservroom.Services.ReservationProviders;
using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Reservroom
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
                .AddViewModels()
                .ConfigureServices((hostContext, services) =>
                {
                    // appsettings.json File Properties -> Copy if newer
                    hostContext.Configuration.GetConnectionString("Default");
                    string connectionString = hostContext.Configuration.GetConnectionString("Default");

                    services.AddSingleton<ReservoomDbContextFactory>(new ReservoomDbContextFactory(connectionString));
                    services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
                    services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
                    services.AddSingleton<IReservationConflictValidator, DatabaseReservationConflictValidator>();

                    services.AddTransient<ReservationBook>(); // ReservationBook bekommt immer eine andere Insanz
                    string hotelName = hostContext.Configuration.GetValue<string>("HotelName");
                    services.AddSingleton<Hotel>((s) => new Hotel(hotelName, s.GetRequiredService<ReservationBook>())); // s ist unser serviceprovider, zweiter parameter bekommt die Instanz von ReservationBook


                    services.AddSingleton<HotelStore>();
                    services.AddSingleton<NavigationStore>();

                    
                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });

                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            ReservoomDbContextFactory reservoomDbContextFactory = _host.Services.GetRequiredService<ReservoomDbContextFactory>();
            using (ReservoomDbContext dbContext = reservoomDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }

            NavigationService<ReservationListingViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<ReservationListingViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
