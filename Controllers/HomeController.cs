using Microsoft.AspNetCore.Mvc;
using Physiosoft.Models;
using System.Diagnostics;

namespace Physiosoft.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error(int? statusCode = null)
        {
            var viewErrorUtil = new ViewErrorUtil();

            if (statusCode.HasValue)
            {              
                switch (statusCode.Value)
                {
                    case 404:
                        viewErrorUtil.Title = "404 Not Found";
                        viewErrorUtil.Message = "The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.";
                        break;
                    case 500:
                        viewErrorUtil.Title = "500 Internal Server Error";
                        viewErrorUtil.Message = "Oops! Something went wrong on our end.";
                        break;
                    case 403:
                        viewErrorUtil.Title = "403 Forbidden";
                        viewErrorUtil.Message = "You do not have permission to access this page.";
                        break;
                    case 400:
                        viewErrorUtil.Title = "400 Bad Request";
                        viewErrorUtil.Message = "Your browser sent a request that this server could not understand.";
                        break;
                    case 401:
                        viewErrorUtil.Title = "401 Unauthorized";
                        viewErrorUtil.Message = "You are not authorized to access this page.";
                        break;
                    default:
                        viewErrorUtil.Title = "Unknown Error";
                        viewErrorUtil.Message = "An unknown error occurred.";
                        break;
                }
            }
            else
            {
                viewErrorUtil.Title = "Unknown Error";
                viewErrorUtil.Message = "An unknown error occurred.";
            }

            viewErrorUtil.StatusCode = statusCode.GetValueOrDefault();
            return View(viewErrorUtil);
        }
    }
}
