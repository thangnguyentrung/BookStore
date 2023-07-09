using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    [Table("Author")]
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }


        [Required(ErrorMessage = "AuthorName must be not null")]
        [StringLength(100, ErrorMessage = "BookName is not more than 100 char")]
        public string AuthorName { get; set; }


        [DataType(DataType.Date, ErrorMessage = "BirthDate is invalid")]
        public DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "Nationality must be not null")]
        public string Nationality { get; set; }


        [Required(ErrorMessage = "Story must be not null")]
        public string Story { get; set; }


        public ICollection<Book> Books { get; set; }
       

    }
}
