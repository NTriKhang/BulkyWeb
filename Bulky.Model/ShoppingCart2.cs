using Microsoft.AspNetCore.Mvc;
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
	public class ShoppingCart2
	{
		[Key]
		public int Id { set; get; }
		[Range(0, 1000)]
		[Required]
		public int Count { set; get; }
		public string ApplicationUserId { set; get; }
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { set; get; }
		public int ProductId { set; get; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product product { set; get; }

	}
}
