using System.ComponentModel.DataAnnotations;

namespace ECommerceMySQL.Web.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string ShippingAddress { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
