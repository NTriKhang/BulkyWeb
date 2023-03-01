using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Model.ViewMd
{
    public class SignRoleVM
    {
        public List<SignRole> signRoles { get; set; }
        public List<SelectListItem> roles { get; set; }
    }
}
