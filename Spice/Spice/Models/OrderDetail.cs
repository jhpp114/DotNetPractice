﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; }
        [Required]
        public int MenuitemId { get; set; }
        [ForeignKey("MenuitemId")]
        public virtual MenuItem MenuItem { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
