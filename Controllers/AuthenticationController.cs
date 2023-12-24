using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Physiosoft.DTO.User;
using Physiosoft.Models;
using Physiosoft.Logger;
using System.Security.Claims;
using Physiosoft.Service;
using Physiosoft.Repisotories;
using Physiosoft.DAO;
using Physiosoft.Security;
using Microsoft.EntityFrameworkCore;

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

        // TODO RETURN WITH ERRORS IF FAIL
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
            ClaimsPrincipal principal = HttpContext.User;
            if (principal.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
                        NLogger.LogError($"Error: {error.ErrorMessage}");
                    }

                    ViewData["ErrorsArray"] = ErrorsArray;
                }
                return View();
            }

            try
            {
                await _userRepository.SignupUserAsync(request);
            }catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    ModelState.AddModelError("", "The entered value already exists. Please use a unique value.");
                }
                else
                {
                    NLogger.LogError($"Error occurred while signing up a user entity.");
                }
            }
            catch (Exception ex)
            {        
                NLogger.LogError($"Error: {ex.Message}");
                ErrorsArray.Add(new Error("", ex.Message, ""));
                ViewData["ErrorsArray"] = ErrorsArray;
                return View();
            }
            return RedirectToAction("Login", "Authentication");
        }

        // DOESNT REUTNR WITH ERRORS
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO credentials, bool KeepLoggedIn)
        {

            if (!ModelState.IsValid)
            {
                return View(credentials);
            }

            try
            {
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
                    return RedirectToAction("Index", "Home");
                }
            } 
            catch (Exception ex)
            {
                NLogger.LogError($"Error: in Login! Exception: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while logging in.");
                return View(credentials);
                
            }
            // If ModelState is not valid, return to the login view with validation errors
            ModelState.AddModelError("", "Username and/or Password is incorrect.");
            return View(credentials);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            } 
            catch (Exception ex)
            {
                NLogger.LogError($"Error: in Logout! Exception: {ex.Message}");
                return BadRequest("An error occurred while logging out.");
            }      
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check if the exception is due to a unique constraint violation
            return ex.InnerException?.Message.Contains("unique constraint") ?? false;
        }
    }
}
