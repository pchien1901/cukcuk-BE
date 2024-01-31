<template>
  <div :class="{'cbx': true, 'cbx-value--error': isError}">
    <!-- LABEL -->
    <label v-if="label !== null && label !== '' && label !== undefined" class="m-textfield__label" :for="id">
      {{ label }}
      <span v-if="isRequired" class="required-star">*</span>
    </label>

    <!-- INPUT + BUTTON DROPDOWN -->
    <div class="cbx-value">
      <input
        type="text"
        :name="name"
        :id="id"
        class="cbx-value__txt m-textfield__text-input"
        :placeholder="placeholder"
        :title="title"
        v-model="inputValue"
        @input="handleInput"
        @keydown="handleKeyDown"
        :required="isRequired"
      />

      <button class="cbx-value__btn m-btn--icon" @click="toggleDropdown">
        <div :class="iconClass"></div>
        <i :class="iconAwsClass"></i>
      </button>
    </div>

    <!-- SUGGESTION + DROPDOWN -->
    <div class="cbx-result">

      <!-- SUGGESTION -->
      <div class="cbx-result__input">
        <ul v-if="showSuggestion">
          <li
            v-for="(item, index) in filteredItems"
            :key="item.vale"
            @click="selectItem(item)"
            :class="{'selected': index === selectedIndex }"
          >
            {{ item.text }}
          </li>
        </ul>

        <!-- ERROR MESSAGE -->
        <div v-if="isError" class="cbx-result__error-message">
          <!-- {{ error ? error : 'Không tìm thấy thông tin'}} -->
          {{ errorMsg }}
        </div>
      </div>

      <!-- DROPDOWN -->
      <div v-if="showDropdown" class="cbx-result__dropdown">
        <ul>
          <li
            v-for="item in this.items"
            :key="item.value"
            @click="selectItem(item)"
            :class="{'liSelected': selectedItem && selectedItem.value == item.value}"
          >
            <div
              :class="{
                iconSelected: selectedItem && selectedItem.value == item.value,
              }"
            ></div>
            {{ item.text }}
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "MCombobox",
  props: {
    // nhãn của combobox
    label: String,

    // name của input
    name: String,

    // id của input
    id: String,

    // placeholder 
    placeholder: {
      type: String,
      default: "- Chọn giá trị -",
    },

    // title hiển thị nếu có viết tắt
    title: String,

    // thuộc tính bắt buộc hay không
    isRequired: {
      type: Boolean,
      default: false,
    },

    // class của icon được cắt từ svg
    iconClass: String,

    // class của icon nếu dùng aws
    iconAwsClass: {
      type: String,
      default: "fas fa-angle-down",
    },

    // dữ liệu vào để hiển thị trong dropdow và suggetion
    items: Array,

    // v-model
    modelValue: String,

    // lỗi truyền từ component cha
    error: String,
  },
  emits: ['update:modelValue', 'update:error']
  ,
  watch: {
    filteredItems(newValue) {
      this.filteredItems = newValue;
    },
    selectedItem(newValue) {
      this.inputValue = newValue.text;
      this.result = newValue.value;
      this.$emit("update:modelValue", this.result);
    },
    modelValue(newValue) {
      let data = this.items.filter(item => item.value === newValue);
      //console.log(data);
      if(data.length > 0) {
        this.inputValue = data[0].text;
      } 
      if(!newValue) {
        this.inputValue = null;
      }
    },
    error(newValue) {
      if(newValue) {
        this.isError = true;
        this.errorMsg = newValue;
      }
    },
    errorMsg(newValue) {
      this.$emit("update:error", newValue);
    },
    items(newValue) {
      this.inputItems = newValue;
      let data = this.inputItems.filter(item => item.value === this.modelValue);
      //console.log("data: ", data, "modelValue: ", this.modelValue, "inputItems: ", this.inputItems);
      if(data.length > 0) {
        this.inputValue = data[0].text;
      }
      else {
        this.inputValue = null;
      }
    }
  },
  created() {
    // if(!this.modelValue) {
    //   this.inputValue = null;
    // }
    // let data = this.inputItems.filter(item => item.value === this.modelValue);
    // console.log("data: ", data, "modelValue: ", this.modelValue, "inputItems: ", this.inputItems);
    // if(data.length > 0) {
    //   this.inputValue = data[0].text;
    // }
    // else {
    //   this.inputValue = null;
    // }
  },
  // update() {
  //   this.inputValue = this.value;
  // },

  methods: {
    /**
     * Chuyển inputValue sang Text để hiển thị cho user
     * Author: PMChien
     */
    changeValueToText() {
      try {
        for(let item of this.items) {
          if(item.value === this.inputValue) {
            return item.value;
          }
          if(item.text === this.inputValue) {
            return item.text;
          }
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * @decription Hàm cập nhật input đưa ra ngoài component cha
     * Author: Phạm Minh Chiến
     */
    handleInput($event) {
      let text = $event.target.value;
      this.inputValue = text;

      // nếu giá trị nhập vào khác ""
      if (text) {
        // lọc các item có text bắt đầu với giá trị của input
        this.filteredItems = this.items.filter((item) =>
          item.text.toLowerCase().startsWith(text.trim().toLowerCase())
        );
        if (this.filteredItems.length === 0) {
          this.isError = true;
          this.errorMsg = "Không tìm thấy thông tin";
          this.showSuggestion = false;
        }
        else {
          this.isError = false;
          this.showSuggestion = true;
        }
      } else {
        // Nếu chuỗi input là rỗng và không focus thì tắt suggestion
        if(this.isInputFocus) {
          this.showSuggestion = false;
        }
      }
      // this.$emit("update:value", this.inputValue);
    },

    /**
     * @description Hàm chọn item
     * @param {*} item Object
     * @Author Phạm Minh Chiến
     */
    selectItem(item) {
      this.selectedItem = item;
      this.inputValue = item.text;
      this.showSuggestion = false;
      this.result = item.value;
      if(this.showDropdown) {
        this.showDropdown = false;
      }
    },

    /**
     * @description Ẩn/ hiện dropdown
     * @author Phạm Minh Chiến
     */
    toggleDropdown() {
      if(!this.showDropdown) {
        this.showDropdown = true;
      }
      else {
        this.showDropdown = false;
      }      
      // this.showDropdown = !this.showDropdown;
    },

    /**
     * @description xử lí khi người dùng chọn phím trong input
     * @param {*} event 
     * @author Phạm Minh Chiến
     */
    handleKeyDown(event) {
      if(event.key === 'ArrowUp') {
        // Xử lý khi ấn mũi tên lên 
        this.selectedIndex = (this.selectedIndex - 1 + this.filteredItems.length) % this.filteredItems.length;
      } else if (event.key === 'ArrowDown') {
        // Xử lý khi ấn mũi tên xuống
        this.selectedIndex = (this.selectedIndex + 1 + this.filteredItems.length) % this.filteredItems.length;
      } else if (event.key === 'Enter') {
        // Xử lý khi ấn enter
        if(this.selectedIndex !== -1) {
          this.selectItem(this.filteredItems[this.selectedIndex]);
        }
        else {
          this.items.forEach((item) => {
            if(item.text.toLowerCase().startsWith(this.inputValue.trim().toLowerCase())) {
              this.selectItem(item);
            }
          });
        }
      }
    }
  },
  data() {
    return {
      // Mảng dữ liệu khởi tạo từ props items
      inputItems: this.items,

      // giá trị của input 
      inputValue: "",

      // dữ liệu được chọn
      selectedItem: this.items.find(item => item.text === this.modelValue),

      // mảng dữ liệu được lọc dựa theo giá trị của thẻ input
      filteredItems: [],

      // kiểm tra thẻ input có focus không
      isInputFocus: false,

      // biến kiểm soát ẩn/hiện dropdown
      showDropdown: false,

      // biến kiểm soát ẩn/hiện suggestion
      showSuggestion: false,

      // biến kiểm soát lỗi
      isError: false,

      // thứ tự của phần tử được chọn
      selectedIndex: -1,

      // result
      result: null,

      // chuỗi lỗi hiển thị
      errorMsg: null,
    };
  },
  mounted() {
    let inputElement = this.$el.querySelector('input');

    inputElement.addEventListener('focus', () => {
      this.isInputFocus = true;
      this.showDropdown = false;
    });

    inputElement.addEventListener('blur', () => {
      this.isInputFocus = false;
    });
  }
};
</script>

<style scoped>
@import url("../../../css/base/combobox.css");
</style>
