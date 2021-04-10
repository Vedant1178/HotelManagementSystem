using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingSystem.Models
{
    public class RoomsModel
    {
        public int RoomId { get; set; }
        public string City { get; set; }
        public HotelsModel HotelsModel { get; set; }
        public int PricePerNight { get; set; }
        public int Beds { get; set; }
        public string Address { get; set; }
        public double Ratings { get; set; }
        public string ImageUrl { get; set; }
        public string Hotel { get; set; }
    }

    public class HotelsModel
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
    }
}
