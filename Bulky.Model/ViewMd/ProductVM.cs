using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model.ViewMd
{
	public class ProductVM
	{
		public Product product { set; get; }
        [ValidateNever]
		public IEnumerable<SelectListItem> Category { set; get; }
        [ValidateNever]
		public IEnumerable<SelectListItem> CoverType { set; get; }
	}
}
