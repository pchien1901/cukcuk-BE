<template>
  <div :class="computedWrapperClasses">
    <label v-if="label !== null && label !== '' && label !== undefined" :for="id">
      {{ label }}
      <span v-if="isRequired" class="required-star">*</span>
    </label>
    <input
      v-if="type === 'text'"
      type="text"
      :class="computedClasses"
      :id="id"
      :name="name"
      :placeholder="this.placeholder"
      v-model="inputValue"
      :title="this.title"
    />
    
    <input 
      v-if="type === 'file'"
      type="file"
      :class="computedClasses"
      :id="id"
      :name="name"
      @change="handleFileChange"
      ref="fileInput"
    />
    <div v-if="errorMessage" class="m-input__error-message">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script>
export default {
  name: "MInput",
  /**
   * type: Kiểu của input
   * error: chuỗi lỗi hiển thị (nếu có)
   * name: name của input
   * id: id của input
   * size: cỡ của text input (small - 32px, medium - 36px, large - 40px)
   * class: class truyền vào để bao ngoài text input và error message
   * isRequired: đánh dấu có bắt buộc không
   * title: chú thích nếu trường có viết tắt
   * placeholder: placeholder
   * label: Tên label
   */
  props: {
    type: {
      type: String,
      default: "text",
    },
    error: String,
    id: String,
    name: String,
    size: {
      type: String,
      default: "medium",
      validator: function (value) {
        return ["small", "medium", "large"].includes(value);
      },
    },
    class: { type: String, default: "", },
    isRequired: { type: Boolean, default: false, },
    title: String,
    placeholder: String,
    label: { type: String, default: ""},
    modelValue: String,
  },
  emits: ["update:modelValue", "fileUploaded"],
  data() {
    return {
      inputValue: this.modelValue,
    };
  },
  computed: {
    computedClasses() {
      return {
        "m-input__input": true,
        [`m-input-${this.size}`]: true,
      };
    },
    computedWrapperClasses() {
      return {
        "m-input": true,
        "m-input--error": this.errorMessage,
        [this.class]: [this.class],
      };
    },
    errorMessage() {
      return this.error || null;
    },
  },
  methods: {
    /**
     * Cập nhật input vào component
     * Author: Phạm Minh Chiến
     */
    // handleInput($event) {
    //   try {
    //     this.inputValue = $event.target.value;
    //     this.$emit("update:value", $event.target.value);
    //   } catch (error) {
    //     console.error("Đã xảy ra lỗi: ", error);
    //   }
    // },
    /**
     * Hàm Emit tới component cha để gửi file
     * Author: PMChien (18/01/2024)
     */
    handleFileChange() {
      try {
        let fileInput = this.$refs.fileInput;
        if(fileInput.files.length > 0) {
          // lấy giá trị khi có sự kiện change
          let selectedFile = fileInput.files[0];
          this.$emit("fileUploaded", selectedFile);
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  },

  watch: {
    inputValue() {
      this.$emit("update:modelValue", this.inputValue);
    },
    modelValue(newValue) {
      this.inputValue = newValue;
    }
  },
};
</script>

<style scoped>
@import url('../../../css/base/input.css');
</style>
