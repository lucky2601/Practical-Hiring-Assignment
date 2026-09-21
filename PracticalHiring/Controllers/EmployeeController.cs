using Microsoft.AspNetCore.Mvc;
using PracticalHiring.Data;
using PracticalHiring.Models;
using System.Linq;

namespace PracticalHiring.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult GetEmployees(string searchTerm, string department, string status, string sortColumn, string sortOrder, int page = 1)
        {
            int pageSize = 6;

            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm) || e.Code.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(department) && department != "All")
            {
                query = query.Where(e => e.Department == department);
            }

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                bool isActive = status == "Active";
                query = query.Where(e => e.IsActive == isActive);
            }

            if (sortColumn == "Name")
            {
                query = sortOrder == "desc" ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
            }
            else if (sortColumn == "Department")
            {
                query = sortOrder == "desc" ? query.OrderByDescending(e => e.Department) : query.OrderBy(e => e.Department);
            }
            else if (sortColumn == "Salary")
            {
                query = sortOrder == "desc" ? query.OrderByDescending(e => e.Salary) : query.OrderBy(e => e.Salary);
            }
            else
            {
                query = query.OrderBy(e => e.Id);
            }

            int totalRecords = query.Count();
            int totalPages = (int)System.Math.Ceiling((double)totalRecords / pageSize);

            var employees = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return PartialView("_EmployeeList", employees);
        }

        
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _context.Employees.Find(id);
            return Json(employee);
        }






        [HttpPost]
        public IActionResult Save(Employee employee)
        {
            bool codeExists = _context.Employees
                .Any(e => e.Code == employee.Code && e.Id != employee.Id);

            if (codeExists)
            {
                ModelState.AddModelError("Code", "This Employee Code is already in use");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage
                    );

                return Json(new { success = false, errors = errors });
            }

            if (employee.Id == 0)
                _context.Employees.Add(employee);
            else
                _context.Employees.Update(employee);

            _context.SaveChanges();
            return Json(new { success = true });
        }

     
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }



        public IActionResult CheckCodeExists(string code, int id = 0)
        {
            bool exists = _context.Employees.Any(e => e.Code == code && e.Id != id);
            return Json(exists);
        }
    }
}