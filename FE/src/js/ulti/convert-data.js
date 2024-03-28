/**
 * Chuyển  trường Gender về chuỗi
 * @param {*} gender
 * Author: Phạm Minh Chiến (6/12/2023)
 */
function getGenderLabel(gender) {
  try {
    if (gender) {
      if (gender === 0) {
        return "Nam";
      } else if (gender === 1) {
        return "Nữ";
      } else {
        return "Khác";
      }
    }
    else if(gender === 0) {
      return "Nam";
    }
  } catch (error) {
    console.error("Đã có lỗi xảy ra: ", error);
  }
}

/**
 * Chuyển trường DateOfBirth về dạng dd/mm/yyyy
 * @param {*} dateOfBirth
 * Author: Phạm Minh Chiến (6/12/2023)
 */
function convertDateFormat(dateOfBirth) {
  try {
    if (dateOfBirth) {
      if (Date.parse(dateOfBirth)) {
        let inputDate = new Date(Date.parse(dateOfBirth));

        // lấy giá trị ngày tháng năm
        let day = inputDate.getDate().toString().padStart(2, "0");
        let month = (inputDate.getMonth() + 1).toString().padStart(2, "0"); // tháng bắt đầu từ 0 nên phải + 1
        let year = inputDate.getFullYear();

        // tạo chuỗi mới
        const outputDateString = `${day}/${month}/${year}`;
        return outputDateString;
      }
      else {
        let day = dateOfBirth.substring(0, 2);
        let month = dateOfBirth.substring(3, 5);
        let year = dateOfBirth.substring(6, 11);
        return `${day}/${month}/${year}`;
      }
    } else {
      return "";
    }
  } catch (error) {
    console.error("Đã có lỗi xảy ra: ", error);
  }
}

/**
 * Tạo ngày mới từ chuỗi
 * @param {string} date chuỗi ngày cần tạo
 * @returns đối tượng Date
 * Author" PMChien
 */
function createDateString(date) {
  try {
    if(date && date.length >= 10) {
      let dateString = date.substring(0, 10);
      let regexDate = /^\d{4}-\d{2}-\d{2}$/;
      if(regexDate.test(dateString)) {
        return dateString;
      }
      else {
        let day = date.substring(0, 2);
        let month = date.substring(3, 5);
        let year = date.substring(6, 10);
        return `${year}-${month}-${day}`;
      }
    }
    else {
      return null;
    }
  } catch (error) {
    console.error("Đã xảy ra lỗi: ", error);
  }
}

/**
 * Xóa key rỗng khỏi object
 * @param {object} data đối tượng cần xóa key rỗng or null
 * @returns object không có key rỗng hoặc null
 * Author: PMChien
 */
function removeBlankKey(data) {
  try {
    if(typeof data === 'object' && data !== null) {
      let newValue = {};
      for (const key in data) {
        if (Object.hasOwnProperty.call(data, key)) {
          if(data[key] === "")
          {
            data[key] = null;
          } else {
            newValue[key] = data[key];
          }
          
        }
      }
      return newValue;
    }
    else {
      return data;
    }
  } catch (error) {
    console.error("Đã xảy ra lỗi");
  }
}

/**
     * định dạng số với dấu phẩy hàng nghìn
     * @param {*} number
     * @returns số được định dạng
     * Author: PHẠM MINH CHIẾN (29/11/2023)
     */
function formatNumberWithCommas(number) {
  try {
    if(number) {
      // Sử dụng toLocaleString để định dạng số với dấu phẩy hàng nghìn
      return number.toLocaleString("vi-VN");
    }
    else {
      return "";
    }
  } catch (error) {
    console.error(error);
  }
}


export { getGenderLabel, convertDateFormat, createDateString, removeBlankKey, formatNumberWithCommas };
