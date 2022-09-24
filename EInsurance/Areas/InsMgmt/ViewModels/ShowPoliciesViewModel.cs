using EInsurance.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EInsurance.Areas.InsMgmt.ViewModels
{
    public class ShowPoliciesViewModel
    {
        [Display(Name = "Select Insurance:")]
        [Required(ErrorMessage = "Please select a Insurance for displaying the Polcies")]
        public int InsuranceId { get; set; }

        public ICollection<Insurance> Insurance { get; set; }
    }
}
