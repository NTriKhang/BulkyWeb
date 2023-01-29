using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly Application _db;

        public ProductRepository(Application db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objBind = _db.products.FirstOrDefault(u => u.Id == obj.Id);
            if(objBind != null)
            {
                objBind.Title = obj.Title;
                objBind.Author = obj.Author;
                objBind.Description = obj.Description;
                objBind.Price = obj.Price;
                objBind.ListPrice = obj.ListPrice;
                objBind.Price50 = obj.Price50;
                objBind.Price100 = obj.Price100;
                objBind.ISBN = obj.ISBN;
                objBind.CoverTypeId = obj.CoverTypeId; 
                objBind.CategoryId = obj.CategoryId;
                if(obj.UrlImage != "")
                {
                    objBind.UrlImage = obj.UrlImage;
                }
            }
        }

	}
}
