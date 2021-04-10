using Microsoft.EntityFrameworkCore;
using RoomBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingSystem.Services
{
    public class RoomBookingContext : DbContext
    {
        public RoomBookingContext(DbContextOptions<RoomBookingContext> options) : base(options)
        {
        }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<RoomReservationModel> RoomReservations { get; set; }
        
    }
}
