// Import styles
import '../css/styles.css';

// Import Vue and Vue Router
import { createApp } from 'vue';
import { createRouter, createWebHashHistory } from 'vue-router';

// Import root app and views
import App from '../App.vue';
import PosTerminal from '../views/PosTerminal.vue';
import ProductsManagement from '../views/ProductsManagement.vue';
import Orders from '../views/Orders.vue';

// Router Configuration
const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    { path: '/', redirect: '/pos' },
    { path: '/pos', component: PosTerminal },
    { path: '/products', component: ProductsManagement },
    { path: '/orders', component: Orders }
  ]
});

// Create and mount the app
const app = createApp(App);
app.use(router);
app.mount('#app');
