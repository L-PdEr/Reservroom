using Reservroom.Commands;
using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Reservroom.ViewModels;

public class MakeReservationViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private string? _username;
    public string? Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    private int _floorNumber;
    public int FloorNumber
    {
        get => _floorNumber;
        set
        {
            _floorNumber = value;
            OnPropertyChanged(nameof(FloorNumber));
        }
    }

    private int _roomNumber;
    public int RoomNumber
    {
        get => _roomNumber;
        set
        {
            _roomNumber = value;
            OnPropertyChanged(nameof(RoomNumber));
        }
    }

    private DateTime _startDate = new DateTime(2023, 1, 1);
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));

            ClearErrors(nameof(StartDate));
            ClearErrors(nameof(EndDate));

            if (EndDate < StartDate)
            {
                List<string> startDateErrors = new List<string>()
                {
                    "The start date cannot be after the end date."
                };
                
                AddError("The start date cannot be after the end date.", nameof(StartDate));
                AddError("The start date cannot be after the end date.", nameof(EndDate));
            }
        }
    }

    private DateTime _endDate = new DateTime(2023, 1, 8);


    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            ClearErrors(nameof(StartDate));
            ClearErrors(nameof(EndDate));

            if (EndDate < StartDate)
            {
                List<string> endDateErrors = new List<string>()
                {
                    
                };

                AddError("The end date cannot be before the start date.", nameof(EndDate));
                AddError("The start date cannot be after the end date.", nameof(StartDate));
            }

            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }

    private void AddError(string errorMessage, string propertyName)
    {
        if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
        {
            _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
        }
        _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);

        OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(propertyName)));
    }

    public ICommand? SubmitCommand { get; }
    public ICommand? CancelCommand { get; }

    private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;

    public bool HasErrors => _propertyNameToErrorsDictionary.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public MakeReservationViewModel(HotelStore hotelStore, NavigationService reservationViewNavigationService)
    {
        _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        SubmitCommand = new MakeReservationCommand(this, hotelStore, reservationViewNavigationService); // wir müssen mit this arbeiten, weil wir die Daten aus dem ViewModel brauchen
        CancelCommand = new NavigateCommand(reservationViewNavigationService);
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
    }

    private void ClearErrors(string propertyName)
    {
        _propertyNameToErrorsDictionary.Remove(nameof(propertyName));
        OnErrorsChanged(propertyName);
    }
}
