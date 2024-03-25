<template>
  <div class="m-import-container">
    <!-- LOADING -->
    <MLoading v-if="isShowLoading"/>

    <!-- IMPORT HEADER -->
    <div class="m-import-header">
      <div class="m-import-header__left">
        Bước {{ importState.mode + 1 }}: {{ importState.label[importState.mode] }}
      </div>
      <div class="m-import-header__right">
        <MIcon
            :class="'icon-cancel-popup'"
            :tooltip="'Thoát'"
            :tooltipPosition="'bottom'"
            @click="closeEmployeeImport"
          />
      </div>
    </div>

    <!-- IMPORT SIDEBAR -->
    <div class="m-import-sidebar">
      <div class="sidebar-item" 
      :class="{'sidebar-item--selected' : this.importState.mode === this.$MEnum.ImportState.SELECT}">
        1. Chọn tệp nguồn
      </div>
      <div class="sidebar-item" 
      :class="{'sidebar-item--selected' : this.importState.mode === this.$MEnum.ImportState.VALIDATION}">
        2. Kiểm tra dữ liệu
      </div>
      <div class="sidebar-item"
      :class="{'sidebar-item--selected' : this.importState.mode === this.$MEnum.ImportState.RESULT}">
        3. Kết quả nhập khẩu
      </div>
    </div>

    <!-- IMPORT CONTENT -->
		<div class="m-import-main">
			<div class="m-import-content">
				
        <!-- VIEW 1: UPLOAD FILE -->
				<div v-if="this.importState.mode === this.$MEnum.ImportState.SELECT" class="view-select-file">
					<p>Chọn dữ liệu Nhân viên đã chuẩn bị để nhập khẩu vào phần mềm</p>
					<MInput type="file"  @fileUploaded="handleFileUploaded"/>
				</div>

        <!-- VIEW 2: VALIDATE FILE -->
        <div v-if="this.importState.mode === this.$MEnum.ImportState.VALIDATION && this.validatedEmployees !== null && this.validatedEmployees.length > 0" class="view-validation-file">
          <!-- <div v-if="true" class="view-validation-file"> -->

					<div class="validation-header">
            <div class="width-300">{{ this.validRows }}/{{ this.validatedEmployees.length }} dòng hợp lệ</div>
            <div class="width-300">{{ this.invalidRows }}/{{ this.validatedEmployees.length }} dòng không hợp lệ</div>          
          </div>
          <div class="validation-body">
            <!-- VALIDATE FILE RESULT TABLE -->
            <EmployeeImportTable :items="validatedEmployees"/>
          </div>
				</div>

        <!-- VIEW 3: RESULT -->
        <div v-if="this.importState.mode === this.$MEnum.ImportState.RESULT" class="view-result-file">
					<div class="result-title">Kết quả nhập khẩu</div>
          <div class="result-infomation">
            <div class="download-result-file">Tải về tập tin chứa kết quả nhập khẩu <a class="link-download-example-file" href="#">tại đây</a></div>
            <div>+ Số dòng nhập khẩu thành công: {{ this.validRows }}</div>
            <div>+ Số dòng nhập khẩu không thành công: {{ this.invalidRows }}</div>
          </div>
				</div>

			</div>
		</div>
    
    <!-- IMPORT FOOTER -->
    <div class="m-import-footer">
      <div class="m-import-footer__left">
        <MButton :type="'second'" :text="'Trợ giúp'" />
      </div>
      <div class="m-import-footer__right">
        <MButton 
          v-if="importState.mode !== this.$MEnum.ImportState.RESULT"
          :type="'primary'" 
          :text="'Quay lại'" 
          :disabled="importState.mode === this.$MEnum.ImportState.SELECT"
          @click="handleBackBtnClick"  
        />
        <MButton 
          v-if="importState.mode === this.$MEnum.ImportState.SELECT"
          :type="'primary'" 
          :text="'Tiếp tục'" 
          :class="'m-import-footer__button-center'" 
          @click="handleValidation"
        />
        <MButton 
          v-if="importState.mode === this.$MEnum.ImportState.VALIDATION"
          :type="'primary'" 
          :text="'Thực hiện'" 
          :class="'m-import-footer__button-center'" 
          @click="handleImportBtn"
          :disabled="validRows && validRows > 0 ? false : true"
        />
        <MButton :type="'second'" :text="this.importState.mode === this.$MEnum.ImportState.RESULT ? 'Đóng' : 'Hủy'" @click="closeEmployeeImport"/>
      </div>
    </div>

    <!-- TOAST -->
    <div class="toast-message">
      <MToast
        v-if="this.toast.showToast"
        :type="toast.type"
        :message="toast.message"
        :action="toast.action"
        :closeFunction="
          () => {
            this.toast.showToast = false;
          }
        "
      />
    </div>
  </div>
</template>

<script>
import { checkAuthentication } from '@/js/services/token.js';
import { validateFile, importFile } from '../../js/services/employee.js';
import EmployeeImportTable from './EmployeeImportTable.vue';
export default {
  name: "EmployeeImport",
  components: [ EmployeeImportTable ],
  async created() {
    await checkAuthentication();
  },
  data() {
    return {
      file: null,
      importState: { 
          mode: this.$MEnum.ImportState.SELECT,
          label: {
            0: this.$MResource["VN"].File.ChooseFileToImport,
            1: this.$MResource["VN"].File.ValidateFileToImport,
            2: this.$MResource["VN"].File.ImportResult,
          }
      },
      validatedEmployees: [],
      validRows: null,
      invalidRows: null,
      toast: {
        showToast: false,
        type: this.$MResource["VN"].ToastTypeSuccess,
        message: "",
        action: null,
      },
      isShowLoading: false,
      keyImport: null,
    }
  },
  methods: {
    /**
     * nhận file đầu vào
     * @param {*} file 
     */
    handleFileUploaded(fileUploaded) {
      try {
        this.file = fileUploaded;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    /**
     * Chuyển importState.mode về importState.mode trước đó
     * Author: PMChien (18/01/2024)
     */
    handleBackBtnClick() {
      if(this.importState.mode !== this.$MEnum.ImportState.SELECT) {
        this.importState.mode = this.$MEnum.ImportState.SELECT;
        //this.file = null;
      }
    },
    /**
     * Gọi api validate file excel và chuyển state sang validation
     * Author: PMChien
     */
    async handleValidation() {
      try {
        // gọi api validate file
        console.log("Tới đây r");
        if(this.file) {
          this.isShowLoading = true;
          let res = await validateFile(this.file);
          let serverResult = this.handleResponse(res);
          if(serverResult) {
            this.isShowLoading = false;
            this.validatedEmployees = res.data.ImportData;
            this.keyImport = res.data.ImportKey;
            // chuyển state sang validation
            this.importState.mode = this.$MEnum.ImportState.VALIDATION;
            this.handleCountValidRow(this.validatedEmployees);
          }
          else {
            this.isShowLoading = false;
          }
        }
        else {
          // chưa có tệp được chọn - hiển thị toast
          this.toast.type = this.$MResource["VN"].ToastTypeWarning;
          this.toast.message = this.$MResource["VN"].Toast.Import.FileIsNotChosen;
          this.toast.showToast = true;
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Hàm gọi khi button Thực hiện click
     * Author: PMChien
     */
    async handleImportBtn() {
      try {
        // gọi api import file
        console.log("Tới đây r");
        if(this.file && this.keyImport) {
          this.isShowLoading = true;
          let res = await importFile(this.keyImport);
          console.log(res);
          let serverResult = this.handleResponse(res);
          if(serverResult) {
            this.isShowLoading = false;
            this.validRows = res.data.Imported;
            this.invalidRows = res.data.Total - res.data.Imported;
            this.importState.mode = this.$MEnum.ImportState.RESULT;
          }
          else {
            this.isShowLoading = false;
            this.importState.mode = this.$MEnum.ImportState.RESULT;
            this.validRows = 0;
            this.invalidRows = this.validatedEmployees.length;
          }
        }
        else {
          // chưa có tệp được chọn - hiển thị toast
          this.toast.type = this.$MResource["VN"].ToastTypeWarning;
          this.toast.message = this.$MResource["VN"].Toast.Import.FileIsNotChosen;
          this.toast.showToast = true;
        }
        
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Cập nhật các giá trị hàng hợp lệ và không hợp lệ
     * @param {Array} items Mảng thông tin import được server trả về
     */
    handleCountValidRow(items) {
      try {
        if(items && items.length > 0) {
          let valildArray = items.filter((item) => item.CanImported === true);
          this.validRows = valildArray.length;
          this.invalidRows = items.length - this.validRows;
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Đóng Import page, reset file về null
     * Author: PMChien
     */
    closeEmployeeImport() {
      try {
        this.file = null;
        this.$router.push('/nhan-vien');
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Xử lí respose từ axios trả về
     * Author: PMChien
     */
     handleResponse(res) {
      try {
        if (Number.isInteger(res)) {
          return true;
        } else {
          let status = res.status;
          let message = "";
          console.log(res);
          console.log("response status: ", status);
          if(res.data) {
            if (res.data.userMsg) {
              message = res.data.userMsg;
            }
          }
          
          switch (status) {
            case 200:
            case 201:
              return true;
              //break;
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
    },
  },
}
</script>

<style scoped>
@import url('../../css/view/employee/employee-import.css');
</style>