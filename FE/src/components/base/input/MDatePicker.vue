<template>
  <div :class="computedWrapperClasses">
    <label :for="id">
      {{ label }}
      <span v-if="isRequired" class="required-star">*</span>
    </label>
    <input
      type="date"
      :name="name"
      :id="id"
      :class="computedClasses"
      v-model="date"
    />
    <div v-if="errorMessage" class="m-date-picker__error-message">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script>
/* eslint-disable */
import { createDateString } from '../../../js/ulti/convert-data.js';
export default {
  name: "MDatePicker",
  props: {
    // Nhãn của trường
    label: String,

    // giá trị truyền vào - giá trị ban đầu
    //value: String,

    // Thông báo lỗi
    error: String,

    // id của datepicker
    id: String,

    // name của datepicker
    name: String,

    // kích thước small: 32px, medium: 36px, large: 40px
    size: {
      type: String,
      default: "medium",
      validator: function (value) {
        return ["small", "medium", "large"].includes(value);
      },
    },

    // class của input type date
    class: {
      type: String,
      default: "",
    },

    // có bắt buộc hay không
    isRequired: {
      type: Boolean,
      default: false,
    },
    modelValue: String,
  },
  emits: ["update:modelValue"],
  data() {
    // let newDate = new Date(this.value);
    return {
      // date: newDate,
      date: this.modelValue,
      //date: this.value,
    };
  },
  watch: {
    date() {
      this.$emit("update:modelValue", this.date);
    },
    modelValue(newValue) {
      this.date = newValue;
    }
  },
  computed: {
    computedClasses() {
      return {
        "m-date-picker__input": true,
        [`m-date-picker__input-${this.size}`]: true,
      };
    },
    computedWrapperClasses() {
      return {
        "m-date-picker": true,
        "m-date-picker--error": this.errorMessage,
        [this.class]: [this.class],
      };
    },
    errorMessage() {
      return this.error || null;
    },
  },
};
</script>

<style scoped>
/* DATE PICKER */
.m-date-picker {
  font-size: 14px;
  display: flex;
  flex-direction: column;
}

.m-date-picker__input-small {
  height: 32px;
}

.m-date-picker__input-medium {
  height: 36px;
}

.m-date-picker__input-large {
  height: 40px;
}

.m-date-picker label {
  font-weight: 500;
  margin-bottom: 8px;
  font-family: "Roboto", sans-serif;
  font-size: 14px;
}

.m-date-picker input[type="date"] {
  box-sizing: border-box;
  padding: 0px 12px;
  border-radius: 4px;
  border: 1px solid #ebebeb;
  outline: none;
  /* color: #9E9E9E; */
  font-family: "Roboto", sans-serif;
  font-size: 14px;
  font-weight: 400;
}

.m-date-picker input[type="date"]::placeholder {
  font-family: "Roboto", sans-serif;
  font-size: 14px;
}

.m-date-picker--error input {
  box-sizing: border-box;
  padding: 0px 12px;
  border-radius: 4px;
  border: 1px solid #e61d1d !important;
  outline: none;
}

.m-date-picker--error input:hover {
  background-color: #f6f6f6;
}

.m-date-picker--error input:focus {
  background-color: #ffffff;
  outline: unset;
}

.m-date-picker__error-message {
  font-family: "Roboto", sans-serif;
  font-weight: 400;
  font-size: 12px;
  color: #e61d1d;
  position: absolute;
    bottom: 2px;
}
</style>
