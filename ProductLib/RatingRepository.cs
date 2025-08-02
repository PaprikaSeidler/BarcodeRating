using ProductLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductLib
{
    public class RatingRepository
    {
        private readonly DatabaseContext _context;

        public RatingRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<Rating> GetAll()
        {
            return _context.Ratings
                .OrderByDescending(r => r.Date)
                .ToList();
        }

        public Rating? GetById(int id)
        {
            return _context.Ratings.FirstOrDefault(r => r.Id == id);
        }

        public List<Rating> GetByProductBarcode(int barcode)
        {
            return _context.Ratings
                .Where(r => r.ProductBarcode == barcode)
                .OrderByDescending(r => r.Date)
                .ToList();
        }

        public Rating Add(Rating rating)
        {
            _context.Ratings.Add(rating);
            _context.SaveChanges();
            return rating;
        }

        public Rating? Delete(int id)
        {
            var rating = GetById(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                _context.SaveChanges();
            }
            return rating;
        }

        public Rating? Update(Rating updated)
        {
            var existing = GetById(updated.Id);
            if (existing != null)
            {
                existing.Score = updated.Score;
                existing.Comment = updated.Comment;
                existing.Date = updated.Date;
                existing.User = updated.User;
                existing.ProductBarcode = updated.ProductBarcode;

                _context.SaveChanges();
                return existing;
            }
            return null;
        }
    }

    
}
