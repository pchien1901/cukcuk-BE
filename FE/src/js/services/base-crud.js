import axios from "axios";
import config from "../config/config.js";
import { router, store } from "@/main.js";
import tinyEmitter from "tiny-emitter/instance";
import MResource from "@/helper/resource.js";
import MApiResource from "@/helper/api-resource.js";
import { checkAuthentication } from "./token.js";

const apiURL = config.API_URL;

// export function handleErr(error, type, emitter) {
//   console.error("Đã xảy ra lỗi: ", error);
//   // if()
// }

// Thêm một bộ đón chặn response
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
      console.log("biến userMsg: ", userMsg);
      console.log("error.response.data.userMsg: ", error.response.data.userMsg);

      switch (error.response.status) {
        case 400:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : "Thông tin gửi đi chưa hợp lệ.",
          });
          break;
        case 401:
          store.commit("changeAuthenticateStatus", false);
          router.push("/login");
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg
              ? userMsg
              : "Hết phiên đăng nhập, vui lòng đăng nhập lại.",
          });
          return null;
          //break;
        case 403:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : "Truy cập bị từ chối.",
          });
          break;
        case 404:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : "Không tìm thấy thông tin.",
          });
          break;
        case 500:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg
              ? userMsg
              : "Lỗi phía máy chủ, vui lòng thử lại sau.",
          });
          break;
        default:
          tinyEmitter.emit(MResource["VN"].Event.Toast.openMainToast, {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : "Đã xảy ra lỗi.",
          });
          break;
      }
    }
  }
);

/**
 *
 * @param {*} method
 * @param {*} subURL
 * @param {*} data
 * @param {*} type
 * @returns
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
        console.log("response: ", res);
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
        return res.data;
        //break;
      }
      case MApiResource.apiMethod.delete: {
        if (data) {
          let res = await axios.delete(`${apiURL}/${subURL}`, data, {
            headers: {
              "Content-Type": type,
              Authorization: `Bearer ${token}`,
            },
          });
          return res.data;
        } else {
          let res = await axios.delete(`${apiURL}/${subURL}`, {
            headers: {
              "Content-Type": type,
              Authorization: `Bearer ${token}`,
            },
          });
          return res.data;
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
