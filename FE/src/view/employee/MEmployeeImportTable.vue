<template>
  <div class="table-wrapper">
    <div class="table-container">
      <table>
        <thead>
          <th>Mã nhân viên</th>
          <th>Tên nhân viên</th>
          <th>Chức danh</th>
          <th>Tên đơn vị</th>
          <th>Số CMTND</th>
          <th>Nơi cấp</th>
          <th>Ngày cấp</th>
          <th>Ngày sinh</th>
          <th>Giới tính</th>
          <th>Quốc tịch</th>
          <th>Dân tộc</th>
          <th>Số điện thoại</th>
          <th>Email</th>
          <th>Số hợp đồng lao động</th>
          <th>Loại hợp đồng lao động</th>
          <th>Mức lương cơ bản</th>
          <th>Nơi ở hiện tại</th>
          <th>Tình trạng</th>
        </thead>
        <tbody>
          <tr
            v-for="(employee, index) in employees"
            :key="index"
          >
            <td>{{ cellValue(employee.EmployeeCode) }}</td>
            <td>{{ cellValue(employee.FullName) }}</td>
            <td>{{ cellValue(employee.PositionName) }}</td>
            <td>{{ cellValue(employee.DepartmentName) }}</td>
            <td>{{ cellValue(employee.IdentityNumber) }}</td>
            <td>{{ cellValue(employee.IdentityPlace) }}</td>
            <td>{{ cellValue(employee.IdentityDateString) }}</td>
            <td>{{ cellValue(employee.DateOfBirthString) }}</td>
            <td>{{ cellValue(employee.GenderString) }}</td>
            <td>{{ cellValue(employee.Nationality) }}</td>
            <td>{{ cellValue(employee.Ethnicity) }}</td>
            <td>{{ cellValue(employee.MobilePhoneNumber) }}</td>
            <td>{{ cellValue(employee.Email) }}</td>
            <td>{{ cellValue(employee.ContractNumber) }}</td>
            <td>{{ cellValue(employee.ContractType) }}</td>
            <td :style="{ 'text-align' : 'right'}">{{ formatNumber(employee.Salary) }}</td>
            <td>{{ cellValue(employee.Address) }}</td>
            <td>
              <div v-if="employee.CanImported === true">Hợp lệ</div>
              <div v-if="!employee.CanImported">
                <ul>
                  <li v-for="(msg, index) in employee.ImportInvalidErrors" :key="index" class="import-errors">{{ msg }}</li>
                </ul>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="table-footer">
      <div class="pagination-footer">
      <div class="total-records">Tổng: <b>{{ this.employees.length }}</b></div>
      <div class="page-number">Trang số: {{ this.CurrentPage }}/{{ this.totalPage }}</div>
      <div class="pagination">
        <label>Số bản ghi/trang</label>
        <MDropdown :items="pagingItem" position="top" v-model="this.pagination.PageSize"/>
        <div class="lists-records"><b>{{this.startIndex}}</b> - <b>{{ this.endIndex }}</b> bản ghi</div>
        <div class="icon-pagination">
          <div class="pageBefore"  :class="{ 'icon-pagination--disabled': this.CurrentPage === 1 }"
          @click="goToPreviousPage"
          ><MIcon :iconAwsClass="'fas fa-chevron-left'" :tooltip="'Trang trước'" :tooltipPosition="'top'"/></div>
          <div class="pageAfter"   :class="{ 'icon-pagination--disabled': this.CurrentPage === this.totalPage }"
          @click="goToTheNextPage"
          ><MIcon :iconAwsClass="'fas fa-chevron-right'" :tooltip="'Trang sau'" :tooltipPosition="'top'"/></div>  
        </div>
      </div>
    </div>
    </div>
  </div>
</template>

<script>
/* eslint-disable */
import { employeeFile } from './test.js';
import { formatNumberWithCommas } from '@/js/ulti/convert-data';
export default {
  name : "MEmployeeImportTable",
  props: {
    items: { type: Array, default: () => []},
  },
  data() {
    return {
      employees: [],
      CurrentPage: 1,
      totalPage: 1,
      pagination: {
        // CurrentPage: 1,
        PageSize: 10,
      },
      pagingItem: [
        {value: 10, text: 10},
        {value: 25, text: 25},
        {value: 50, text: 50},
        {value: 100, text: 100},
      ],
    };
  },
  created() {
    this.employees = this.items.slice(0, this.pagination.PageSize);
    this.totalPage = Math.ceil(this.items.length / this.pagination.PageSize);
  },
  computed: {
    startIndex() {
      return (this.CurrentPage - 1)*this.pagination.PageSize + 1;
    },
    endIndex() {
      if(this.CurrentPage === this.totalPage) {
        return  this.items.length;
      }
      else {
        return (this.CurrentPage - 1)*this.pagination.PageSize + this.pagination.PageSize;
      }
    }
  },
  watch: {
    pagination: {
      handler() {
        this.CurrentPage = 1;
        this.totalPage = Math.ceil(this.items.length / this.pagination.PageSize);
        let tempItems = [];
        let startIndex = (this.CurrentPage - 1)*this.pagination.PageSize;
        let endIndex = startIndex + this.pagination.PageSize;
        if(endIndex >= this.items.length) {
          endIndex = this.items.length
        }
        for(let i = startIndex; i < endIndex; i++) {
          tempItems.push(this.items[i]);
        }
        this.employees = tempItems;
        //console.log("startIndex: ", startIndex, "endIndex: ", endIndex, "tempItems: ", tempItems, "employees: ", this.employees);
      },
      deep: true
    },
    CurrentPage() {
      let tempItems = [];
        let startIndex = (this.CurrentPage - 1)*this.pagination.PageSize;
        let endIndex = startIndex + this.pagination.PageSize;
        if(endIndex >= this.items.length) {
          endIndex = this.items.length
        }
        for(let i = startIndex; i < endIndex; i++) {
          tempItems.push(this.items[i]);
        }
        this.employees = tempItems;
        //console.log("startIndex: ", startIndex, "endIndex: ", endIndex, "tempItems: ", tempItems, "employees: ", this.employees);
    }
  },
  methods: {
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

    /**
     * Chuyển tới trang trước
     * Author: PMChien
     */
     goToPreviousPage() {
      try {
        if(this.CurrentPage > 1) {
          this.CurrentPage -= 1;
          //this.employees = this.items.slice((this.CurrentPage - 1)*this.pagination.PageSize, this.pagination.PageSize);
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
          // this.employees = this.items.slice((this.CurrentPage - 1)*this.pagination.PageSize, this.pagination.PageSize);
        }
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },

    /**
     * format lại tiền tệ
     * @param {*} number chuỗi tiền tệ cần format
     * Author: PMChien
     */
    formatNumber(number) {
      try {
        let result = formatNumberWithCommas(number);
        return result;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  }
}
</script>

<style scoped>
@import url('../../css/view/employee/employee-import-table.css');
</style>