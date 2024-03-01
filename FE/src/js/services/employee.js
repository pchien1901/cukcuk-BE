import axios from 'axios';
import config from '../config/config.js';
import { checkAuthentication } from './token.js';
import { store } from '@/main.js';
import { router } from '@/main.js';
import tinyEmitter from "tiny-emitter/instance";
import MResource from '@/helper/resource.js';

const apiURL = config.API_URL;

// Thêm một bộ đón chặn request
axios.interceptors.request.use(async function (config) {
  // kiểm tra xác thực người dùng
  await checkAuthentication();
  let token = localStorage.getItem("accessToken");

  if(token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, function (error) {
  console.error("Đã có lỗi khi gửi request", error );
  return Promise.reject(error);
});


// Thêm một bộ đón chặn response
// axios.interceptors.response.use(function (response) {
//   // Bất kì mã trạng thái nào nằm trong tầm 2xx đều khiến hàm này được trigger
//   // Làm gì đó với dữ liệu response
//   return response;
// }, function (error) {
//   // Bất kì mã trạng thái nào lọt ra ngoài tầm 2xx đều khiến hàm này được trigger\
//   console.error("Đã có lỗi khi gửi request", error );
//   //return Promise.reject(error);
//   if(error?.response?.status) {
//     let errorResponse = error.response;

//     switch(error.response.status) {
//       case 401: 
//         store.commit('changeAuthenticateStatus', false);
//         router.push("/login");
//         tinyEmitter.emit(
//           MResource["VN"].Event.Toast.openMainToast,
//           {
//             type: MResource["VN"].ToastTypeError,
//             message: userMsg ? userMsg : "Hết phiên đăng nhập, vui lòng đăng nhập lại."
//           }
//         );
//         break;
//       default:
//         return error.response;
//     }
//   }
// });


/**
 * gọi api lấy tất cả Employee
 * @returns danh sách Employee/ error {data {}} nếu lỗi/ -1 nếu lỗi không xác định
 * Author: PMChien
 */
export const getAllEmployees = async () => {
  try {
    
    // lấy token từ local storage
    // await checkAuthentication();
    // let token = localStorage.getItem("accessToken");

    // let res = await axios.get(
    //     `${apiURL}/Employees`,
    //     {
    //       headers: {
    //         Authorization: `Bearer ${token}`
    //       }
    //     }
    //   );
    let res = await axios.get(`${apiURL}/Employees`);
    return res.data;
  }
  catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400) {
      if(status === 401) {
        let userMsg = null;
        if(error?.response?.data?.UserMsg) {
          userMsg = error.response.data.UserMsg;
        }
        store.commit('changeAuthenticateStatus', false);
        router.push("/login");
        tinyEmitter.emit(
          MResource["VN"].Event.Toast.openMainToast,
          {
            type: MResource["VN"].ToastTypeWarning,
            message: userMsg ? userMsg : "Hết phiên đăng nhập, vui lòng đăng nhập lại."
          }
        );
      }
      else {
        return error.response;
      }
    }
  }
}

/**
 * Lấy một Employee theo id
 * @param {*} id id của Employee cần lấy
 * @returns Employee - đối tượng có id truyên vào
 * Author: PMChien
 */
export const getEmployeeById = async (id) => {
  try {
    // lấy token từ local storage
    // await checkAuthentication();
    // let token = localStorage.getItem("accessToken");

    // let res = await axios.get(
    //     `${apiURL}/Employees/${id}`,
    //     {
    //       headers: {
    //         Authorization: `Bearer ${token}`
    //       }
    //     }
    //   );
    let res = await axios.get(`${apiURL}/Employees/${id}`);
    return res;
  } catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      if(status === 401) {
        let userMsg = null;
        if(error?.response?.data?.UserMsg) {
          userMsg = error.response.data.UserMsg;
        }
        store.commit('changeAuthenticateStatus', false);
        router.push("/login");
        tinyEmitter.emit(
          MResource["VN"].Event.Toast.openMainToast,
          {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : "Hết phiên đăng nhập, vui lòng đăng nhập lại."
          }
        );
      }
      else {
        return error.response;
      }
    }
  }
}

/**
 * Hàm lấy mã code mới
 * @returns EmployeeCode
 * Author: PMChien
 */
export const getNewEmployeeCode = async () => {
  try {
    // lấy token từ local storage
    // await checkAuthentication();
    // let token = localStorage.getItem("accessToken");
    // let res = await axios.get(
    //     `${apiURL}/Employees/NewCode`,
    //     {
    //       headers: { Authorization: `Bearer ${token}`}
    //     }
    //   );
    let res = await axios.get(`${apiURL}/Employees/NewCode`);
    return res.data;
  } catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      if(status === 401) {
        let userMsg = null;
        if(error?.response?.data?.UserMsg) {
          userMsg = error.response.data.UserMsg;
        }
        store.commit('changeAuthenticateStatus', false);
        router.push("/login");
        tinyEmitter.emit(
          MResource["VN"].Event.Toast.openMainToast,
          {
            type: MResource["VN"].ToastTypeError,
            message: userMsg ? userMsg : "Hết phiên đăng nhập, vui lòng đăng nhập lại."
          }
        );
      }
      return error.response;
    }
  }
}

/**
 * Hàm lấy tất cả thông tin nhân viên cả DepartmentName và PositionName
 * @returns Danh sách nhân viên gồm cả DepartmentName và PositionName
 * Author: PMChien 28/01/2024
 */
export const getEmployeeInfo = async ()  => {
  try {
    // await checkAuthentication();
    // let token = localStorage.getItem("accessToken");
    // let res = await axios.get(
    //     `${apiURL}/Employees/information`,
    //     {
    //       headers: { Authorization: `Bearer ${token}`}
    //     }
    //   );
    let res = await axios.get(`${apiURL}/Employees/information`);
    return res.data;
  } catch (error) {
    let status = error.response.status;
    if(status === 401) {
      let userMsg = null;
      if(error?.response?.data?.UserMsg) {
        userMsg = error.response.data.UserMsg;
      }
      store.commit('changeAuthenticateStatus', false);
      router.push("/login");
      tinyEmitter.emit(
        MResource["VN"].Event.Toast.openMainToast,
        {
          type: MResource["VN"].ToastTypeError,
          message: userMsg ? userMsg : "Hết phiên đăng nhập, vui lòng đăng nhập lại."
        }
      );
    }
    console.error("Đã xảy ra lỗi");
  }
}

/**
 * Hàm lấy thông tin nhân viên theo chuỗi tìm kiếm và phân trang
 * @param {int} currentPage số trang hiện tại
 * @param {int} pageSize số bản ghi/ trang
 * @param {string} text từ khóa tìm kiếm
 * @returns object gồm có ListRecord: danh sách bản ghi, CurrentPage: trang hiện tại, TotalPage: tổng số trang
 * Author: PMChien
 */
export const getEmployeeInfoByPage = async (currentPage = 1, pageSize = 10, text = "") => {
  try {
    if(!text) {
      text = "";
    }
    if(currentPage < 1) {
      currentPage = 1;
    }
    //await checkAuthentication();
    // let token = localStorage.getItem("accessToken");
    // console.log("store: ", store.state.isAuthenticate);
    // if(store.state.isAuthenticate) {
    //   let res = await axios.get(
    //       `${apiURL}/Employees/paging/?page=${currentPage}&size=${pageSize}&text=${text}`,
    //       { 
    //         headers: { Authorization: `Bearer ${token}`}
    //       }
    //     );
    //   return res.data;
    // }
    let res = await axios.get(
      `${apiURL}/Employees/paging/?page=${currentPage}&size=${pageSize}&text=${text}`
    );
  return res.data;
    
  } catch (error) {
    let status = error.response.status;
    if(status === 401) {
      let userMsg = null;
      if(error?.response?.data?.UserMsg) {
        userMsg = error.response.data.UserMsg;
      }
      store.commit('changeAuthenticateStatus', false);
      router.push("/login");
      tinyEmitter.emit(
        MResource["VN"].Event.Toast.openMainToast,
        {
          type: MResource["VN"].ToastTypeError,
          message: userMsg ? userMsg : "Hết phiên đăng nhập, vui lòng đăng nhập lại."
        }
      );
    }
    console.error("Đã xảy ra lỗi: ", error);
  }
}

/**
 * Kiểm tra trùng mã trước khi Insert và Update
 * @param {object} data : đối tượng chứa các trường {EmployeeId, EmployeeCode, IsCreate: (true/false), IsUpate: (true/false)}
 * @returns true - đã bị trùng, false -chưa
 * Author: PMChien
 */
export const checkEmployeeCodeBeforeCU = async (data) => {
  try {
    let res = await axios.post(`${apiURL}/Employees/validation-code`, data);
    console.log(res);
    return res.data;
  } catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      return error.response;
    }
  }
}

/**
 * Validate dữ liệu trong file excel
 * @param {FormData} data dữ liệu gửi bằng file
 * @returns Danh sách thông tin lấy từ file excel
 * Author: PMChien
 */
export const validateFile = async (file) => {
  try {
    let formData = new FormData();
    formData.append("fileImport", file);
    let res = await axios.post(`${apiURL}/Employees/validation-import`, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    }
  });
    return res;
  } catch (error) {
    console.error("Đã xảy ra lỗi: ", error);
    return error.response;
  }
}

/**
 * thực hiện import
 * @param {string} importKey chuỗi để thực hiện import
 * @returns thông tin { Imported: số dòng import được, Total: tổng số dòng}
 * Author: PMChien
 */
export const importFile = async (importKey) => {
  try {
    let formData = new FormData();
    formData.append("keyImport", importKey);
    let res = await axios.post(`${apiURL}/Employees/${importKey}`, formData);
    return res;
  } catch (error) {
    console.error("Đã xảy ra lỗi: ", error);
    return error.response;
  }
}

/**
 * Gọi api thêm mới Employee
 * @param {*} data thông tin Employee thêm mới
 * @returns số bản ghi thay đổi
 * Author: PMChien
 */
export const createEmployee = async(data) => {
  try {
    let res = await axios.post(`${apiURL}/Employees`, data);
    return res;
  } catch (error) {
    console.error('Đã xảy ra lỗi trong lúc post dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      return error.response;
    }
  }
}

/**
 * Gọi api sửa Employee
 * @param {*} data thông tin Employee cần sửa
 * @param {*} id EmployeeId
 * @returns số bản ghi bị thay đổi
 * Author: PMChien
 */
export const updateEmployee = async (data, id) => {
  try {
    let res = await axios.put(`${apiURL}/Employees/${id}`, data);
    return res;
  } catch (error) {
    console.error("Đã xảy ra lỗi trong put employee: ", error);
    let status = error.response.status;
    if(status >= 400) {
      return error.response;
    }
  }
}

/**
 * gọi API xóa 1 người dùng
 * @param {*} id EmployeeId
 * @returns số bản ghi bị thay đổi
 * Author: PMChien
 */
export const deleteEmployeeById = async (id) => {
  try {
    let res = await axios.delete(`${apiURL}/Employees/${id}`);
    return res;
  } catch (error) {
    console.error("Đã xảy ra lỗi lúc delete Employee: ", error);
    let status = error.response.status;
    if(status >= 400) {
      return error.response;
    }
  }
}

/**
 * Gọi api xóa nhiều Employee
 * @param {*} data mảng các id của Employee cần xóa
 * @returns số bản ghi bị thay đổi
 * Author: PMChien
 */
// export const deleteEmployee = async(data) => {
//   try {
//     let res = await axios.post(`${apiURL}/Employees/batch-deletion`, data);
//     return res;
//   } catch (error) {
//     console.error("Đã xảy ra lỗi lúc xóa danh sách Employee: ", error);
//     let status = error.response.status;
//     if(status >= 400) {
//       return error.response;
//     }
//   }
// }

export const deleteEmployee = async (data) => {
  try {
    let result = 0;
    for (const id of data.ids) {
      let res = await deleteEmployeeById(id);
      if(res.data) {
        result += res.data;
      }
    }
    return result;
  } catch (error) {
    console.error("Đã xảy ra lỗi tại xóa nhiều employee: ", error);
  }
}