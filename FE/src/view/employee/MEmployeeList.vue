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
      <div class="header">
        <div class="header-left">
          <!-- <div v-if="showMultiAction" class="dropdown" @click="toogleMultiActionContextMenu">
            <div class="dropdown-text">Thực hiện hàng loạt </div>
            <i class="fas fa-caret-down"></i>
            <div v-if="showContextMenuMultiAction" class="context-menu-wrapper">
              <div class="context-menu">
                <div class="delete-multi">Xóa</div>
                <div class="unSelected">Bỏ chọn</div>
              </div>
            </div>
          </div> -->
          <div v-if="showMultiAction" class="multi-action">
            <div class="multi-action__text">Đã chọn: {{ employeeToActionMulti.length }}</div>
            <MButton :type="'second'" :text="'Bỏ chọn'" :class="'multi-action__cancel'" />
            <MButton :type="'primary-icon'" :text="'Xóa tất cả'" :class="'multi-action__delete'" :iconClass="'icon-trash-bin-red'"/>
          </div>
        </div>
        <div class="header-right">
          <MInput :type="'text'" :placeholder="'Tìm theo mã, tên nhân viên'" />
          <MButton
            :type="'icon'"
            :iconClass="'icon-reload'"
            title="Tải lại"
            tooltip="Tải lại"
            tooltipPosition="bottom"
            :class="'button-center'"
            @click="handleLoadingData"
          />
          <MButton
            :type="'icon'"
            :iconClass="'icon-excel-svg'"
            title="Nhập"
            tooltip="Nhập"
            tooltipPosition="bottom"
          />
        </div>
      </div>

      <div class="employee-table">
        <MEmployeeTable :items="employees" v-model:chosenItems="employeeToActionMulti"/>
      </div>
    </div>

    <!-- POPUP -->
    <MEmployeePopup v-if="showPopup" :inputData="inputData" />

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
// import { allEmployees } from "./total-employee-data.js";
// import { allPosition } from "./total-position.js";
// import { allDepartment } from "./total-department.js";
import {
  getAllEmployees,
  getEmployeeById,
  createEmployee,
  updateEmployee,
  deleteEmployee,
  deleteEmployeeById,
  getNewEmployeeCode,
} from "../../js/services/employee.js";
import { getAllDepartments, getDepartmentById } from '../../js/services/department.js';
import { getAllPositions, getPositionById } from '../../js/services/position.js';
import { allEmployees } from './total-employee-data';
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
        title: "",
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
      inputData: {},
      isShowLoading: true,
    };
  },
  async created() {
    this.isShowLoading = true;
    this.loadData();
    setTimeout(() => { this.isShowLoading = false}, 2000);
  },
  mounted() {
    // Khai báo Emitter
    this.$tinyEmitter.on("closePopup", this.closePopup);
    this.$tinyEmitter.on("openValidateDialog", this.openValidateDialog);

    // Emit CRUD
    this.$tinyEmitter.on("createEmployee", this.createEmployee);
    this.$tinyEmitter.on("duplicateEmployee", this.setValueToDuplicateEmployee);
    this.$tinyEmitter.on("setValueToUpdateEmployee", this.setValueToUpdateEmployee)
    this.$tinyEmitter.on("updateEmployee", this.updateEmployee);
    this.$tinyEmitter.on("deleteEmployee", this.openDeleteDialog);
    this.$tinyEmitter.on("setValueToDeleteEmployeeAny", this.showMultiActionBtn);
  },
  beforeUnmount() {
    // Hủy Emitter
    this.$tinyEmitter.off("closePopup");
    this.$tinyEmitter.off("openValidateDialog");

    this.$tinyEmitter.off("createEmployee");
    this.$tinyEmitter.off("duplicateEmployee");
    this.$tinyEmitter.off("updateEmployee");
    this.$tinyEmitter.off("deleteEmployee");
    this.$tinyEmitter.off("setValueToDeleteEmployeeAny");
  },
  watch: {
    employeeToActionMulti(newValue) {
      if(newValue.length > 0) {
        this.showMultiAction = true;
      }
      else {
        this.showMultiAction = false;
        this.showContextMenuMultiAction = false;
      }
    }
  },
  methods: {
    /**======================= ĐÓNG MỞ CÁC THÀNH PHẦN ================================= */
    //#region  Đóng mở các thành phần
    /**
     * Hiện Nút hành động hàng loạt
     * Author: PMChien(26/01/2024)
     */
    showMultiActionBtn(selectedItems) {
      try {
        this.employeeToActionMulti = selectedItems;
        if(this.employeeToActionMulti.length > 0) {
          this.showMultiAction = true;
        }
        else {
          this.showMultiAction = false;
          this.showContextMenuMultiAction = false;
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Ẩn/ hiện context menu hành động hàng loạt
     * Author: PMChien (26/01/2024)
     */
    toogleMultiActionContextMenu() {
      try {
        this.showContextMenuMultiAction = !this.showContextMenuMultiAction;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Mở popup
     * Author: PMChien
     */
    openPopup() {
      try {
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
     * Mở validate dialog
     * @param {*} message danh sách lỗi validte
     * Author: PMChien
     */
    openValidateDialog(message) {
      try {
        console.log("openValidateDialog tại list");
        if (message) {
          this.dialog.text = message;
          this.dialog.mode = this.$MEnum.DialogMode.WARNING;
          this.dialog.type = this.$MEnum.DialogType.ERROR;
          this.dialog.primaryBtnText = "Đóng";
          this.dialog.primaryAction = () => {this.closeDialog()};
          this.showDialog = true;
        }
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    },
    /**
     * Mở dialog xác nhận xóa 1 nhân viên, tạo các giá trị cho Dialog
     * @param {object} employee employee cần được xóa
     */
     openDeleteDialog(employee) {
      try {
        if(employee) {
          this.dialog.text = [`Bạn có chắc chắn muốn xóa nhân viên ${employee.EmployeeCode} không?`];
          this.dialog.mode = this.$MEnum.DialogMode.DELETE;
          this.dialog.type = this.$MEnum.DialogType.WARNING;
          this.dialog.primaryBtnText = "Có";
          this.dialog.cancelBtnText = "Không";
          this.dialog.primaryAction = () => { this.deleteEmployee(employee.EmployeeId) };
          this.showDialog = true;
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi tại openDeleteDialog: ", error);
      }
    },
    /**
     * Mở dialog xác nhận xóa nhiều nhân viên, tạo các giá trị cho Dialog
     * @param {Array} ids Mảng các id của nhân viên cần xóa
     */
    openDeleteAnyDialog(ids) {
      try {
        if(Array.isArray(ids) && ids.length > 0) {
          this.dialog.text = [`Bạn có thực sự muốn xóa những nhân viên đã chọn không?`];
          this.dialog.mode = this.$MEnum.DialogMode.DELETE;
          this.dialog.type = this.$MEnum.DialogType.WARNING;
          this.dialog.primaryBtnText = "Có";
          this.dialog.cancelBtnText = "Không";
          this.dialog.primaryAction = () => { this.deleteEmployeeAny(ids) };
          this.showDialog = true;
        }
      } catch (error) {
        console.error("Có lỗi tại openDeleteAnyDialog: ", error);
      }
    },
    /**
     * Đóng Dialog
     * Author: PMChien
     */
    closeDialog() {
      try {
        this.showDialog = false;
      } catch (error) {
        console.error("Đã có lỗi: ", error);
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
    /*================= */
    /**
     * Mở Popup để thực hiện tạo nhân viên mới, reset lại inputData
     * Author: PMChien
     */
    openPopupToCreate(){
      try {
        this.inputData = {};
        this.openPopup();
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
    async loadData() {
      try {
        // gọi api
        let allEmployees = await getAllEmployees();
        let allDepartment = await getAllDepartments();
        let allPosition = await getAllPositions();
          this.employees = allEmployees;

          /** 
           * thêm trường:
           * DepartmentName: Tên đơn vị 
           * PositionName: Tên vị trí
           * IsChecked: đánh dấu checkbox được chọn
           * IsShowMenu: show menu context
           * */ 
          for (const employee of this.employees) {
            for (const department of allDepartment) {
              if (employee.DepartmentId === department.DepartmentId) {
                employee.DepartmentName = department.DepartmentName;
              }
            }

            for (const position of allPosition) {
              if (employee.PositionId === position.PositionId) {
                employee.PositionName = position.PositionName;
              }
            }

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
    handleLoadingData() {
      try {
        // Tạo hiệu ứng loading
        this.isShowLoading = true;
        // LoadData
        this.loadData();
        // Tắt hiệu ứng loading
        setTimeout(() => { this.isShowLoading = false }, 2000);
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },
    //#endregion

    /*====================================================CRUD================================================= */
    //#region CRUD
    /**
     * Gọi Api create
     * Author: PMChien
     */
    async createEmployee(employee) {
      try {
        let res = await createEmployee(employee);
        if(this.handleResponse(res)) {
          this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
          this.toast.message = this.$MResource["VN"].AddEmployeeSuccess;
          this.$tinyEmitter.emit("resetEmployeeForm");
          this.openToast();
          this.loadData();
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
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
          this.inputData = employee;
          this.openPopup();
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
        // Lấy mã code mới  nhất về
        this.inputData.EmployeeCode = await getNewEmployeeCode();
        this.openPopup();
      } catch (error) {
        console.log("Đã có lỗi xảy ra: ", error);
      }
    }
    ,
    /**
     * Hàm gọi api updateEmployee
     * @param {*} data employee cần update
     * @param {*} id id của employee đó
     * Author: PMChien
     */
    async updateEmployee(data, id) {
      try {
        let res = await updateEmployee(data, id);
        let status = res.status;
        if(this.handleResponse(res)) {
          this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
          this.toast.message = this.$MResource["VN"].EditEmployeeSuccess;
          this.$tinyEmitter.emit("resetEmployeeForm");
          this.openToast();
          this.handleLoadingData();
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
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
          this.handleLoadingData();
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
          let res = await deleteEmployee(ids);
          if(this.handleResponse(res)) {
            this.toast.message = this.$MResource["VN"].DeleteEmployeeSuccess;
            this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
            this.openToast();
            this.handleLoadingData();
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
        let status = res.status;
        let message = "";
        console.log(res);
        console.log("response status: ", status);
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
            this.toast.message = "Đã xảy ra lỗi";
            this.openToast();
            break;
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
