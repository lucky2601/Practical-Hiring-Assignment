using Microsoft.AspNetCore.Mvc;
using PracticalHiring.Data;
using System;
using System.Linq;

namespace PracticalHiring.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;

            ViewBag.TotalEmployees = _context.Employees.Count();
            ViewBag.ActiveEmployees = _context.Employees.Count(e => e.IsActive);
            ViewBag.InactiveEmployees = _context.Employees.Count(e => !e.IsActive);
            ViewBag.PresentToday = _context.Attendances.Count(a => a.Date.Date == today && a.Status == "Present");
            ViewBag.AbsentToday = _context.Attendances.Count(a => a.Date.Date == today && a.Status == "Absent");

            return View();
        }
    }
}