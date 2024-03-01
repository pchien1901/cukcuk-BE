import axios from 'axios';
import config from '../config/config.js';
import { store } from '@/main.js';
import { router } from '@/main.js';
import MResource from '@/helper/resource.js';
import MApiResource from '@/helper/api-resource.js';
import tinyEmitter from "tiny-emitter/instance";

const apiURL = config.API_URL;

/**
 * Lấy accessToken và refreshToken mới
 * @param {*} emitter tinyEmitter truyền vào
 * Author: PMChien
 */
export async function getNewToken() {
  try {
    let now = new Date();
    // lấy thời gian hết hạn refresh token từ localStorage
    let expirationRefreshTokenString = localStorage.getItem("expirationRefreshToken");
    let expirationRefreshTokenTime = new Date(expirationRefreshTokenString);

    // lấy thời gian hết hạn access token từ localStorage
    let expirationAccessTokenString = localStorage.getItem("expirationToken");
    let expirationAccessTokenTime = new Date(expirationAccessTokenString);

    // khi thời gian hết hạn refresh token < hiện tại => chuyển về trang đăng nhập
    if(expirationRefreshTokenTime <= now) {
      store.commit('changeAuthenticateStatus', false);
      router.push("/login");
      tinyEmitter.emit(
        MResource["VN"].Event.Toast.openMainToast,
        {
          type: MResource["VN"].ToastTypeWarning,
          message: MResource["VN"].Toast.Auth.TokenExpiration,
          action: null,
        }
      );
    }
    else if(expirationAccessTokenTime <= now) {
      console.log("access token hết hạn");
      let data = {
        AccessToken: localStorage.getItem("accessToken"),
        RefreshToken: localStorage.getItem("refreshToken")
      };
      let res = await axios.post(
        `${apiURL}/refresh-token`,
        data,
        {
          headers: {
            "Content-Type": MApiResource.apiHeaderContentType.applicationType,
          }
        }
      );
      console.log("token mới: ", res);
      localStorage.setItem("accessToken", res.data.accessToken);
      localStorage.setItem("refreshToken", res.data.refreshToken);
    }
  } catch (error) {
    console.error("Đã có lỗi: ", error);
  }
}

/**
 * Kiểm tra người dùng đã được xác thực chưa
 * Author: PMChien (29/02/2024)
 */
export async function checkAuthentication() {
  try {
    if(!store.state.isAuthenticate) {
      router.push("/login");
    }
    let now = new Date();
    let expirationAccessTokenString = localStorage.getItem("expirationToken");
    let expirationAccessTokenTime = new Date(expirationAccessTokenString);
    if(store.state.isAuthenticate && expirationAccessTokenTime <= now) {
      await getNewToken();
    }
  } catch (error) {
    console.error("Đã xảy ra lỗi: ", error);
  }
}