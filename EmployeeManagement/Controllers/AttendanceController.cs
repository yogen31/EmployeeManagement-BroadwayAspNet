using EmployeeManagement.Data;
using EmployeeManagement.Migrations;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{

    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AttendanceController(ApplicationDbContext applicationDbContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Attendance()
        {
            //get logged in user
            var logedInUser = await _userManager.GetUserAsync(User);
            ViewBag.CheckInCheck = "Check In";
            ViewBag.AttendanceId = null;
            if (logedInUser != null)
            {
                //get employee from logged in user id
                var employeeDetails = _applicationDbContext.Employee
                    .Where(emp => emp.IdentityUserId ==
                logedInUser.Id).FirstOrDefault();
                if (employeeDetails != null)
                {
                    //take last shift data
                    var employeeShiftLog = _applicationDbContext.EmployeeShiftLog
                        .Where(es => es.EmployeeId == employeeDetails.Id)
                        .OrderByDescending(es => es.Id).FirstOrDefault();
                    if (employeeShiftLog != null)
                    {
                        if (employeeShiftLog.CheckInTime != null &&
                            employeeShiftLog.CheckOutTime == null)
                        {
                            ViewBag.CheckInCheck = "Check Out";
                            ViewBag.AttendanceId = employeeShiftLog.Id;
                        }
                    }
                }
            }
            return View();
        }
        public async Task<IActionResult> CheckInEmployee(int? attendanceId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser != null)
            {
                //get employee from logged in user id
                var employeeDetails = _applicationDbContext.Employee
                    .Where(emp => emp.IdentityUserId ==
                loggedInUser.Id).FirstOrDefault();
                if (employeeDetails != null)
                {
                    var employeeShiftId = _applicationDbContext.EmployeeShift.Where(x => x.EmployeeId1 == employeeDetails.Id)
                        .Select(x => x.Id).FirstOrDefault();
                    if (attendanceId == null)
                    {
                        EmployeeShiftLog employeeShiftLog = new EmployeeShiftLog();
                        employeeShiftLog.CheckInTime = DateTime.Now;
                        employeeShiftLog.EmployeeId = employeeDetails.Id;
                        employeeShiftLog.EmployeeShiftId = employeeShiftId;
                        _applicationDbContext.EmployeeShiftLog.Add(employeeShiftLog);
                        _applicationDbContext.SaveChanges();
                    }
                    else
                    {
                        EmployeeShiftLog? employeeShiftLog = _applicationDbContext.EmployeeShiftLog
                            .Where(es => es.Id == attendanceId).FirstOrDefault();
                        if (employeeShiftLog != null)
                        {
                            employeeShiftLog.CheckOutTime = DateTime.Now;
                            _applicationDbContext.EmployeeShiftLog.Update(employeeShiftLog);
                            _applicationDbContext.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("Attendance");
        }
    }
}
