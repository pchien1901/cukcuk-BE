<template>
  <div class="employee-page-container">
    <MLoading v-if="isShowLoading"/>
    <!-- HEADER -->
    <div class="employee-page-header">
      <div class="employee-page-header__left">
        <div class="title">Nhân viên</div>
      </div>
      <div class="employee-page-header__right">
        <MButton
          :type="'primary'"
          :text="'Thêm mới nhân viên'"
          @click="openPopupToCreate"
        />
      </div>
    </div>

    <!-- BODY -->
    <div class="employee-page-body">
      <MEmployeeTable :items="employees" :totalPage="totalPage" :totalRecord="totalRecord"/>
    </div>

    <!-- POPUP -->
    <MEmployeePopup v-if="showPopup" :closeFunction="() => this.closePopup()" :inputData="inputData"/>

    <!-- DIALOGL -->
    <MDialog
      v-if="showDialog"
      :type="this.dialog.type"
      :title="this.dialog.title"
      :text="this.dialog.text"
      :mode="this.dialog.mode"
      :closeFunction="closeDialog"
      :primaryAction="this.dialog.primaryAction"
      :primaryBtnText="this.dialog.primaryBtnText"
      :cancelBtnText="this.dialog.cancelBtnText"
    />

    <!-- TOAST -->
    <div class="toast-message">
      <MToast
        v-if="showToast"
        :type="toast.type"
        :message="toast.message"
        :action="toast.action"
        :closeFunction="closeToast"
      />
    </div>
  </div>
</template>

<script>
/* eslint-disable */
import MEmployeeTable from "./MEmployeeTable.vue";
import MEmployeePopup from "./MEmployeePopup.vue";
import {
  getAllEmployees,
  getEmployeeById,
  createEmployee,
  updateEmployee,
  deleteEmployee,
  deleteEmployeeById,
  getNewEmployeeCode,
getEmployeeInfo,
getEmployeeInfoByPage,
} from "../../js/services/employee.js";
import { getAllDepartments, getDepartmentById } from '../../js/services/department.js';
import { getAllPositions, getPositionById } from '../../js/services/position.js';
import { checkAuthentication } from "@/js/services/token";
export default {
  name: "MEmployeeList",
  components: { MEmployeeTable, MEmployeePopup },
  data() {
    return {
      /**
       * employees: Danh sách nhân viên
       * employeeToActionMulti: mảng id của các Employee để thực hiện hành động hàng loạt
       * showPopup: quản lí đóng mở popup
       * showDialog: quản lí đóng mở dialod
       * showToast: quản lí đóng mở toast
       * dialog (object) quản lí thông tin dialog
       * inputData: dữ liệu Employee đưa vào popup thực hiện edit
       * isShowLoading: Quản lí đóng hiện loading khi server lỗi không vào được
       */
      employees: [],
      employeeToActionMulti: [],
      showPopup: false,
      showDialog: false,
      showToast: false,
      showMultiAction: false,
      showContextMenuMultiAction: false,
      dialog: {
        type: this.$MEnum.DialogType.WARNING,
        mode: this.$MEnum.DialogMode.WARNING,
      //   title: "",
        text: ["Xóa người dùng"],
        primaryAction: null,
        primaryBtnText: null,
        cancelBtnText: null,
      },
      toast: {
        type: "success",
        message: "",
        action: null,
      },
      totalPage: null,
      totalRecord: null,
      inputData: {},
      isShowLoading: true,
    };
  },
  async created() {
    await checkAuthentication();
    this.isShowLoading = true;
    await this.loadData(1, 10, "");
    //setTimeout(() => { this.isShowLoading = false}, 2000);
    this.isShowLoading = false;
  },
  mounted() {
    // Khai báo Emitter
    this.$tinyEmitter.on("loadData", this.handleLoadingData);
    this.$tinyEmitter.on("openPopup", this.openPopup);
    this.$tinyEmitter.on("openToast", this.openToastEmit)

    // Emit CRUD
    this.$tinyEmitter.on("duplicateEmployee", this.setValueToDuplicateEmployee);
    this.$tinyEmitter.on("setValueToUpdateEmployee", this.setValueToUpdateEmployee);
    this.$tinyEmitter.on("deleteEmployee", this.deleteEmployee);
    this.$tinyEmitter.on("deleteEmployeeAny", this.deleteEmployeeAny);
  },
  beforeUnmount() {
    // Hủy Emitter
    this.$tinyEmitter.off("openPopup");
    this.$tinyEmitter.off("loadData");
    this.$tinyEmitter.off("openToast");

    this.$tinyEmitter.off("duplicateEmployee");
    this.$tinyEmitter.off("setValueToUpdateEmployee");
    this.$tinyEmitter.off("deleteEmployee");
    this.$tinyEmitter.off("deleteEmployeeAny");
  },
  watch: {
    
  },
  methods: {
    /**======================= ĐÓNG MỞ CÁC THÀNH PHẦN ================================= */
    //#region  Đóng mở các thành phần
    /**
     * Mở popup và reset form
     * Author: PMChien
     */
    openPopupToCreate() {
      try {
        this.inputData = {
          IsCreate: true,
          IsUpdate: false,
          IsDuplicate: false,
        };
        this.showPopup = true;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Đóng popup
     * Author: PMChien
     */
    closePopup() {
      try {
        this.showPopup = false;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Mở toast
     * Author: PMChien
     */
    openToast() {
      try {
        this.showToast = true;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Mở toast khi được emit
     * @param {object} data chứa thông tin về toast: type - loại (error/ success), message: nội dung toast
     * Author: PMChien
     */
    openToastEmit(data) {
      try {
        this.toast.type = data.type;
        this.toast.message = data.message;
        this.showToast = true;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Đóng toast
     * Author: PMChien
     */
    closeToast() {
      try {
        this.showToast = false;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    //#endregion

    /*====================================================LOAD DATA==============================================*/
    //#region Load data, tạo hiệu ứng loaddata
    /**
     * Lấy các dữ liệu từ api
     * Author: PMChien (25/01/2024)
     */
    async loadData(page = 1, pageSize = 10, text = "") {
      try {
        // gọi api
        let allEmployees = await getEmployeeInfoByPage(page, pageSize, text);
          this.employees = allEmployees.ListRecord;
          this.totalPage = allEmployees.TotalPage;
          this.totalRecord = allEmployees.TotalRecord;
          /** 
           * thêm trường
           * IsChecked: đánh dấu checkbox được chọn
           * IsShowMenu: show menu context
           * */ 
          for (const employee of this.employees) {
            employee.IsChecked = false;
            employee.IsShowMenu = false;
          }  
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Hàm gọi loadData và tạo hiệu ứng loading khi button Tải lại click
     * Author: PMChien
     */
    async handleLoadingData(page, pageSize, text) {
      try {
        // Tạo hiệu ứng loading
        this.isShowLoading = true;
        // LoadData
        await this.loadData(page, pageSize, text);
        // Tắt hiệu ứng loading
        //setTimeout(() => { this.isShowLoading = false }, 2000);
        this.isShowLoading = false;
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },
    //#endregion

    /*====================================================CRUD================================================= */
    //#region CRUD
    /**
     * cập nhật giá trị truyền vào inputData cho popup để sửa
     * @param {string} id id của employee cần sửa
     * Author: PMChien
     */
    async setValueToUpdateEmployee(id) {
      try {
        // res nếu thành công thì status = 200 ( >=200 && < 300), employee = res.data
        let res = await getEmployeeById(id);
        let status = res.status;
        if(status >= 200 && status < 300) {
          let employee = res.data;
          this.inputData = {...employee, IsUpdate: true, IsDuplicate: false, IsCreate: false};
          this.showPopup = true;
        }
      } catch (error) {
        console.error("Có lỗi update employee tại list: ", error);
      }
    },

    /**
     * Truyền giá trị vào inputData thực hiện nhân bản
     * @param {*} data dữ liệu truyền vào (là 1 hàng từ bảng Employee)
     * Author: PMChien
     */
    async setValueToDuplicateEmployee(data) {
      try {
        // nếu key khác EmployeeId, IsChecked, IsShowMenu thì thêm vào inputData thực hiện Duplicate
        for (const key in data) {
          if (Object.hasOwnProperty.call(data, key)) {
            if(key !== 'EmployeeId' && key !== 'IsCheck' && key !== 'IsChecked' && key !== 'IsShowMenu') {
              this.inputData[key] = data[key];
            }
          }
        }
        this.inputData = {...this.inputData, IsUpdate: false, IsDuplicate: true, IsCreate: false},
        // Lấy mã code mới  nhất về
        this.inputData.EmployeeCode = await getNewEmployeeCode();
        this.showPopup = true;
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    }
    ,
    
    /**
     * Hàm gọi api xóa nhân viên có id là id, xóa thành công sẽ mở toast và loading data
     * @param {*} id id của nhân viên cần xóa
     * Author: PMChien (25/01/2024)
     */
    async deleteEmployee(id) {
      try {
        let res = await deleteEmployeeById(id);
        if(this.handleResponse(res)) {
          this.toast.message = this.$MResource["VN"].DeleteEmployeeSuccess;
          this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
          this.handleLoadingData("");
          this.openToast();
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Hàm gọi api xóa nhiều nhân viên có id thuộc mảng ids, xóa thành công mở toast và loading data
     * @param {Array} ids mảng các id của nhân viên cần xóa
     * Author: PMChien(26/01/2023)
     */
    async deleteEmployeeAny(ids) {
      try {
        if(Array.isArray(ids) && ids.length > 0) {
          let data = { ids: ids };
          let res = await deleteEmployee(data);
          if(this.handleResponse(res)) {
            this.toast.message = this.$MResource["VN"].DeleteEmployeeSuccess;
            this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
            this.handleLoadingData("");
            this.openToast();
          }
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi tại deleteEmployeeAny: ", error);
      }
    }
    ,
    //#endregion

    /**
     * Xử lí respose từ axios trả về
     * Author: PMChien
     */
    handleResponse(res) {
      try {
        if(Number.isInteger(res)) {
          return true;
        }
        else {
          let status = res.status;
          let message = "";
          if(res.userMsg) {
            message = res.userMsg
          }
          switch (status) {
            case 200:
            case 201:
              return true;
              break;
            case 400:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.openToast();
              break;
            case 403:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.openToast();
              break;
            case 404:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.openToast();
              break;
            case 500:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.openToast();
              break;
            default:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = this.$MResource["VN"].Error;
              this.openToast();
              break;
          }
        }
        
        return false;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  },
};
</script>

<style scoped>
@import url("../../css/view/employee/employee-list.css");
</style>
