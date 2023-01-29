using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
	public class CompanyRepository : Repository<Company>, ICompanyRepository
	{
		private readonly Application _db;
		public CompanyRepository(Application db) : base(db)
		{
			_db = db;
		}
		public void Update(Company company)
		{
			_db.companies.Update(company);
		}
	}
}
