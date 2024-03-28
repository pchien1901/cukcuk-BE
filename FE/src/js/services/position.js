
 /* eslint-disable */ 
import axios from 'axios';
import config from '../config/config.js';
import { checkAuthentication } from './token.js';

const apiURL = config.API_URL;

/**
 * gọi api lấy tất cả Position
 * @returns danh sách Position
 * Author: PMChien
 */
export const getAllPositions = async () => {
  try {
    let res = await axios.get(`${apiURL}/Positions`);
    return res.data;
  }
  catch (error) {
    console.error('Đã xảy ra lỗi trong lúc lấy dữ liệu: ', error);
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
  }
}
