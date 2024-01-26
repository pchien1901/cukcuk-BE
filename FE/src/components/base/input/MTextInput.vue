<template>
  <div :class="computedWrapperClasses">
    <label :for="fieldId">
      {{ label }}
      <span v-if="isRequired" class="required-star">*</span>
    </label>
    <input
      type="text"
      :class="computedClasses"
      :id="fieldId"
      :name="inputName"
      :placeholder="inputPlaceholder"
      v-model="inputValue"
      :title="inputTitle"
    />
    <div v-if="errorMessage" class="m-text-input__error-message">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script>
export default {
  name: "MTextInput",
  props: {
    label: String,
    value: String,
    error: String,
    fieldId: String,
    inputName: String,
    size: {
      type: String,
      default: "medium",
      validator: function (value) {
        return ["small", "medium", "large"].includes(value);
      },
    },
    inputClass: {
      type: String,
      default: "",
    },
    isRequired: {
      type: Boolean,
      default: false,
    },
    inputTitle: String,
    inputPlaceholder: String,
  },
  emits: ["update:modelValue"],
  data() {
    return {
      inputValue: null,
    };
  },
  computed: {
    computedClasses() {
      return {
        "m-text-input__input": true,
        [`m-text-input-${this.size}`]: true,
      };
    },
    computedWrapperClasses() {
      return {
        "m-text-input": true,
        "m-text-input--error": this.errorMessage,
        [this.inputClass]: [this.inputClass],
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
  },

  watch: {
    inputValue() {
      this.$emit("update:modelValue", this.inputValue);
    },
    // value(newValue) {
    //   this.inputValue = newValue;
    // },
  },
};
</script>

<style scoped>
:root {
  --text-color: #1f1f1f;
  --border-color: #ebebeb;
  --error-color: #e61d1d;
}
.required-star {
  color: #e61d1d;
}
.m-text-input {
  font-family: "Roboto", sans-serif;
  display: flex;
  flex-direction: column;
}

.m-text-input-small {
  height: 32px;
}

.m-text-input-medium {
  height: 36px;
}

.m-text-input-large {
  height: 40px;
}

.m-text-input label {
  font-weight: 500;
  font-size: 14px;
  color: #1f1f1f;
  margin-bottom: 8px;
  width: 100%;
}

.m-text-input input {
  box-sizing: border-box;
  padding: 0px 12px;
  border-radius: 4px;
  border: 1px solid #e0e0e0;
  outline: none;
  width: 100%;
}

.m-text-input input::placeholder {
  font-weight: 400px;
  font-size: 14px;
  color: #9e9e9e;
}

.m-text-input input:hover {
  background-color: #f6f6f6;
}

.m-text-input input:focus {
  background-color: #ffffff;
  outline: 1px solid #50b83c;
}

.m-text-input--error input {
  box-sizing: border-box;
  padding: 0px 12px;
  border-radius: 4px;
  border: 1px solid #e61d1d !important;
  outline: none;
}

.m-text-input--error input:hover {
  background-color: #f6f6f6;
}

.m-text-input--error input:focus {
  background-color: #ffffff;
  outline: unset;
}

.m-text-input__error-message {
  font-family: "Roboto", sans-serif;
  font-weight: 400;
  font-size: 12px;
  color: #e61d1d;
  position: absolute;
  bottom: 2px;
}
</style>
