const barcodeRatingUrl = 'http://localhost:5199/api/products';
const openFoodUrl = 'https://world.openfoodfacts.org/api/v0/product/';

Vue.createApp({
  data() {
    return {
      barcode: '',
      productInfo: null,
      error: null,
      loading: false
    };
  },

    created() {
      this.getAllProducts();
    },

  methods: {
    async getAllProducts() {
      try {
        this.loading = true;
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
        this.loading = true;
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
      } finally {
        this.loading = false;
      }
    },
    async submitBarcode() {
      if (!this.barcode.trim()) {
        this.error = 'Please enter a barcode';
        return;
      }
      
      try {
        this.loading = true;
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
      } finally {
        this.loading = false;
      }
    }
  }
}).mount('#app');