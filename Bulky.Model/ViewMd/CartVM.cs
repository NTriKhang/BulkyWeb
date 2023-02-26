using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model.ViewMd
{
	public class CartVM
	{
		public string ApplicationUserId { get; set; }
		public bool? emailcofirm { get; set; } = false;
		public int shoppingCartId { get; set; }
		[ValidateNever]
		public Product Products { get; set; }
		[Range(1,1000, ErrorMessage = "It must range 1 to 1000")]
		public int Count { get; set; }
		public int Total { get; set; }

	}
}
