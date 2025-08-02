namespace ProductLib.Models
{
    public class Product
    {
        private int _barcode;
        private string _category;

        public int Barcode
        {
            get => _barcode;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Barcode must be a positive integer.");
                }
                _barcode = value;
            }
        }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; set; }
        public string Category
        {
            get => _category;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Category cannot be null.");
                }
                if (value != "Proteinbar" && value != "Coffee capsule")
                {
                    throw new ArgumentException("Category must be either 'Proteinbar' or 'Coffee capsule'.");
                }
                _category = value;
            }
        }

        public Product() { }

        public Product(Product copy) 
        {
            _barcode = copy._barcode;
            Name = copy.Name;
            ImageUrl = copy.ImageUrl;
            Brand = copy.Brand;
            _category = copy._category;
        }

        public override string ToString()
        {
            return $"Product: {Name}, Barcode: {Barcode}, Brand: {Brand}, Category: {Category}, ImageUrl: {ImageUrl}";
        }
    }
}
