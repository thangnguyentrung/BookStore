using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookStore.Models
{
    [Table("Admin")]
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }


        [Required(ErrorMessage = "UserName must be not null")]
        [RegularExpression("^[a-zA-Z ]{5,20}$,",ErrorMessage = "UserName contain from 5 to 20 char")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password must be not null")]
        [RegularExpression("^[a-zA-Z0-9 ]{5,20}$", ErrorMessage = "Password contain from 5 to 20 char")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Email must be not null")]
        [RegularExpression("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$", ErrorMessage = "Email is invalid")]

        public string Email { get; set; }


        [Required(ErrorMessage = "FullName must be not null")]
        [StringLength(50)]
        public string FullName { get; set; }
    }
}
