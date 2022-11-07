using BaseProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();
            if (User.Claims.Where(x => x.Type == Settings.claimDbType).FirstOrDefault() != null)
            {
                settings.DatabaseType = (EnumDatabaseType)int.Parse(User.Claims.First(x => x.Type == Settings.claimDbType).Value);
            }
            else
                settings.DatabaseType = settings.GetDefaultDbType;

            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var claim = new Claim(Settings.claimDbType, databaseType.ToString());

            var claims = await _userManager.GetClaimsAsync(user);

            var hasDbTypeClaim = claims.FirstOrDefault(x => x.Type == Settings.claimDbType);

            if (hasDbTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDbTypeClaim, claim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, claim);
            }

            await _signInManager.SignOutAsync();

            var authResult= await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, authResult.Properties);

            return RedirectToAction(nameof(Index));
        }
    }
}
