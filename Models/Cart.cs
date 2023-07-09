
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Schema;

namespace BookStore.Models
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }


        [Required(ErrorMessage = "BookName must not be null")]
        [StringLength(100, ErrorMessage = "BookName cannot exceed 100 characters")]
        public string BookName { get; set; }


        [Required(ErrorMessage = "Image must not be null")]
        [StringLength(500, ErrorMessage = "Image cannot exceed 500 characters")]
        public string Image { get; set; }


        [Required(ErrorMessage = "Quantity must be not null")]
        public int Quantity { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal TotalPrice { get; set; }



        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        public ICollection<Order> Order { get; }

    }
}
