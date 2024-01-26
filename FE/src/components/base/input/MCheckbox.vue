<template>
  <label :class="computedClasses">
    <input
      type="checkbox"
      :name="inputName"
      :id="id"
      v-model="isChecked"
    />
    <span class="checkmark"></span>
    <label v-if="isShowLabel" class="checkbox-label" :for="inputName">
      {{ label }}
    </label>
  </label>
</template>

<script>
export default {
  name: "MCheckbox",
  props: {
    label: {
      type: String,
      default: "",
    },
    inputName: {
      type: String,
      default: "",
    },
    id: {
      type: String,
      default: "",
    },
    class: {
      type: String,
      default: "",
    },
    fOnChange: Function,
    //value: Boolean,
    modelValue: Boolean,
  },
  emits: ["update:modelValue"],
  created() {
    this.isChecked = this.modelValue;
  },
  data() {
    return {
      isChecked: null,
    };
  },
  watch: {
    isChecked() {
      this.$emit("update:modelValue", this.isChecked);
    },
    modelValue(newValue) {
      this.isChecked = newValue;
    }
  },
  methods: {
  },
  computed: {
    isShowLabel() {
      if (this.label != null && this.label != "") {
        return true;
      }
      return false;
    },
    computedClasses() {
      return {
        "checkbox-container": true,
        [this.class]: [this.class],
        "checked": this.isChecked,
      }
    }
  },
};
</script>

<style scoped>
.checkbox-container {
  display: block;
  position: relative;
  cursor: pointer;
  font-size: 22px;
  font-family: "Roboto", sans-serif;
  font-size: 14px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
  width: 20px;
  height: 20px;
}

/* Hide the browser's default checkbox */
.checkbox-container input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
  height: 0;
  width: 0;
}

/* Create a custom checkbox */
.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 20px;
  width: 20px;
  background-color: #ffffff;
  border: 2px solid #757575;
  border-radius: 4px;
  box-sizing: border-box;
}

/* On mouse-over, add a grey background color */
.checkbox-container:hover input ~ .checkmark {
  background-color: #f2f2f2;
}

/* When the checkbox is checked, add a blue background */
.checkbox-container.checked input ~ .checkmark {
  background-color: #00ba48;
  border-color: #00ba48;
}

/* Create the checkmark/indicator (hidden when not checked) */
.checkmark:after {
  content: "";
  position: absolute;
  display: none;
  border-color: transparent;
}

/* Show the checkmark when checked */
/* .checkbox-container input:checked ~ .checkmark:after {
  display: block;
} */

/* .checkbox-container[modevalue="true"] .checkmark:after {
  display: block;
} */

/* Style the checkmark/indicator */
.checkbox-container.checked .checkmark:after {
  left: 4px;
  top: 2px;
  width: 4px;
  height: 7px;
  border: solid white;
  border-width: 0 3px 3px 0;
  -webkit-transform: rotate(45deg);
  -ms-transform: rotate(45deg);
  transform: rotate(45deg);
  display: block;
}

.checkbox-container .checkbox-label {
  padding-left: 24px;
}
</style>
