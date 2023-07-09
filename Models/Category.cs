using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "CategoriesName must be not null")]
        [StringLength(50, ErrorMessage = "BookName is not more than 50 char")]
        public string CategoryName { get; set; }

        public ICollection<Book> Books { get; set; }
     
    }
}
