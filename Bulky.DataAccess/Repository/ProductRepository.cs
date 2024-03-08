using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(Product product)
        {
	        var newProduct = _db.Products.SingleOrDefault(p => p.Id == product.Id);

	        if (newProduct != null)
	        {
		        newProduct.Title = product.Title;
                newProduct.Description = product.Description;
                newProduct.ISBN = product.ISBN;
                newProduct.Price = product.Price;
                newProduct.Price50 = product.Price50;
                newProduct.ListPrice = product.ListPrice;
                newProduct.Price100 = product.Price100;
                newProduct.CategoryId = product.CategoryId;
                newProduct.Author = product.Author;
                if(product.ImageUrl != null)
                    newProduct.ImageUrl = product.ImageUrl;
	        }
        }
    }
}
