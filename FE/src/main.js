import { createApp } from 'vue';
import { createStore } from 'vuex';
import App from './App.vue';

//   base component
import MNavItem from '../src/components/base/sidebar/MNavItem.vue';
import MNavAction from '../src/components/base/sidebar/MNavAction.vue';
import MButton from '../src/components/base/button/MButton.vue';
import MDialog from '../src/components/base/dialog/MDialog.vue';
import MToast from '../src/components/base/toast/MToast.vue';
import MInput from '../src/components/base/input/MInput.vue';
import MCheckbox from '../src/components/base/input/MCheckbox.vue';
import MDatePicker from '../src/components/base/input/MDatePicker.vue';
import MRadioInput from '../src/components/base/input/MRadioInput.vue';
import MTextInput from '../src/components/base/input/MTextInput.vue';
import MCombobox from '../src/components/base/combobox/MCombobox.vue';
import MLoading from '../src/components/base/loading/MLoading.vue';
import MIcon from '../src/components/base/icon/MIcon.vue';
import MDropdown from '../src/components/base/combobox/MDropdown.vue';

//  custom component
import MEmployeeImportTable from '../src/view/employee/MEmployeeImportTable.vue';

// view
import MEmployeeList from '../src/view/employee/MEmployeeList.vue';
import MImportEmployee from '../src/view/employee/MImportEmployee.vue';
import MLogin from '../src/view/login/MLogin.vue';

//  lib
import axios from "axios";
import tinyEmitter from "tiny-emitter/instance";
//import { createRouter, createWebHistory } from "vue-router";

// resource
import MResource from './helper/resource';
import MApiResource from './helper/api-resource.js';
import MEnum from './helper/enum';
import { createRouter, createWebHistory } from 'vue-router';


//createApp(App).mount('#app')
const app = createApp(App);

// Khai báo component sẽ sử dụng
app
    .component("MNavItem", MNavItem)
    .component("MNavAction", MNavAction)
    .component('MButton', MButton)
    .component('MDialog', MDialog)
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
    .component("MEmployeeList", MEmployeeList)
    .component("MEmployeeImportTable", MEmployeeImportTable)


// router
const routes = [
  {path: "/", redirect: "/login"},
  {path: "/nhan-vien", name: 'EmployeeRouter', component: MEmployeeList},
  {path: "/nhan-vien/nhap-khau", name: 'ImportEmployeeRouter', component: MImportEmployee},
  {path: "/login", name: 'LoginRouter', component: MLogin},

]

// tạo router
export const router = createRouter({
    history: createWebHistory(),
    routes: routes
});


// vuex
const store = createStore({
  state: {
    isAuthenticate: false,

    role: {
      USER: "USER",
      ADMIN: "ADMIN"
    },

    currentRole: null,
  },
  mutations: {
    changeAuthenticateStatus(state, status) {
      state.isAuthenticate = status;
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


// vuex
app.use(store);

app.mount("#app");

export { store };
export default { app };