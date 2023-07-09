
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required(ErrorMessage = "BookName must not be null")]
        [StringLength(100, ErrorMessage = "BookName cannot exceed 100 characters")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Description must not be null")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string Description { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid Date")]
        public DateTime DateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity must not be null")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Image must not be null")]
        [StringLength(500, ErrorMessage = "Image cannot exceed 500 characters")]
        public string Image { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Cart> Cart { get; set; }
    }
}
