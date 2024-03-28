<template>
  <div :class="computedWrapperClasses">
    <label :for="id">
      {{ label }}
      <span v-if="isRequired" class="required-star">*</span>
    </label>
    <!-- <input
      type="date"
      :name="name"
      :id="id"
      :class="computedClasses"
      v-model="date"
    /> -->
    
    <div class="m-date-picker-wrap">
      <flat-pickr 
        v-model="date"
        :class="computedClasses"
        placeholder="dd/mm/yyyy"
        :name="name"
        :config="config"
      />
      <div class="icon-calendar"></div>
    </div>
    
    <div v-if="errorMessage" class="m-date-picker__error-message">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script>
/* eslint-disable */
import { createDateString } from '../../../js/ulti/convert-data.js';
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import "flatpickr/dist/themes/material_green.css";
import { Vietnamese } from "flatpickr/dist/l10n/vn.js";
export default {
  name: "MDatePicker",
  components: { flatPickr },
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
    modelValue: [String, Date],
  },
  emits: ["update:modelValue"],
  data() {
    // let newDate = new Date(this.value);
    return {
      // date: newDate,
      date: this.modelValue,
      //date: this.value,
      config: {
        altFormat: "d/m/Y",
        altInput: true,
        enableTime: false,
        defaultHour:0,
        allowInput : true,
        altInputClass: "flat width-1",
        locale: Vietnamese,
      }
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
@import url("../../../css/component/date-picker.css");
</style>
