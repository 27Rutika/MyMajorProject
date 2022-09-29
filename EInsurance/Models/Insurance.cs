using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [JsonIgnore]       // Suppress the information about the FK Collection to the API.
        #region 
        public ICollection<InsurancePolicy> InsurancePolicy { get; set; }

        #endregion
    }
}
