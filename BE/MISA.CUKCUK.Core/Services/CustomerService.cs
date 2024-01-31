using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        #region Declaration
        ICustomerRepository _customerRepository;
        #endregion

        #region Constructor
        public CustomerService(ICustomerRepository customerRepository, IMemoryCache memoryCache): base(customerRepository, memoryCache)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra nghiệp vụ import file
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ImportService(IFormFile excel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate nghiệp vụ của Customer khi Insert
        /// </summary>
        /// <param name="customer">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Trả về ngoại lệ khi không thỏa mãn</exception>
        /// Created by: PMCHIEN (08/01/2024)
        protected override void ValidateObject(Customer customer)
        {
            // kiểm tra CustomerCode đã có trong Database chưa
            var isDuplicate = _customerRepository.CheckCodeIsExist(customer.CustomerCode);
            if (isDuplicate)
            {
                throw new MISAValidateException(Resources.MISAResource.CustomerCodeIsDuplicated);
            }
        }

        /// <summary>
        /// Validate nghiệp vụ của Customer khi Update
        /// </summary>
        /// <param name="customer">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Trả về ngoại lệ khi không thỏa mãn</exception>
        /// Created by: PMCHIEN (08/01/2024)
        protected override void ValidateUpdate(Customer customer)
        {   
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _customerRepository.Get(customer.CustomerId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra customerCode có bị trùng với khách hàng khác không
                var customerByCode = _customerRepository.GetByCode(customer.CustomerCode);
                switch (customerByCode.Count)
                {
                    // không có bản ghi nào trùng mã
                    case 0:
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (customerByCode[0].CustomerId == customer.CustomerId)
                        {
                            break;
                        }
                        else
                        {
                            throw new MISAValidateException(Resources.MISAResource.CustomerCodeIsDuplicated);
                        }
                    default:
                        throw new MISAValidateException(Resources.MISAResource.CustomerCodeIsDuplicated);
                }

            }
        }


        ///// <summary>
        ///// Validate dữ liệu
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns>MiSAServiceResult - kết quả</returns>
        ///// 
        //public MISAServiceResult InsertService(Customer customer)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    var isDuplicate = _customerRepository.CheckCodeIsExist(customer.CustomerCode);
        //    if (isDuplicate)
        //    {
        //        throw new MISAValidateException(Resources.MISAResource.CustomerCodeIsDuplicated);
        //    }
        //    serviceResult.Success = true;
        //    return serviceResult;
        //}

        ///// <summary>
        ///// Sửa đổi bản ghi
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <returns>MiSAServiceResult - kết quả</returns>
        ///// Created by: PMCHIEN(27/12/2023)
        //public MISAServiceResult UpdateService(Customer customer)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    // Kiểm tra bản ghi đã tồn tại chưa
        //    var isExist = _customerRepository.Get(customer.CustomerId.ToString());
        //    if (isExist == null)
        //    {
        //        serviceResult.Success = false;
        //    }
        //    else
        //    {
        //        serviceResult.Success = true;
        //        // Kiểm tra customerCode có bị trùng với khách hàng khác không
        //        var customerByCode = _customerRepository.GetByCode(customer.CustomerCode);
        //        switch (customerByCode.Count)
        //        {
        //            // không có bản ghi nào trùng mã
        //            case 0:
        //                serviceResult.Success = true;
        //                break;
        //            case 1:
        //                // có 1 bản ghi trùng mã
        //                if (customerByCode[0].CustomerId == customer.CustomerId)
        //                {
        //                    serviceResult.Success = true;
        //                }
        //                else
        //                {
        //                    throw new MISAValidateException(Resources.MISAResource.CustomerCodeIsDuplicated);
        //                }
        //                break;
        //            default:
        //                throw new MISAValidateException(Resources.MISAResource.CustomerCodeIsDuplicated);
        //                break;
        //        }

        //    }
        //    return serviceResult;
        //}

        ///// <summary>
        ///// Kiểm tra người dùng cần xóa có tồn tại không
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>MiSAServiceResult - kết quả</returns>
        ///// Created By: PMCHIEN(27/12/2023)
        //public MISAServiceResult DeleteService(string id)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    // kiểm tra xem người dùng cần xóa có tồn tại trong database không
        //    var isExist = _customerRepository.Get(id);

        //    if (isExist != null)
        //    {
        //        serviceResult.Success = true;
        //    }
        //    else
        //    {
        //        serviceResult.Success = false;
        //    }
        //    return serviceResult;
        //}
        #endregion
    }
}
