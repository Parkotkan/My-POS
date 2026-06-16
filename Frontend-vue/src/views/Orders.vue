<template>
  <div class="orders">
    <h2>ประวัติการสั่งซื้อ</h2>
    <table class="table">
      <thead>
        <tr>
          <th>รหัสสั่งซื้อ</th>
          <th>ผู้ขาย</th>
          <th>วันที่</th>
          <th>รวม</th>
          <th>สถานะ</th>
          <th>การจัดการ</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.orderId">
          <td>{{ order.orderId }}</td>
          <td>{{ order.user ? `${order.user.firstName} ${order.user.lastName}` : 'N/A' }}</td>
          <td>{{ new Date(order.orderDate).toLocaleString() }}</td>
          <td>{{ (order.totalAmount || 0).toFixed(2) }}</td>
          <td>
            <span :class="['status-badge', order.status.toLowerCase()]">
              {{ order.status }}
            </span>
          </td>
          <td>
            <button @click="viewOrder(order)" class="btn btn-info btn-small">ดูรายละเอียด</button>
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
        const response = await fetch(`${import.meta.env.VITE_API_URL}/api/orders`);
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

.status-badge {
  padding: 4px 10px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: capitalize;
}

.status-badge.completed {
  background-color: #dcfce7;
  color: #166534;
}

.status-badge.pending {
  background-color: #fef9c3;
  color: #854d0e;
}

.status-badge.cancelled {
  background-color: #fee2e2;
  color: #991b1b;
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