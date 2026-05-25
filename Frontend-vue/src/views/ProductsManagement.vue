<template>
  <div class="products-management">
    <h2>Products Management</h2>
    <div class="action-buttons">
      <button @click="showAddForm = true" class="btn btn-primary">Add Product</button>
    </div>

    <table class="table">
      <thead>
        <tr>
          <th>Product Code</th>
          <th>Name</th>
          <th>Description</th>
          <th>Price</th>
          <th>Category</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in products" :key="product.productId">
          <td>{{ product.productId}}</td>
          <td>{{ product.productName}}</td>
          <td>{{ product.description || 'N/A' }}</td>
          <td>${{ (product.price || 0).toFixed(2) }}</td>
          <td>{{ product.category}}</td>
          <td style="text-align: center;">
            <button @click="editProduct(product)" class="btn btn-warning btn-small">Edit</button>
            <button @click="deleteProduct(product.productId)" class="btn btn-danger btn-small">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: 'ProductsManagement',
  data() {
    return {
      products: [],
      showAddForm: false
    };
  },
  mounted() {
    this.loadProducts();
  },
  methods: {
    async loadProducts() {
      try {
        const response = await fetch('http://localhost:5000/api/products');
        if (response.ok) {
          this.products = await response.json();
        }
      } catch (error) {
        console.error('Failed to load products:', error);
      }
    },
    editProduct(product) {
      // TODO: Implement edit functionality
      console.log('Edit product:', product);
    },
    async deleteProduct(id) {
      if (confirm('Are you sure you want to delete this product?')) {
        try {
          const response = await fetch(`http://localhost:5000/api/products/${id}`, {
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
.products-management {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.action-buttons {
  margin-bottom: 20px;
}

.table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.table th {
  padding: 12px 15px;
  text-align: center;
  border-bottom: 1px solid #eee;
  background-color: #f8f9fa;
  font-weight: 600;
  color: #495057;
}

.table td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.table tbody tr:hover {
  background-color: #f8f9fa;
}

.btn-small {
  padding: 6px 12px;
  font-size: 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  margin-right: 5px;
}

.btn-small:hover {
  opacity: 0.8;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-warning {
  background-color: #ffc107;
  color: #212529;
}

.btn-danger {
  background-color: #dc3545;
  color: white;
}

@media (max-width: 768px) {
  .table {
    font-size: 14px;
  }

  .table th,
  .table td {
    padding: 8px 10px;
  }
}
</style>