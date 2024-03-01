<template>
  <header class="header">
    <div class="header-left">
      <div class="icon-menu-line"></div>
      <div class="header-left__company-name">CÔNG TY TNHH SẢN XUẤT - THƯƠNG MẠI - DỊCH VỤ QUI PHÚC</div>
      <div class="icon-arrow-down"></div>
    </div>
    <div class="header-right">
      <div class='icon-user-avt'></div>
      <div class="user-dropdown-container" >
        <div class="user-control" @click="toggleDropdown">
          <div class="user-name">Phạm Huy Hoàng</div>
          <div class="icon-arrow-down"></div>
        </div>
        
        <div v-if="showDropdown" class="user-dropdown" >
          <div class="user-dropdown__item profile">Hồ sơ</div>
          <div class="user-dropdown__item logout" @click="logout">Đăng xuất</div>
        </div>
      </div>
    </div>
  </header>
</template>

<script>
export default {
  name: "MHeader",
  data() {
    return {
      showDropdown: false,
    };
  },
  methods: {
    /**
     * Đóng mở user dropdown
     * Author: PMChien
     */
    toggleDropdown() {
      this.showDropdown = !this.showDropdown;
    },
    /**
     * Đăng xuất khỏi web
     * Author: PMChien
     */
    logout() {
      try {
        this.toggleDropdown();
        // xóa các accessToken, refreshToke, expirationToken, expirationRefreshToken ra khỏi localStorage
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("expirationToken");
        localStorage.removeItem("expirationRefreshToken");

        // đổi trạng thái đăng nhập trong store
        this.$store.commit("changeAuthenticateStatus", false);

        // quay về trang login
        this.$router.push("/login");
      } catch (error) {
        console.error('Đã có lỗi xảy ra: ', error);
      }
    }
  }
};
</script>

<style scoped>
@import url('../../css/layout/header.css');
</style>