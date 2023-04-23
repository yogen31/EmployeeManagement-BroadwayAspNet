using EmployeeManagement.Data;
using EmployeeManagement.DTO;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //Accessing Db Context to interact with database in entity framework
        private readonly ApplicationDbContext _applicationDbContext;
        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEmployee()
        {
            ViewBag.DepartmentList = _applicationDbContext.Department.
                ToList();

            return View();
        }
        public IActionResult GetAllEmployee()
        {
            List<Employee> employees = _applicationDbContext.Employee.
                Include(x => x.Department).ToList();
            return View(employees);
        }
        public IActionResult GetEmployeeById(int id)
        {
            //get employee by id
            Employee employee = _applicationDbContext.Employee.
                Include(x => x.Department).
                FirstOrDefault(emp => emp.Id == id);

            ViewBag.DepartmentList = _applicationDbContext.
                Department.ToList();
            return View(employee);
        }
        [HttpPost]
        public IActionResult UpdateEmployeeById(Employee employee)
        {
            //update employee by id
            _applicationDbContext.Employee.Update(employee);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("GetAllEmployee");
        }
        public IActionResult RemoveEmployeeById(int Id)
        {
            //update employee by id
            Employee employee = _applicationDbContext.Employee.
                FirstOrDefault(emp => emp.Id == Id);
            _applicationDbContext.Employee.Remove(employee);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("GetAllEmployee");
        }
        public IActionResult AddEmployeeDetails(Employee employee)
        {
            //Add employee instance in Employee table
            _applicationDbContext.Employee.Add(employee);
            //Save the data
            _applicationDbContext.SaveChanges();
            //Redirect to Add Employee view
            return RedirectToAction("AddEmployee");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult GetEmployeeInformation()
        {
            //get employee and department Information using Linq query
            //dynamic object
            var employeeInformationTest = from emp in _applicationDbContext.Employee
                                          join dep in _applicationDbContext.Department on
                                          emp.DepartmentId equals dep.Id
                                          select new
                                          {
                                              EmployeeName = emp.Name,
                                              EmployeeAddress = emp.Address,
                                              DepartmentName = dep.Name,
                                              DepartmentDescription = dep.Description,
                                          };
            //object mapped to DTO
            var employeeInformation = from emp in _applicationDbContext.Employee
                                      join dep in _applicationDbContext.Department on
                                      emp.DepartmentId equals dep.Id into empDep
                                      from empDetails in empDep.DefaultIfEmpty()
                                      select new EmployeeInformationDTO(emp.Name,
                                      emp.Address, empDetails.Name, empDetails.Description);
            ViewBag.EmployeeInformation = employeeInformation;

            return View();
        }
        public IActionResult GetEmployeeDetails()
        {
            //get employee and department Information using Linq query
            //dynamic object
            var employeeInformationTest = from emp in _applicationDbContext.Employee
                                          join dep in _applicationDbContext.Department on
                                          emp.DepartmentId equals dep.Id
                                          select new
                                          {
                                              EmployeeName = emp.Name,
                                              EmployeeAddress = emp.Address,
                                              DepartmentName = dep.Name,
                                              DepartmentDescription = dep.Description,
                                          };
            //object mapped to DTO
            var employeeInformation = from emp in _applicationDbContext.Employee
                                      join dep in _applicationDbContext.Department on
                                      emp.Name equals dep.Name into empDep
                                      from empDetails in empDep.DefaultIfEmpty()
                                      select new EmployeeInformationDTO(emp.Name,
                                      emp.Address, empDetails.Name, empDetails.Description);
            //right join
            var employeeInformationRight = from dep in _applicationDbContext.Department
                                           join emp in _applicationDbContext.Employee on
                                      dep.Name equals emp.Name into empDep
                                      from empDetails in empDep.DefaultIfEmpty()
                                      select new EmployeeInformationDTO(empDetails.Name,
                                      empDetails.Address, dep.Name, dep.Description);
            ViewBag.EmployeeInformation = employeeInformationRight;
            ViewBag.CountryName = "Nepal";

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}