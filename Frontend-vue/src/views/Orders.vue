<template>
  <div class="orders">
    <h2>Orders History</h2>
    <table class="table">
      <thead>
        <tr>
          <th>Order ID</th>
          <th>Customer</th>
          <th>Date</th>
          <th>Total</th>
          <th>Status</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.id">
          <td>{{ order.id }}</td>
          <td>{{ order.customerName }}</td>
          <td>{{ new Date(order.orderDate).toLocaleDateString() }}</td>
          <td>${{ order.totalAmount.toFixed(2) }}</td>
          <td>{{ order.status }}</td>
          <td>
            <button @click="viewOrder(order)" class="btn btn-info btn-small">View</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: 'Orders',
  data() {
    return {
      orders: []
    };
  },
  mounted() {
    this.loadOrders();
  },
  methods: {
    async loadOrders() {
      try {
        const response = await fetch('http://localhost:5000/api/orders');
        if (response.ok) {
          this.orders = await response.json();
        }
      } catch (error) {
        console.error('Failed to load orders:', error);
      }
    },
    viewOrder(order) {
      // TODO: Implement order details view
      console.log('View order:', order);
    }
  }
};
</script>

<style scoped>
.orders {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.table th,
.table td {
  padding: 12px 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #495057;
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
}

.btn-info {
  background-color: #17a2b8;
  color: white;
}

.btn-small:hover {
  opacity: 0.8;
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