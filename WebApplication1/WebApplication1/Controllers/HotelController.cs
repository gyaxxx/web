using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HotelController : Controller
    {
        private readonly HotelContext database;
        public float roomPrice;
        public string roomDes;

        public HotelController(HotelContext context)
        {
            this.database = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Staff inputData)
        {
            foreach(Staff s in database.Staffs)
            {
                if (s.Username == inputData.Username && s.Password == inputData.Password && s.Role == "Staff")
                {
                    return RedirectToAction("Staff");
                }
                else if (s.Username == inputData.Username && s.Password == inputData.Password && s.Role == "Employer")
                {
                    return RedirectToAction("Employer");
                }
            }

            return View();
        }

        public IActionResult Book() {
            return View();
        }

        [HttpGet]
        public IActionResult BookConfirmed() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult BookConfirmed(GuestRoom_Booking data)
        {
            if(ModelState.IsValid)
            {
                Guest newGuest = new()
                {
                    FirstName = data.guest.FirstName,
                    LastName = data.guest.LastName,
                    Email = data.guest.Email,
                    PhoneNumber = data.guest.PhoneNumber,
                };

                if (data.room.RoomTypeName == "Superior")
                {
                    roomPrice = 1000000;
                    roomDes = "01 Double bed, city view, 21m2.";

                }
                else if (data.room.RoomTypeName == "Deluxe")
                {
                    roomPrice = 1200000;
                    roomDes = "02 Double beds, city view, 23m2.";
                }
                else if (data.room.RoomTypeName == "Senior Deluxe")
                {
                    roomPrice = 1400000;
                    roomDes = "02 Double beds, city view, 32m2.";
                }
                else if (data.room.RoomTypeName == "Family Room")
                {
                    roomPrice = 1600000;
                    roomDes = "02 Double beds, city view, 41m2.";
                }

                Room room = new()
                {
                    RoomNumber = data.room.RoomNumber,
                    Floor = data.room.Floor,
                    RoomTypeName = data.room.RoomTypeName,
                    Price = roomPrice,
                    Description = roomDes,
                    IsActive = true,
                };
                    database.Guests.Add(newGuest);
                    database.Rooms.Add(room);
                    database.SaveChanges();

                int newGuestid = newGuest.ID;
                int roomid = room.Id;
                Reservations reservations = new()
                {
                    CreatedDate = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    GuestID = newGuestid,
                    RoomID = roomid,
                    CheckInDate = data.reservations.CheckInDate,
                    CheckOutDate = data.reservations.CheckOutDate,
                    Status = 1,
                };

                database.Reservations.Add(reservations);
                database.SaveChanges();
                TempData["successMessage"] = "Employee created successfully!";
                return RedirectToAction("Index");
            }
            return View();   
        }

        // quan hệ 1 - n -> trong bảng nhiều sẽ lưu id của 1
        // quan hệ n - n -> 
        public IActionResult Staff()
        {
            return View();
        }

        public IActionResult Employer()
        {
            return View();
        }
    }
}