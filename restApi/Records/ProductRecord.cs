using BarcodeRatingLib.Models;

namespace restApi.Records
{
    public record ProductRecord(int? Barcode, string? Name, string? ImageUrl, string? Brand, string? Category);

    public static class ProductRecordHelper
    {
        public static Product ConvertProductRecord(ProductRecord record)
        {
            if (record.Barcode == null)
            {
                throw new ArgumentNullException("" + record.Barcode);
            }
            if (record.Category == null)
            {
                throw new ArgumentNullException("" + record.Category);
            }
            return new Product()
            {
                Barcode = (int)record.Barcode,
                Name = record.Name,
                ImageUrl = record.ImageUrl,
                Brand = record.Brand,
                Category = record.Category
            };
        }
    }
}