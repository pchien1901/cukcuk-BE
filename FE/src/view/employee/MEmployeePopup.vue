<template>
  <div class="overlay">
    <div class="popup">
      <div class="popup-header">
        <div class="popup-header__left">
          <div class="title">Thông tin nhân viên</div>
          <div class="checkbox-wrapper">
            <MCheckbox v-model="formData.isCustomer" :value="formData.isCustomer" /> <span>Là khách hàng</span>
          </div>
          <div class="checkbox-wrapper">
            <MCheckbox v-model="formData.isSupplier" :value="formData.isSupplier" /> <span>Là nhà cung cấp</span>
          </div>
        </div>
        <div class="popup-header__right">
          <MIcon
            :class="'icon-question-circle-popup'"
            :tooltip="'Hướng dẫn'"
            :tooltipPosition="'top'"
          />
          <MIcon
            :class="'icon-cancel-popup'"
            :tooltip="'Thoát'"
            :tooltipPosition="'top'"
            @click="closePopup"
          />
        </div>
      </div>
      <div class="popup-body">
        <div class="group-1">
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'Mã'"
              :isRequired="true"
              :class="'width-1 mr'"
              :error="formError.employeeCode"
              v-model="formData.employeeCode"
            />
            <MInput
              :type="'text'"
              :label="'Tên'"
              :isRequired="true"
              :class="'width-2'"
              :error="formError.fullName"
              v-model="formData.fullName"
            />
          </div>
          <div class="popup-control">
            <MCombobox
              :label="'Đơn vị'"
              :isRequired="true"
              :items="departments"
              v-model="formData.departmentId"
              :iconAwsClass="'fas fa-caret-down'"
              v-model:error="formError.department"
            />
          </div>
          <div class="popup-control">
            <MCombobox 
              :label="'Chức danh'" 
              :items="positions" 
              v-model="formData.positionId" 
              :iconAwsClass="'fas fa-caret-down'"
            />
          </div>
        </div>

        <div class="group-2">
          <div class="row popup-control">
            <MDatePicker :label="'Ngày sinh'" :class="'width-1 mr'" 
            v-model="formData.dateOfBirth" :error="formError.dateOfBirth"/>
            <MRadioInput
              :inputClass="'m-width-2'"
              :label="'Giới tính'"
              :options="gender"
              v-model="formData.gender"
            />
          </div>
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'Số CMTND'"
              :title="'Số Chứng minh thư nhân dân'"
              :class="'width-2 mr'"
              :error="formError.identityNumber"
              v-model="formData.identityNumber"
              :value="formData.identityNumber"
            />
            <MDatePicker 
              :label="'Ngày cấp'" 
              :class="'width-1'" 
              :error="formError.identityDate" 
              v-model="formData.identityDate"
            />
          </div>
          <div class="popup-control">
            <MInput :type="'text'" :label="'Nơi cấp'" v-model="formData.identityPlace"
            :value="formData.identityPlace"
            />
          </div>
        </div>

        <div class="group-3">
          <div class="popup-control">
            <MInput :type="'text'" :label="'Địa chỉ'" v-model="formData.address" :value="formData.address"/>
          </div>
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'ĐT di động'"
              :title="'Điện thoại di động'"
              class="width-4 mr"
              :error="formError.mobilePhoneNumber"
              v-model="formData.mobilePhoneNumber"
              :value="formData.mobilePhoneNumber"
            />
            <MInput
              :type="'text'"
              :label="'ĐT cố định'"
              :title="'Số điện thoại cố định'"
              class="width-4 mr"
              :error="formError.landlinePhoneNumber"
              v-model="formData.landlinePhoneNumber"
              :value="formData.landlinePhoneNumber"
            />
            <MInput :type="'text'" :label="'Email'" class="width-4" :error="formError.email" v-model="formData.email"
            :value="formData.email"
            />
          </div>
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'Tài khoản ngân hàng'"
              class="width-4 mr"
              v-model="formData.bankAccount"
              :value="formData.bankAccount"
            />
            <MInput
              :type="'text'"
              :label="'Tên ngân hàng'"
              class="width-4 mr"
              v-model="formData.bankName"
              :value="formData.bankName"
            />
            <MInput :type="'text'" :label="'Chi nhánh'" class="width-4" v-model="formData.branch" :value="formData.branch"/>
          </div>
        </div>
      </div>
      <div class="popup-footer">
        <div class="popup-footer__left">
          <MButton :type="'second'" :text="'Hủy'" @click="closePopup" />
        </div>
        <div class="popup-footer__right row">
          <MButton :type="'second'" :text="'Cất'" class="mr" @click="handleSubmit" />
          <MButton :type="'primary'" :text="'Cất và Thêm'" @click="handleSubmitPro"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
/* eslint-disable */
import MIcon from "../../components/base/icon/MIcon.vue";
import { allDepartment } from "./total-department.js";
import { allPosition } from "./total-position.js";
import { removeBlankKey, createDateString } from '../../js/ulti/convert-data.js';
import { getNewEmployeeCode, checkEmployeeCodeBeforeCU } from "../../js/services/employee.js";
import { getPositionById } from "../../js/services/position.js";
import { getDepartmentById } from "../../js/services/department.js";

export default {
  name: "MEmployeePopup",
  components: { MIcon },
  props: {
    /**
     * inputData: employee để thực hiện update
     */
    inputData: {
      type: Object,
      default: () => {
        return {};
      }
    },
  },
  data() {
    /**
     * departments: danh sách department truyền vào combobox
     * positions: mảng position truyền vào combobox
     * gender: giá trị truyền vào combobox gender
     * dialogMessagee: lỗi hiển trị trên dialog
     * formData: dữ liệu form để gửi server
     * formError: lỗi của các trường
     * formMode: trạng thái của popup: ADD - THÊM, UPDATE - SỬA
     */
    return {
      departments: [],
      positions: [],
      // formMode: null,
      // giá trị truyền vào cho radio input gender
      gender: [
        { label: "Nam", value: 0 },
        { label: "Nữ", value: 1 },
        { label: "Khác", value: 2 },
      ],
      dialogMessage: [],
      formData: {
        employeeCode: null,
        fullName:null,
        departmentId: null,
        positionId: null,
        dateOfBirth: null,
        gender: null,
        identityNumber: null,
        identityDate: null,
        identityPlace: null,
        address: null,
        mobilePhoneNumber: null,
        landlinePhoneNumber: null,
        email: null,
        bankAccount: null,
        bankName: null,
        branch: null,
        isCustomer: null,
        isSupplier: null,
      },
      formError: {
        employeeCode: "",
        fullName:"",
        department: "",
        position: "",
        dateOfBirth: "",
        gender: "",
        identityNumber: "",
        identityDate: "",
        identityPlace: "",
        address: "",
        mobilePhoneNumber: "",
        landlinePhoneNumber: "",
        email: "",
        bankAccount: "",
        bankName: "",
        branch: "",
      }, 
      formMode: this.$MEnum.FormMode.ADD,
    };
  },
  computed: {
    // async formMode() {
    //   if (
    //     this.inputData.EmployeeId !== null &&
    //     this.inputData.EmployeeId !== undefined
    //   ) {
    //     return this.$MEnum.FormMode.UPDATE;
    //   } else {
    //     let newEmployeeCode = await getNewEmployeeCode();
    //     console.log("newEmployeeCode: ", newEmployeeCode);
    //     if(newEmployeeCode === this.inputData.EmployeeCode) {
    //       return this.$MEnum.FormMode.DUPLICATE;
    //     }
    //     else {
    //       return this.$MEnum.FormMode.ADD;
    //     }
    //   }
    // },
  },
  async created() {
    // tính toán formMode
    if (
        this.inputData.EmployeeId !== null &&
        this.inputData.EmployeeId !== undefined
      ) {
        this.formMode = this.$MEnum.FormMode.UPDATE;
      } else {
        let newEmployeeCode = await getNewEmployeeCode();
        console.log("newEmployeeCode: ", newEmployeeCode);
        if(newEmployeeCode === this.inputData.EmployeeCode) {
          this.formMode = this.$MEnum.FormMode.DUPLICATE;
        }
        else {
          this.formMode = this.$MEnum.FormMode.ADD;
        }
      }

    // tạo giá trị mặc định cho form
    if(this.formMode === this.$MEnum.FormMode.UPDATE || this.formMode === this.$MEnum.FormMode.DUPLICATE) {
      this.formData.isCustomer = this.inputData.IsCustomer;
      this.formData.isSupplier = this.inputData.IsSupplier;
      this.formData.employeeCode = this.inputData.EmployeeCode;
      this.formData.fullName = this.inputData.FullName;
      this.formData.departmentId = this.inputData.DepartmentId;
      this.formData.positionId = this.inputData.PositionId;
      this.formData.dateOfBirth = createDateString(this.inputData.DateOfBirth);
      this.formData.gender = this.inputData.Gender;
      this.formData.identityNumber = this.inputData.IdentityNumber;
      this.formData.identityDate = createDateString(this.inputData.IdentityDate);
      this.formData.identityPlace = this.inputData.IdentityPlace;
      this.formData.address = this.inputData.Address; 
      this.formData.mobilePhoneNumber = this.inputData.MobilePhoneNumber;
      this.formData.landlinePhoneNumber = this.inputData.LandlinePhoneNumber;
      this.formData.email = this.inputData.Email;
      this.formData.bankAccount = this.inputData.BankAccount;
      this.formData.bankName = this.inputData.BankName;
      this.formData.branch = this.inputData.Branch;
      if(this.formMode === this.$MEnum.FormMode.UPDATE) {
        this.formData.employeeId = this.inputData.EmployeeId;
      }
    }
    else if( this.formMode === this.$MEnum.FormMode.ADD)
    {
      // reset Form
      for (const key in this.formData) {
        if (Object.hasOwnProperty.call(this.formData, key)) {
          this.formData[key] = null;
        }
      }

      // lấy mã code mới
      debugger
      this.formData.employeeCode = await getNewEmployeeCode();
      console.log("new code: ", this.formData.employeeCode);
    }

    let departmentArr = [];
    let positionArr = [];
    // Thêm giá trị cho combobox Departments
    for (const department of allDepartment) {
      let item = {
        text: department.DepartmentName,
        value: department.DepartmentId,
      };
      departmentArr.push(item);
    }

    // Thêm giá trị cho combobox Postions
    for (const position of allPosition) {
      let item = {
        text: position.PositionName,
        value: position.PositionId,
      };
      positionArr.push(item);
    }
    this.departments = departmentArr;
    this.positions = positionArr;
  },
  watch: {
    // inputValue(newValue) {
    //   if(this.formMode === this.$MEnum.FormMode.UPDATE || this.formMode === this.$MEnum.FormMode.DUPLICATE) {
    //   this.formData.isCustomer = newValue.IsCustomer;
    //   this.formData.isSupplier = newValue.IsSupplier;
    //   this.formData.employeeCode = newValue.EmployeeCode;
    //   this.formData.fullName = newValue.FullName;
    //   this.formData.departmentId = newValue.DepartmentId;
    //   this.formData.positionId = newValue.PositionId;
    //   this.formData.dateOfBirth = createDateString(newValue.DateOfBirth);
    //   this.formData.gender = newValue.Gender;
    //   this.formData.identityNumber = newValue.IdentityNumber;
    //   this.formData.identityDate = createDateString(newValue.IdentityDate);
    //   this.formData.identityPlace = newValue.IdentityPlace;
    //   this.formData.address = newValue.Address; 
    //   this.formData.mobilePhoneNumber = newValue.MobilePhoneNumber;
    //   this.formData.landlinePhoneNumber = newValue.LandlinePhoneNumber;
    //   this.formData.email = newValue.Email;
    //   this.formData.bankAccount = newValue.BankAccount;
    //   this.formData.bankName = newValue.BankName;
    //   this.formData.branch = newValue.Branch;
    //   if(this.formMode === this.$MEnum.FormMode.UPDATE) {
    //     this.formData.employeeId = newValue.EmployeeId;
    //   }
    // }
    // }
  },
  mounted() {
    this.$tinyEmitter.on("resetEmployeeForm", this.resetForm);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("resetEmployeeForm");
  },
  methods: {
    /**
     * Gọi tinyEmitter đóng popup
     * Author: PMChien
     */
    closePopup() {
      try {
        this.resetForm();
        this.$tinyEmitter.emit("closePopup");
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    },
    /**
     * Hàm gọi khi button "Cất và thêm mới được gọi", thêm(sửa) vào database sau đó 
     * Author: PMChien
     */
    async handleSubmitPro() {
      try {
        if(this.validate()) {
          let isDuplicateNewCode = await this.checkNewCode();
          if(isDuplicateNewCode) {
            let msg = [`Mã nhân viên ${this.formData.employeeCode} đã tồn tại trong hệ thống`];
            this.$tinyEmitter.emit("openValidateDialog", msg);
          }
          else {
            this.emitData();
          }
        }
        else {
          console.log("Không hợp lệ", this.formError);
        }
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    },
    /**
     * Hàm thêm(sửa), reset và đóng popup khi nút cất được Click
     * Author: PMChien
     */
    async handleSubmit() {
      try {
        // Validate dữ liệu
        if(this.validate()) {
          let isDuplicateNewCode = await this.checkNewCode();
          if(isDuplicateNewCode) {
            let msg = [`Mã nhân viên ${this.formData.employeeCode} đã tồn tại trong hệ thống`];
            this.$tinyEmitter.emit("openValidateDialog", msg);
          }
          else {
            this.emitData();
            this.closePopup();
          }
        }
        else {
          console.log("Không hợp lệ", this.formError);
        }
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    },
    /**
     * Kiểm tra EmployeeCode mới có bị trùng không
     */
    async checkNewCode() {
      try {
        /**
           * Đối tượng khởi tạo để kiểm tra mã đã tồn tại chưa gồm
           * employeeId: id của employee cần check
           * employeeCode: Mã cần check,
           * isCreate: có phải đang trạng thái ADD không
           * isUpdate: có phải đang trạng tái UPDATE không
           */
           let DTOCheckCode = {
              employeeId: this.formData.employeeId ? this.formData.employeeId : null,
              employeeCode: this.formData.employeeCode,
              isCreate: this.formMode === this.$MEnum.FormMode.ADD,
              isUpdate: this.formMode === this.$MEnum.FormMode.UPDATE
            };
            let checkEmployeeCode = await checkEmployeeCodeBeforeCU(DTOCheckCode);
            return checkEmployeeCode;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * hàm gửi tinyEmitter createEmployee(Thêm) và updateEmployee(Sửa)
     * Author: PMChien
     */
    emitData() {
      try {
        let data = removeBlankKey(this.formData);
        if (this.formMode === this.$MEnum.FormMode.ADD || this.formMode === this.$MEnum.FormMode.DUPLICATE) {
          this.$tinyEmitter.emit("createEmployee", data);
        } else if (this.formMode === this.$MEnum.FormMode.UPDATE) {
          this.$tinyEmitter.emit("updateEmployee", data, this.formData.employeeId);
        }
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    }
    ,
    /**
     * Hàm Validate dữ liệu form
     * Author: PMChien
     */
    validate() {
      try {
        let isValid = true;
        let checkRequired = true;
        
        //reset lỗi
        for (const key in this.formError) {
          if (Object.hasOwnProperty.call(this.formError, key)) {
            this.formError[key] = "";
          }
        }
        this.dialogMessage = [];
        
        // validate EmployeeCode
        if(!this.formData.employeeCode) {
          this.formError.employeeCode = this.$MResource["VN"].FieldIsNotEmpty;
          this.dialogMessage.push(this.$MResource["VN"].EmployeeCodeIsNotEmpty);
          isValid = false;
          checkRequired = false;
        }

        // validate fullname
        if(!this.formData.fullName) {
          this.formError.fullName = this.$MResource["VN"].FieldIsNotEmpty;
          this.dialogMessage.push(this.$MResource["VN"].FullNameIsNotEmpty);
          isValid = false;
          checkRequired = false;
        }

        // departmentId
        if(!this.formData.departmentId) {
          this.formError.department = this.$MResource["VN"].FieldIsNotEmpty;
          this.dialogMessage.push(this.$MResource["VN"].DepartmentIdIsNotEmpty);
          isValid = false;
          checkRequired = false;
        }

        // dateOfBirth
        if(this.formData.dateOfBirth) {
          let today = new Date();
          let dob = new Date(this.formData.dateOfBirth);
          if(
            dob > today ||
            (dob.getFullYear() === today.getFullYear() &&
              dob.getMonth() === today.getMonth() &&
              dob.getDate() === today.getDate())
          ) {
            this.formError.dateOfBirth = this.$MResource["VN"].DoBInvalid;
            isValid = false;
          }
        }

        // identityNumber 
        if(this.formData.identityNumber) {
         let regexIdentity =  /^\d{9}$/;
         if(!regexIdentity.test(this.formData.identityNumber)) {
          this.formError.identityNumber = this.$MResource["VN"].MemberCardCodeIsNotFormat;
          isValid = false;
         }
        }

        // identityDate
        if(this.formData.identityDate) {
          let today = new Date();
          let identityDateCheck = new Date(this.formData.identityDate);
          if(identityDateCheck > today) {
            this.formError.identityDate = this.$MResource["VN"].DoRInvalid;
            isValid = false
          }
        }

        // mobilePhoneNumber
        if(this.formData.mobilePhoneNumber) {
          let phoneRegex = /^[^a-zA-Z]*$/;
          if(!phoneRegex.test(this.formData.mobilePhoneNumber)) {
            this.formError.mobilePhoneNumber = this.$MResource["VN"].PhoneNumberIsNotFormat;
            isValid = false;
          }
        }

        // landlinePhoneNumber
        if(this.formData.landlinePhoneNumber) {
          let phoneRegex = /^[^a-zA-Z]*$/;
          if(!phoneRegex.test(this.formData.landlinePhoneNumber)) {
            this.formError.landlinePhoneNumber = this.$MResource["VN"].PhoneNumberIsNotFormat;
            isValid = false;
          }
        }

        // Email
        if(this.formData.email) {
          const regexEmail =
            /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/i;
          if (!regexEmail.test(this.formData.email)) {
            this.formError.email = this.$MResource["VN"].EmailIsNotFormat;
            isValid = false;
          }
        }

        // Emit sự kiện ẩn hiện Dialog thông báo validate thiếu
        if(!checkRequired) {
          console.log("emit openvalidateDialog từ popup");
          this.$tinyEmitter.emit("openValidateDialog", this.dialogMessage);
        }
        return isValid && checkRequired;
      } catch (error) {
        console.error("Có lỗi tại validate employee: ", error);
      }
    },
    /**
     * Reset form
     * Author: PMChien
     */
    async resetForm() {
      try {
        for (const key in this.formData) {
          if (Object.hasOwnProperty.call(this.formData, key)) {
            this.formData[key] = null;
          }
        }
        this.formData.employeeCode = await getNewEmployeeCode();
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  },
};
</script>

<style scoped>
@import url("../../css/view/employee/employee-popup.css");
</style>
