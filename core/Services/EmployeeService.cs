using core.Entities;
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
    public class EmployeeService: BaseService<Employee>, IEmployeeService
    {
        #region Declaration
        IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository): base (employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra Employee trước khi xóa
        /// </summary>
        /// <param name="id">id của Employee cần xóa</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MISAServiceResult DeleteService(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kiểm tra Employee trươc Insert
        /// </summary>
        /// <param name="employee">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN(08/01/2024)
        protected override void ValidateObject(Employee employee)
        {
            var isDuplicate = _employeeRepository.CheckCodeIsExist(employee.EmployeeCode);
            if (isDuplicate)
            {
                throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
            }
        }

        /// <summary>
        /// Kiểm tra Employee trươc Insert
        /// </summary>
        /// <param name="employee">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN (08/01/2024)
        protected override void ValidateUpdate(Employee employee)
        {
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _employeeRepository.Get(employee.EmployeeId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra employeeCode có bị trùng với nhân viên khác không
                var employeeByCode = _employeeRepository.GetByCode(employee.EmployeeCode);
                switch (employeeByCode.Count)
                {
                    // không có bản ghi nào trùng mã
                    case 0:
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (employeeByCode[0].EmployeeId == employee.EmployeeId)
                        {
                            break;
                        }
                        else
                        {
                            throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
                        }
                    default:
                        throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
                }

            }
        }

        //public MISAServiceResult InsertService(Employee employee)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    var isDuplicate = _employeeRepository.CheckCodeIsExist(employee.EmployeeCode);
        //    if (isDuplicate)
        //    {
        //        throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
        //    }
        //    serviceResult.Success = true;
        //    return serviceResult;
        //}

        //public MISAServiceResult UpdateService(Employee employee)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    // Kiểm tra bản ghi đã tồn tại chưa
        //    var isExist = _employeeRepository.Get(employee.EmployeeId.ToString());
        //    if (isExist == null)
        //    {
        //        serviceResult.Success = false;
        //    }
        //    else
        //    {
        //        serviceResult.Success = true;
        //        // Kiểm tra employeeCode có bị trùng với nhân viên khác không
        //        var employeeByCode = _employeeRepository.GetByCode(employee.EmployeeCode);
        //        switch (employeeByCode.Count)
        //        {
        //            // không có bản ghi nào trùng mã
        //            case 0:
        //                serviceResult.Success = true;
        //                break;
        //            case 1:
        //                // có 1 bản ghi trùng mã
        //                if (employeeByCode[0].EmployeeId == employee.EmployeeId)
        //                {
        //                    serviceResult.Success = true;
        //                }
        //                else
        //                {
        //                    throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
        //                }
        //                break;
        //            default:
        //                throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
        //                break;
        //        }

        //    }
        //    return serviceResult;
        //}
        #endregion
    }
}
