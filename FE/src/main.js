import { createApp } from 'vue'
import App from './App.vue'

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

// view
import MEmployeeList from '../src/view/employee/MEmployeeList.vue';
import MImportEmployee from '../src/view/employee/MImportEmployee.vue';

//  lib
import axios from "axios";
import tinyEmitter from "tiny-emitter/instance";
//import { createRouter, createWebHistory } from "vue-router";

// resource
import MResource from './helper/resource';
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

// router
const routes = [
  {path: "/nhan-vien", name: 'EmployeeRouter', component: MEmployeeList},
  {path: "/nhan-vien/nhap-khau", name: 'ImportEmployeeRouter', component: MImportEmployee}
]

const router = createRouter({
    history: createWebHistory(),
    routes: routes
});

app.use(router);

// Biến toàn cục
app.config.globalProperties.$axios = axios;
app.config.globalProperties.$tinyEmitter = tinyEmitter;
app.config.globalProperties.$MEnum = MEnum;
app.config.globalProperties.$MResource = MResource;

app.mount("#app");