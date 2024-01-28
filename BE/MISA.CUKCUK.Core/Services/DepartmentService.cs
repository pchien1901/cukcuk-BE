//using core.Entities;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Services
{
    public class DepartmentService: BaseService<Department>, IDepartmentService
    {
        #region Declaration
        IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructor
        public DepartmentService(IDepartmentRepository departmentRepository): base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra Department trước khi xóa
        /// </summary>
        /// <param name="id">id của Department cần xóa</param>
        /// <returns>MISAServiceResult {Success: có thỏa mãn không, Data: Thông tin}</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Created by: PMCHIEN (08/01/2024)
        public MISAServiceResult DeleteService(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kiểm tra Department trươc khi Insert
        /// </summary>
        /// <param name="department">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lễ nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN (08/01/2024)
        protected override void ValidateObject(Department department)
        {
            var isDuplicate = _departmentRepository.CheckCodeIsExist(department.DepartmentCode);
            if (isDuplicate)
            {
                throw new MISAValidateException(Resources.MISAResource.DepartmentCodeIsDuplicated);
            }
        }

        /// <summary>
        /// Kiểm tra Department trước khi Update
        /// </summary>
        /// <param name="department">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN (08/01/2024)
        protected override void ValidateUpdate(Department department)
        {
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _departmentRepository.Get(department.DepartmentId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra departmentCode có bị trùng với đơn vị khác không
                var departmentByCode = _departmentRepository.GetByCode(department.DepartmentCode);
                switch (departmentByCode.Count)
                {
                    // không có bản ghi nào trùng mã
                    case 0:
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (departmentByCode[0].DepartmentId == department.DepartmentId)
                        {
                            break;
                        }
                        else
                        {
                            throw new MISAValidateException(Resources.MISAResource.DepartmentCodeIsDuplicated);
                        }
                    default:
                        throw new MISAValidateException(Resources.MISAResource.DepartmentCodeIsDuplicated);
                        
                }

            }
        }

        //public MISAServiceResult InsertService(Department department)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    var isDuplicate = _departmentRepository.CheckCodeIsExist(department.DepartmentCode);
        //    if (isDuplicate)
        //    {
        //        throw new MISAValidateException(Resources.MISAResource.DepartmentCodeIsDuplicated);
        //    }
        //    serviceResult.Success = true;
        //    return serviceResult;
        //}

        //public MISAServiceResult UpdateService(Department department)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    // Kiểm tra bản ghi đã tồn tại chưa
        //    var isExist = _departmentRepository.Get(department.DepartmentId.ToString());
        //    if (isExist == null)
        //    {
        //        serviceResult.Success = false;
        //    }
        //    else
        //    {
        //        serviceResult.Success = true;
        //        // Kiểm tra departmentCode có bị trùng với đơn vị khác không
        //        var departmentByCode = _departmentRepository.GetByCode(department.DepartmentCode);
        //        switch (departmentByCode.Count)
        //        {
        //            // không có bản ghi nào trùng mã
        //            case 0:
        //                serviceResult.Success = true;
        //                break;
        //            case 1:
        //                // có 1 bản ghi trùng mã
        //                if (departmentByCode[0].DepartmentId == department.DepartmentId)
        //                {
        //                    serviceResult.Success = true;
        //                }
        //                else
        //                {
        //                    throw new MISAValidateException(Resources.MISAResource.DepartmentCodeIsDuplicated);
        //                }
        //                break;
        //            default:
        //                throw new MISAValidateException(Resources.MISAResource.DepartmentCodeIsDuplicated);
        //                break;
        //        }

        //    }
        //    return serviceResult;
        //}
        #endregion
    }
}
