/* eslint-disable */
import axios from "axios";
import config from "../config/config.js";
import { checkAuthentication } from "./token.js";

const apiURL = config.API_URL;

/**
 * gọi api lấy tất cả Department
 * @returns danh sách Department
 * Author: PMChien
 */
export const getAllDepartments = async () => {
  try {
    // await axios.get(`${apiURL}/Departments`)
    // .then(response => {
    //   console.log("response tại get allemployee: ", response);
    //   return response;
    // })
    // .catch(error => {
    //   console.log("error tại get all employee: ", error);
    //   if(error.response) {
    //     if(error.response.status) {
    //       return error.response;
    //     }
    //   }
    //   else {
    //     return { data: -1};
    //   }
    // })
    let res = await axios.get(`${apiURL}/Departments`);
    return res.data;
  } catch (error) {
    console.error("Đã xảy ra lỗi trong lúc lấy dữ liệu: ", error);
    let status = error.response.status;
    if (status >= 400) {
      return error.response;
    }
  }
};

/**
 * Lấy một Department theo id
 * @param {*} id id của Department cần lấy
 * @returns Department - đối tượng có id truyên vào
 * Author: PMChien
 */
export const getDepartmentById = async (id) => {
  try {
    let res = await axios.get(`${apiURL}/Departments/${id}`);
    return res;
  } catch (error) {
    console.error("Đã xảy ra lỗi trong lúc lấy dữ liệu: ", error);
    let status = error.response.status;
    if (status >= 400) {
      return error.response;
    }
  }
};
