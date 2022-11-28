using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Observer.Models;
using WebApp.Observer.Observer;

namespace BaseProject.Controllers
{
    public class AccountController : Controller
    { 
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserObserverSubject _userObserverSubject;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UserObserverSubject userObserverSubject)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userObserverSubject = userObserverSubject;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if(hasUser == null) return View("Error");

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);

            if (!signInResult.Succeeded) return View();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateViewModel userCreateViewModel)
        {
            var appuser = new AppUser { UserName = userCreateViewModel.Email, Email = userCreateViewModel.Email };
            var result = await _userManager.CreateAsync(appuser, userCreateViewModel.Password);

            if (result.Succeeded)
            {
                ViewBag.message = "Create User Succeeded";
                _userObserverSubject.NotifyObserver(appuser);
            }
            else
            {
                ViewBag.message = result.Errors.ToList().First().Description;
            }

            return View();
        }
    }
}
