import axios from 'axios';
import config from '../config/config.js';

const apiURL = config.API_URL;

/**
 * gọi api lấy tất cả Position
 * @returns danh sách Position
 * Author: PMChien
 */
export const getAllPositions = async () => {
  try {
    // await axios.get(`${apiURL}/Positions`)
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
    let res = await axios.get(`${apiURL}/Positions`);
    return res.data;
  }
  catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      return error.response;
    }
  }
}

/**
 * Lấy một Position theo id
 * @param {*} id id của Position cần lấy
 * @returns Position - đối tượng có id truyên vào
 * Author: PMChien
 */
export const getPositionById = async (id) => {
  try {
    let res = await axios.get(`${apiURL}/Positions/${id}`);
    return res;
  } catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
    let status = error.response.status;
    if(status >= 400)  {
      return error.response;
    }
  }
}
