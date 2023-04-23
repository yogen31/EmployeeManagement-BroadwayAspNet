using EmployeeManagement.Data;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //manages users
        private readonly UserManager<IdentityUser> _userManager;
        //helps in signin process
        private readonly SignInManager<IdentityUser> _signInManager;
        public LoginController(ApplicationDbContext applicationDbContext,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegisterUser()
        {
            return View();
        }
        public async  Task<IActionResult> RegisterUsers(RegisterViewModel registerViewModel)
        {
            var user = new IdentityUser()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("LoginUser");
            }
            return RedirectToAction("RegisterUser");
        }
        public IActionResult LoginUser()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            //for logout
            await _signInManager.SignOutAsync();

            //retirect to login view after sign out
            return RedirectToAction("LoginUser");
        }
        public async Task<IActionResult> LoginUserDetails(LoginViewModel loginViewModel)
        {
            //passwordsigninasync takes email,password,rememberme,lockoutenabled
            var identiityResult = await _signInManager.
                PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);
            if (identiityResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("LoginUser");
        }
    }
}
