using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarketList.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int IdCategory { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int BaseQty { get; set; }
        
        public string BaseUnit { get; set; }
        
        public virtual Category Category { get; set; }
    }
}
