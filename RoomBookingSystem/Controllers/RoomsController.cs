using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBookingSystem.Models;
using RoomBookingSystem.Services;

namespace RoomBookingSystem.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        readonly RoomBookingContext _bokingDBContext;


        public RoomsController(RoomBookingContext bokingDBContext)
        {
            _bokingDBContext = bokingDBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReadRooms(string draw, string start, int length)
        {
            // Sort Column Name  
            var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)  
            var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  
            var searchValue = Request.Query["search[value]"].FirstOrDefault();

            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            // Getting all News data  
            var Rooms = GetRooms();

            //Sorting  
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortValue = typeof(RoomsModel).GetProperty(sortColumn);
                if (sortColumnDirection == "asc")
                    Rooms = Rooms.OrderBy(c => sortValue.GetValue(c, null)).ToList();
                else
                    Rooms = Rooms.OrderByDescending(c => sortValue.GetValue(c, null)).ToList();
            }

            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                Rooms = Rooms.Where(m => m.HotelsModel.HotelName.Contains(searchValue) || m.City.Contains(searchValue) || m.Address.Contains(searchValue)).ToList();
            }

            //total number of rows count 
            recordsTotal = Rooms.Count();
            //Paging   
            var data = Rooms.Skip(skip).Take(length).ToList();

            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].Hotel = data[i].HotelsModel.HotelName;
                }
            }

            //Returning Json Data  
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpGet]
        public IActionResult Booking(int RoomId)
        {
            var roomDetails = GetRooms().Where(r => r.RoomId == RoomId).FirstOrDefault();
            RoomReservationModel roomReservationModel = new RoomReservationModel
            {
                Rate = roomDetails.PricePerNight,
                RoomId = roomDetails.RoomId,
                RoomDetails = roomDetails
            };
            return View(roomReservationModel);
        }

        [HttpPost]
        public IActionResult Booking(RoomReservationModel Model)
        {
            RoomReservationModel roomReservation = new RoomReservationModel();
            var roomDetails = GetRooms().Where(r => r.RoomId == Model.RoomId).FirstOrDefault();
            roomReservation.HotelId = roomDetails.HotelsModel.HotelId;
            roomReservation.RoomId = roomDetails.RoomId;
            roomReservation.Rate = roomDetails.PricePerNight;
            roomReservation.CheckIn = Model.CheckIn;
            roomReservation.CheckOut = Model.CheckOut;
            roomReservation.Guests = 1;
            roomReservation.UserId = GetUserId();
            roomReservation.TotalBillAmount = Model.TotalBillAmount;
            roomReservation.RoomReservationDate = DateTime.Now;
            _bokingDBContext.Add(roomReservation);
            _bokingDBContext.SaveChanges();
            SendEmail(GetUserId(), roomDetails.HotelsModel.HotelName, roomDetails.PricePerNight, roomDetails.Beds, Model.CheckIn, Model.CheckOut, Model.TotalBillAmount);
            return RedirectToAction("Confirmation");
        }

        private int GetUserId()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.Upn).FirstOrDefault().Value;
            return Convert.ToInt16(userId);
        }

        private void SendEmail(int UserId, string HotelName, int PerNightRate, int BedsAvailable, DateTime CheckInDate, DateTime CheckOutDate, int Payment)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            UserModel userModel = _bokingDBContext.Users.Where(u => u.UserId == UserId && u.Isactive == true).FirstOrDefault();
            MailAddress fromAddress = new MailAddress("akhilnvedant@gmail.com");
            message.From = fromAddress;
            message.To.Add(userModel.Email);
            message.Subject = "Hotel Room Booking";
            message.IsBodyHtml = true;
            message.Body = "Dear " + userModel.FirstName + " " + userModel.LastName + ",<br/>";
            message.Body = message.Body + "We have recived new hotel booking from you, below are the booking details.<br/><br/>";
            message.Body = message.Body + "Hotel Name     :" + HotelName + "<br/>";
            message.Body = message.Body + "Per Night Rent :" + PerNightRate + "<br/>";
            message.Body = message.Body + "Beds Available :" + BedsAvailable + "<br/>";
            message.Body = message.Body + "Checkin Date   :" + CheckInDate.ToShortDateString() + "<br/>";
            message.Body = message.Body + "Checkout Date  :" + CheckOutDate.ToShortDateString() + "<br/>";
            message.Body = message.Body +  "Pyament Made   :" + Payment + "<br/><br/>";
            message.Body = message.Body + "Thank You.<br/><br/>";
            // We use gmail as our smtp client
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(
                "akhilnvedant@gmail.com", "akhilvedant123");
            smtpClient.Send(message);
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        private List<RoomsModel> GetRooms()
        {
            List<RoomsModel> rooms = new List<RoomsModel>();
            // 1
            rooms.Add(new RoomsModel
            {
                RoomId = 1,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Wellington Hotel",
                    HotelId = 1
                },
                PricePerNight = 59,
                Beds = 1,
                Address = "871 7th Ave, New York, NY 10019, United States",
                Ratings = 7.8,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/242/242273416.jpg"
            });
            // 2
            rooms.Add(new RoomsModel
            {
                RoomId = 2,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "The Manhattan Club",
                    HotelId = 2
                },
                PricePerNight = 267,
                Beds = 2,
                Address = "200 W 56th St, New York, NY 10019, United States",
                Ratings = 8.4,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/133/133000102.jpg"
            });
            // 3
            rooms.Add(new RoomsModel
            {
                RoomId = 3,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "MOXY NYC Times Square",
                    HotelId = 3
                },
                PricePerNight = 143,
                Beds = 2,
                Address = "485 7th Ave, New York, NY 10018, United States",
                Ratings = 8.4,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/188/188494364.jpg"
            });
            // 4
            rooms.Add(new RoomsModel
            {
                RoomId = 4,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Hyatt Place New York City/Times Square",
                    HotelId = 4
                },
                PricePerNight = 79,
                Beds = 1,
                Address = "350 W 39th St, New York, NY 10018, United States",
                Ratings = 8.1,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/188/188494816.jpg"
            });
            // 5
            rooms.Add(new RoomsModel
            {
                RoomId = 5,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Doubletree By Hilton New York Times Square West",
                    HotelId = 5
                },
                PricePerNight = 74,
                Beds = 1,
                Address = "350 W 40th St, New York, NY 10018, United States",
                Ratings = 7.4,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/374/37453972.jpg"
            });
            // 6
            rooms.Add(new RoomsModel
            {
                RoomId = 6,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "New York Marriott Marquis ",
                    HotelId = 6
                },
                PricePerNight = 249,
                Beds = 2,
                Address = "1535 Broadway, New York, NY 10036, United States",
                Ratings = 9.2,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/883/88387237.jpg"
            });
            // 7
            rooms.Add(new RoomsModel
            {
                RoomId = 7,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Hilton Garden Inn Central Park South",
                    HotelId = 7
                },
                PricePerNight = 350,
                Beds = 3,
                Address = "237 W 54th St, New York, NY 10019, United States",
                Ratings = 9.3,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/883/88388083.jpg"
            });
            // 8
            rooms.Add(new RoomsModel
            {
                RoomId = 8,
                City = "New York City",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Warwick New York",
                    HotelId = 8
                },
                PricePerNight = 379,
                Beds = 2,
                Address = "65 W 54th St, New York, NY 10019, United States",
                Ratings = 9.8,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/230/230520090.jpg"
            });
            // 9
            rooms.Add(new RoomsModel
            {
                RoomId = 9,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Hampton Inn & Suites by Hilton Toronto Airport",
                    HotelId = 9
                },
                PricePerNight = 105,
                Beds = 1,
                Address = "3279 Caroga Dr, Mississauga, ON L4V 1A3, Canada",
                Ratings = 8.4,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/967/9675953.jpg"
            });
            // 10
            rooms.Add(new RoomsModel
            {
                RoomId = 10,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Sheraton Gateway Hotel in Toronto International Airport",
                    HotelId = 10
                },
                PricePerNight = 174,
                Beds = 2,
                Address = "Terminal 3 Toronto Amf, Toronto, ON L5P 1C4, Canada",
                Ratings = 7.2,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/885/88569950.jpg"
            });
            // 11
            rooms.Add(new RoomsModel
            {
                RoomId = 11,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Alt Hotel Toronto Airport",
                    HotelId = 11
                },
                PricePerNight = 126,
                Beds = 1,
                Address = "6080 Viscount Rd, Mississauga, ON L4V 0A1, Canada",
                Ratings = 9.8,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/132/132996864.jpg"
            });
            // 12
            rooms.Add(new RoomsModel
            {
                RoomId = 12,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Sandman Signature Toronto Airport Hotel",
                    HotelId = 12
                },
                PricePerNight = 99,
                Beds = 1,
                Address = "55 Reading Ct, Toronto, ON M9W 7K7, Canada",
                Ratings = 8.1,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/247/247107633.jpg"
            });
            // 13
            rooms.Add(new RoomsModel
            {
                RoomId = 13,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Fairfield Inn & Suites by Marriott Toronto Airport",
                    HotelId = 13
                },
                PricePerNight = 246,
                Beds = 2,
                Address = "3299 Caroga Dr, Mississauga, ON L4V 1A3, Canada",
                Ratings = 8.7,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/230/230162400.jpg"
            });
            // 14
            rooms.Add(new RoomsModel
            {
                RoomId = 14,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Element Toronto Airport",
                    HotelId = 14
                },
                PricePerNight = 99,
                Beds = 1,
                Address = "6257 Airport Rd, Mississauga, ON L4V 1E4, Canada",
                Ratings = 8.9,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/256/256190952.jpg"
            });
            // 15
            rooms.Add(new RoomsModel
            {
                RoomId = 15,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Crowne Plaza Toronto Airport, an IHG hotel",
                    HotelId = 15
                },
                PricePerNight = 375,
                Beds = 2,
                Address = "33 Carlson Ct, Toronto, ON M9W 6H5, Canada",
                Ratings = 9.5,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/216/216881296.jpg"
            });
            // 16
            rooms.Add(new RoomsModel
            {
                RoomId = 16,
                City = "Toronto",
                HotelsModel = new HotelsModel
                {
                    HotelName = "Hampton Inn by Hilton Toronto Airport Corporate Centre",
                    HotelId = 16
                },
                PricePerNight = 242,
                Beds = 3,
                Address = "5515 Eglinton Ave W, Toronto, ON M9C 5K5, Canada",
                Ratings = 7.5,
                ImageUrl = "https://cf.bstatic.com/images/hotel/max1280x900/269/269737852.jpg"
            });
            return rooms;
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string ErrorMessage)
        {
            return View(new ErrorViewModel { Message = ErrorMessage, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
