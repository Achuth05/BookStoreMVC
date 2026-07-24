using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVC.Models
{
    [Table("orderitems")]
    public class OrderItem
    {
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("book_id")]
        public int BookId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        public Order? Order { get; set; }

        public Book? Book { get; set; }
    }
}