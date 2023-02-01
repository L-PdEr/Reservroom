using Microsoft.EntityFrameworkCore;
using Reservroom.DbContexts;
using Reservroom.Exceptions;
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
        private const string CONNECTION_STRING = "Data Source=Reservroom.db";
        private readonly Hotel _hotel;
        private readonly NavigationStore _navigationStore;
        private ReservoomDbContextFactory _reservoomDbContextFactory;

        public App()
        {
            _reservoomDbContextFactory = new ReservoomDbContextFactory(CONNECTION_STRING);
            IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_reservoomDbContextFactory);
            IReservationProvider reservationProvider = new DatabaseReservationProvider(_reservoomDbContextFactory);
            IReservationCreator reservationCreator = new DatabaseReservationCreator(_reservoomDbContextFactory);
            
            ReservationBook reservationBook = new ReservationBook(reservationProvider,  reservationCreator, reservationConflictValidator);
            _hotel = new Hotel("SingeltonSean Suites", reservationBook);
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(CONNECTION_STRING).Options;
            using (ReservoomDbContext dbContext = _reservoomDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }
            
            _navigationStore.CurrentViewModel = CreateMakeReservationViewModel();
            MainWindow = new MainWindow() {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotel, new NavigationService(_navigationStore, CreateReservationViewModel));
        }

        private ReservationListingViewModel CreateReservationViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotel, new NavigationService(_navigationStore, CreateMakeReservationViewModel));
        }
    }
}
