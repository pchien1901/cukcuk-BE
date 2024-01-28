using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Const
{
    public static class MISAConst
    {
        // base
        public const string ERROR_EMAIL_REQUIRED = "Email không được để trống";
        public const string ERROR_EMAIL_INVALID = "Email không đúng định dạng";
        public const string ERROR_PHONENUMBER_INVALID = "Số điện thoại không đúng định dạng";
        public const string ERROR_GENDER_INVALID = "Giới tính không đúng định dạng";
        public const string ERROR_FULLNAME_REQUIRED = "Họ và tên không được để trống";
        public const string ERROR_DATEOFBIRTH_INVALID = "Ngày sinh không hợp lệ";
        public const string ERROR_CURRENCY_INVALID = "Số tiền nhập không đúng định dạng";
        public const string ERROR_DATE_INVALID = "Ngày nhập không hợp lệ";

        // Customer
        public const string ERROR_CUSTOMERCODE_REQUIRED = "Mã khách hàng không được để trống";
        public const string ERROR_DEBITAMOUT_IS_NOT_NEGATIVE = "Số tiền nợ không được âm";

        // CustomerGroup
        public const string ERROR_CUSTOMERGROUPNAME_REQUIRED = "Tên nhóm khách hàng không được bỏ trống";

        // Department
        public const string ERROR_DEPARTMENTNAME_REQUIRED = "Tên đơn vị không được để trống";

        // Employee
        public const string ERROR_EMPLOYEECODE_REQUIRED = "Mã nhân viên không được để trống";
        public const string ERROR_DEPARTMENTID_REQUIRED = "Đơn vị làm việc không được để trống";
        public const string ERROR_IDENTITYDATE_INVALID = "Ngày cấp CMTND không hợp lệ";
    }
}
