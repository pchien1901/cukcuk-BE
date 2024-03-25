<template>
  <nav :class="computedClasses">
    <div class="sidebar-header">
      <div class="icon-menu"></div>
      <div class="icon-logo"></div>
      <div class="sidebar-header__text">KẾ TOÁN</div>
    </div>
    <div class="sidebar-body">
      <router-link to="/nhan-vien">
        <MNavItem :text="'Nhân viên'" :iconClass="'icon-employee'" :iconActive="'icon-employee'"/>
      </router-link>
      <router-link v-if="isAdmin" to="/phong-ban">
        <MNavItem :text="'Phòng ban'" :iconClass="'icon-department'" :iconActive="'icon-department'"/>
      </router-link>
      <router-link v-if="isAdmin" to="/nhan-vien">
        <MNavItem :text="'Chức danh'" :iconClass="'icon-position'" :iconActive="'icon-position'"/>
      </router-link>
      <MNavItem :text="'Tổng quan'" :iconClass="'icon-overview'" :iconActive="'icon-overview'"/>
      <MNavItem :text="'Tiền mặt'" :iconClass="'icon-cash'" :iconActive="'icon-cash'"/>
      <MNavItem :text="'Tiền gửi'" :iconClass="'icon-deposits'" :iconActive="'icon-deposits'"/>
      <MNavItem :text="'Mua hàng'" :iconClass="'icon-buy'" :iconActive="'icon-buy'"/>
      <MNavItem :text="'Bán hàng'" :iconClass="'icon-sell'" :iconActive="'icon-sell'"/>
      <MNavItem :text="'Quản lý hóa đơn'" :iconClass="'icon-bill'" :iconActive="'icon-bill'"/>
      <MNavItem :text="'Kho'" :iconClass="'icon-warehouse'" :iconActive="'icon-warehouse'"/>
      <MNavItem :text="'Công cụ dụng cụ'" :iconClass="'icon-tools'" :iconActive="'icon-tools'"/>
      <MNavItem :text="'Tài sản cố định'" :iconClass="'icon-fixed-assets'" :iconActive="'icon-fixed-assets'"/>
      <MNavItem :text="'Thuế'" :iconClass="'icon-tax'" :iconActive="'icon-tax'"/>
      <MNavItem :text="'Giá thành'" :iconClass="'icon-price'" :iconActive="'icon-price'"/>
      <MNavItem :text="'Tổng hợp'" :iconClass="'icon-synthetic'" :iconActive="'icon-synthetic'"/>
      <MNavItem :text="'Ngân sách'" :iconClass="'icon-budget'" :iconActive="'icon-budget'"/>
      <MNavItem :text="'Báo cáo'" :iconClass="'icon-report'" :iconActive="'icon-report'"/>
      <MNavItem :text="'Phân tích tài chính'" :iconClass="'icon-financial-analysis'" :iconActive="'icon-financial-analysis'"/>
    </div>
    <div class="sidebar-footer">
      <MNavAction :text="'Thu gọn'"/>
    </div>
  </nav>
</template>

<script>
export default {
  name: "TheSidebar",
  created() {
    this.$tinyEmitter.on("lessSidebar", this.handleLessSidebar);
    this.$tinyEmitter.on("showAdminOption", this.showAdminOption);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("lessSidebar");
    this.$tinyEmitter.off("showAdminOption");
  },
  computed: {
    computedClasses() {
      return {
        "sidebar": true,
        "sidebar-less": this.isLess,
      };
    }
  },
  data() {
    return {
      isLess: false,
      isAdmin: false,
    };
  },
  methods: {
    /**
     * Thu gọn sidebar
     * Author: PMChien
     */
    handleLessSidebar(newValue) {
      try {
        this.isLess = newValue;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * thay đổi isAdmin (thay đổi UI) với người dùng có quyền Admin
     * Author: PMChien
     */
    showAdminOption(option) {
      try {
        this.isAdmin = option;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  }
}
</script>

<style scoped>
@import url('../../css/layout/sidebar.css');
</style>