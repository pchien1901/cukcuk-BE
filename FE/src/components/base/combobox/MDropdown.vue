<template>
  <div class="m-dropdown-container">
    <div class="m-dropdown__result" @click="toogleDropdown">
      {{ resultValue }} 
      <i class="fas fa-chevron-down"></i>
    </div>
    <ul v-if="showDropdown" :class="computedClass">
      <li v-for="item in this.items" :key="item.value" class="m-dropdown__option" @click="selectItems(item)" 
      :class="{liSelected : selectedItem && selectedItem.value == item.value}"    
      >
        {{ item.text }}
        <div :class="{iconSelected : selectedItem && selectedItem.value == item.value}"></div>
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  name: "MDropDown",
  props: {
    items: Array,
    modelValue: [String, Number],
    position: { 
      type: String,
      default: 'bottom',
      validator: function(value) {
        return ["top", "right", "bottom", "left"].includes(value);
      }
    }
  },
  emits: ["update:modelValue"],
  data() {
    return {
      inputItems: this.items,
      resultValue: this.modelValue,
      selectedItem: this.items.find(item => item.value === this.modelValue),
      showDropdown: false,
    }
  },
  computed: {
    computedClass() {
      return {
        "m-dropdown__list-wrapper": true,
        [`m-dropdown--${this.position}`] : true,
      }
    }
  },
  watch: {
    modelValue(newValue) {
      this.selectedItem = this.items.find(item => item.value === newValue);
    }
  },
  methods: {
    /**
     * Ẩn/ hiện dropdown
     * Author: PMChien
     */
    toogleDropdown() {
      this.showDropdown = !this.showDropdown;
    },

    /**
     * Chọn item để cập nhật
     * @param {object} item item được chọn
     * Author: PMChien
     */
    selectItems(item) {
      this.selectedItem = item;
      this.resultValue = item.text;
      this.$emit("update:modelValue", this.selectedItem.value)
      if(this.showDropdown) {
        this.showDropdown = false;
      }
    }
  }
}
</script>

<style scoped>
.m-dropdown-container {
  position: relative;
}
.m-dropdown__result {
  width: 60px;
  height: 36px;
  padding: 4px 8px;
  box-sizing: border-box;
  cursor: pointer;
  display: flex;
  align-items: center;
  border-radius: 4px;
}

.m-dropdown__result i {
  margin-left: 8px;
}

.m-dropdown__result:hover {
  background-color: var(--primary-boder);
}

.m-dropdown__list-wrapper {
  position: absolute;  
}

.m-dropdown--top {
  bottom: calc(100% + 8px)
}

.m-dropdown__list-wrapper {
  width: 95px;
  max-height: 308px;
  padding: 8px 0px 8px 0px;
  margin: 0px;
  box-sizing: border-box;
  border: 1px solid var(--primary-boder);
  /* box-shadow: 0 4px 16px rgba(23,27,42,0.24); */
  background-color: #FFFFFF;
  overflow-y: scroll;
  box-shadow:  0px 2px 8px rgba(23,27,42,0.24);
}

.m-dropdown__list-wrapper::-webkit-scrollbar {
  width: 8px;
}

.m-dropdown__list-wrapper li {
  display: flex;
  align-items: center;
  list-style: none;
  font-family: "Roboto", sans-serif;
  font-size: 14px;
  font-weight: 400;
  margin-left: 8px;
  padding-left: 8px;
  height: 36px;
  border-radius: 4px;
  position: relative;
}

.m-dropdown__list-wrapper li:hover {
  background-color: var(--primary-boder);
  font-weight: 400;
}

.iconSelected {
  background: url("../../../assets/img/Sprites.64af8f61.svg") no-repeat -1225px -363px;
  width: 14px;
  height: 11px;
  position: absolute;
  right: 8px;
}

.liSelected {
  color: #50B83C;
}

</style>