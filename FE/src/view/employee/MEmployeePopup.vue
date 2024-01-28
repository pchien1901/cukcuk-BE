<template>
  <div class="overlay">
    <div class="popup">
      <div class="popup-header">
        <div class="popup-header__left">
          <div class="title">Thông tin nhân viên</div>
          <div class="checkbox-wrapper">
            <MCheckbox v-model="formData.IsCustomer" :value="formData.IsCustomer" /> <span>Là khách hàng</span>
          </div>
          <div class="checkbox-wrapper">
            <MCheckbox v-model="formData.IsSupplier" :value="formData.IsSupplier" /> <span>Là nhà cung cấp</span>
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
            @click="closeFunction"
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
              :error="formError.EmployeeCode"
              v-model="formData.EmployeeCode"
            />
            <MInput
              :type="'text'"
              :label="'Tên'"
              :isRequired="true"
              :class="'width-2'"
              :error="formError.FullName"
              v-model="formData.FullName"
            />
          </div>
          <div class="popup-control">
            <MCombobox
              :label="'Đơn vị'"
              :isRequired="true"
              :items="departments"
              v-model="formData.DepartmentId"
              :iconAwsClass="'fas fa-caret-down'"
              v-model:error="formError.department"
            />
          </div>
          <div class="popup-control">
            <MCombobox 
              :label="'Chức danh'" 
              :items="positions" 
              v-model="formData.PositionId" 
              :iconAwsClass="'fas fa-caret-down'"
            />
          </div>
        </div>

        <div class="group-2">
          <div class="row popup-control">
            <MDatePicker :label="'Ngày sinh'" :class="'width-1 mr'" 
            v-model="formData.DateOfBirth" :error="formError.DateOfBirth"/>
            <MRadioInput
              :inputClass="'m-width-2'"
              :label="'Giới tính'"
              :options="Gender"
              v-model="formData.Gender"
            />
          </div>
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'Số CMTND'"
              :title="'Số Chứng minh thư nhân dân'"
              :class="'width-2 mr'"
              :error="formError.IdentityNumber"
              v-model="formData.IdentityNumber"
              :value="formData.IdentityNumber"
            />
            <MDatePicker 
              :label="'Ngày cấp'" 
              :class="'width-1'" 
              :error="formError.IdentityDate" 
              v-model="formData.IdentityDate"
            />
          </div>
          <div class="popup-control">
            <MInput :type="'text'" :label="'Nơi cấp'" v-model="formData.IdentityPlace"
            :value="formData.IdentityPlace"
            />
          </div>
        </div>

        <div class="group-3">
          <div class="popup-control">
            <MInput :type="'text'" :label="'Địa chỉ'" v-model="formData.Address" :value="formData.Address"/>
          </div>
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'ĐT di động'"
              :title="'Điện thoại di động'"
              class="width-4 mr"
              :error="formError.MobilePhoneNumber"
              v-model="formData.MobilePhoneNumber"
              :value="formData.MobilePhoneNumber"
            />
            <MInput
              :type="'text'"
              :label="'ĐT cố định'"
              :title="'Số điện thoại cố định'"
              class="width-4 mr"
              :error="formError.LandlinePhoneNumber"
              v-model="formData.LandlinePhoneNumber"
              :value="formData.LandlinePhoneNumber"
            />
            <MInput :type="'text'" :label="'Email'" class="width-4" :error="formError.Email" v-model="formData.Email"
            :value="formData.Email"
            />
          </div>
          <div class="row popup-control">
            <MInput
              :type="'text'"
              :label="'Tài khoản ngân hàng'"
              class="width-4 mr"
              v-model="formData.BankAccount"
              :value="formData.BankAccount"
            />
            <MInput
              :type="'text'"
              :label="'Tên ngân hàng'"
              class="width-4 mr"
              v-model="formData.BankName"
              :value="formData.BankName"
            />
            <MInput :type="'text'" :label="'Chi nhánh'" class="width-4" v-model="formData.Branch" :value="formData.Branch"/>
          </div>
        </div>
      </div>
      <div class="popup-footer">
        <div class="popup-footer__left">
          <MButton :type="'second'" :text="'Hủy'" @click="closeFunction" />
        </div>
        <div class="popup-footer__right row">
          <MButton :type="'second'" :text="'Cất'" class="mr" @click="handleSubmit" />
          <MButton :type="'primary'" :text="'Cất và Thêm'" @click="handleSubmitPro"/>
        </div>
      </div>
    </div>
    <!-- DIALOGL -->
    <MDialog
      v-if="this.dialog.showDialog"
      :type="this.dialog.type"
      :title="this.dialog.title"
      :text="this.dialog.text"
      :mode="this.dialog.mode"
      :closeFunction="() => { this.dialog.showDialog = false; }"
      :primaryAction="this.dialog.primaryAction"
      :secondAction="this.dialog.secondAction"
      :primaryBtnText="this.dialog.primaryBtnText"
      :cancelBtnText="this.dialog.cancelBtnText"
    />

    <!-- TOAST -->
    <div class="toast-message">
      <MToast
        v-if="this.toast.showToast"
        :type="toast.type"
        :message="toast.message"
        :action="toast.action"
        :closeFunction="() => {this.toast.showToast = false}"
      />
    </div>
  </div>
</template>

<script>
/* eslint-disable */
import MIcon from "../../components/base/icon/MIcon.vue";
import { allDepartment } from "./total-department.js";
import { allPosition } from "./total-position.js";
import { removeBlankKey, createDateString } from '../../js/ulti/convert-data.js';
import { getNewEmployeeCode, checkEmployeeCodeBeforeCU, createEmployee, updateEmployee } from "../../js/services/employee.js";
import { getPositionById } from "../../js/services/position.js";
import { getDepartmentById } from "../../js/services/department.js";

export default {
  name: "MEmployeePopup",
  components: { MIcon },
  props: {
    /**
     * closeFunction: Hàm đóng Popup
     * 
     */
    closeFunction: Function,
    inputData: { type: Object },
  },
  data() {
    /**
     * departments: danh sách department truyền vào combobox
     * positions: mảng position truyền vào combobox
     * Gender: giá trị truyền vào combobox Gender
     * dialogMessagee: lỗi hiển trị trên dialog
     * formData: dữ liệu form để gửi server
     * formError: lỗi của các trường
     * formMode: trạng thái của popup: ADD - THÊM, UPDATE - SỬA
     * dialog: chứa thông tin của dialog 
     *  + showDialog: ẩn/ hiện dialog
     *  + type: Loại của icon :error, warning, info
     *  + mode: kiểu của dialog: warning - cảnh báo cỏ 1 nút bấm, error - xác nhận lỗi có 2 nút, full - có đủ 3 nút
     *  + title: tiêu đề dialog
     *  + text: nội dung của dialog, kiểu Array
     * toast: chứa thông tin của toast message
     *  + showToast: ẩn/ hiện toast
     *  
     */
    return {
      departments: [],
      positions: [],
      // giá trị truyền vào cho radio input Gender
      Gender: [
        { label: "Nam", value: 0 },
        { label: "Nữ", value: 1 },
        { label: "Khác", value: 2 },
      ],
      dialogMessage: [],
      formData: {
        EmployeeCode: null,
        FullName:null,
        DepartmentId: null,
        PositionId: null,
        DateOfBirth: null,
        Gender: null,
        IdentityNumber: null,
        IdentityDate: null,
        IdentityPlace: null,
        Address: null,
        MobilePhoneNumber: null,
        LandlinePhoneNumber: null,
        Email: null,
        BankAccount: null,
        BankName: null,
        Branch: null,
        IsCustomer: null,
        IsSupplier: null,
      },
      formError: {
        EmployeeCode: "",
        FullName:"",
        department: "",
        position: "",
        DateOfBirth: "",
        Gender: "",
        IdentityNumber: "",
        IdentityDate: "",
        IdentityPlace: "",
        Address: "",
        MobilePhoneNumber: "",
        LandlinePhoneNumber: "",
        Email: "",
        BankAccount: "",
        BankName: "",
        Branch: "",
      }, 
      formMode: this.$MEnum.FormMode.ADD,
      dialog: {
        showDialog: false,
        type: this.$MEnum.DialogType.WARNING,
        mode: this.$MEnum.DialogMode.WARNING,
        title: this.$MResource["VN"].ValidateDialogTitle,
        text: [],
        primaryAction: null,
        secondAction: null,
        primaryBtnText: this.$MResource["VN"].PrimaryBtnText,
        secondBtnText: this.$MResource["VN"].SecondBtnTextConfirm,
      },
      toast: {
        showToast: false,
        type: this.$MResource["VN"].ToastTypeSuccess,
        message: "",
        action: null,
      },
    };
  },
  computed: {
  },
  async created() {
    if(this.inputData.IsUpdate === true) {
          this.formMode = this.$MEnum.FormMode.UPDATE;
          this.formData.EmployeeId = this.inputData.EmployeeId;
          this.formData.IsCustomer = this.inputData.IsCustomer;
          this.formData.IsSupplier = this.inputData.IsSupplier;
          this.formData.EmployeeCode = this.inputData.EmployeeCode;
          this.formData.FullName = this.inputData.FullName;
          this.formData.DepartmentId = this.inputData.DepartmentId;
          this.formData.PositionId = this.inputData.PositionId;
          this.formData.DateOfBirth = createDateString(this.inputData.DateOfBirth);
          this.formData.Gender = this.inputData.Gender;
          this.formData.IdentityNumber = this.inputData.IdentityNumber;
          this.formData.IdentityDate = createDateString(this.inputData.IdentityDate);
          this.formData.IdentityPlace = this.inputData.IdentityPlace;
          this.formData.Address = this.inputData.Address; 
          this.formData.MobilePhoneNumber = this.inputData.MobilePhoneNumber;
          this.formData.LandlinePhoneNumber = this.inputData.LandlinePhoneNumber;
          this.formData.Email = this.inputData.Email;
          this.formData.BankAccount = this.inputData.BankAccount;
          this.formData.BankName = this.inputData.BankName;
          this.formData.Branch = this.inputData.Branch;
        }
        else if(this.inputData.IsDuplicate === true) {
          this.formMode === this.$MEnum.FormMode.DUPLICATE
          this.formData.IsCustomer = this.inputData.IsCustomer;
          this.formData.IsSupplier = this.inputData.IsSupplier;
          this.formData.EmployeeCode = this.inputData.EmployeeCode;
          this.formData.FullName = this.inputData.FullName;
          this.formData.DepartmentId = this.inputData.DepartmentId;
          this.formData.PositionId = this.inputData.PositionId;
          this.formData.DateOfBirth = createDateString(this.inputData.DateOfBirth);
          this.formData.Gender = this.inputData.Gender;
          this.formData.IdentityNumber = this.inputData.IdentityNumber;
          this.formData.IdentityDate = createDateString(this.inputData.IdentityDate);
          this.formData.IdentityPlace = this.inputData.IdentityPlace;
          this.formData.Address = this.inputData.Address; 
          this.formData.MobilePhoneNumber = this.inputData.MobilePhoneNumber;
          this.formData.LandlinePhoneNumber = this.inputData.LandlinePhoneNumber;
          this.formData.Email = this.inputData.Email;
          this.formData.BankAccount = this.inputData.BankAccount;
          this.formData.BankName = this.inputData.BankName;
          this.formData.Branch = this.inputData.Branch;
        }
        else {
          this.resetForm();
          console.log("resetForm");
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
  mounted() {
    this.$tinyEmitter.on("resetEmployeeForm", this.resetForm);
  },
  beforeUnmount() {
    this.$tinyEmitter.off("resetEmployeeForm");
  },
  watch: {
  },
  methods: {
    /**
     * Hàm gọi khi button "Cất và thêm mới được gọi", thêm(sửa) vào database sau đó 
     * Author: PMChien
     */
    async handleSubmitPro() {
      try {
        if(this.validate()) {
          let isDuplicateNewCode = await this.checkNewCode();
          if(isDuplicateNewCode) {
            // let msg = [`Mã nhân viên ${this.formData.EmployeeCode} đã tồn tại trong hệ thống`];
            // this.$tinyEmitter.emit("openValidateDialog", msg);
            this.dialog.type = this.$MEnum.DialogType.WARNING;
            this.dialog.mode = this.$MEnum.DialogMode.WARNING;
            this.dialog.text = [this.$MResource["VN"].EmployeeCodeDuplicate];
            this.dialog.showDialog = true;
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
          console.log("Hợp lệ");
          let isDuplicateNewCode = await this.checkNewCode();
          if(isDuplicateNewCode) {
            this.dialog.type = this.$MEnum.DialogType.WARNING;
            this.dialog.mode = this.$MEnum.DialogMode.WARNING;
            this.dialog.text = [this.$MResource["VN"].EmployeeCodeDuplicate];
            this.dialog.showDialog = true;
          }
          else {
            if(this.formMode === this.$MEnum.FormMode.ADD || this.formMode === this.$MEnum.FormMode.DUPLICATE) {
              this.emitData();
              this.$tinyEmitter.emit("loadData");
              this.$tinyEmitter.emit("openToast", this.toast);
              this.closeFunction();
            }
            else {
              this.emitData();
            }
            
            // this.closeFunction();
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
           * EmployeeId: id của employee cần check
           * EmployeeCode: Mã cần check,
           * isCreate: có phải đang trạng thái ADD không
           * isUpdate: có phải đang trạng tái UPDATE không
           */
           let DTOCheckCode = {
              EmployeeId: this.formData.EmployeeId ? this.formData.EmployeeId : null,
              EmployeeCode: this.formData.EmployeeCode.trim(),
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
     * hàm gọi api thêm, sửa và gửi emit lên EmployeeList để tạo toast
     * Author: PMChien
     */
    async emitData() {
      try {
        let data = removeBlankKey(this.formData);
        if (this.formMode === this.$MEnum.FormMode.ADD || this.formMode === this.$MEnum.FormMode.DUPLICATE) {
          let res = await createEmployee(data);
          if(this.handleResponse(res)) {
            this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
            this.toast.message = this.$MResource["VN"].AddEmployeeSuccess;
          }

        } else if (this.formMode === this.$MEnum.FormMode.UPDATE) {
          // kiểm tra dữ liệu có thay đổi không
          let isChange = false;
          for (const key in this.formData) {
            if (Object.prototype.hasOwnProperty.call(this.formData, key)) {
              if(key !== 'DateOfBirth') {
                if(this.formData[key] !== this.inputData[key]) {
                  isChange = true;
                }
              }
              else {
                let stringDate = createDateString(this.inputData);
                if(this.formData[key] !== stringDate) {
                  isChange = true;
                }
              }
              
            }
          }

          // Nếu bị thay đổi
          if(isChange) {
            // Mở Dialog
            this.dialog.title = this.$MResource["VN"].UpdateEmployeeTitle;
            this.dialog.text = [this.$MResource["VN"].UpdateEmployeeMessage];
            this.dialog.mode = this.$MEnum.DialogMode.FULL;
            this.dialog.type = this.$MEnum.DialogType.INFO;
            // nút chính sẽ lưu dữ liệu và đóng form
            this.dialog.primaryAction = async () => {
              let res = await updateEmployee(data, this.formData.EmployeeId);
              if(this.handleResponse(res)) {
                this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
                this.toast.message = this.$MResource["VN"].EditEmployeeSuccess;
                this.toast.showToast = true;
                this.$tinyEmitter.emit("loadData");
              }
              this.resetForm();
            };
            // nút phụ sẽ đóng form
            this.dialog.secondAction = () => {
              this.dialog.showDialog = false;
              this.closeFunction();
            };
            this.dialog.showDialog = true;
          }
          else {
            // nếu không có gì thay đổi thì gọi api
            let res = await updateEmployee(data, this.formData.EmployeeId);
              if(this.handleResponse(res)) {
                this.toast.type = this.$MResource["VN"].ToastTypeSuccess;
                this.toast.message = this.$MResource["VN"].EditEmployeeSuccess;
                this.toast.showToast = true;
              }
          }
          
        }
      } catch (error) {
        console.error("Đã có lỗi: ", error);
      }
    },
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
              this.toast.showToast = true;
              break;
            case 403:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.toast.showToast = true;
              break;
            case 404:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.toast.showToast = true;
              break;
            case 500:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = message;
              this.toast.showToast = true;
              break;
            default:
              this.toast.type = this.$MResource["VN"].ToastTypeError;
              this.toast.message = this.$MResource["VN"].Error;
              this.toast.showToast = true;
              break;
          }
        }
        
        return false;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
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
        if(!this.formData.EmployeeCode) {
          this.formError.EmployeeCode = this.$MResource["VN"].FieldIsNotEmpty;
          this.dialogMessage.push(this.$MResource["VN"].EmployeeCodeIsNotEmpty);
          isValid = false;
          checkRequired = false;
        }

        // validate fullname
        if(!this.formData.FullName) {
          this.formError.FullName = this.$MResource["VN"].FieldIsNotEmpty;
          this.dialogMessage.push(this.$MResource["VN"].FullNameIsNotEmpty);
          isValid = false;
          checkRequired = false;
        }

        // DepartmentId
        if(!this.formData.DepartmentId) {
          this.formError.department = this.$MResource["VN"].FieldIsNotEmpty;
          this.dialogMessage.push(this.$MResource["VN"].DepartmentIdIsNotEmpty);
          isValid = false;
          checkRequired = false;
        }

        // DateOfBirth
        if(this.formData.DateOfBirth) {
          let today = new Date();
          let dob = new Date(this.formData.DateOfBirth);
          if(
            dob > today ||
            (dob.getFullYear() === today.getFullYear() &&
              dob.getMonth() === today.getMonth() &&
              dob.getDate() === today.getDate())
          ) {
            this.formError.DateOfBirth = this.$MResource["VN"].DoBInvalid;
            isValid = false;
          }
        }

        // IdentityNumber 
        if(this.formData.IdentityNumber) {
         let regexIdentity =  /^\d{9}$/;
         if(!regexIdentity.test(this.formData.IdentityNumber)) {
          this.formError.IdentityNumber = this.$MResource["VN"].MemberCardCodeIsNotFormat;
          isValid = false;
         }
        }

        // IdentityDate
        if(this.formData.IdentityDate) {
          let today = new Date();
          let IdentityDateCheck = new Date(this.formData.IdentityDate);
          if(IdentityDateCheck > today) {
            this.formError.IdentityDate = this.$MResource["VN"].DoRInvalid;
            isValid = false
          }
        }

        // MobilePhoneNumber
        if(this.formData.MobilePhoneNumber) {
          let phoneRegex = /^[^a-zA-Z]*$/;
          if(!phoneRegex.test(this.formData.MobilePhoneNumber)) {
            this.formError.MobilePhoneNumber = this.$MResource["VN"].PhoneNumberIsNotFormat;
            isValid = false;
          }
        }

        // LandlinePhoneNumber
        if(this.formData.LandlinePhoneNumber) {
          let phoneRegex = /^[^a-zA-Z]*$/;
          if(!phoneRegex.test(this.formData.LandlinePhoneNumber)) {
            this.formError.LandlinePhoneNumber = this.$MResource["VN"].PhoneNumberIsNotFormat;
            isValid = false;
          }
        }

        // Email
        if(this.formData.Email) {
          const regexEmail =
            /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/i;
          if (!regexEmail.test(this.formData.Email)) {
            this.formError.Email = this.$MResource["VN"].EmailIsNotFormat;
            isValid = false;
          }
        }

        if(!checkRequired) {
          this.dialog.type = this.$MEnum.DialogType.ERROR;
          this.dialog.mode = this.$MEnum.DialogMode.WARNING;
          this.dialog.text = this.dialogMessage;
          this.dialog.showDialog = true;
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
        this.formData.EmployeeCode = await getNewEmployeeCode();
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
