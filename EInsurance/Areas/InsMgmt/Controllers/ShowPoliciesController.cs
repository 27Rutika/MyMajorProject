using EInsurance.Areas.InsMgmt.ViewModels;
using EInsurance.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EInsurance.Areas.InsMgmt.Controllers
{
    [Area("InsMgmt")]
    public class ShowPoliciesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ShowPoliciesController> _logger;

        public ShowPoliciesController(
            ApplicationDbContext dbContext,
            ILogger<ShowPoliciesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            PopulateDropDownListToSelectCategory();

            _logger.LogInformation("--- extracted Policies");
            return View();
        }

        private void PopulateDropDownListToSelectCategory()
        {
            List<SelectListItem> insurances = new List<SelectListItem>();
            insurances.Add(new SelectListItem
            {
                Text = "----- select a policy -----",
                Value = "",
                Selected = true
            });
            insurances.AddRange(new SelectList(_dbContext.Insurances, "InsuranceId", "InsuranceName"));

            ViewData["InsuranceCollection"] = insurances;
        }

        // NOTE: Error messages added by Server-side code will disappear only on "SUBMIT"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("InsuranceId")] ShowPoliciesViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDownListToSelectCategory();

                // Something is wrong with the viewmodel.  So, just return it back to the view with the ModelState errors!
                return View(viewmodel);
            }

            // Now performing server-side validation - checking if any books exist for the selected category
            bool policyExist = _dbContext.InsurancePolicy.Any(b => b.InsuranceId == viewmodel.InsuranceId);
            if (!policyExist)
            {
                //--- Error will be shown as part of the Validation Summary
                ModelState.AddModelError("", "No policies were found for the selected Insurance category!");

                //--- Error will be attached to the UI Control mapped by the asp-for attribute.
                // ModelState.AddModelError("CategoryId", "No books were found for this category");

                PopulateDropDownListToSelectCategory();
                return View(viewmodel);         // return the viewmodel with the ModelState errors!
            }

            return RedirectToAction(
                actionName: "GetPolicyOfCategory",
                controllerName: "InsurancePolicies",
                routeValues: new { area = "InsMgmt", filterInsuranceId = viewmodel.InsuranceId });
        }

    }
}
