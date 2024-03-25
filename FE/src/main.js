/* eslint-disable */
import { createApp } from "vue";
import { createStore } from "vuex";
import VuexPersister from "vuex-persister";
import { saveAs } from "file-saver";
import App from "./App.vue";

//   base component
import MNavItem from "../src/components/base/sidebar/MNavItem.vue";
import MNavAction from "../src/components/base/sidebar/MNavAction.vue";
import MButton from "../src/components/base/button/MButton.vue";
import MDialog from "../src/components/base/dialog/MDialog.vue";
import MToast from "../src/components/base/toast/MToast.vue";
import MInput from "../src/components/base/input/MInput.vue";
import MCheckbox from "../src/components/base/input/MCheckbox.vue";
import MDatePicker from "../src/components/base/input/MDatePicker.vue";
import MRadioInput from "../src/components/base/input/MRadioInput.vue";
import MTextInput from "../src/components/base/input/MTextInput.vue";
import MCombobox from "../src/components/base/combobox/MCombobox.vue";
import MLoading from "../src/components/base/loading/MLoading.vue";
import MIcon from "../src/components/base/icon/MIcon.vue";
import MDropdown from "../src/components/base/combobox/MDropdown.vue";

//  custom component
import EmployeeImportTable from "./view/employee/EmployeeImportTable.vue";

// view
import EmployeeList from "./view/employee/EmployeeList.vue";
import EmployeeImport from "./view/employee/EmployeeImport.vue";
import DepartmentList from "./view/department/DepartmentList.vue";
import Login from "./view/login/Login.vue";

//  lib
import axios from "axios";
import tinyEmitter from "tiny-emitter/instance";
//import { createRouter, createWebHistory } from "vue-router";

// resource
import MResource from "./helper/resource";
import MApiResource from "./helper/api-resource.js";
import MEnum from "./helper/enum";
import { createRouter, createWebHistory } from "vue-router";
import { checkAuthentication, getNewToken } from "./js/services/token";

//createApp(App).mount('#app')
const app = createApp(App);

// Khai báo component sẽ sử dụng
app
  .component("MNavItem", MNavItem)
  .component("MNavAction", MNavAction)
  .component("MButton", MButton)
  .component("MDialog", MDialog)
  .component("MToast", MToast)
  .component("MInput", MInput)
  .component("MTextInput", MTextInput)
  .component("MCheckbox", MCheckbox)
  .component("MDatePicker", MDatePicker)
  .component("MRadioInput", MRadioInput)
  .component("MCombobox", MCombobox)
  .component("MLoading", MLoading)
  .component("MIcon", MIcon)
  .component("MDropdown", MDropdown)

  // view
  .component("EmployeeList", EmployeeList)
  .component("EmployeeImportTable", EmployeeImportTable)
  .component("DepartmentList", DepartmentList);

// VuexPersister - để khi làm mới sẽ còn store
const vuexPersister = new VuexPersister({
  key: "MISA_AMIS",
});

// vuex
const store = createStore({
  state: {
    isAuthenticate: false,

    role: {
      USER: "User",
      ADMIN: "Admin",
    },

    currentRole: null,
  },
  mutations: {
    changeAuthenticateStatus(state, status) {
      state.isAuthenticate = status;
    },

    changeUserRole(state, status) {
      if (status === state.role.ADMIN) {
        state.currentRole = state.role.ADMIN;
      } else {
        state.currentRole = state.role.USER;
      }
    },

    refreshCurrentRole(state) {
      state.currentRole = null;
    },

    logout(state) {
      state.isAuthenticate = false;
      state.currentRole = null;
    },
  },
  plugins: [vuexPersister.persist],
});

// vuex
app.use(store);

// router
const routes = [
  //{path: "/", redirect: "/login"},
  { path: "/nhan-vien", name: "EmployeeRouter", component: EmployeeList },
  {
    path: "/nhan-vien/nhap-khau",
    name: "EmployeeImportRouter",
    component: EmployeeImport,
  },
  { path: "/phong-ban", name: "DepartmentRouter", component: DepartmentList },
  { path: "/login", name: "LoginRouter", component: Login },
];

// tạo router
export const router = createRouter({
  history: createWebHistory(),
  routes: routes,
});

/**
 * Xử lí :
 *     Chưa đăng nhập thì luôn chuyển router về /login
 *     Đã đăng nhập mà truy cập router '/login' thì chuyển router về trang chủ '/'
 */
router.beforeEach(async (to, from, next) => {
  console.log("Trạng thái state: ", store.state);
  const isAuthenticated = store.state.isAuthenticate;

  if (!isAuthenticated && to.path !== "/login") {
    next("/login"); // Chuyển hướng đến trang đăng nhập nếu chưa xác thực
  } else {
    if (isAuthenticated && to.path === "/login") {
        
      let now = new Date();
      // lấy thời gian hết hạn refresh token từ localStorage
      let expirationRefreshTokenString = localStorage.getItem(
        "expirationRefreshToken"
      );
      let expirationRefreshTokenTime = new Date(expirationRefreshTokenString);

      // lấy thời gian hết hạn access token từ localStorage
      let expirationAccessTokenString = localStorage.getItem("expirationToken");
      let expirationAccessTokenTime = new Date(expirationAccessTokenString);

      // khi thời gian hết hạn refresh token < hiện tại => chuyển về trang đăng nhập
      if (expirationRefreshTokenTime <= now) {
        // console.log("Hết thời gian đăng nhập: ", expirationAccessTokenTime);
        store.commit("logout");
        // console.log("thay đổi store thành: ", store.state.isAuthenticate);

        next("/login"); // Chuyển hướng đến trang đăng nhập nếu chưa xác thực
        tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
          type: MResource["VN"].ToastTypeWarning,
          message: MResource["VN"].Toast.Auth.TokenExpiration,
          action: null,
        });
      } else if (expirationAccessTokenTime <= now) {
        //console.log("access token hết hạn");
        let data = {
          AccessToken: localStorage.getItem("accessToken"),
          RefreshToken: localStorage.getItem("refreshToken"),
        };
        let res = await axios.post(
          `${apiURL}/${MApiResource.apiUrl.refreshToken}`,
          data,
          {
            headers: {
              "Content-Type": MApiResource.apiHeaderContentType.applicationType,
            },
          }
        );
        //console.log("token mới: ", res);
        localStorage.setItem("accessToken", res.data.accessToken);
        localStorage.setItem("refreshToken", res.data.refreshToken);
        localStorage.setItem("expirationToken", res.data.Expiration);
        localStorage.setItem(
          "expirationRefreshToken",
          res.data.ExpirationRefreshToken
        );


        next("/");
      }
    } else {
      next(); // Tiếp tục điều hướng bình thường
    }
  }
});

app.use(router);

// Biến toàn cục
app.config.globalProperties.$axios = axios;
app.config.globalProperties.$tinyEmitter = tinyEmitter;
app.config.globalProperties.$MEnum = MEnum;
app.config.globalProperties.$MResource = MResource;
app.config.globalProperties.$MApiResource = MApiResource;
app.config.globalProperties.$saveAs = saveAs;

app.mount("#app");

export { store };
export default { app };
