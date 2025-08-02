using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductLib.Models
{
    public class Rating
    {
        public int _score;
        private DateTime _date;
        private string? _user;

        public int Id { get; set; }
        public int ProductBarcode { get; set; }
        public int Score
        {
            get => _score;
            set
            {
                if (value < 1 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("Rating must be between 1 and 5.");
                }
                _score = value;
            }
        }
        public string? Comment { get; set; }
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentOutOfRangeException("Date cannot be in the future.");
                }
                _date = value;
            }
        }
        public string User
        {
            get => _user;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("User cannot be null or empty.");
                }
                if (value.Length < 3 || value.Length > 15)
                {
                    throw new ArgumentException("User must be between 3 and 15 characters long.");
                }
                _user = value;
            }
        }

        public Rating()
        {
        }

        public Rating(Rating copy)
        {
            Id = copy.Id;
            ProductBarcode = copy.ProductBarcode;
            Score = copy.Score;
            Comment = copy.Comment;
            Date = copy.Date;
            User = copy.User;
        }

        public override string ToString()
        {
            return $"Score: {Score}, Comment: {Comment}, Date: {Date.ToShortDateString()}, User: {User}";
        }
    }
}
