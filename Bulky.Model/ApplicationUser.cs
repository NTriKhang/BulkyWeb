﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; } = "";
		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? PostalCode { get; set;}
		[ValidateNever]
		public int? CompanyId { get; set;}
		[ForeignKey("CompanyId")]
		[ValidateNever]
		public Company CompanyList { get; set; }
	}
}
