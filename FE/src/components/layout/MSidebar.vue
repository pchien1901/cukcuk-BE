<template>
  <nav :class="computedClasses">
    <div class="sidebar-header">
      <div class="icon-menu"></div>
      <div class="icon-logo"></div>
      <div class="sidebar-header__text">KẾ TOÁN</div>
    </div>
    <div class="sidebar-body">
      <router-link to="/nhan-vien">
        <MNavItem :text="'Tổng quan'" :iconClass="'icon-overview-active'" :iconActive="'icon-overview-active'"/>
      </router-link>
      <MNavItem :text="'Tiền mặt'" :iconClass="'icon-cash-active'" :iconActive="'icon-cash-active'"/>
      <MNavItem :text="'Tiền gửi'" :iconClass="'icon-deposits-active'" :iconActive="'icon-deposits-active'"/>
      <MNavItem :text="'Mua hàng'" :iconClass="'icon-buy-active'" :iconActive="'icon-buy-active'"/>
      <MNavItem :text="'Bán hàng'" :iconClass="'icon-sell-active'" :iconActive="'icon-sell-active'"/>
      <MNavItem :text="'Quản lý hóa đơn'" :iconClass="'icon-bill-active'" :iconActive="'icon-bill-active'"/>
      <MNavItem :text="'Kho'" :iconClass="'icon-warehouse-active'" :iconActive="'icon-warehouse-active'"/>
      <MNavItem :text="'Công cụ dụng cụ'" :iconClass="'icon-tools-active'" :iconActive="'icon-tools-active'"/>
      <MNavItem :text="'Tài sản cố định'" :iconClass="'icon-fixed-assets-active'" :iconActive="'icon-fixed-assets-active'"/>
      <MNavItem :text="'Thuế'" :iconClass="'icon-tax-active'" :iconActive="'icon-tax-active'"/>
      <MNavItem :text="'Giá thành'" :iconClass="'icon-price-active'" :iconActive="'icon-price-active'"/>
      <MNavItem :text="'Tổng hợp'" :iconClass="'icon-synthetic-active'" :iconActive="'icon-synthetic-active'"/>
      <MNavItem :text="'Ngân sách'" :iconClass="'icon-budget-active'" :iconActive="'icon-budget-active'"/>
      <MNavItem :text="'Báo cáo'" :iconClass="'icon-report-active'" :iconActive="'icon-report-active'"/>
      <MNavItem :text="'Phân tích tài chính'" :iconClass="'icon-financial-analysis-active'" :iconActive="'icon-financial-analysis-active'"/>
    </div>
    <div class="sidebar-footer">
      <MNavAction :text="'Thu gọn'"/>
    </div>
  </nav>
</template>

<script>
export default {
  name: "MSidebar",
  created() {
    this.$tinyEmitter.on("lessSidebar", this.handleLessSidebar);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("lessSidebar");
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
    };
  },
  methods: {
    /**
     * 
     */
    handleLessSidebar(newValue) {
      try {
        this.isLess = newValue;
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