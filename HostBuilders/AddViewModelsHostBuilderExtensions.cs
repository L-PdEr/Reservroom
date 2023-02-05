using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reservroom.Services;
using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((services) =>
            {
                // services.AddTransient<ReservationListingViewModel>(); ruft den konstruktor dadurch bleibt die liste leer
                services.AddTransient((s) => CreateReservationListingViewModel(s));
                services.AddSingleton<Func<ReservationListingViewModel>>((s) => () => s.GetRequiredService<ReservationListingViewModel>());
                services.AddSingleton<NavigationService<ReservationListingViewModel>>();

                services.AddTransient<MakeReservationViewModel>();
                // wenn sich Func regestrieren komisch anfühlt kann man auch eigene delegates bauen
                services.AddSingleton<Func<MakeReservationViewModel>>((s) => () => s.GetRequiredService<MakeReservationViewModel>());
                services.AddSingleton<NavigationService<MakeReservationViewModel>>();

                services.AddSingleton<MainViewModel>();
            });
            return hostBuilder;
        }

        private static ReservationListingViewModel CreateReservationListingViewModel(IServiceProvider services)
        {
            return ReservationListingViewModel.LoadViewModel(
                services.GetRequiredService<HotelStore>(),
                services.GetRequiredService<NavigationService<MakeReservationViewModel>>()
                );
        }
    }
}
