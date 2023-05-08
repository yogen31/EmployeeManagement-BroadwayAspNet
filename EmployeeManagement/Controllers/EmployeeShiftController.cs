using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeShiftController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public EmployeeShiftController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext= applicationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddEmployeeShift()
        {
            ViewBag.EmployeeList = _applicationDbContext.Employee.ToList();
            return View();
        }
        public IActionResult AddEmployeeShiftDetails( EmployeeShift employeeShift)
        {
            _applicationDbContext.EmployeeShift.Add(employeeShift);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("AddEmployeeShift");
        }
    }
}
