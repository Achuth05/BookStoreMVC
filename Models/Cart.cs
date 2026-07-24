using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreMVC.Models
{
    [Table("cart")]
    public class Cart
    {
        [Key]
        [Column("cart_id")]
        public int CartId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("book_id")]
        public int BookId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}