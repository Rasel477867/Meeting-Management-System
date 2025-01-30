using Autofac;
using Azure.Core;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Mvc.Rendering;
using MMSCore;
using MMSCore.Enum;
using MMSService.Contact;

namespace MMSWeb.Models
{
    public class BookingModel:Booking
    {
        private readonly IBookingService _bookingService;
        public SelectList? SelectRepeatItems { get; set; }
        public DaysofworkEnum SelectedDays { get; set; } // Enum Value for Days

        public BookingModel()
        {
           _bookingService=Startup.AutofacContainer.Resolve<IBookingService>();
        }
        public async Task<bool> AddBooking(Booking booking)
        {
           
            var state = await _bookingService.IsBookingAvailableAsync(booking);
            if (state)
            {
               return await _bookingService.AddAsync(booking);
            }
            else
            {
                return false;
            }
          
        }
       
        public void Init()
        {
            var items = Enum.GetValues(typeof(RepeatOption))
                .Cast<RepeatOption>()
                .Select(option => new SelectListItem()
                {
                    Text = option.ToString().Replace("_", "/"), 
                    Value = ((int)option).ToString() 
                }).ToList(); 

            SelectRepeatItems = new SelectList(items, "Value", "Text");
        }

    }
}
