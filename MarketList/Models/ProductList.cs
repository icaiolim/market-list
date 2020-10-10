using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MarketList.Models
{
    public class ProductList
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("Product")]
        public int IdProduct { get; set; }
        
        [Required]
        public int Qty { get; set; }
        
        public bool Checked { get; set; }

        public virtual Product Product { get; set; }

    }
}
