<template>
  <div :class="computedClasses" @click="handleLessSidebar">
    <i v-if="!isLess" class="fas fa-chevron-left"></i>
    <i v-if="isLess" class="fas fa-chevron-right"></i>
    <div v-if="!isLess" class="nav-action__text">{{ inputText }}</div>
    <span v-if="isLess" :class="tooltipClasses">
      Mở rộng
    </span>
  </div>
</template>

<script>
export default {
  name: 'MNavAction',
  props: {
    // Nội dung của nút Action
    text: String,

    //Nội dụng của tooltip
    tooltip: { type: String, default: "Mở rộng",
    },

    // Vị trí của tooltip
    tooltipPosition: {
      type: String,
      default: "right",
      validator: function(value) {
        return ["top", "right", "bottom", "left"].includes(value);
      },
    }
  },
  created() {
    this.isLess = false;
  },
  watch: {
    // isLess() {
    //   this.$tinyEmitter.emit("lessSideBar", this.isLess);
    // }
  },
  methods: {
    handleLessSidebar() {
      this.isLess = !this.isLess;
      this.$tinyEmitter.emit("lessSidebar", this.isLess);
      console.log("isLess tại navAction: ", this.isLess);
    }
  },
  data() {
    return {
      isLess: false,
    };
  },
  computed: {
    inputText() {
      return this.text || "Thu gọn"
    },
    computedClasses() {
      return {
        'nav-action': true,
        'nav-action--less': this.isLess,
      };
    },
    tooltipClasses() {
      return {
        "tooltip": true,
        "tooltip-right": this.tooltipPosition === "right",
        "tooltip-left": this.tooltipPosition === "left",
        "tooltip-bottom": this.tooltipPosition === "bottom",
        "tooltip-top": this.tooltipPosition === "top",
      };
    }
  }
}
</script>

<style scoped>
@import url('../../../css/base/nav-action.css');
</style>