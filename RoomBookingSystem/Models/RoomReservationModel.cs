using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingSystem.Models
{
    [Table("tblRoomReservations")]
    public class RoomReservationModel
    {
        [Key]
        public int RoomReservationId {get;set;}
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public int Rate { get; set; }
        [Required(ErrorMessage ="Select checkin date")]
        public DateTime CheckIn { get; set; }
        [Required(ErrorMessage = "Select checkout date")]
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public int UserId { get; set; }
        public int TotalBillAmount { get; set; }
        public DateTime RoomReservationDate { get; set; }
        [NotMapped]
        public RoomsModel RoomDetails { get; set; }
        [NotMapped]
        [Required(ErrorMessage ="Card Number is required")]
        public string CardNumber { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Card Holder Name is required")]
        public string CardHolderName { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "CVV is required")]
        public string CardCVV { get; set; }
    }
}
