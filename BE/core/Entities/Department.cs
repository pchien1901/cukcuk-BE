using MISA.CUKCUK.Core.Const;
using MISA.CUKCUK.Core.CustomValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Entities
{
    public class Department
    {
        /// <summary>
        /// id đơn vị
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// Tên đơn vị (Bắt buộc)
        /// </summary>
        /// 
        [MISARequired(ErrorMessage = MISAConst.ERROR_DEPARTMENTNAME_REQUIRED)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
