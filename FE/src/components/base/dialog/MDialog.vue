<template>
  <div :class="{ overlay: true }">
    <div class="dialog-container">
      <div class="dialog-header">
        <div class="dialog-header__title">{{ title }}</div>
        <MButton
          :type="'icon'"
          :class="'m-cancel-btn btn-close-dialog'"
          :size="'medium'"
          :iconClass="'icon-cancel-red'"
          @click="closeDialog"
        />
      </div>
      <div class="dialog-content">
        <!-- <i v-if="type === this.$MEnum.DialogType.ERROR" class="fas fa-question-circle"></i> -->
        <div v-if="type === this.$MEnum.DialogType.ERROR" class="iconError"></div>
        <div v-if="type === this.$MEnum.DialogType.WARNING" class="iconWarning"></div>
        <div v-if="type === this.$MEnum.DialogType.INFO" class="iconInfo"></div>
        <div class="dialog-content__text">
          <ul>
            <li
              v-for="(item, index) in text"
              :key="index"
              :class="{ unsetLiStyle: !this.listStyle }"
            >
              {{ item }}
            </li>
          </ul>
        </div>
      </div>
      <div class="dialog-footer">
        <div class="dialog-footer__left">
          <MButton
            v-if="mode === this.$MEnum.DialogMode.DELETE || mode === this.$MEnum.DialogMode.FULL"
            :type="'second'"
            :text="cancelBtnText"
            :class="'dialog-footer__btn-cancel btn-close-dialog'"
            @click="closeDialog"
          />
        </div>
        <div class="dialog-footer__right">
          <MButton
            v-if="mode === this.$MEnum.DialogMode.FULL"
            :type="'second'"
            :text="secondBtnText"
            :class="'dialog-footer__btn-cancel btn-close-dialog'"
            @click="secondAction"
          />
          <MButton
            :type="'primary'"
            :text="'Đồng ý'"
            :class="'m-primary-btn btn-accept-dialog'"
            @click="handleClickBtn"
          />
        </div>
        
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "MDialog",
  created() {},
  props: {
    // tên của cảnh báo
    title: String,

    // Nội dung cảnh báo
    text: Array,

    // Hàm đóng Dialog
    closeFunction: { type: Function, required: true },

    // loại dialog: thông báo có hảnh động hay thông báo cảnh báo
    mode: {
      type: Number,
      default: 0,
    },

    // loại của icon: Waring info hay error
    type: {
      type: Number,
      default: 0,
    },

    // hàm thao tác với nút chính dialog
    primaryAction: Function,
    secondAction: Function,
    primaryBtnText: { type: String, default: "Đồng ý" },
    secondBtnText: { type: String, default: "Không" },
    cancelBtnText: { type: String, default: "Hủy" },
    listStyle: { type: Boolean, default: false}
  },
  data() {
    return {
      showWaringDialog: false,
    };
  },
  methods: {
    /**
     * Đóng Dialog
     * Author: Phạm Minh Chiến
     */
    closeDialog() {
      try {
        this.showWaringDialog = false;
        this.closeFunction();
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Xử lí khi click primary trong Dialog
     * Author: Phạm Minh Chiến
     */
    handleClickBtn() {
      try {
        if (this.mode === this.$MEnum.DialogMode.DELETE || this.mode === this.$MEnum.DialogMode.FULL) {
          this.primaryAction();
          this.closeDialog();
        } else {
          this.closeDialog();
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
  },
  watch: {
    isShow(newValue) {
      this.showWaringDialog = newValue;
    },
  },
};
</script>

<style scoped>
@import url('../../../css/base/dialog.css');
</style>
