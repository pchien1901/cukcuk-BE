<template>
  <div class="table-wrapper">
    <div class="table-wrapper__header">
      <div class="header-left">
        <div v-if="showMultiAction" class="multi-action">
          <div class="multi-action__text">Đã chọn: {{ selectedItems.length }}</div>
          <MButton :type="'second'" :text="'Bỏ chọn'" :class="'multi-action__cancel'" @click="cancelSelectedItems"/>
          <MButton :type="'primary-icon'" :text="'Xóa tất cả'" :class="'multi-action__delete'" :iconClass="'icon-trash-bin-red'" @click="openDeleteDialogAny"/>
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
          @click="emitLoadData"
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
    <div class="table-container">
      <table>
        <thead>
          <th>
            <MCheckbox v-model="selectAll" />
          </th>
          <th>Mã nhân viên</th>
          <th>Tên nhân viên</th>
          <th>Giới tính</th>
          <th>Ngày sinh</th>
          <th>Số CMTND</th>
          <th>Chức danh</th>
          <th>Tên đơn vị</th>
          <th>Số tài khoản</th>
          <th>Tên ngân hàng</th>
          <th>Chi nhánh ngân hàng</th>
        </thead>
        <tbody>
          <tr
            v-for="employee in employees"
            :key="employee.EmployeeId"
            :class="{ checked: employee.IsChecked }"
          >
            <td>
              <MCheckbox v-model="employee.IsChecked" />
            </td>
            <td>{{ cellValue(employee.EmployeeCode) }}</td>
            <td>{{ cellValue(employee.FullName) }}</td>
            <td>{{ gender(employee.Gender) }}</td>
            <td>{{ dateOfBirth(employee.DateOfBirth) }}</td>
            <td>{{ cellValue(employee.IdentityNumber) }}</td>
            <td>{{ cellValue(employee.PositionName) }}</td>
            <td>{{ cellValue(employee.DepartmentName) }}</td>
            <td>{{ cellValue(employee.BankAccountNumber) }}</td>
            <td>{{ cellValue(employee.BankName) }}</td>
            <td>{{ cellValue(employee.Branch) }}</td>
            <td class="action-btns">
              <MButton type="icon" :iconClass="'icon-edit'" :class="'btn-action-table'" :tooltip="'Sửa'" :tooltipPosition="'top'"
              @click="() => emitSetValueToUpdateEmployee(employee.EmployeeId)"/>
              <div class="btn-context-menu">
                <MButton :type="'icon'" :iconClass="'icon-three-dot'" :class="'btn-action-table'" :tooltip="'Lựa chọn'" :tooltipPosition="'top'"
                    @click="
                      () => {
                        employee.IsShowMenu = !employee.IsShowMenu;
                      }
                    "
                />
                <div v-if="employee.IsShowMenu" class="context-menu">
                  <div
                    class="duplicate"
                    @click="() => this.emitDuplicate(employee)"
                  >
                    Nhân bản
                  </div>
                  <div
                    class="delete"
                    @click="() => this.openDeleteDialog(employee.EmployeeId)"
                  >
                    Xóa
                  </div>
                </div>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="table-footer">
      <div class="pagination-footer">
    <div class="total-records">Tổng: <b>{{ items.length }}</b></div>
    <div class="pagination">
      <label>Số bản ghi/trang</label>
      <select name="" id="" class="dropdown-records">
        <option value="10">10</option>
        <option value="25">25</option>
        <option value="50">50</option>
        <option value="100">100</option>
      </select>
      <!-- <div class="lists-records"><b>1</b> - <b>9</b> records</div> -->
      <div class="icon-pagination">
        <i class="fas fa-chevron-left"></i>
        <i class="fas fa-chevron-right"></i>
      </div>
    </div>
  </div>
    </div>
  </div>
  
  <MDialog v-if="this.dialog.showDeleteDialog" :type="this.dialog.type" :mode="this.dialog.mode"
  :title="this.dialog.title" :text="this.dialog.text"
  :primaryAction="this.dialog.primaryAction" 
  :closeFunction="closeDeleteDialog"
  />
</template>

<script>
/* eslint-disable */
// import { getAllEmployees } from "../../js/services/employee.js";
// import { getAllDepartments } from "../../js/services/department.js";
// import { getAllPositions } from "../../js/services/position.js";
import {
  convertDateFormat,
  getGenderLabel,
} from "../../js/ulti/convert-data.js";
import { baseEmit, baseEmitWithParam } from '../../js/ulti/emit.js';

export default {
  name: "MEmployeeTable",
  props: {
    /**
     * items: danh sách dữ liệu để render
     * inputData: thông tin Employee truyền vào để thực hiện update
     */
    items: { type: Array, default: () => [] },
    /**dữ liệu để update */
    inputData: {
      type: Object,
      default: () => {
        return {};
      },
    },
  },
  created() {
    this.employees = this.items;
  },
  updated() {
    this.employees = this.items;
  },
  data() {
    return {
      /**
       * employees: mảng employee hiển thị
       * selectAll: Biến quản lí trạng thái của checkbox selectAll
       * selectedItems: mảng các Id đã được chọn để xóa
       * showMultiAction: biến boolen ẩn/ hiện multi action
       * showDeleteDialog: ẩn / hiện dialog
       */
      employees: this.items,
      selectAll: false,
      selectedItems: [],
      primaryAction: null,
      showMultiAction: false,
      dialog: {
        showDeleteDialog: false,
        type: this.$MEnum.DialogType.WARNING,
        mode: this.$MEnum.DialogMode.DELETE,
        title: this.$MResource["VN"].DeleteEmployeeTitle,
        text: [this.$MResource["VN"].DeleteEmployeeMessage],
        primaryAction: null,
      }
    };
  },
  watch: {
    selectAll() {
      if (this.selectAll) {
        for (let employee of this.employees) {
          employee.IsChecked = true;
          this.selectedItems.push(employee.EmployeeId);
        }
      } else {
        for (let employee of this.employees) {
          employee.IsChecked = false;
        }
        this.selectedItems = [];
      }
    },
    employees: {
      handler() {
        this.selectedItems = this.employees
          .filter((employee) => employee.IsChecked === true)
          .map((employee) => employee.EmployeeId);
      },
      deep: true,
    },
    selectedItems() {
      try {
        if(this.selectedItems.length > 0) {
          this.showMultiAction = true;
        }
        else {
          this.showMultiAction = false;
          this.selectAll = false;
        }
      } catch (error) {
        console.error("Đã có lỗi : ", error);
      }
      
    },
  },
  methods: {
    //#region  Emit : emit các sự kiện loadData, update, duplicate
    /**
     * Gọi Emit để load lại dữ liệu
     * Author: PMChien
     */
    emitLoadData() {
      try {
        this.$tinyEmitter.emit("loadData");
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Emit để update Employee
     * @param {*} id id của Employee cần update
     * Author: PMChien
     */
    emitSetValueToUpdateEmployee(id) {
      try {
        console.log("id: ", id);
        this.$tinyEmitter.emit("setValueToUpdateEmployee", id);
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Emit "duplicateEmployee" để nhân bản
     * @param {*} data Dữ liệu cần nhân bản
     * Author: PMChien
     */
    emitDuplicate(data) {
      try {
        this.$tinyEmitter.emit("duplicateEmployee", data);
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    //#endregion

    //#region Dialog
    /**
     * Hàm đóng delete Dialog
     * Author: PMChien
     */
    closeDeleteDialog() {
      try {
        this.dialog.showDeleteDialog = false;
        console.log("Đóng dialog");
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Mở Dialog xác nhận xóa 1 nhân viên, truyền id vào để xóa 1 nhân viên
     * Author: PMChien
     */
    openDeleteDialog(id) {
      try {
        // set giá trị cho Dialog
        this.dialog.primaryAction = () => { baseEmitWithParam("deleteEmployee", id) };
        this.dialog.showDeleteDialog = true;
      } catch (error) {
        console.error("Đã xảy ra lỗi:  ", error);
      }
    },
    /**
     * Mở Dialog xác nhận xóa nhiều nhân viên, truyền mảng các id vào để xóa nhiều nhân viên
     */
    openDeleteDialogAny() {
      try {
        // tạo hành động emit xóa nhiều cho primary button
        this.dialog.primaryAction = () => { baseEmitWithParam("deleteEmployeeAny", this.selectedItems) };
        this.dialog.showDeleteDialog = true;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    //#endregion

    //#region MultiAction
    cancelSelectedItems() {
      try {
        for (let employee of this.employees) {
          if(employee.IsChecked) {
            employee.IsChecked = false;
          }
        }
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },
    //#endregion

    //#region Hàm bổ trợ
    /**
     * Kiểm tra EmployeeId có thuộc selectedItems không
     * @param {*} id
     * @returns Boolean
     * Author: Phạm Minh Chiến (12/12/2023)
     */
    isCheck(id) {
      let inSelectedItems = this.selectedItems.includes(id);
      return inSelectedItems;
    },

    /**
     * chuyển giới tính (enum) về string
     * @param {int} gender giới tính (dạng enum)
     * Author: PMChien
     */
    gender(gender) {
      try {
        let result = getGenderLabel(gender);
        return result;
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },

    /**
     * Chuyển trường DateOfBirth về dạng dd/mm/yyyy
     * @param {string} dob ngày sinh
     */
    dateOfBirth(dob) {
      try {
        let result = convertDateFormat(dob);
        return result;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * tạo giá trị mặc định cho ô là null
     * @param {string} text
     */
    cellValue(text) {
      try {
        if (text) {
          return text;
        } else {
          return "";
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    //#endregion
  },
};
</script>

<style scoped>
@import url("../../css/view/employee/employee-table.css");
</style>
