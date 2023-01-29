using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        [Required]
        public string ISBN { set; get; } = "";
        [Required]
        public string Author { set; get; } = "";
        [Required]
        [Range(1, 10000)]
        public int ListPrice { set; get; }
        [Required]
        [Range(1, 10000)]
        public int Price { set; get; }
        [Required]
        [Range(1, 10000)]
        public int Price50 { set; get; }
        [Required]
        [Range(1, 10000)]
        public int Price100 { set; get; }
        [ValidateNever]
        public string UrlImage { set; get; }
        [Required]
		public int CategoryId { set; get; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { set; get; }
        [Required]
		public int CoverTypeId { set; get; }
        [ForeignKey("CoverTypeId")]
        [ValidateNever]
        public CoverType CoverType { set; get; } 
    }
}
