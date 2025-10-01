using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WorkingSpaces.Pages
{
    public class BookingModel : PageModel
    {
        public static BookingManager Manager = new BookingManager();
        static BookingModel()
        {
            Manager.Spaces.AddRange(new List<Space>
            {
                new Space { RoomId = 1, NumberOfSeats = 3, AvailableEquipment = Equipment.TV | Equipment.Board },
                new Space { RoomId = 2, NumberOfSeats = 10, AvailableEquipment = Equipment.Projector | Equipment.Computers },
                new Space { RoomId = 3, NumberOfSeats = 5, AvailableEquipment = Equipment.Board },
                new Space { RoomId = 4, NumberOfSeats = 8, AvailableEquipment = Equipment.TV | Equipment.Computers }
            });
        }


            [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? SelectedDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Start Time is required")]
        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "End Time is required")]
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string UserEmail { get; set; }

        [BindProperty]
        public int SpaceId { get; set; } 

        public required List<Space> AvailableSpaces { get; set; }
        public required string BookingMessage { get; set; } 

         public required List<Booking> CurrentBookings { get; set; }

        public void OnGet()
        {
            CurrentBookings = Manager.Bookings
                .OrderBy(b => b.StartTime)
                .ToList();
        }

        public void OnPost()
        {
            if (SelectedDate.HasValue && SelectedDate.Value.Date >= DateTime.Today)
            {
                AvailableSpaces = Manager.Spaces;
            }
            else
            {
                AvailableSpaces = new List<Space>();
                if (SelectedDate.HasValue && SelectedDate.Value.Date < DateTime.Today)
                {
                    ModelState.AddModelError("SelectedDate", "Date cannot be in the past.");
                }
            }
            CurrentBookings = Manager.Bookings
                .OrderBy(b => b.StartTime)
                .ToList();
        }

        public IActionResult OnPostBook()
        {

            if (ModelState.IsValid && SelectedDate.HasValue && StartTime.HasValue && EndTime.HasValue)
            {
                var space = Manager.Spaces.FirstOrDefault(s => s.RoomId == SpaceId);

                if (space == null)
                {
                    BookingMessage = "Error: Selected room not found.";
                    return Page();
                }

                var startDate = SelectedDate.Value.Date;
                var startDateTime = startDate.Add(StartTime.Value);
                var endDateTime = startDate.Add(EndTime.Value);

                var startOffset = new DateTimeOffset(startDateTime, TimeZoneInfo.Local.GetUtcOffset(startDateTime));
                var endOffset = new DateTimeOffset(endDateTime, TimeZoneInfo.Local.GetUtcOffset(endDateTime));

                var booking = Manager.CreateBooking(space, UserEmail, startOffset, endOffset);

                if (booking != null)
                {
                    BookingMessage = $"Success! Booking created: {booking.ToString()}";
                }
                else
                {
                    BookingMessage = "Booking failed! The selected time slot overlaps with an existing booking, or the time is invalid (e.g., in the past, after 23:00, or end time before start time, or duration less than 15 minutes).";
                }
            }
            else
            {
                BookingMessage = "Validation Error: Please check your input.";
            }

            if (SelectedDate.HasValue)
            {
                 AvailableSpaces = Manager.Spaces;
            }

            if (SelectedDate.HasValue)
            {
                 AvailableSpaces = Manager.Spaces;
            }
            CurrentBookings = Manager.Bookings
                .OrderBy(b => b.StartTime)
                .ToList();

            return Page();
        }
    }
}