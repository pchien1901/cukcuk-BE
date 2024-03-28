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
        <MInput :type="'text'" :placeholder="'Tìm theo mã, tên nhân viên'" v-model="searchText" :iconClass="'icon-search'"
        @handleEnter="handleSearchEnter" :title="this.$MResource['VN'].SearchEmployeeTitle"
        @input="handleSearchInput"
        />
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
          :iconClass="'icon-export-excel'"
          title="Xuất file"
          tooltip="Xuất file"
          tooltipPosition="bottom"
          :class="'button-export-excel'"
          @click="exportExcelFile"
        />

        <MButton
          :type="'icon'"
          :iconClass="'icon-excel-svg'"
          title="Nhập"
          tooltip="Nhập"
          tooltipPosition="bottom"
          @click="goToImportPage"
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
          <th title="Số Chứng minh thư nhân dân">Số CMTND</th>
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
            <div class="action-btns-wrapper">
              <div class="action-btns">
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
              </div>
            </div>
            
          </tr>
        </tbody>
      </table>
    </div>
    <div class="table-footer">
      <div class="pagination-footer">
      <div class="total-records">Tổng: <b>{{ totalRecord }}</b></div>
      <div class="pagination">
        <label>Số bản ghi/trang</label>
        <MDropdown :items="pagingItem" position="top" v-model="this.PageSize"/>
        <div class="lists-records"><b>{{this.startIndex}}</b> - <b>{{ this.endIndex }}</b> bản ghi</div>
        <div class="icon-pagination">
          <div class="pageBefore" @click="goToPreviousPage" :class="{ 'icon-pagination--disabled': this.CurrentPage === 1 }"
          ><MIcon :iconAwsClass="'fas fa-chevron-left'" :tooltip="'Trang trước'" :tooltipPosition="'top'"/></div>
          <div class="pageAfter"  @click="goToTheNextPage" :class="{ 'icon-pagination--disabled': this.CurrentPage === this.totalPage }"
          ><MIcon :iconAwsClass="'fas fa-chevron-right'" :tooltip="'Trang sau'" :tooltipPosition="'top'"/></div>  
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
import { debounce } from "lodash";
import { checkAuthentication } from "@/js/services/token.js";
import {
  convertDateFormat,
  getGenderLabel,
} from "../../js/ulti/convert-data.js";
import { baseEmit, baseEmitWithParam } from '../../js/ulti/emit.js';
import { exportFile, exportAllEmployees } from "@/js/services/employee.js";

export default {
  name: "EmployeeTable",
  props: {
    /**
     * items: danh sách dữ liệu để render
     * inputData: thông tin Employee truyền vào để thực hiện update
     * totalPage: tổng số trang
     */
    items: { type: Array, default: () => [] },
    /**dữ liệu để update */
    inputData: {
      type: Object,
      default: () => {
        return {};
      },
    },
    totalPage: Number,
    totalRecord: Number,
  },
  // async created() {
  //   this.employees = this.items;
  //   let totalEmployees = await getEmployeeInfo();
  //   this.totalRecords = totalEmployees.length;
  // },
  updated() {
    this.employees = this.items;
  },
  data() {
    return {
      /**
       * employees: mảng employee hiển thị
       * selectAll: Biến quản lí trạng thái của checkbox selectAll
       * selectedItems: mảng các Id đã được chọn để xóa
       * searchText: giá trị tại ô tìm kiếm theo mã, tên nhân viên
       * showMultiAction: biến boolen ẩn/ hiện multi action
       * dialog: biến object quản lí dialog khi người dùng xóa
       *  + showDeleteDialog: ẩn/hiện dialog
       *  + type: Loại icon/ icon delete
       *  + mode: kiểu dialog - kiểu DELETE có 2 nút 
       *  + title: Tiêu đề dialog
       *  + text: nội dung dialog - kiểu mảng
       *  + primaryAction: hành động của nút chính trong dialog
       * pagination: biến object quản lí pagination
       *  + CurrentPage: trang hiện tại
       *  + PageSize: số lượng bản ghi / trang
       *  + startIndex: số thứ tự bản ghi bắt đầu = (CurrentPage - 1)*PageSize + 1
       */
      employees: this.items,
      selectAll: false,
      selectedItems: [],
      searchText: "",
      showMultiAction: false,
      dialog: {
        showDeleteDialog: false,
        type: this.$MEnum.DialogType.WARNING,
        mode: this.$MEnum.DialogMode.DELETE,
        title: this.$MResource["VN"].DeleteEmployeeTitle,
        text: [this.$MResource["VN"].DeleteEmployeeMessage],
        primaryAction: null,
      },
      // pagination: {
      //   CurrentPage: 1,
      //   PageSize: 10,
      // },
      CurrentPage: 1,
      PageSize: 10,
      pagingItem: [
        {value: 10, text: 10},
        {value: 25, text: 25},
        {value: 50, text: 50},
        {value: 100, text: 100},
      ]
    };
  },
  computed: {
    startIndex() {
      return (this.CurrentPage - 1)*this.PageSize + 1;
    },
    endIndex() {
      if(this.CurrentPage === this.totalPage) {
        return  (this.CurrentPage - 1)*this.PageSize + this.items.length;
      }
      else {
        return (this.CurrentPage - 1)*this.PageSize + this.PageSize;
      }
    }
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
    // pagination: {
    //   handler(newValue, oldValue) {
    //     console.log("newValue.PageSize: ", newValue.PageSize, "oldValue.PageSize: ", oldValue.PageSize);
    //     if(newValue.PageSize !== oldValue.PageSize) {
    //       console.log("Có vấn đề");
    //       this.CurrentPage = 1;
    //       this.emitLoadData();
    //     }
    //     else {
    //       console.log("Không có vấn đề gì cả");
    //       this.emitLoadData();
    //     }
    //   },
    //   deep: true,
    // },
    CurrentPage() {
      this.emitLoadData();
    },
    PageSize(newValue, oldValue) {
      if(newValue !== oldValue) {
        console.log("Trở về trang số 1");
        this.CurrentPage = 1;
        this.emitLoadData();
      }
      else {
        console.log("Tiếp tục");
        this.emitLoadData();
      }
    },
    // async items() {
    //   this.employees = this.items;
    //   let totalEmployees = await getEmployeeInfo();
    //   this.totalRecords = totalEmployees.length;
    // }
  },
  methods: {
    //#region  Emit : emit các sự kiện loadData, update, duplicate
    /**
     * Gọi Emit để load lại dữ liệu
     * Author: PMChien
     */
    emitLoadData() {
      try {
        //this.$tinyEmitter.emit("loadData", this.CurrentPage, this.PageSize, this.searchText.trim());
        if(this.searchText) {
          this.CurrentPage = 1;
          this.PageSize = 10;
          this.$tinyEmitter.emit("loadData", this.CurrentPage, this.PageSize, this.searchText.trim());
        }
        else {
          this.$tinyEmitter.emit("loadData", this.CurrentPage, this.PageSize, this.searchText.trim());
        }
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
     * Gọi khi thanh tìm kiếm nhấn enter, gọi hàm emit và xóa dữ liệu tìm kiếm
     * Author: PMChien
     */
    handleSearchEnter() {
      try {
        this.emitLoadData();
        this.searchText = "";
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Gọi debounce tìm kiếm khi người dùng nhập
     * Author: PMChien
     */
    handleSearchInput: debounce(function(){
      this.emitLoadData();
    }, 1500)
    ,
    //#endregion

    //#region Import / Export file
    /**
     * Chuyển đến trang import Employee khi ấn button Nhập
     * Author: PMChien
     */
    goToImportPage() {
      try {
        this.$router.push('/nhan-vien/nhap-khau');
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    async exportExcelFile() {
      try {
        await checkAuthentication();
        //const res = await exportFile(this.CurrentPage, this.PageSize, this.searchText);
        const res = await exportAllEmployees();
        let blob = new Blob([res.data], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        });

        const fileName = this.$MResource["VN"].File.EmployeeFile;
        // file-saver download file
        this.$saveAs(blob, fileName);
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
        this.selectedItems = [];
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

    //#region Phân trang
    /**
     * Chuyển tới trang trước
     * Author: PMChien
     */
    goToPreviousPage() {
      try {
        if(this.CurrentPage > 1) {
          this.CurrentPage -= 1;
        }
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },

    /**
     * Đi tới trang sau
     * Author: PMChien
     */
    goToTheNextPage() {
      try {
        if(this.CurrentPage < this.totalPage) {
          this.CurrentPage += 1;
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
