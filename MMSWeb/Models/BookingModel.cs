using Autofac;
using Azure.Core;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Mvc.Rendering;
using MMSCore;
using MMSCore.Enum;
using MMSCore.NotMap;
using MMSService.Contact;

namespace MMSWeb.Models
{
    public class BookingModel:Booking
    {
        private readonly IBookingService _bookingService;
        public SelectList? SelectRepeatItems { get; set; }
        public DaysofworkEnum SelectedDays { get; set; } // Enum Value for Days
        public Pagination? Pagination { get; set; }
        public BookingQuery? BookingQuery { get; set; }
        public IEnumerable<Booking>? Bookings { get; set; }
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
        public async Task<List<object>> BookingList()
        {
            var bookings= await _bookingService.GetBookingList();
            var events = new List<object>(); 

            foreach (var booking in bookings)
            {
                if (booking.RepetitionOption == RepeatOption.NoRepeat)
                {
                    events.Add(new
                    {
                        title = booking.Subject ?? "No Repeat ",
                        start = booking.BookingDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                        color = "red"
                    });
                }
                else if (booking.RepetitionOption == RepeatOption.Daily && booking.EndRepeatedDate.HasValue)
                {
                    DateTime currentDate = booking.BookingDate;
                    while (currentDate <= booking.EndRepeatedDate.Value)
                    {
                        events.Add(new
                        {
                            title = booking.Subject ?? "Daily Booking",
                            start = currentDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                            color = "blue"
                        });
                        currentDate = currentDate.AddDays(1);
                    }
                }
                else if (booking.RepetitionOption == RepeatOption.Weekly && booking.EndRepeatedDate.HasValue)
                {
                    DateTime currentDate = booking.BookingDate;
                    while (currentDate <= booking.EndRepeatedDate.Value)
                    {
                        if (booking.DaysToRepeatedOn.HasValue &&
                            booking.DaysToRepeatedOn.Value.HasFlag((DaysofworkEnum)(1 << (int)currentDate.DayOfWeek)))
                        {
                            events.Add(new
                            {
                                title = booking.Subject ?? "Weekly Booking",
                                start = currentDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                                color = "green"
                            });
                        }
                        currentDate = currentDate.AddDays(1);
                    }
                }
            }
            return events;
        }
        public async Task<(IEnumerable<Booking> bookings, int TotalCount)> GetCategorysAsync(BookingQuery bookingQuery, int page, int pageSize)
        {
            return await _bookingService.GetBookingAsync(bookingQuery, page, pageSize);
        }

    }
}
