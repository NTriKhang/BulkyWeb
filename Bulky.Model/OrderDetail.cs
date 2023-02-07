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
    public class OrderDetail
    {
        [Key]
        public int Id { set; get; }
        public int OrderId { set; get; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public OrderHeader OrderHeader { set; get; }
        public int ProductId { set; get; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { set; get; }
        public int Count { set; get; }
        public int Price { set; get; }
    }
}
