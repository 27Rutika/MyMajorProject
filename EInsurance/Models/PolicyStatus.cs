using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EInsurance.Models
{
    public class PolicyStatus
    {

        [Display(Name = "Approved Policy Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovedPolicyId { get; set; }


        //-------Status----------//
        [Display(Name = "Status")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [RegularExpression(@"(^[a-zA-Z''-'\s]{1,40}$)", ErrorMessage = "Must be Character")]
        [StringLength(50)]
        public string Status { get; set; }


        [Display(Name = "Apporoval Date")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [DataType(DataType.Date)]
        public DateTime ApporovalDate { get; set; }


        #region Navigation Properties to the Customer model
        public int CustomerId { get; set; }

        [ForeignKey(nameof(PolicyStatus.CustomerId))]
        public Customer Customer { get; set; }

        #endregion
    }
}

