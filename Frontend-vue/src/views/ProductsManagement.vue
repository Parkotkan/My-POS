<template>
  <div class="container">
    <div class="management-header">
      <div class="title-section">
        <h2>จัดการสินค้า</h2>
        <p class="text-muted">จัดการข้อมูลสินค้า ราคา และหมวดหมู่</p>
      </div>
      <div class="action-section">
        <div class="search-box">
          <input type="text" v-model="searchQuery" placeholder="ค้นหาสินค้า...">
        </div>
        <button @click="showAddForm = true" class="btn btn-primary">
          <span class="plus-icon">+</span> เพิ่มสินค้า
        </button>
      </div>
    </div>

    <div class="table-container">
      <table class="table">
        <thead>
          <tr>
            <th>รหัสสินค้า</th>
            <th>ชื่อสินค้า</th>
            <th>หมวดหมู่</th>
            <th>ราคา</th>
            <th class="text-center">การจัดการ</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in filteredProducts" :key="product.productId">
            <td class="text-muted">{{ product.productId }}</td>
            <td>
              <div class="product-cell">
                <span class="product-name">{{ product.productName }}</span>
                <span class="product-desc">{{ product.description || 'ไม่มีคำอธิบาย' }}</span>
              </div>
            </td>
            <td>
              <span class="category-tag">{{ product.category }}</span>
            </td>
            <td class="font-bold">{{ (product.price || 0).toFixed(2) }}</td>
            <td>
              <div class="action-buttons">
                <button @click="editProduct(product)" class="btn-icon edit" title="แก้ไข">✏️</button>
                <button @click="deleteProduct(product.productId)" class="btn-icon delete" title="ลบ">🗑️</button>
              </div>
            </td>
          </tr>
          <tr v-if="filteredProducts.length === 0">
            <td colspan="5" class="empty-state">
              ไม่พบข้อมูลสินค้าที่ค้นหา
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductsManagement',
  data() {
    return {
      products: [],
      searchQuery: '',
      showAddForm: false
    };
  },
  computed: {
    filteredProducts() {
      const query = this.searchQuery.toLowerCase();
      return this.products.filter(p => 
        (p.productName || '').toLowerCase().includes(query) || 
        (p.category || '').toLowerCase().includes(query) ||
        (p.description && p.description.toLowerCase().includes(query))
      );
    }
  },
  mounted() {
    this.loadProducts();
  },
  methods: {
    async loadProducts() {
      try {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/products`);
        if (response.ok) {
          this.products = await response.json();
        }
      } catch (error) {
        console.error('Failed to load products:', error);
      }
    },
    editProduct(product) {
      console.log('Edit product:', product);
    },
    async deleteProduct(id) {
      if (confirm('คุณแน่ใจหรือไม่ว่าต้องการลบสินค้านี้?')) {
        try {
          const response = await fetch(`${import.meta.env.VITE_API_URL}/api/products/${id}`, {
            method: 'DELETE'
          });
          if (response.ok) {
            this.loadProducts();
          }
        } catch (error) {
          console.error('Failed to delete product:', error);
        }
      }
    }
  }
};
</script>

<style scoped>
.management-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 2rem;
}

.action-section {
  display: flex;
  gap: 5rem;
  align-items: center;
}

.search-box input {
  min-width: 250px;
  padding: 0.625rem 1rem;
}

.plus-icon {
  margin-right: 8px;
  font-size: 1.2rem;
}

.product-cell {
  display: flex;
  flex-direction: column;
}

.product-name {
  font-weight: 600;
  color: #1e293b;
}

.product-desc {
  font-size: 0.75rem;
  color: #94a3b8;
}

.category-tag {
  background-color: #f1f5f9;
  color: #475569;
  padding: 4px 10px;
  border-radius: 6px;
  font-size: 0.75rem;
  font-weight: 600;
}

.font-bold {
  font-weight: 700;
  color: #3b82f6;
}

.action-buttons {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
}

.btn-icon {
  background: none;
  border: 1px solid #e2e8f0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-icon:hover {
  background-color: #f8fafc;
  transform: scale(1.1);
}

.btn-icon.delete:hover {
  border-color: #ef4444;
  color: #ef4444;
}

.btn-icon.edit:hover {
  border-color: #3b82f6;
  color: #3b82f6;
}

.empty-state {
  text-align: center;
  padding: 3rem !important;
  color: #94a3b8;
}

.text-center {
  text-align: center !important;
}

@media (max-width: 768px) {
  .management-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
  
  .action-section {
    width: 100%;
    flex-direction: column;
  }
  
  .search-box, .search-box input {
    width: 100%;
  }
}
</style>