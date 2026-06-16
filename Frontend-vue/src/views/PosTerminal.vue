<template>
  <div class="pos-container">
    <!-- Left Sidebar: Categories -->
    <aside class="category-sidebar">
      <div class="sidebar-header">
        <h3>หมวดหมู่สินค้า</h3>
      </div>
      <nav class="category-list">
        <button 
          class="category-item" 
          :class="{ active: selectedCategory === 'All' }"
          @click="selectCategory('All')"
        >
          ทั้งหมด
        </button>
        <button 
          v-for="cat in categories" 
          :key="cat"
          class="category-item" 
          :class="{ active: selectedCategory === cat }"
          @click="selectCategory(cat)"
        >
          {{ cat }}
        </button>
      </nav>
    </aside>

    <!-- Center: Product Selection -->
    <main class="product-area">
      <div class="product-header">
        <div class="search-wrapper">
          <span class="search-icon">🔍</span>
          <input
            type="text"
            v-model="searchQuery"
            placeholder="ค้นหาสินค้า..."
            @input="filterProducts"
          >
        </div>
      </div>
      
      <div class="product-grid">
        <div v-for="product in filteredProducts" 
             :key="product.productId" 
             class="product-card" 
             @click="addToCart(product)">
          <div class="product-info">
            <h3>{{ product.productName }}</h3>
            <p class="category-label">{{ product.category }}</p>
          </div>
          <div class="product-footer">
            <span class="price">{{ (product.price || 0).toFixed(2) }}</span>
            <span class="add-btn">+</span>
          </div>
        </div>
      </div>
    </main>

    <!-- Right Sidebar: Cart -->
    <aside class="cart-sidebar">
      <div class="cart-header">
        <h2>ตะกร้า</h2>
        <span class="item-count">{{ cart.length }} รายการ</span>
      </div>

      <div class="cart-items">
        <div v-if="cart.length === 0" class="empty-cart">
          <p>ไม่มีสินค้าในตะกร้า</p>
        </div>
        <div v-for="item in cart" :key="item.id" class="cart-item">
          <div class="item-details">
            <h4>{{ item.name }}</h4>
            <p class="unit-price">{{ item.price.toFixed(2) }}</p>
          </div>
          <div class="item-actions">
            <div class="quantity-controls">
              <button @click="updateQuantity(item.id, item.quantity - 1)" class="q-btn">-</button>
              <span class="q-val">{{ item.quantity }}</span>
              <button @click="updateQuantity(item.id, item.quantity + 1)" class="q-btn">+</button>
            </div>
            <button @click="removeFromCart(item.id)" class="remove-btn">🗑️</button>
          </div>
        </div>
      </div>

      <div class="cart-footer">
        <div class="summary-details">
          <div class="summary-line">
            <span>ราคารวม</span>
            <span>{{ total.toFixed(2) }}</span>
          </div>
          <div class="summary-line total">
            <span>จำนวนเงินทั้งหมด</span>
            <span>{{ total.toFixed(2) }}</span>
          </div>
        </div>

        <div class="checkout-controls">
          <div class="cashier-box">
            <label>แคชเชียร์</label>
            <select v-model="selectedUserId">
              <option v-for="user in users" :key="user.userId" :value="user.userId">
                {{ user.firstName }} {{ user.lastName }}
              </option>
            </select>
          </div>
          <button @click="checkout" class="checkout-button" :disabled="cart.length === 0">
            ชำระเงินทันที
          </button>
        </div>
      </div>
    </aside>

    <!-- Modern Modal -->
    <BaseModal 
      :show="modal.show"
      :title="modal.title"
      :message="modal.message"
      :type="modal.type"
      @close="modal.show = false"
    />
  </div>
</template>

<script>
import BaseModal from '../components/BaseModal.vue';

export default {
  name: 'PosTerminal',
  components: {
    BaseModal
  },
  data() {
    return {
      products: [],
      cart: [],
      searchQuery: '',
      filteredProducts: [],
      users: [],
      selectedUserId: 1,
      categories: [],
      selectedCategory: 'All',
      modal: {
        show: false,
        title: '',
        message: '',
        type: 'info'
      }
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
    showAlert(title, message, type = 'info') {
      this.modal.title = title;
      this.modal.message = message;
      this.modal.type = type;
      this.modal.show = true;
    },
    async loadUsers() {
      try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/users/active`);
        if (response.ok) {
          this.users = await response.json();
          if (this.users.length > 0) {
            this.selectedUserId = this.users[0].userId;
          }
        }
      } catch (error) {
        console.error('Failed to load users:', error);
      }
    },
    async loadProducts() {
      try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/products`);
        if (response.ok) {
          this.products = await response.json();
          this.extractCategories();
          this.filterProducts();
        }
      } catch (error) {
        console.error('Failed to load products:', error);
      }
    },
    extractCategories() {
      const cats = new Set();
      this.products.forEach(p => {
        if (p.category) cats.add(p.category);
      });
      this.categories = Array.from(cats).sort();
    },
    selectCategory(cat) {
      this.selectedCategory = cat;
      this.filterProducts();
    },
    filterProducts() {
      let result = this.products;
      
      // Filter by Category
      if (this.selectedCategory !== 'All') {
        result = result.filter(p => p.category === this.selectedCategory);
      }
      
      // Filter by Search
      const query = this.searchQuery.toLowerCase();
      if (query) {
        result = result.filter(product =>
          (product.productName || '').toLowerCase().includes(query) ||
          (product.category || '').toLowerCase().includes(query)
        );
      }
      
      this.filteredProducts = result;
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
        this.showAlert('แจ้งเตือน', 'กรุณาเลือกสินค้าลงตะกร้าก่อนชำระเงิน', 'warning');
        return;
      }

      const order = {
        userId: this.selectedUserId,
        order_items: this.cart.map(item => ({
          productId: item.id,
          quantity: item.quantity,
          unitPrice: item.price
        })),
        totalAmount: this.total,
        orderDate: new Date().toISOString(),
        status: 'Completed'
      };

      try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/orders`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(order)
        });

        if (response.ok) {
          this.showAlert('สำเร็จ', 'ชำระเงินเสร็จสิ้นเรียบร้อยแล้ว!', 'success');
          this.cart = [];
        } else {
          const errorData = await response.json().catch(() => ({}));
          const errorMessage = errorData.message || JSON.stringify(errorData) || 'Unknown error';
          this.showAlert('ผิดพลาด', 'ไม่สามารถทำรายการได้: ' + errorMessage, 'error');
        }
      } catch (error) {
        console.error('Checkout error:', error);
        this.showAlert('ผิดพลาด', 'เกิดข้อผิดพลาดระหว่างชำระเงิน: ' + error.message, 'error');
      }
    }
  }
};
</script>

<style scoped>
.pos-container {
  display: flex;
  height: calc(100vh - 64px); /* Header is approx 64px */
  background-color: #f8fafc;
  overflow: hidden;
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
}

/* Sidebar: Categories */
.category-sidebar {
  width: 200px;
  background-color: #ffffff;
  border-right: 1px solid #e2e8f0;
  display: flex;
  flex-direction: column;
}

.sidebar-header {
  padding: 20px;
  border-bottom: 1px solid #e2e8f0;
}

.sidebar-header h3 {
  margin: 0;
  font-size: 1.1rem;
  color: #1e293b;
}

.category-list {
  padding: 10px;
  overflow-y: auto;
}

.category-item {
  width: 100%;
  text-align: left;
  padding: 12px 15px;
  margin-bottom: 5px;
  border: none;
  background: none;
  border-radius: 8px;
  cursor: pointer;
  color: #64748b;
  font-weight: 500;
  transition: all 0.2s;
}

.category-item:hover {
  background-color: #f1f5f9;
  color: #3b82f6;
}

.category-item.active {
  background-color: #3b82f6;
  color: white;
  box-shadow: 0 4px 6px -1px rgba(59, 130, 246, 0.5);
}

/* Center: Product Area */
.product-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  padding: 20px;
  overflow: hidden;
}

.product-header {
  margin-bottom: 20px;
}

.search-wrapper {
  position: relative;
  max-width: 500px;
}

.search-icon {
  position: absolute;
  left: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #94a3b8;
}

.search-wrapper input {
  width: 100%;
  padding: 12px 12px 12px 40px;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  font-size: 1rem;
  outline: none;
  transition: border-color 0.2s;
}

.search-wrapper input:focus {
  border-color: #3b82f6;
}

.product-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 20px;
  overflow-y: auto;
  padding-bottom: 20px;
}

.product-card {
  background-color: white;
  border-radius: 12px;
  padding: 15px;
  border: 1px solid #e2e8f0;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.product-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  border-color: #3b82f6;
}

.product-info h3 {
  margin: 0 0 5px 0;
  font-size: 1rem;
  color: #1e293b;
}

.category-label {
  font-size: 0.8rem;
  color: #94a3b8;
  margin: 0;
}

.product-footer {
  margin-top: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.price {
  font-weight: 700;
  color: #3b82f6;
  font-size: 1.1rem;
}

.add-btn {
  background-color: #f1f5f9;
  color: #3b82f6;
  width: 28px;
  height: 28px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  font-weight: bold;
}

/* Right Sidebar: Cart */
.cart-sidebar {
  width: 350px;
  background-color: #ffffff;
  border-left: 1px solid #e2e8f0;
  display: flex;
  flex-direction: column;
}

.cart-header {
  padding: 20px;
  border-bottom: 1px solid #e2e8f0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.cart-header h2 {
  margin: 0;
  font-size: 1.2rem;
  color: #1e293b;
}

.item-count {
  background-color: #f1f5f9;
  color: #64748b;
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 0.8rem;
  font-weight: 600;
}

.cart-items {
  flex: 1;
  overflow-y: auto;
  padding: 20px;
}

.empty-cart {
  text-align: center;
  color: #94a3b8;
  margin-top: 50px;
}

.cart-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 15px;
  padding-bottom: 15px;
  border-bottom: 1px dotted #e2e8f0;
}

.item-details h4 {
  margin: 0 0 5px 0;
  font-size: 0.95rem;
  color: #1e293b;
}

.unit-price {
  margin: 0;
  font-size: 0.85rem;
  color: #64748b;
  font-weight: 600;
}

.item-actions {
  display: flex;
  align-items: center;
  gap: 12px;
}

.quantity-controls {
  display: flex;
  align-items: center;
  background-color: #f1f5f9;
  border-radius: 6px;
  padding: 2px;
}

.q-btn {
  border: none;
  background: white;
  width: 24px;
  height: 24px;
  border-radius: 4px;
  cursor: pointer;
  font-weight: bold;
  color: #64748b;
  box-shadow: 0 1px 2px rgba(0,0,0,0.05);
}

.q-val {
  padding: 0 10px;
  font-size: 0.9rem;
  font-weight: 600;
  color: #1e293b;
}

.remove-btn {
  border: none;
  background: none;
  cursor: pointer;
  font-size: 1.1rem;
  opacity: 0.6;
}

.remove-btn:hover {
  opacity: 1;
}

.cart-footer {
  padding: 20px;
  background-color: #f8fafc;
  border-top: 1px solid #e2e8f0;
}

.summary-details {
  margin-bottom: 20px;
}

.summary-line {
  display: flex;
  justify-content: space-between;
  margin-bottom: 8px;
  color: #64748b;
  font-size: 0.9rem;
}

.summary-line.total {
  margin-top: 12px;
  padding-top: 12px;
  border-top: 1px solid #e2e8f0;
  color: #1e293b;
  font-weight: 800;
  font-size: 1.1rem;
}

.cashier-box {
  margin-bottom: 15px;
}

.cashier-box label {
  display: block;
  font-size: 0.75rem;
  text-transform: uppercase;
  color: #94a3b8;
  font-weight: 700;
  margin-bottom: 5px;
}

.cashier-box select {
  width: 100%;
  padding: 10px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  background-color: white;
}

.checkout-button {
  width: 100%;
  padding: 15px;
  background-color: #3b82f6;
  color: white;
  border: none;
  border-radius: 10px;
  font-weight: 700;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.2s;
  letter-spacing: 1px;
}

.checkout-button:hover {
  background-color: #2563eb;
}

.checkout-button:disabled {
  background-color: #cbd5e1;
  cursor: not-allowed;
}

@media (max-width: 1024px) {
  .pos-container {
    flex-direction: column;
    height: auto;
    overflow: visible;
  }
  
  .category-sidebar, .cart-sidebar {
    width: 100%;
    height: auto;
  }
}
</style>