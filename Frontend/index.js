const productsUrl = 'http://localhost:5199/api/products';
const ratingUrl = 'http://localhost:5199/api/ratings';
const openFoodUrl = 'https://world.openfoodfacts.org/api/v0/product/';

Vue.createApp({
  data() {
    return {
      barcode: '',
      productInfo: null,
      error: null,
      rating: 0,
      products: [],
      ratings: [],
      searchQuery: '',
      selectedCategory: '',
      newProduct: { barcode: '', name: '', category: '', brand: '', imageUrl: '' },
      newRating: { productBarcode: '', score: 0, comment: '', ratingDate: '', user: '' },
    };
  },

  created() {
    this.getAllProducts();
    this.getAllRatings();
  },

  methods: {
    async getAllProducts() {
      try {
        this.error = null;
        const response = await axios.get(productsUrl);
        console.log('All products:', response.data);
        this.products = response.data;
      } catch (error) {
        console.error('Error fetching all products:', error);
        this.products = [];
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

        const response = await axios.post(productsUrl, {
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

        const response = await axios.post(productsUrl, this.newProduct);
        console.log('New product added:', response.data);
        alert('Product added successfully!');
        this.newProduct = { barcode: '', name: '', category: '', brand: '', imageUrl: '' };

        $('#newProductModal').modal('hide');
        this.getAllProducts();
      } catch (error) {
        console.error('Error adding new product:', error);
        this.error = 'Failed to add new product';
      }
    },

    addProductFromOpenFood() {
      if (!this.productInfo || !this.productInfo.product) {
        this.error = 'No product information available to add';
        return;
      }

      const product = this.productInfo.product;
      this.newProduct = {
        barcode: this.barcode.trim(),
        name: product.product_name,
        category: '',
        brand: product.brands || '',
        imageUrl: product.image_url || ''
      };

      $('#newProductModal').modal('show');
      this.error = null;
    },

    setRating(stars) {
      this.rating = stars;
    },

    fetchAllProducts() {
      $('#newProductModal').modal('show');
    },

    async deleteProduct(barcode) {
      if (!confirm('Are you sure you want to delete this product?')) {
        return;
      }

      try {
        this.error = null;
        await axios.delete(`${productsUrl}/${barcode}`);
        alert('Product deleted successfully!');
        this.getAllProducts();
      } catch (error) {
        console.error('Error deleting product:', error);
        this.error = 'Failed to delete product';
      }
    },

    prepareRating(barcode) {
      this.newRating.productBarcode = barcode;
      this.newRating.ratingDate = new Date().toISOString().split('T')[0];
      this.newRating.score = 0;
      this.newRating.comment = '';
      this.newRating.user = '';
      this.error = null;

      $('#ratingModal').modal('show');
    },

    async submitRating() {
      try {
        this.error = null;

        const ratingToSend = {
          productBarcode: parseInt(this.newRating.productBarcode),
          score: this.newRating.score,
          comment: this.newRating.comment,
          ratingDate: this.newRating.ratingDate,
          user: this.newRating.user
        };
        console.log('Submitting rating:', ratingToSend);

        const response = await axios.post(ratingUrl, ratingToSend);
        console.log('Rating submitted:', response.data);
        alert('Rating submitted successfully!');

        this.newRating = { productBarcode: '', score: 0, comment: '', ratingDate: '', user: '' };

        $('#ratingModal').modal('hide');
        this.getAllRatings(); // Opdater ratings
      } catch (error) {
        console.error('Error submitting rating:', error);
        this.error = 'Failed to submit rating';
      }
    },

    async getAllRatings() {
      try {
        this.error = null;
        const response = await axios.get(ratingUrl);
        console.log('All ratings:', response.data);
        this.ratings = response.data;
      } catch (error) {
        console.error('Error fetching all ratings:', error);
        this.ratings = [];
      }
    },


  },
  computed: {
    filteredProducts() {
      return this.products.filter(product => {
        const matchesCategory = !this.selectedCategory || product.category === this.selectedCategory;
        const matchesSearch = !this.searchQuery || product.name.toLowerCase().includes(this.searchQuery.toLowerCase());
        return matchesCategory && matchesSearch;
      });
    },
    sortByRating() {
      return this.filteredProducts.slice().sort((a, b) => {
        return (b.rating || 0) - (a.rating || 0);
      });
    },

    getAverageRating() {
      return (barcode) => {
        const productRatings = this.ratings.filter(rating => 
          rating.productBarcode == barcode
        );
        
        if (productRatings.length === 0) return 0;
        
        const total = productRatings.reduce((sum, rating) => sum + rating.score, 0);
        return Math.round((total / productRatings.length) * 10) / 10;
      };
    }
  },
}).mount('#app');