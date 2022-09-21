using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EInsurance.Pages
{
    public class MyServiceModel : PageModel
    {
        private readonly ILogger<MyServiceModel> _logger;

        public MyServiceModel(ILogger<MyServiceModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
