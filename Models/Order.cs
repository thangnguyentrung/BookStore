using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace BookStore.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int OrderId { get; set; }
        [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
        public string OrderName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        public string PhoneNumbers { get; set; }
        [Required(ErrorMessage = "Địa chỉ nhận hàng là bắt buộc")]
        public string DeliveryAddress { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
