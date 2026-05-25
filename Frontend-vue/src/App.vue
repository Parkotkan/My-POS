<template>
  <div class="container">
    <header class="header">
      <h1>POS System</h1>
      <nav class="nav-menu">
        <router-link to="/pos" class="nav-link">POS Terminal</router-link>
        <router-link to="/products" class="nav-link">Products</router-link>
        <router-link to="/orders" class="nav-link">Orders</router-link>
      </nav>
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
