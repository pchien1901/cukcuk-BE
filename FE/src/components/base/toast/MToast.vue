<template>
  <div class="toast" :class="`toast--${type}`">
    <div class="toast-left">
      <div class="toast-icon">
        <i v-if="type === 'error'" class="fas fa-times-circle"></i>
      </div>
      <div class="toast-title">{{ toastInfo[type] }}</div>
      <div class="toast-msg">&nbsp;{{ message }}</div>
    </div>
    <div class="toast-right">
      <div class="toast-action">{{ action }}</div>
      <div class="toast-close-icon" @click="closeToast"></div>
    </div>
  </div>
</template>

<script>
export default {
  name: "MToast",
  props: {
    // Loại của thông báo
    type: {
      type: String,
      default: "success",
      validator: function (value) {
        return ["success", "warning", "info", "error"].includes(value);
      },
    },

    // Nội dung của thông báo
    message: {
      type: String,
      default: "",
    },

    // Hành động
    action: {
      type: String,
      default: "",
    },

    // Thời gian tồn tại
    duration: {
      type: Number,
      default: 5000,
    },

    // Hàm đóng thông báo
    closeFunction: Function,
  },
  data() {
    return {
      // showToast: true,
    };
  },
  computed: {
    toastInfo() {
      return {
        success: "Thành công!",
        error: "Lỗi!",
        warning: "Cảnh báo!",
        info: "Thông tin!",
      };
    },
  },
  mounted() {
    setTimeout(() => {
      this.closeToast();
    }, this.duration);
  },
  watch: {
    // showToast(newValue) {
    //   if (newValue) {
    //     setTimeout(() => {
    //       this.closeToast();
    //     }, this.duration);
    //   }
    // },
  },
  methods: {
    /**
     * Đóng Toast
     * Author: Phạm Minh Chiến
     */
    closeToast() {
      try {
        // this.showToast = false;
        this.closeFunction();
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
  },
};
</script>

<style scoped>
@import url('../../../css/base/toast.css');
</style>
