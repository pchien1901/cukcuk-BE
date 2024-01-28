//using core.Entities;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
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
        IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra Employee trước khi xóa
        /// </summary>
        /// <param name="id">id của Employee cần xóa</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override bool ValidateDelete(string id)
        {
            var isExist = _unitOfWork.Employees.Get(id);
            if(isExist == null)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Kiểm tra Employee trươc Insert
        /// </summary>
        /// <param name="employee">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN(08/01/2024)
        protected override void ValidateObject(Employee employee)
        {
            var isDuplicate = _unitOfWork.Employees.CheckCodeIsExist(employee.EmployeeCode);
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
            var isExist = _unitOfWork.Employees.Get(employee.EmployeeId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra employeeCode có bị trùng với nhân viên khác không
                var employeeByCode = _unitOfWork.Employees.GetByCode(employee.EmployeeCode);
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

        public string MaxCode()
        {
            var maxEmployeeCodeInDb = _unitOfWork.Employees.GetMaxCode();
            string prefix = maxEmployeeCodeInDb.Substring(0, 2);
            int number = int.Parse(maxEmployeeCodeInDb.Substring(3)) + 1;
            return $"{prefix}-{number}";
        }

        /// <summary>
        /// Kiểm tra EmployeeCode trước khi thực hiện Insert hoặc Update tại Frontend
        /// </summary>
        /// <param name="checkEmployeeCode">Đối tượng cần kiểm tra</param>
        /// <returns>true - đã tồn tại, false - chưa tồn tại</returns>
        /// Created by: PMChien
        public bool CheckEmployeeCodeBeforeCU(CheckEmployeeCode checkEmployeeCode)
        {
            var isDuplicate = false;
            // lấy tất cả bản ghi cùng code
            var employeeByCode = _unitOfWork.Employees.GetByCode(checkEmployeeCode.EmployeeCode);
            switch (employeeByCode.Count)
            {
                // không có bản ghi nào trùng => không bị trùng
                case 0:
                    isDuplicate = false;
                    break;
                /* Có  1 bản ghi trùng:
                 *      + Nếu đang create (IsCreate = true) => bị trùng
                 *      + Nếu đang update (IsUpdate = true) 
                 *          - Nếu bản ghi Db trùng bản ghi truyền vào => không trùng
                 *          - Không trùng id => trả về true
                */
                case 1:
                    if(checkEmployeeCode.IsCreate == true)
                    {
                        isDuplicate = true;
                    }
                    if(checkEmployeeCode.IsUpdate == true)
                    {
                        if(checkEmployeeCode.EmployeeId == employeeByCode[0].EmployeeId.ToString())
                        {
                            isDuplicate = false;
                        }
                        else
                        {
                            isDuplicate = true;
                        }
                    }
                    break;
                default:
                    isDuplicate = true;
                    break;
            }
            return isDuplicate;
        }

        public MISAServiceResult PageService(int page, int pageSize, string? text)
        {
            if(page < 1)
            {
                throw new MISAValidateException(Resources.MISAResource.PageError);
            }
            List<EmployeeInfo> employeeInfoByPage = _unitOfWork.Employees.GetEmployeeInfoByPage(
               (page - 1)*pageSize, pageSize, text );
            if(employeeInfoByPage.Count >= 0)
            {
                var employeeInfos = new Page<EmployeeInfo>
                {
                    ListRecord = employeeInfoByPage,
                    CurrentPage = page,
                    TotalPage = _unitOfWork.Employees.GetPageCount<EmployeeInfo>(pageSize,text)
                };
                return new MISAServiceResult
                {
                    Success = true,
                    DataObject = employeeInfos
                };
            }
            else
            {
                return new MISAServiceResult
                {
                    DataObject = null,
                    Success = false,
                };
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
