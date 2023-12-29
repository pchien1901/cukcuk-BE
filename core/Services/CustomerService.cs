using core.Entities;
using Microsoft.AspNetCore.Http;
using MISA.CUKCUK.Core.DTOs;
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
    public class CustomerService : ICustomerService
    {
        ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public object ImportService(IFormFile excel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MISAServiceResult InsertService(Customer customer)
        {
            var serviceResult = new MISAServiceResult();
            var isDuplicate = _customerRepository.CheckCodeIsExist(customer.CustomerCode);
            if(isDuplicate)
            {
                throw new MISAValidateException("Mã khách hàng đã tồn tại", "CustomerCode");
            }
            serviceResult.Success = true;
            return serviceResult;

            /*
            var serviceResult = new MISAServiceResult();
            serviceResult.Success = false;
            // Check CustomerCode đã tồn tại chưa
            var isDuplicate = _customerRepository.CheckCodeIsExist(customer.CustomerCode);
            if (isDuplicate)
            {
                throw new MISAValidateException("Mã khách hàng đã tồn tại", "CustomerCode");
            }
            else
            {
                serviceResult.Success = true;
            }

            return serviceResult;
            */

            /*
            try
            {
                
                // Check CustomerCode đã tồn tại chưa
                var isDuplicate = _customerRepository.CheckCodeIsExist(customer.CustomerCode);
                if (isDuplicate == true)
                {
                    throw new MISAValidateException("Mã khách hàng đã tồn tại", "CustomerCode");
                }
                else
                {
                    serviceResult.Success = true;
                }

                return serviceResult;
            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
            }
            return serviceResult;
            */
        }

        /// <summary>
        /// Sửa đổi bản ghi
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>MiSAServiceResult - kết quả</returns>
        /// Created by: PMCHIEN(27/12/2023
        public MISAServiceResult UpdateService(Customer customer)
        {
            var serviceResult = new MISAServiceResult();
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _customerRepository.Get(customer.CustomerId.ToString());
            if (isExist == null)
            {
                serviceResult.Success = false;
            }
            else
            {
                serviceResult.Success = true;
                // Kiểm tra customerCode có bị trùng với khách hàng khác không
                var customerByCode = _customerRepository.GetByCode(customer.CustomerCode);
                switch (customerByCode.Count)
                {
                    // không có bản ghi nào trùng mã
                    case 0:
                        serviceResult.Success = true;
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (customerByCode[0].CustomerId == customer.CustomerId)
                        {
                            serviceResult.Success = true;
                        }
                        else
                        {
                            throw new MISAValidateException("Mã khách hàng đã tồn tại", "CustomerCode");
                        }
                        break;
                    default:
                        throw new MISAValidateException("Mã khách hàng đã tồn tại", "CustomerCode");
                        break;
                }

            }
            return serviceResult;
        }

        /// <summary>
        /// Kiểm tra người dùng cần xóa có tồn tại không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created By: PMCHIEN(27/12/2023)
        public MISAServiceResult DeleteService(string id)
        {
            var serviceResult = new MISAServiceResult();
            // kiểm tra xem người dùng cần xóa có tồn tại trong database không
            var isExist = _customerRepository.Get(id);

            if (isExist != null)
            {
                serviceResult.Success = true;
            }
            else
            {
                serviceResult.Success = false;
            }
            return serviceResult;
        }
    }
}
