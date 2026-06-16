<template>
  <Transition name="fade">
    <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
      <Transition name="pop">
        <div v-if="show" class="modal-content" :class="type">
          <div class="modal-icon">
            <span v-if="type === 'success'">✅</span>
            <span v-else-if="type === 'error'">❌</span>
            <span v-else-if="type === 'warning'">⚠️</span>
            <span v-else>ℹ️</span>
          </div>
          <h3 class="modal-title">{{ title }}</h3>
          <p class="modal-message">{{ message }}</p>
          <div class="modal-actions">
            <button @click="$emit('close')" class="modal-btn">ตกลง</button>
          </div>
        </div>
      </Transition>
    </div>
  </Transition>
</template>

<script>
export default {
  name: 'BaseModal',
  props: {
    show: Boolean,
    title: String,
    message: String,
    type: {
      type: String,
      default: 'info' // success, error, warning, info
    }
  },
  emits: ['close']
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
  backdrop-filter: blur(4px);
}

.modal-content {
  background: white;
  padding: 30px;
  border-radius: 20px;
  width: 90%;
  max-width: 400px;
  text-align: center;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
}

.modal-icon {
  font-size: 3rem;
  margin-bottom: 15px;
}

.modal-title {
  margin: 0 0 10px 0;
  font-size: 1.5rem;
  color: #1e293b;
  font-weight: 700;
}

.modal-message {
  color: #64748b;
  margin-bottom: 25px;
  line-height: 1.5;
}

.modal-btn {
  background-color: #3b82f6;
  color: white;
  border: none;
  padding: 12px 30px;
  border-radius: 10px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  width: 100%;
}

.modal-btn:hover {
  background-color: #2563eb;
  transform: translateY(-1px);
}

/* Success variation */
.modal-content.success .modal-btn {
  background-color: #10b981;
}
.modal-content.success .modal-btn:hover {
  background-color: #059669;
}

/* Error variation */
.modal-content.error .modal-btn {
  background-color: #ef4444;
}
.modal-content.error .modal-btn:hover {
  background-color: #dc2626;
}

/* Transitions */
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

.pop-enter-active, .pop-leave-active {
  transition: transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1), opacity 0.3s ease;
}
.pop-enter-from, .pop-leave-to {
  opacity: 0;
  transform: scale(0.9);
}
</style>
