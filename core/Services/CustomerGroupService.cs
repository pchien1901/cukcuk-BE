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
    public class CustomerGroupService : ICustomerGroupService
    {
        ICustomerGroupRepository _customerGroupRepository;
        public CustomerGroupService(ICustomerGroupRepository customerGroupRepository)
        {
            _customerGroupRepository = customerGroupRepository;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi thêm vào 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>MISAServiceResult.Success = true - thỏa mãn</returns>
        /// 
        public MISAServiceResult InsertService(CustomerGroup customerGroup)
        {
            var serviceResult = new MISAServiceResult();
            serviceResult.Success = true;
            var isExistName = _customerGroupRepository.CheckNameIsExist(customerGroup.CustomerGroupName);

            if (isExistName == true)
            {
                throw new MISAValidateException("Tên nhóm khách hàng đã tồn tại", "CustomerGroupName");
            }

            return serviceResult;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trước khi update
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns>MISAServiceResult.Success = true - thỏa mãn </returns>
        /// Created by: PMCHIEN
        public MISAServiceResult UpdateService(CustomerGroup customerGroup)
        {
            var serviceResult = new MISAServiceResult();
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _customerGroupRepository.Get(customerGroup.CustomerGroupId.ToString());
            if (isExist == null)
            {
                serviceResult.Success = false;
            }
            else
            {
                serviceResult.Success = true;
                // Kiểm tra customerGroupName có bị trùng không
                var customerGroupByName = _customerGroupRepository.GetByName(customerGroup.CustomerGroupName);
                switch (customerGroupByName.Count)
                {
                    // không có bản ghi nào trùng
                    case 0:
                        serviceResult.Success = true;
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (customerGroupByName[0].CustomerGroupId == customerGroup.CustomerGroupId)
                        {
                            serviceResult.Success = true;
                        }
                        else
                        {
                            throw new MISAValidateException("Tên nhóm khách hàng đã tồn tại", "CustomerGroupName");
                        }
                        break;
                    default:
                        throw new MISAValidateException("Tên nhóm khách hàng đã tồn tại", "CustomerGroupName");
                        break;
                }

            }
            return serviceResult;
        }

        public MISAServiceResult DeleteService(string id)
        {
            throw new NotImplementedException();
        }
    }
}
