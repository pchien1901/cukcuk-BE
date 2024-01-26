<template>
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
        <th>Chức năng</th>
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
          <td>
            <div class="action-cell">
              <MButton
                :type="'link'"
                :text="'Sửa'"
                @click="
                  () => this.emitSetValueToUpdateEmployee(employee.EmployeeId)
                "
              />
              <div class="btn-context-menu">
                <MButton
                  :type="'icon'"
                  :iconClass="'icon-arrow-blue'"
                  @click="
                    () => {
                      employee.IsShowMenu = !employee.IsShowMenu;
                    }
                  "
                  :tooltip="'Lựa chọn'"
                  :tooltipPosition="'top'"
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
                    @click="() => this.emitSetValueToDeleteEmployee(employee)"
                  >
                    Xóa
                  </div>
                  <!-- <div class="stop">Ngừng sử dụng</div> -->
                </div>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
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
//import { allEmployees } from './total-employee-data.js';

export default {
  name: "MEmployeeTable",
  emits: ["update:chosenItems"],
  props: {
    /**
     * items: danh sách dữ liệu để render
     * inputData: thông tin Employee truyền vào để thực hiện update
     * chosenItems: prop lưu id của các phần tử đã chọn
     */
    items: { type: Array, default: () => [] },
    /**dữ liệu để update */
    inputData: {
      type: Object,
      default: () => {
        return {};
      },
    },
    chosenItems: { type: Array, default: []},
  },
  created() {
    this.employees = this.items;
    this.selectedItems = this.chosenItems;
    // let employeeRes = await getAllEmployees();
    // let departmentRes = await getAllDepartments();
    // let positionRes = await getAllPositions();
    // let allEmployees = employeeRes.data;
    // let allDepartment = departmentRes.data;
    // let allPosition = positionRes.data;

    // // console.log("employee: ", allEmployees);
    // console.log(allEmployees);
    // //Thêm PositionName và DepartmentName cho employees
    // this.employees = allEmployees;

    // for (const employee of this.employees) {
    //   for (const department of allDepartment) {
    //     if (employee.DepartmentId === department.DepartmentId) {
    //       employee.DepartmentName = department.DepartmentName;
    //     }
    //   }

    //   for (const position of allPosition) {
    //     if (employee.PositionId === position.PositionId) {
    //       employee.PositionName = position.PositionName;
    //     }
    //   }

    //   employee.IsChecked = false;
    //   employee.IsShowMenu = false;
    // }
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
       */
      employees: this.items,
      selectAll: false,
      selectedItems: [],
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
    selectedItems(newValue) {
      try {
        // console.log("selectedItems: ", this.selectedItems);
        // if (this.selectedItems.length > 0) {
        //   this.$tinyEmitter.emit("setValueToDeleteEmployeeAny", this.selectedItems);
        // }
        //this.$tinyEmitter.emit("setValueToDeleteEmployeeAny", this.selectedItems);
        this.$emit("update:chosenItems", newValue);
      } catch (error) {
        console.error("Đã có lỗi tại emit set giá trị xóa nhiều", error);
      }
      
    },
  },
  methods: {
    /**
     * Emit để update Employee
     * @param {*} id id của Employee cần update
     * Author: PMChien
     */
    emitSetValueToUpdateEmployee(id) {
      try {
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
    /**
     * Emit "deleteEmployee" để xóa
     * @param {*} data employee gửi đi để thực hiện xóa (cần có EmployeeId và EmployeeCode)
     * Author: PMChien
     */
    emitSetValueToDeleteEmployee(data) {
      try {
        console.log("Emit deleteEmployee tại hàng: ", data);
        this.$tinyEmitter.emit("deleteEmployee", data);
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Emit setValueToDeleteEmployeeAny để xóa nhiều bản ghi
     * Author: PMChien (25/01/2024)
     */
    emitDeleteEmployeeAny() {
      try {
        this.$tinyEmitter.emit("setValueToDeleteEmployeeAny", this.selectedItems);
      } catch (error) {
        console.error("Đã xảy ra lỗi :", error);
      }
    },
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
  },
};
</script>

<style scoped>
@import url("../../css/view/employee/employee-table.css");
</style>
