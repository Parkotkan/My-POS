using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_API.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public string Status { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000.00)]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<Order_item>? Order_items { get; set; }
    }
}