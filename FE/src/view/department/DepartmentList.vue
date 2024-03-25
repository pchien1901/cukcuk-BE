<template>
  <div class="department-page-container">
    <!-- HEADER -->
    <div class="department-page-header">
      <div class="department-page-header__left">
        <div class="title">Phòng ban</div>
      </div>
      <div class="department-page-header__right">
        <MButton
          :type="'primary'"
          :text="'Thêm mới phòng ban'"
        />
      </div>
    </div>

    <!-- BODY -->
    <div class="department-page-body"></div>

    <!-- POPUP -->


    <!-- DIALOG -->
    <!-- <MDialog 
    /> -->

    <!-- TOAST -->
    <div class="toast-message">
      <MToast 
        v-if="showToast"
        :type="toast.type"
        :message="toast.message"
        :action="closeToast"
      />
    </div>
  </div>
</template>

<script>
import { checkAuthentication } from '@/js/services/token';
export default {
  name: "DepartmentList",
  data() {
    return {
      showToast: false,
      toast: {
        type: "success",
        message: "",
        action: null,
      }
    };
  },
  async created() {
    await checkAuthentication();
    
  },
  method: {
    /**
     * Đóng Toast
     * Author: PMChien
     */
    closeToast() {
      try {
        this.showToast = false;
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    }
  }
}
</script>

<style scoped>
@import url('../../css/view/department/department-list.css');
</style>