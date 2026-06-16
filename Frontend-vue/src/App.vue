<template>
  <div class="app-wrapper">
    <header class="header">
      <div class="brand">
        <h1>MyPOS</h1>
      </div>
      <nav class="nav-menu">
        <router-link to="/pos" class="nav-link">สั่งซื้อสินค้า</router-link>
        <router-link to="/products" class="nav-link">จัดการสินค้า</router-link>
        <router-link to="/orders" class="nav-link">ประวัติการขาย</router-link>
      </nav>
      <div class="user-info">
        <!-- Optional: Add user profile / logout here -->
      </div>
    </header>

    <main>
      <router-view />
    </main>
  </div>
</template>

<script>
export default {
  name: 'App'
};
</script>

<style>
/* Global styles can also be here or in styles.css */
</style>
