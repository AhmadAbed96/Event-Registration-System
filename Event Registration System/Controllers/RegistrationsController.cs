using Event_Registration_System.Models;
using Event_Registration_System.Data;
using Event_Registration_System.Models;
using EventRegistrationSystem.Repositories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventRegistrationSystem.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly EventDbContext _context;
        private readonly EmailService _emailService;

        public RegistrationsController(EventDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Register(int eventId)
        {
            var eventItem = await _context.events.FindAsync(eventId);
            if (eventItem == null)
            {
                return NotFound();
            }

            ViewBag.EventTitle = eventItem.Title;

            // Create a new Registration object
            var registration = new Registration
            {
                EventId = eventItem.Id,
                // Add any other properties you need to populate
            };

            return View("Register", registration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.registrations.Add(registration);
                await _context.SaveChangesAsync();

                // Trigger email notification
                var eventItem = await _context.events.FindAsync(registration.EventId);
                Console.WriteLine(registration.Email);
                await _emailService.SendEmail(registration.Email, registration.ParticipantName, eventItem.Title);

                return RedirectToAction("Index", "Home");
            }
            var eventTitle = (await _context.events.FindAsync(registration.EventId))?.Title;
            ViewBag.EventTitle = eventTitle;
            return View(registration);
        }
    }
}