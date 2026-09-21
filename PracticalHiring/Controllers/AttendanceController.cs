using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticalHiring.Data;
using PracticalHiring.Models;
using System.Linq;

namespace PracticalHiring.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        public IActionResult GetAttendances(int employeeId, string status, string sortColumn, string sortOrder, int page = 1)
        {
            int pageSize = 5;

            var query = _context.Attendances.Include(a => a.Employee).AsQueryable();

            if (employeeId != 0)
            {
                query = query.Where(a => a.EmployeeId == employeeId);
            }

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(a => a.Status == status);
            }

            if (sortColumn == "Date")
            {
                query = sortOrder == "desc" ? query.OrderByDescending(a => a.Date) : query.OrderBy(a => a.Date);
            }
            else
            {
                query = query.OrderByDescending(a => a.Date);
            }

            int totalRecords = query.Count();
            int totalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            var attendances = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return PartialView("_AttendanceList", attendances);
        }

        public IActionResult GetAttendanceById(int id)
        {
            var attendance = _context.Attendances.Find(id);
            return Json(attendance);
        }

        [HttpPost]
        public IActionResult Save(Attendance attendance)
        {
            if (attendance.Id == 0)
            {
                _context.Attendances.Add(attendance);
            }
            else
            {
                _context.Attendances.Update(attendance);
            }
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var attendance = _context.Attendances.Find(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }

        public IActionResult Summary()
        {
            var attendances = _context.Attendances.Include(a => a.Employee).ToList();

            var summary = attendances
                .GroupBy(a => a.Employee.Name)
                .Select(g => new AttendanceSummaryViewModel
                {
                    EmployeeName = g.Key,
                    PresentCount = g.Count(a => a.Status == "Present"),
                    AbsentCount = g.Count(a => a.Status == "Absent"),
                    LeaveCount = g.Count(a => a.Status == "Leave")
                })
                .ToList();

            return View(summary);
        }
    }
}