using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVC.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Pending";

        public User? User { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}