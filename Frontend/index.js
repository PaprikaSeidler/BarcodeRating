const barcodeRatingUrl = 'http://localhost:5199/api/products';
const openFoodUrl = 'https://world.openfoodfacts.org/api/v0/product/';

Vue.createApp({
  data() {
    return {
      barcode: '',
      productInfo: null,
      error: null,
      rating: 0,
      newProduct: {
        barcode: '',
        name: '',
        category: '',
        brand: '',
        imageUrl: ''
      }
    };
  },

    created() {
      this.getAllProducts();
    },

  methods: {
    async getAllProducts() {
      try {
        this.error = null;
        const response = await axios.get(barcodeRatingUrl);
        console.log('All products:', response.data);
      } catch (error) {
        console.error('Error fetching all products:', error);
      }
    },
    async fetchProductInfo() {
      if (!this.barcode.trim()) {
        this.error = 'Please enter a barcode';
        return;
      }
      
      try {
        this.error = null;
        this.productInfo = null;
        
        const response = await axios.get(`${openFoodUrl}${this.barcode.trim()}.json`);
        
        if (response.data.status === 1) {
          this.productInfo = response.data;
        } else {
          this.error = 'Product not found in OpenFoodFacts database';
        }
      } catch (error) {
        console.error('Error fetching product info:', error);
        this.error = 'Failed to fetch product information';
      }
    },
    async submitBarcode() {
      if (!this.barcode.trim()) {
        this.error = 'Please enter a barcode';
        return;
      }
      
      try {
        this.error = null;
        
        const response = await axios.post(barcodeRatingUrl, { 
          barcode: this.barcode.trim() 
        });
        
        console.log('Barcode rating response:', response.data);
        this.error = null; 
        alert('Barcode submitted successfully!');
      } catch (error) {
        console.error('Error submitting barcode:', error);
        if (error.response) {
          this.error = `Backend error: ${error.response.status} - ${error.response.data?.message || 'Unknown error'}`;
        } else if (error.request) {
          this.error = 'Cannot connect to backend server. Make sure it\'s running on port 5000.';
        } else {
          this.error = 'An unexpected error occurred';
        }
      }
    },

    async addNewProduct() {
      if (!this.newProduct.barcode || !this.newProduct.name) {
        this.error = 'Please fill in all required fields';
        return;
      }

      try {
        this.error = null;

        const response = await axios.post(barcodeRatingUrl, this.newProduct);
        console.log('New product added:', response.data);
        alert('Product added successfully!');
        this.newProduct = { barcode: '', name: '', category: '', brand: '', imageUrl: '' };
        
        // Close the modal after successful add
        $('#newProductModal').modal('hide');
      } catch (error) {
        console.error('Error adding new product:', error);
        this.error = 'Failed to add new product';
      }
    },

    // Fill form with OpenFoodFacts data - user still needs to select category and submit manually
    addProductFromOpenFood() {
      if (!this.productInfo || !this.productInfo.product) {
        this.error = 'No product information available to add';
        return;
      }

      const product = this.productInfo.product;
      
      // Pre-fill the form with OpenFoodFacts data
      this.newProduct = {
        barcode: this.barcode.trim(), // Use the barcode from input field
        name: product.product_name,
        category: '', // User must select this manually (Proteinbar or Coffee capsule)
        brand: product.brands || '',
        imageUrl: product.image_url || ''
      };

      // Open the manual product modal so user can select category and submit
      $('#newProductModal').modal('show');
      this.error = null;
    },

    setRating(stars) {
      this.rating = stars;
    },

    fetchAllProducts() {
      $('#newProductModal').modal('show');
    }
  }
}).mount('#app');