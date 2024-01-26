<template>
  <div class="main-content">
    <MLoading v-if="isErrorServer" />
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: "MMain",
  created() {
    this.$tinyEmitter.on("lessSidebar", this.handleChangeWidth);
    this.$tinyEmitter.on("serverError", this.toogleLoading);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("lessSidebar");
    this.$tinyEmitter.off("serverError");
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
     * @param {boolen} isShow ẩn hiện Loading
     */
    toogleLoading(isShow) {
      try {
        this.isErrorServer = isShow;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  }
}
</script>

<style scoped>

</style>