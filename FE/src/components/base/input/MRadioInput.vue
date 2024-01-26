<!-- RadioInput.vue -->
<template>
  <div :class="{'m-radio-textfield-row': true, [this.inputClass]: this.inputClass}">
    <label>{{ label }}</label>
    <div class="m-radio-option-row">
      <label v-for="option in options" :key="option.value" class="radio-option">
        <input
          type="radio"
          :name="name"
          :value="option.value"
          v-model="selectedOption"
        />
        {{ option.label }}
      </label>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    label: String,
    name: String,
    options: {
      type: Array,
      default: () => [],
    },
    modelValue: [String, Number],
    inputClass: String,
  },
  data() {
    return {
      selectedOption: this.value,
    };
  },
  emits: ['update:modelValue'],
  watch: {
    selectedOption(newValue) {
      this.$emit('update:modelValue', newValue);
    },
    modelValue(newValue) {
      this.selectedOption = newValue;
    },
  },
};
</script>

<style scoped>
.m-radio-textfield-row {
  display: flex;
  flex-direction: column;
}

.m-radio-option-row {
  display: flex;
  align-items: center;
  height: 36px;
}

.m-radio-textfield-row {
  font-family: "Roboto", sans-serif;
  font-size: 14px;
}
.m-radio-textfield-row > label {
  font-family: "Roboto", sans-serif;
  font-weight: 500;
  margin-bottom: 8px;
}

.m-radio-option-row :nth-child(2) {
  margin: 0px 12px;
}

.radio-option {
  display: flex;
  align-items: center;
}

.m-radio-textfield-row input[type="radio"] {
  border: 2px solid #707070;
}

input[type="radio"] {
  width: 20px;
  height: 20px;
}
</style>