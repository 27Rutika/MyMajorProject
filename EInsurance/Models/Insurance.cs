using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EInsurance.Models
{
   
    public class Insurance
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Insurance Id")]
        public int InsuranceId { get; set; }//Itemcategory Id

        [Display(Name = "Insurance Name")]
        [Required]
        [StringLength(100)]
        public string InsuranceName { get; set; }//Itemcategory Name

        #region 
        public ICollection<InsurancePolicy> InsurancePolicy { get; set; }

        #endregion
    }
}
