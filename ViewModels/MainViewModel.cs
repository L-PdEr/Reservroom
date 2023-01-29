﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel { get; }

    public MainViewModel(Models.Hotel _hotel)
    {
        CurrentViewModel = new ReservationListingViewModel();
    }


}
