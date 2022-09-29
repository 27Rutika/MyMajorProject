using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EInsurance.Models
{
    [Table("InsurancePoilcies")]
    public class InsurancePolicy
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Policy Id")]
        public int PolicyId { get; set; }

        [Display(Name = "Policy Name")]
        [Required(ErrorMessage = "{0} Cannot be Empty")]
        [StringLength(80)]
        public string PolicyName { get; set; }

        [Display(Name = "Sum Assurance")]
        [Required(ErrorMessage = "{0} cannot be Empty!")]
        public int SumAssurance { get; set; }

        [Display(Name = "Premium")]
        [Required(ErrorMessage = "{0} cannot be Empty!")]
        public int Premium { get; set; }

        [Display(Name = "Tenure")]
        [Required(ErrorMessage = "{0} cannot be Empty!")]
        public int Tenure { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} cannot be Empty!")]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        #region Navigation Properties to the Insurance model

        [Display(Name = "Insurance Name")]
        public int InsuranceId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]   // Suppress the information about the FK Collection to the API.
        [ForeignKey(nameof(InsurancePolicy.InsuranceId))]

        public Insurance Insurance { get; set; }

        #endregion

        #region
        public ICollection<Customer> Customers { get; set; }
        #endregion
    }
}
