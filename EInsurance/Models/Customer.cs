using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EInsurance.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }


        [Display(Name = "Full Name")]
        [Required]
        [StringLength(50)]
        [RegularExpression(@"(^[a-zA-Z''-'\s]{1,40}$)", ErrorMessage = "Incorrect Name")]
        public string FullName { get; set; }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        public int Age { get; set; }


        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "{0} is not valid.")]
        public string Email { get; set; }


        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} Cannot be Empty")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required.")]
        [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Invalid date format.")]
        public string BirthDate { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "{0} Cannot be empty")]
        [RegularExpression(@"(^[a-zA-Z''-'\s]{1,40}$)", ErrorMessage = "Incorrect Gender")]
        public string Gender { get; set; }


        [Display(Name = "Address")]
        [Required(ErrorMessage = "{0} Cannot be empty")]
        public string Address { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "{0} Cannot be empty")]
        public string Country { get; set; }

        #region Navigation Properties to InsurancePolicy Model
        public int PolicyId { get; set; }
        [ForeignKey(nameof(Customer.PolicyId))]

        public InsurancePolicy InsurancePolicy { get; set; }


        #endregion

        public ICollection<PolicyStatus> PolicyStatus { get; set; }
    }
}
