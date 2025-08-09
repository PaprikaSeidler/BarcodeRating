using BarcodeRatingLib.Models;

namespace restApi.Records
{
    public record RatingRecord(int Id, long ProductBarcode, int Score, string? Comment, DateOnly RatingDate, string User);

    public static class RatingRecordHelper
    {
        public static Rating ConvertRatingRecord(RatingRecord record)
        {
            if (record.Score < 1 || record.Score > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(record.Score), "Rating must be between 1 and 5.");
            }
                
            if (string.IsNullOrWhiteSpace(record.User))
            { 
                throw new ArgumentNullException(nameof(record.User), "User cannot be null or empty.");
            }   

            if (record.User.Length < 3 || record.User.Length > 15)
            { 
                throw new ArgumentException("User must be between 3 and 15 characters long.", nameof(record.User));
            }

            return new Rating()
            {
                Id = record.Id,
                ProductBarcode = record.ProductBarcode,
                Score = record.Score,
                Comment = record.Comment,
                RatingDate = record.RatingDate,
                User = record.User
            };
        }
    }
}
