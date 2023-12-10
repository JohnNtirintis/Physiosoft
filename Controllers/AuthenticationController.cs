using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Physiosoft.DTO.User;
using Physiosoft.Models;

using System.Security.Claims;
using Physiosoft.Service;
using Physiosoft.Repisotories;
using Physiosoft.DAO;
using Physiosoft.Security;

namespace Physiosoft.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserAuthenticationService _userAuthenticationService;
        private readonly IUserRepository _userRepository;
        private readonly IUserDAO _userDAO;
        public List<Error> ErrorsArray { get; set; } = new();

        public AuthenticationController(UserAuthenticationService userAuthenticationService, IUserRepository userRepository, IUserDAO userDAO)
        {
            _userAuthenticationService = userAuthenticationService;
            _userRepository = userRepository;
            _userDAO = userDAO;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal principal = HttpContext.User;
            if (principal.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserSignupDTO request)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState.Values)
                {
                    foreach (var error in entry.Errors)
                    {
                        ErrorsArray.Add(new Error("", error.ErrorMessage, ""));
                    }

                    ViewData["ErrorsArray"] = ErrorsArray;
                }
                return View();
            }

            try
            {
                await _userRepository.SignupUserAsync(request);
            }
            catch (Exception e)
            {
                ErrorsArray.Add(new Error("", e.Message, ""));
                ViewData["ErrorsArray"] = ErrorsArray;
                return View();
            }

            // 1st param is actionName (webpage/view)
            // 2nd param is controller (~/Controllers)
            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO credentials, bool KeepLoggedIn)
        {
            //var user = await _userAuthenticationService.AuthenticateUserAsync(credentials.Username, credentials.Password);
            var user = await _userDAO.GetUserAsync(credentials.Username);

            if (user != null && EncryptionUtil.IsValidPassword(credentials.Password, user.Password))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, credentials.Username),
                };

                if (user.IsAdmin)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = KeepLoggedIn,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                // Successful login 
                Console.WriteLine("Successful login");
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(" Else case");
                // TODO
                // handle else case

            // If ModelState is not valid, return to the login view with validation errors
            return View(credentials);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
