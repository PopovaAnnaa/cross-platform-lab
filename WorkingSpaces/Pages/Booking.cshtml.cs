using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkingSpaces.Pages
{
    public class BookingModel : PageModel
    {
        // In-memory demo data
        public class Space
        {
            public int Id { get; set; }
            public string Room { get; set; }
            public int TableNumber { get; set; }
        }

        public static List<Space> AllSpaces = new List<Space>
        {
            new Space { Id = 1, Room = "A", TableNumber = 1 },
            new Space { Id = 2, Room = "A", TableNumber = 2 },
            new Space { Id = 3, Room = "B", TableNumber = 1 },
            new Space { Id = 4, Room = "B", TableNumber = 2 }
        };

        public static List<(DateTime Date, int TableId)> Bookings = new List<(DateTime, int)>();

        [BindProperty]
        public DateTime? SelectedDate { get; set; }

        public List<Space> AvailableSpaces { get; set; }

        // Handles GET requests
        public void OnGet()
        {
            AvailableSpaces = null; // Initially no data
        }

        // Handles form submission to check availability
        public void OnPost()
        {
            if (SelectedDate == null) return;

            AvailableSpaces = AllSpaces
                .Where(s => !Bookings.Any(b => b.Date == SelectedDate && b.TableId == s.Id))
                .ToList();
        }

        // Handles booking action
        public IActionResult OnPostBook(int TableId)
        {
            if (SelectedDate != null)
            {
                Bookings.Add((SelectedDate.Value, TableId));
            }
            // Redirect to the same page to refresh available spaces
            return RedirectToPage(new { SelectedDate });
        }
    }
}
