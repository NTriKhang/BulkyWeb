using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model.ViewMd
{
    public class SignRole
    {
        public ApplicationUser applicationUser { get; set; }
        public string specificRole { get; set; }

    }
}
