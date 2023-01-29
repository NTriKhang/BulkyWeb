using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly Application _db;
        public CoverTypeRepository(Application db) : base(db)
        {
            _db = db;
        }
        public void Update(CoverType obj)
        {
            _db.coverTypes.Update(obj);
        }
    }
}
