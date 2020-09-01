using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CouponType { get; set; }
        public enum Coupon_Enum { Percent_Coupon, Dolloar_Coupon }
        [Required]
        public double Discount { get; set; }
        [Required]
        public double MinimumAmount { get; set; }
        [Required]
        public byte[] CouponImage { get; set; }
        [Required]
        public bool isCouponActive { get; set; }
    }
}
