using Microsoft.EntityFrameworkCore;
using Reservroom.DbContexts;
using Reservroom.DTOs;
using Reservroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Services.ReservationProviders;

public class DatabaseReservationProvider : IReservationProvider
{
    private readonly ReservoomDbContextFactory _dbContextFactory;

    public DatabaseReservationProvider(ReservoomDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    private static Reservation ToReservation(ReservationDTO dto)
    {
        return new Reservation(new RoomID(dto.FloorNumber, dto.RoomNumber), dto.Username, dto.StartTime, dto.EndTime);
    }

    public async Task<IEnumerable<Reservation>> GetAllReservations()
    {
        using (ReservoomDbContext context = _dbContextFactory.CreateDbContext())
        {
            IEnumerable<ReservationDTO> reservationDTOs = await context.Reservations.ToListAsync();

            return reservationDTOs.Select(r => ToReservation(r));
        }
    }
}
