<template>
  <div class="main-content">
    <MLoading v-if="isErrorServer" />
    <router-view></router-view>

    <!-- TOAST -->
    <div class="toast-message">
      <MToast
        v-if="showToast"
        :type="toast.type"
        :message="toast.message"
        :action="toast.action"
        :closeFunction="closeToast"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "MMain",
  created() {
    this.$tinyEmitter.on("lessSidebar", this.handleChangeWidth);
    this.$tinyEmitter.on("serverError", this.toogleLoading);
    this.$tinyEmitter.on("openMainToast", this.openToastEmit)
  },
  beforeUnmount() {
    this.$tinyEmitter.off("lessSidebar");
    this.$tinyEmitter.off("serverError");
    this.$tinyEmitter.off("openMainToast");
  },
  computed: {
    /**
     * Tính toán class cho main content
     * Author: PMChien
     */
    computedClasses() {
      return {
        "main-content": true,
        "main-content--large": this.isLarge,
      }
    }
  },
  data() {
    return {
      isLarge: false,
      isErrorServer: false,
      showToast: false,
      toast: {
        type: "success",
        message: "",
        action: null,
      },
    };
  },
  methods: {
    /**
     * Chỉnh lại isLarge theo dữ liệu nhận từ tinyEmitter
     * Author: PMChien
     */
    handleChangeWidth(isLess) {
      try {
        this.isLarge = isLess;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Hàm ẩn hiện loading
     * @param {Boolen} isShow ẩn hiện Loading
     * Author: PMChien
     */
    toogleLoading(isShow) {
      try {
        this.isErrorServer = isShow;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Mở toast khi được emit
     * @param {object} data chứa thông tin về toast: type - loại (error/ success), message: nội dung toast
     * Author: PMChien
     */
     openToastEmit(data) {
      try {
        this.toast.type = data.type;
        this.toast.message = data.message;
        this.showToast = true;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Đóng toast
     * Author: PMChien
     */
    closeToast() {
      try {
        this.showToast = false;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
  }
}
</script>

<style scoped>

</style>