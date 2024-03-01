<template>
  <div class="amis-login">
    <div class="limiter">
      <div class="container-login" view="login" bg="9">
        <div class="wrap-login" type="login">
          <div class="login-form validate-form">
            <span class="login-form-logo"></span>
            <div class="login-form-inputs login-class">
              <form>
                <!-- <div class="wrap-input username-wrap validate-input">
                  <input class="input ap-lg-input" type="text" name="username" placeholder="Số điện thoại/Email"/>
                  <span class="error-info">Tên đăng nhập không được để trống</span>
                </div>
                <div class="wrap-input password-wrap validate-input">
                  <input class="input ap-lg-input" type="password" name="password" placeholder="Mật khẩu"/>
                  <span class="error-info">Mật khẩu không được để trống</span>
                </div> -->
                  <!-- USERNAME -->
                  <div class="form-control">
                    <MInput 
                      :size="'large'" 
                      :placeholder="'Số điện thoại/Email'"
                      :error="formError.Username"
                      v-model="formData.Username"
                      ref="Username"
                      @focus="() => handleInputFocus('Username')"
                    />
                  </div>

                  <!-- PASSWORD -->
                  <div class="form-control">
                    <MInput 
                      :type="'password'" 
                      :size="'large'" 
                      :placeholder="'Mật khẩu'" 
                      :error="formError.Password"
                      v-model="formData.Password"
                      ref="Password"
                      @focus="() => handleInputFocus('Password')"
                    />
                  </div>
              </form>
              

                <!-- FORGOT PASSWORD -->
                <div class="forgot-password">
                  <a>Quên mật khẩu ?</a>
                </div>

                <!-- LOGIN BUTTON  -->
                <!-- <div class="login-form-btn">Đăng nhập</div> -->
                <MButton :text="'Đăng nhập'" :class="'login-form-btn'" @click="handleSubmitLoginForm"/>

                <div class="method-block">
                  <div class="method-title">
                    <div class="title-line"></div>
                    <div class="title-text">Hoặc đăng nhập với</div>
                    <div class="title-line"></div>
                  </div>
                  <div class="method-list">
                    <div class="method-item" method="Google" title="Google"></div>
                    <div class="method-item" method="Apple" title="Apple"></div>
                    <div class="method-item" method="Microsoft" title="Microsoft"></div>
                  </div>
                </div>
            </div>
          </div>
          <div class="text-center copy-right-text">Copyright © 2012 - 2024 MISA JSC</div>
        </div>
      </div>
    </div>
  </div>
  <div class="lang-container login-class" @click="toggleLangDropDown">
    <div class="language-icon" :class="this.language === this.$MEnum.Language.VIETNAMESE ? 'icon-vi' : 'icon-en'"></div>
    <!-- <div class="language-icon icon-en"></div> -->
    <div class="selected-value">
      {{ this.language === this.$MEnum.Language.VIETNAMESE ? 'Tiếng Việt' : 'English' }}
    </div>
    <div v-if="isShowLangDropDown" class="lang-dropdown" >
      <div class="vi lang-item" @click="() => {this.language = this.$MEnum.Language.VIETNAMESE}">
        Tiếng Việt
      </div>
      <div class="en lang-item" @click="() => {this.language = this.$MEnum.Language.ENGLISH}">
        English
      </div>
    </div>
  </div>
</template>

<script>
/* eslint-disable */ 
import { apiHandle } from '@/js/services/base-crud';
import MApiResource from '@/helper/api-resource.js';
export default {
  name : "MLogin",
  data() {
    return {
      isShowLangDropDown: false,
      formData: {
        Username: null,
        Password: null,
      },
      formError: {
        Username: null,
        Password: null,
      },
      language: this.$MEnum.Language.VIETNAMESE
    }
  },
  methods: {
    /**
     * Ẩn hiện language dropdown
     * Author: PMChien
     */
    toggleLangDropDown() {
      try {
        this.isShowLangDropDown = !this.isShowLangDropDown;
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Xóa hiệu ứng error khi focus
     * @param {String} inputRef Ref của input
     * Author: PMChien
     */
    handleInputFocus(inputRef) {
      try {
        if(this.$refs[inputRef]) {
          this.$refs[inputRef].removeError();
        }
      } catch (error) {
        console.error("Đã xảy ra lỗi: ", error);
      }
    },

    /**
     * Xử lí khi button Đăng nhập/Login click
     * Author: PMChien
     */
    async handleSubmitLoginForm() {
      try {
        if(this.validate()) {
          console.log(this.formData);
          // gọi api login
          let res = await apiHandle(
            this.$MApiResource.apiMethod.post,
            this.$MApiResource.apiUrl.login,
            this.formData,
            MApiResource.apiHeaderContentType.applicationType
          );
          // console.log(this.$MApiResource.apiMethod.post,
          //   this.$MApiResource.apiUrl.login,
          //   this.formData,
          //   MApiResource.apiHeaderContentType.applicationType);
          console.log(res);
          // nếu đăng nhập thành công
          if(res) {
            this.$store.commit("changeAuthenticateStatus", true);
            this.$router.push("/nhan-vien");
            localStorage.setItem("accessToken", res.Token)
            localStorage.setItem("refreshToken", res.RefreshToken);
            localStorage.setItem("expirationToken", res.Expiration);
            localStorage.setItem("expirationRefreshToken", res.ExpirationRefreshToken);

            // kiểm tra xem localStorage có token chưa
            console.log("accessToken: ", localStorage.getItem("accessToken"));
            console.log("refreshToken: ", localStorage.getItem("refreshToken"));
          }
        }
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    },

    /**
     *kiểm tra thông tin trường username và password
     * Author: PMChien
     */
    validate() {
      try {
        let isValid = true;

        //reset lỗi
        for (const key in this.formError) {
          if (Object.hasOwnProperty.call(this.formError, key)) {
            this.formError[key] = "";
          }
        }

        // validate username
        if(!this.formData.Username) {
          // Nếu không nhập Username
          this.formError.Username = this.$MResource['VN'].Login.Username.UsernameIsRequired;
          isValid= false;
        }
        else {
          // Kiểm tra Username có phải Email không
          const regexEmail =
            /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/i;
          if (regexEmail.test(this.formData.Username)) {
            isValid = true;
          }
          // Kiểm tra Username có phải số điện thoại không
          else {
            let phoneRegex = /^[^a-zA-Z]*$/;
            if (phoneRegex.test(this.formData.Username)) {
              isValid = true;
            }
            else {
              isValid = false;
            }
          }
          
          // Nếu đều không phải => thêm thông báo lỗi
          if(!isValid) {
            this.formError.Username = this.$MResource["VN"].Login.Username.UsernameIsNotFormat;
          }
        }

        // validate password
        if(!this.formData.Password) {
          this.formError.Password = this.$MResource["VN"].Login.Password.PasswordIsRequired;
        }
        else {
          const passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()-+=])[A-Za-z\d!@#$%^&*()-+=]+$/;
          if(!passwordRegex.test(this.formData.Password)) {
            isValid = false;
            this.formError.Password = this.$MResource["VN"].Login.Password.PasswordMustBeInFormat;
            console.log("Mật khẩu không đúng định dạng");
          }
        }

        // Nếu không thỏa mãn, thêm hiệu ứng lỗi cho lần submit thứ 2
        if(!isValid) {
          for (const key in this.formError) {
            if (Object.hasOwnProperty.call(this.formError, key)) {
              if(this.formError[key]) {
                this.$refs[key].addError();
              }
            }
          }
        }
        return isValid;
      } catch (error) {
        console.error("Đã có lỗi xảy ra: ", error);
      }
    }
  }
}
</script>

<style scoped>
@import url('../../css/view/login/login.css');
</style>