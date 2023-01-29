using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model.ViewMd
{
	public class CartVM
	{
		public int shoppingCartId { get; set; }
		public Product Products { get; set; }
		public int Count { get; set; }
	}
}
