import axios from "axios";
import config from "../config/config.js";
import { router, store } from "@/main.js";
import tinyEmitter from "tiny-emitter/instance";
import MResource from "@/helper/resource.js";
import MApiResource from "@/helper/api-resource.js";
import { checkAuthentication } from "./token.js";

const apiURL = config.API_URL;

//#region config axios request, response
/**
 * Thêm một bộ đón chặn request
 *  - Thêm Token vào header
 * Author: PMChien
 */
axios.interceptors.request.use(
  async function (config) {
    if (!isUnauthenticatedRequest(config)) {
      console.log(
        "url: ",
        config,
        "Hàm isUnauthenticateRequest: ",
        isUnauthenticatedRequest(config)
      );
      // kiểm tra xác thực người dùng
      await checkAuthentication();

      let token = localStorage.getItem("accessToken");

      //console.log("token nè: ", token);
      //console.log("store.state.isAuthenticate: ", store.state.isAuthenticate);
      if (token) {
        if (store.state.isAuthenticate === true) {
          config.headers.Authorization = `Bearer ${token}`;
        }
      }
    }

    return config;
  },
  function (error) {
    console.error("Đã có lỗi khi gửi request", error);
    return Promise.reject(error);
  }
);

/**
 * Thêm một bộ đón chặn response
 *  - Bắt và xử lí lỗi
 */
axios.interceptors.response.use(
  function (response) {
    // Bất kì mã trạng thái nào nằm trong tầm 2xx đều khiến hàm này được trigger
    // Làm gì đó với dữ liệu response
    return response;
  },
  function (error) {
    // Bất kì mã trạng thái nào lọt ra ngoài tầm 2xx đều khiến hàm này được trigger\
    // Làm gì đó với lỗi response
    //return Promise.reject(error);
    console.error("Đã có lỗi xảy ra", error);
    if (error?.response?.status) {
      let errorResponse = error.response;
      let userMsg = errorResponse?.data?.userMsg
        ? errorResponse.data.userMsg
        : null;

      switch (error.response.status) {
        case 400:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : MResource["VN"].BadRequest,
          });
          break;
        case 401:
          store.commit("logout");
          router.push("/login");
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg
              ? userMsg
              : MResource["VN"].Unauthorized,
          });
          return null;
        //break;
        case 403:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : MResource["VN"].Forbidden,
          });
          break;
        case 404:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : MResource["VN"].NotFound,
          });
          break;
        case 500:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg
              ? userMsg
              : MResource["VN"].ServerError,
          });
          break;
        default:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : MResource["VN"].Error,
          });
          break;
      }
    }
  }
);

/**
 * Hàm kiểm tra các api url không cần phải xác thực (login, register, registerAdmin, revoke, refreshToken)
 * @param {*} config đối tượng đại diện cho cấu hình của yêu cầu HTTP
 * Author: PMChien
 */
function isUnauthenticatedRequest(config) {
  try {
    return (
      config.url.includes(MApiResource.apiUrl.login) ||
      config.url.includes(MApiResource.apiUrl.refreshToken) ||
      config.url.includes(MApiResource.apiUrl.register) ||
      config.url.includes(MApiResource.apiUrl.registerAdmin)
    );
  } catch (error) {
    console.error("Đã xảy ra lỗi: ", error);
  }
}
//#endregion

//#region call api
/**
 * Hàm gọi api dựa vào tham số truyền vào
 * @param {*} method method của HTTP Request - get, put, post, delete
 * @param {*} subURL Đường dẫn URL tới địa chỉ cần gọi (VD: /Authenticate/login)
 * @param {*} data Dữ liệu gửi trong body
 * @param {*} type  Type của HTTP
 * @returns response hoặc response.data
 * Author: PMChien
 */
export async function apiHandle(
  method,
  subURL,
  data,
  type = MApiResource.apiHeaderContentType.applicationType
) {
  try {
    await checkAuthentication();
    var token = localStorage.getItem("accessToken");
    switch (method) {
      case MApiResource.apiMethod.get: {
        let res = await axios.get(`${apiURL}/${subURL}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });
        return res.data;
        //break;
      }
      case MApiResource.apiMethod.post: {
        let res = await axios.post(`${apiURL}/${subURL}`, data, {
          headers: {
            "Content-Type": type,
            Authorization: `Bearer ${token}`,
          },
        });
        if (res) {
          return res?.data ? res.data : res;
        }
        break;
      }
      case MApiResource.apiMethod.put: {
        let res = await axios.put(`${apiURL}/${subURL}`, data, {
          headers: {
            "Content-Type": type,
            Authorization: `Bearer ${token}`,
          },
        });
        if (res) {
          return res?.data ? res.data : res;
        }
        break;
      }
      case MApiResource.apiMethod.delete: {
        if (data) {
          let res = await axios.delete(`${apiURL}/${subURL}`, data, {
            headers: {
              "Content-Type": type,
              Authorization: `Bearer ${token}`,
            },
          });
          if (res) {
            return res?.data ? res.data : res;
          }
          break;
        } else {
          let res = await axios.delete(`${apiURL}/${subURL}`, {
            headers: {
              "Content-Type": type,
              Authorization: `Bearer ${token}`,
            },
          });
          if (res) {
            return res?.data ? res.data : res;
          }
          break;
        }
        //break;
      }
      default:
        break;
    }
  } catch (error) {
    console.error("Đã có lỗi xảy ra: ", error);
  }
}
//#endregion