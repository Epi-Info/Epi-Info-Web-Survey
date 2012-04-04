using System.ComponentModel.DataAnnotations;

namespace Epi.Web.MVC.Models
{

    public class PassCodeModel
    {
        [Required]
        public string PassCode { get; set; }

         
    }
}