using System.Diagnostics;
using System.Security.Claims;
using FluentValidation;
using Intranet_NEW.Models;
using Intranet_NEW.Models.WEB;
using Intranet_NEW.Services;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Intranet_NEW.Controllers
{
    public class HomeController : Controller
    {
        
        [Authorize]
        public IActionResult Feed()
        {
            return View();
        }


    }


}
