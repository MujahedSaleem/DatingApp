using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class FallbackController : Controller
    {
        private readonly IHostingEnvironment hosting;
        public FallbackController(IHostingEnvironment hosting)
        {
            this.hosting = hosting;
        }
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(hosting.ContentRootPath,
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}