import tinyEmitter from "tiny-emitter/instance";

/**
 * Hàm gọi tinyEmitter không có tham số
 * @param {string} emitName Tên sự kiện để dùng tinyEmitter
 * Author: PMChien
 */
export const baseEmit = (emitName) => {
  tinyEmitter.emit(emitName);
}

/**
 * Hàm gọi tinyEmitter có tham số
 * @param {string} emitName Tên sự kiện dùng tinyEmitter
 * @param {*} data Tham số truyền
 * Author: PMChien
 */
export const baseEmitWithParam = (emitName, data) => {
  tinyEmitter.emit(emitName, data);
}