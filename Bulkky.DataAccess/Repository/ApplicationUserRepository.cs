using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public class ApplicationUserRepository : Repository<ApplicationUser> ,IApplicationUserRepository
    {
        private readonly Application _db;
        public ApplicationUserRepository(Application db) : base(db) 
        {
            _db = db;
        }
    }
}
