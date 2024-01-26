<template>
  <button v-if="type === 'primary' || type === 'second'" :class="computedClasses" :id="id" @click="handleClick" :disabled="disabled">
    {{ buttonText }}
  </button>

  <button
    v-if="type === 'icon'"
    :class="computedClasses"
    :id="id"
    @click="handleClick"
    :title="title"
    :disabled="disabled"
  >
    <div :class="iconClass"></div>
    <span v-if="isShowTooltip" :class="tooltipClasses">
      {{ tooltip }}
    </span>
  </button>

  <button v-if="type === 'primary-icon'" :class="computedClasses" :id="id" @click="handleClick" :disabled="disabled">
    <div v-if="isShowIcon" :class="computedIconClass"></div>
    <i :class="iconAwsClass"></i>
    {{ text }}
  </button>

  <button v-if="type === 'link'" :class="computedClasses" :id="id" :disabled="disabled" @click="handleClick">{{ text }}</button>
</template>

<script>
export default {
  name: "MButton",
  /**
   * type: loại của button
   * size: size của button (small -32px, medium -36px, large - 40px)
   * disabled: button có hoạt động hay không
   * id: id của button
   * class: class của button
   * iconClass: class của icon (nếu là icon button hoặc primary icon button)
   * text: Chữ hiển thị trong button
   * title: chú thích nếu cần
   * tooltip: nội dung của tooltip nếu là icon button
   * tooltipPosition: vị trí hiển thị tooltip so với button
   */
  props: {
    type: {
      type: String,
      default: "primary",
      validator: function (value) {
        return ["primary", "second", "link", "icon", "primary-icon"].includes(value);
      },
    },
    size: {
      type: String,
      default: "medium",
      validator: function (value) {
        return ["small", "medium", "large"].includes(value);
      },
    },
    disabled: { type: Boolean, default: false, },
    id: String,
    text: { type: String, default: "",},
    class: String,
    title: String,
    iconClass: String,
    tooltip: { type: String, default: "", },
    tooltipPosition: {
      type: String,
      default: "right",
      validator: function(value) {
        return ['top', 'right', 'bottom', 'left'].includes(value);
      }
    },
  },
  emits: ["click"],
  computed: {
    /**
     * Tính toán class cho button
     * Author": PMChien
     */
    computedClasses() {
      return {
        "m-btn": true,
        [`m-btn-${this.type}`]: true,
        [`m-btn-${this.size}`]: true,
        "m-btn--disabled": this.disabled,
        [this.class]: this.class,
      };
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
    /**
     * Tính toán class cho icon
     * Author: PMChien
     */
    computedIconClass() {
      return {
        "icon": true,
        [this.iconClass]: this.iconClass
      }
    },
    /**
     * Cho biết có hiển thị icon không dựa trên giá trị của iconClass tồn tại
     * Author: PMChien
     */
    isShowIcon() {
      if(this.iconClass) {
        return true;
      }
      else {
        return false;
      }
    },
    /**
     * Tính toán giá trị hiển trị trên button
     * Author: PMChien
     */
    buttonText() {
      return this.text || "";
    },
  },
  methods: {
    /**
     * Gọi sự kiện click được truyền vào component
     * Author: Phạm Minh Chiến
     */
     handleClick() {
      try {
        if (!this.disabled) {
          this.$emit("click");
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
  }
}
</script>

<style scoped>
@import url("../../../css/base/button.css");
</style>