using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EInsurance.Pages
{
    public class ServiceModel : PageModel
    {
        private readonly ILogger<ServiceModel> _logger;

        public ServiceModel(ILogger<ServiceModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
