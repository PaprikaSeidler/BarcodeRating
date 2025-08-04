using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeRatingLib.Models;


namespace BarcodeRatingLib
{
    public class ProductRepository
    {
        private readonly DatabaseContext _context;
        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            List<Product> result = _context.Products.AsQueryable()
                .OrderBy(p => p.Category)
                .ToList();
            return result;
        }

        public Product Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product? GetByBarcode(int barcode)
        {
            return _context.Products.FirstOrDefault(p => p.Barcode == barcode);
        }

        public Product? Delete(int barcode)
        {
            Product? product = GetByBarcode(barcode);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return product;
        }


    }
}