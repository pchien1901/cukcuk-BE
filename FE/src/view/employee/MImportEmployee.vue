<template>
  <div class="m-import-container">
    <div class="m-import-header">Bước {{ importState.mode + 1 }}: {{ importState.label[importState.mode] }}</div>
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
		<div class="m-import-main">
			<div class="m-import-content">
				
				<div v-if="this.importState.mode === this.$MEnum.ImportState.SELECT" class="view-select-file">
					<p>Chọn dữ liệu Nhân viên đã chuẩn bị để nhập khẩu vào phần mềm</p>
					<MInput type="file" class="width-300" @fileUploaded="handleFileUploaded"/>
				</div>

        <div v-if="this.importState.mode === this.$MEnum.ImportState.VALIDATION && this.validatedCustomers !== null && this.validatedCustomers.length > 0" class="view-validation-file">
					<div class="validation-header">
            <span>{{ this.validRows }}/{{ this.validatedCustomers.length }} dòng hợp lệ</span>
            <span>{{ this.invalidRows }}/{{ this.validatedCustomers.length }} dòng không hợp lệ</span>          
          </div>
          <div class="validation-body">
            <!-- <MCustomerImportTable :items="validatedCustomers"/> -->
          </div>
				</div>

        <div v-if="this.importState.mode === this.$MEnum.ImportState.RESULT" class="view-select-file">
					<p>result</p>
					
				</div>

			</div>
		</div>
    
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
          :customClass="'m-import-footer__button-center'" 
          @click="handleValidation"
        />
        <MButton 
          v-if="importState.mode === this.$MEnum.ImportState.VALIDATION"
          :type="'primary'" 
          :text="'Thực hiện'" 
          :customClass="'m-import-footer__button-center'" 
        />
        <MButton :type="'second'" :text="'Hủy bỏ'" @click="() => { this.$router.push('/nhan-vien');}"/>
      </div>
    </div>
  </div>
</template>

<script>
import { validateFile } from '../../js/services/employee.js';
export default {
  name: "MImportEmployee",
  data() {
    return {
      file: null,
      importState: { 
          mode: this.$MEnum.ImportState.SELECT,
          label: {
            0: "Chọn tệp nguồn",
            1: "Kiểm tra dữ liệu",
            2: "Kết quả nhập khẩu",
          }
      },
      validatedCustomers: [],
      validRows: null,
      invalidRows: null,
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
        this.file = null;
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
        
        let res = await validateFile(this.file);
        console.log(res);
        
        
        // if(res && res.status !== null) {
        //   // nếu không có lỗi
        //   if(res.status >= 200 && res.status < 300) {
        //     this.validatedCustomers = res.data;
        //     // chuyển state sang validation
        //     this.importState.mode = this.$MEnum.ImportState.VALIDATION;
        //   }

        //   // nếu có lỗi
        //   if(res.status >= 400) {
        //     let toastInfo = { 
        //       toastType: "error", 
        //       toastMessage: res.data.userMsg, 
        //     };
        //     this.$tinyEmitter.emit("showToast", toastInfo);
        //     console.log("emit lỗi");
        //   }
        // }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },
    // handleValidation() {
    //   try {
    //     this.validatedCustomers = data;
    //     this.handleCountValidRow(this.validatedCustomers);
    //     this.importState.mode = this.$MEnum.ImportState.VALIDATION;
    //   } catch (error) {
    //     console.error("Đã xảy ra lỗi: ", error);
    //   }
    // },
    /**
     * Cập nhật các giá trị hàng hợp lệ và không hợp lệ
     * @param {Array} items Mảng thông tin import được server trả về
     */
    handleCountValidRow(items) {
      try {
        if(items && items.length > 0) {
          let valildArray = items.filter((item) => item.CanImport === true);
          this.validRows = valildArray.length;
          this.invalidRows = items.length - this.validRows;
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    }
  },
}
</script>

<style scoped>
@import url('../../css/view/employee/employee-import.css');
</style>