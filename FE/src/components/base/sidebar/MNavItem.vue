<template>
  <div class="nav-item" @click="handleClick">
    <!-- <div class="icon-wrap">
      <div :class="computedClasses">
        <i :class="iconAwsClass"></i>
      </div>
    </div> -->
    <div :class="computedClasses"></div>
    <div :class="computedIconActive"></div>

    <div v-if="!this.isLess" class="nav-item__text" :class="{ 'fade-in-text': !this.isLess}">{{ inputText }}</div>
    <span v-if="isLess" :class="tooltipClasses">
      {{ inputText }}
    </span>
  </div>
</template>

<script>
export default {
  name: "MNavItem",
  /**
   * text: thông tin hiển thị trên sidebar và hiển thị trên tooltip khi bị thu gọn
   * iconClass: tên class của icon
   * iconActive: tên class của icon khi active
   * iconAwsClass: tên class của thẻ i trong font awesome (không dùng)
   * tooltipPosition: vị trí xuất hiện tooltip (mặc định bên phải)
   */
  props: {
    text: {
      type: String,
      default: "",
    },
    iconClass: String,
    iconActive: String,
    iconAwsClass: String,
    tooltipPosition: {
      type: String,
      default: "right",
    },
  },
  computed: {
    /**
     * Tính toán các giá trị cho icon
     * Author: PMChien
     */
    computedClasses() {
      return {
        "nav-item__icon": true,
        [this.iconClass]: this.iconClass,
      };
    },
    /**
     * Tính toán class cho icon khi active
     * Author: PMChien
     */
    computedIconActive() {
      return {
        "nav-item__icon--active": true,
        [this.iconActive]: this.iconActive,
      };
    },
    /**
     * Tính toán giá trị của text hiển thị
     * Author: PMChien
     */
    inputText() {
      return this.text || "";
    },
    /**
     * Tính toán class cho tooltip
     * Author: PMChien
     */
    tooltipClasses() {
      return {
        tooltip: true,
        "tooltip-right": this.tooltipPosition === "right",
        "tooltip-left": this.tooltipPosition === "left",
        "tooltip-bottom": this.tooltipPosition === "bottom",
        "tooltip-top": this.tooltipPosition === "top",
      };
    },
  },
  data() {
    return {
      isLess: false,
    };
  },
  created() {
    this.$tinyEmitter.on("lessSidebar", this.handleLessSidebar);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("lessSidebar");
  },
  emits: ["click"],
  methods: {
    /**
     * Gọi sự kiện click từ component cha
     * Author: PMChien
     */
    handleClick() {
      try {
        this.$emit("click");
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Gán lại giá trị isLess (để chuyển sidebar về dạng thu gọn
     * Author: PMChien)
     */
    handleLessSidebar(newValue) {
      try {
        // if(newValue === true) {
        //   this.isLess = newValue;
        //   this.isShowText = true;
        // }
        // else {
        //   this.isLess = newValue;
        //   setTimeout(() => { this.isShowText = true }, 10);
        // }
        this.isLess = newValue;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
      
    },
  },
};
</script>

<style scoped>
@import url("../../../css/base/nav-item.css");
</style>
