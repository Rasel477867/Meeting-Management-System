using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using MMSCore;
using MMSCore.Enum;
using MMSWeb.Models;

namespace MMSWeb.Controllers
{
    public class BookingController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await Task.Run(() => new BookingModel());
            return View(model);
        }
        public async Task<IActionResult> Add()
        {
            var model = await Task.Run(() => new BookingModel());
            model.Init();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(BookingModel bookingModel)
        {
            if (ModelState.IsValid)
            {
              

                DaysofworkEnum selectedDays = DaysofworkEnum.None;
                if (Request.Form["SelectedDays"].Any())
                {
                    foreach (var day in Request.Form["SelectedDays"])
                    {
                        selectedDays |= (DaysofworkEnum)int.Parse(day);
                    }
                    bookingModel.DaysToRepeatedOn = selectedDays;
                }
              
                var state = await Task.Run(() => new BookingModel().AddBooking(bookingModel));
                if (state)
                {
                    TempData["SuccessNotify"] = "Booking Add Successfully";
                }
                else
                {
                    TempData["InfoNotify"] = "All Ready Book this Slot";
                }
                return RedirectToAction("Add");

            }
            return RedirectToAction("Add");
        }
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var events = await Task.Run(() => new BookingModel().BookingList());
           

            return Json(events); // Convert events list to JSON and return it to FullCalendar
        }

    }
    }
