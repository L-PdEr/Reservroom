using Microsoft.EntityFrameworkCore;
using Reservroom.DTOs;
using Reservroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.DbContexts;

public class ReservoomDbContext : DbContext
{
    public ReservoomDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ReservationDTO> Reservations { get; set; }
}
