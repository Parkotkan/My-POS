<template>
  <div class="pos-terminal">
    <div class="pos-layout">
      <!-- Product -->
      <div class="product-selection">
        <h2>Select Products</h2>
        <div class="search-bar">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="Search products..."
            @input="filterProducts"
          >
        </div>
        <div class="product-grid">
          <div v-for="product in filteredProducts" :key="product.productId" class="product-card" @click="addToCart(product)">
            <h3>{{ product.productName}}</h3>
            <p class="price">${{ (product.price || 0).toFixed(2) }}</p>
          </div>
        </div>
      </div>

      <!-- Cart -->
      <div class="cart-section">
        <h2>Shopping Cart</h2>
        <div class="cart-items">
          <div v-for="item in cart" :key="item.id" class="cart-item">
            <div class="item-info">
              <h4>{{ item.name }}</h4>
              <p>${{ item.price.toFixed(2) }} x {{ item.quantity }}</p>
            </div>
            <div class="item-controls">
              <button @click="updateQuantity(item.id, item.quantity - 1)" class="btn-small">-</button>
              <span>{{ item.quantity }}</span>
              <button @click="updateQuantity(item.id, item.quantity + 1)" class="btn-small">+</button>
              <button @click="removeFromCart(item.id)" class="btn-danger btn-small">×</button>
            </div>
          </div>
        </div>

        <div class="cart-summary">
          <div class="summary-row total">
            <span>Total:</span>
            <span>${{ total.toFixed(2) }}</span>
          </div>
        </div>

        <div class="checkout-section">
          <div class="user-section">
            <label for="user-select">Cashier:</label>
            <select id="user-select" v-model="selectedUserId" class="user-select">
              <option v-for="user in users" :key="user.id" :value="user.id">
                {{ user.firstName }} {{ user.lastName }}
              </option>
            </select>
          </div>
          <button @click="checkout" class="btn btn-primary checkout-btn">
            Complete Sale
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PosTerminal',
  data() {
    return {
      products: [],
      cart: [],
      searchQuery: '',
      filteredProducts: [],
      customerName: '',
      customerEmail: '',
      customerPhone: '',
      users: [],
      selectedUserId: 1 // Default to first user
    };
  },
  computed: {
    total() {
      return this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    }
  },
  mounted() {
    this.loadProducts();
    this.loadUsers();
  },
  methods: {
    async loadUsers() {
      try {
        const response = await fetch('http://localhost:5000/api/users/active');
        if (response.ok) {
          this.users = await response.json();
          if (this.users.length > 0) {
            this.selectedUserId = this.users[0].id;
          }
        }
      } catch (error) {
        console.error('Failed to load users:', error);
      }
    },
    async loadProducts() {
      try {
        const response = await fetch('http://localhost:5000/api/products');
        if (response.ok) {
          this.products = await response.json();
          this.filteredProducts = this.products;
        }
      } catch (error) {
        console.error('Failed to load products:', error);
      }
    },
    filterProducts() {
      const query = this.searchQuery.toLowerCase();
      this.filteredProducts = this.products.filter(product =>
        (product.productName || '').toLowerCase().includes(query) ||
        (product.category || '').toLowerCase().includes(query)
      );
    },
    addToCart(product) {
      const productId = product.productId;
      const existingItem = this.cart.find(item => item.id === productId);
      if (existingItem) {
        existingItem.quantity++;
      } else {
        this.cart.push({
          id: productId,
          name: product.productName,
          price: product.price,
          quantity: 1
        });
      }
    },
    updateQuantity(productId, newQuantity) {
      if (newQuantity <= 0) {
        this.removeFromCart(productId);
        return;
      }

      const item = this.cart.find(item => item.id === productId);
      if (item) {
        item.quantity = newQuantity;
      }
    },
    removeFromCart(productId) {
      this.cart = this.cart.filter(item => item.id !== productId);
    },
    async checkout() {
      if (this.cart.length === 0) {
        alert('Cart is empty!');
        return;
      }

      const order = {
        userId: this.selectedUserId,
        items: this.cart.map(item => ({
          productId: item.id,
          quantity: item.quantity,
          unitPrice: item.price
        })),
        totalAmount: this.total,
        orderDate: new Date().toISOString(),
        status: 'Completed'
      };

      try {
        const response = await fetch('http://localhost:5000/api/orders', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(order)
        });

        if (response.ok) {
          alert('Sale completed successfully!');
          this.cart = [];
          this.customerName = '';
        } else {
          alert('Failed to complete sale');
        }
      } catch (error) {
        console.error('Checkout error:', error);
        alert('Error during checkout');
      }
    }
  }
};
</script>

<style scoped>
.pos-terminal {
  padding: 20px;
}

.pos-layout {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.product-selection {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.search-bar {
  margin-bottom: 20px;
}

.search-bar input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 16px;
}

.product-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  gap: 15px;
}

.product-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 15px;
  cursor: pointer;
  transition: all 0.3s ease;
  background: white;
}

.product-card:hover {
  border-color: #007bff;
  box-shadow: 0 2px 8px rgba(0,123,255,0.2);
}

.product-card h3 {
  margin: 0 0 10px 0;
  font-size: 16px;
}

.product-card .price {
  font-size: 18px;
  font-weight: bold;
  color: #28a745;
  margin: 5px 0;
}

.product-card .stock {
  font-size: 14px;
  color: #6c757d;
  margin: 5px 0;
}

.cart-section {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  height: fit-content;
}

.cart-items {
  margin-bottom: 20px;
}

.cart-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid #eee;
}

.cart-item:last-child {
  border-bottom: none;
}

.item-info h4 {
  margin: 0 0 5px 0;
  font-size: 14px;
}

.item-info p {
  margin: 0;
  color: #6c757d;
  font-size: 12px;
}

.item-controls {
  display: flex;
  align-items: center;
  gap: 10px;
}

.btn-small {
  padding: 5px 10px;
  font-size: 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-small:hover {
  opacity: 0.8;
}

.cart-summary {
  border-top: 2px solid #eee;
  padding-top: 15px;
  margin-bottom: 20px;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
  font-size: 14px;
}

.summary-row.total {
  font-weight: bold;
  font-size: 16px;
  border-top: 1px solid #eee;
  padding-top: 10px;
  margin-top: 10px;
}

.checkout-section {
  border-top: 1px solid #eee;
  padding-top: 20px;
}

.user-section {
  margin-bottom: 15px;
}

.user-section label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.user-select {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.customer-info {
  margin-bottom: 20px;
}

.customer-input {
  width: 100%;
  padding: 8px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.checkout-btn {
  width: 100%;
  padding: 12px;
  background-color: #28a745;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 16px;
  cursor: pointer;
}

.checkout-btn:hover {
  background-color: #218838;
}

@media (max-width: 768px) {
  .pos-layout {
    grid-template-columns: 1fr;
  }

  .product-grid {
    grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  }
}
</style>