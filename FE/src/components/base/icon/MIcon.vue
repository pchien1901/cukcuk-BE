<template>
  <div :class="computedIconClasses" @click="handleClick">
    <i :class="iconAwsClass">
    
    </i>
    <span v-if="isShowTooltip" :class="tooltipClasses">
      {{ tooltip }}
    </span>
  </div>
    
</template>

<script>
export default {
  name: "MIcon",
  emits: ["click"],
  props: {
    tooltip: { type: String, default: ""},
    iconClass: { type: String, default: ""},
    iconAwsClass: String,
    tooltipPosition: {
      type: String,
      default: "right",
      validator: function(value) {
        return ['top', 'right', 'bottom', 'left'].includes(value);
      }
    },
  },
  computed: {
    computedIconClasses() {
      return {
        'm-icon': true,
        [this.iconClass]: this.iconClass,
      }
    },
    /**
     * Tính toán class cho tooltip
     * Author: PMChien
     */
     tooltipClasses() {
      return {
        'tooltip': true,
        "tooltip-right": this.tooltipPosition === "right",
        "tooltip-left": this.tooltipPosition === "left",
        "tooltip-bottom": this.tooltipPosition === "bottom",
        "tooltip-top": this.tooltipPosition === "top",
      };
    },

    isShowTooltip() {
      if (
        this.tooltip !== "" &&
        this.tooltip !== null &&
        this.tooltip !== undefined
      ) {
        return true;
      } else {
        return false;
      }
    },
  },
  methods: {
    /**
     * Gọi emit tới hàm cha
     * Author: PMChien
     */
    handleClick() {
      try {
        this.$emit("click");
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    }
  }
}
</script>

<style>
@import url('../../../css/base/tooltip.css');
@import url('../../../css/icon/icon.css');

.m-icon:hover .tooltip {
  visibility: visible;
}

.m-icon {
  position: relative;
}
</style>