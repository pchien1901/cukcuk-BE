import axios from 'axios';
import config from '../config/config.js';

const apiURL = config.API_URL;

/**
 * gọi api lấy tất cả Employee
 * @returns danh sách Employee/ error {data {}} nếu lỗi/ -1 nếu lỗi không xác định
 * Author: PMChien
 */
export const getAllEmployees = async () => {
  try {
    // await axios.get(`${apiURL}/Employees`)
    // .then(response => {
    //   return response;
    // }) 
    // .catch(error => {
    //   if(error.response) {
    //     if(error.response.status) {
    //       return error.response;
    //     }
    //   }
    //   else {
    //     return { data: -1};
    //   }  
    // })
    let res = await axios.get(`${apiURL}/Employees`);
    return res.data;
  }
  catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400) {
      return error.response;
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
    let res = await axios.get(`${apiURL}/Employees/${id}`);
    return res;
  } catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      return error.response;
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
    let res = await axios.get(`${apiURL}/Employees/NewCode`);
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
 * Kiểm tra trùng mã trước khi Insert và Update
 * @param {*} employee Đối tượng cần kiểm tra
 * @returns true - đã bị trùng, false -chưa
 * Author: PMChien
 */
export const checkEmployeeCodeBeforeCU = async (data) => {
  try {
    console.log("Dữ liệu gửi đi: ", data);
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
export const deleteEmployee = async(data) => {
  try {
    let res = await axios.delete(`${apiURL}/Employees`, data);
    return res;
  } catch (error) {
    console.error("Đã xảy ra lỗi lúc xóa danh sách Employee: ", error);
    let status = error.response.status;
    if(status >= 400) {
      return error.response;
    }
  }
}