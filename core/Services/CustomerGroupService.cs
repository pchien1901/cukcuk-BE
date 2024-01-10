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
    public class CustomerGroupService : BaseService<CustomerGroup>, ICustomerGroupService
    {
        #region Declaration
        ICustomerGroupRepository _customerGroupRepository;
        #endregion

        #region Constructor
        public CustomerGroupService(ICustomerGroupRepository customerGroupRepository): base(customerGroupRepository)
        {
            _customerGroupRepository = customerGroupRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra nghiệp vụ CustomerGroup trước khi Insert
        /// </summary>
        /// <param name="customerGroup">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN(08/01/2024)
        protected override void ValidateObject(CustomerGroup customerGroup)
        {
            // Kiểm tra CustomerGroupName đã tồn tại trong database chưa
            var isExistName = _customerGroupRepository.CheckNameIsExist(customerGroup.CustomerGroupName);

            if (isExistName == true)
            {
                throw new MISAValidateException(Resources.MISAResource.CustomerGroupNameIsDuplicated);
            }
        }

        /// <summary>
        /// Kiểm tra nghiệp vụ CustomerGroup trước khi Update
        /// </summary>
        /// <param name="customerGroup">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN(08/01/2024)
        protected override void ValidateUpdate(CustomerGroup customerGroup)
        {
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _customerGroupRepository.Get(customerGroup.CustomerGroupId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra customerGroupName có bị trùng không
                var customerGroupByName = _customerGroupRepository.GetByName(customerGroup.CustomerGroupName);
                switch (customerGroupByName.Count)
                {
                    // không có bản ghi nào trùng
                    case 0:
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (customerGroupByName[0].CustomerGroupId == customerGroup.CustomerGroupId)
                        {
                            break;
                        }
                        else
                        {
                            throw new MISAValidateException(Resources.MISAResource.CustomerGroupNameIsDuplicated);
                        }
                    default:
                        throw new MISAValidateException(Resources.MISAResource.CustomerGroupNameIsDuplicated);
                }

            }
        }

        ///// <summary>
        ///// Kiểm tra dữ liệu trước khi thêm vào 
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns>MISAServiceResult.Success = true - thỏa mãn</returns>
        ///// 
        //public MISAServiceResult InsertService(CustomerGroup customerGroup)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    serviceResult.Success = true;
        //    var isExistName = _customerGroupRepository.CheckNameIsExist(customerGroup.CustomerGroupName);

        //    if (isExistName == true)
        //    {
        //        throw new MISAValidateException(Resources.MISAResource.CustomerGroupNameIsDuplicated);
        //    }

        //    return serviceResult;
        //}

        ///// <summary>
        ///// Kiểm tra dữ liệu trước khi update
        ///// </summary>
        ///// <param name="customerGroup"></param>
        ///// <returns>MISAServiceResult.Success = true - thỏa mãn </returns>
        ///// Created by: PMCHIEN
        //public MISAServiceResult UpdateService(CustomerGroup customerGroup)
        //{
        //    var serviceResult = new MISAServiceResult();
        //    // Kiểm tra bản ghi đã tồn tại chưa
        //    var isExist = _customerGroupRepository.Get(customerGroup.CustomerGroupId.ToString());
        //    if (isExist == null)
        //    {
        //        serviceResult.Success = false;
        //    }
        //    else
        //    {
        //        serviceResult.Success = true;
        //        // Kiểm tra customerGroupName có bị trùng không
        //        var customerGroupByName = _customerGroupRepository.GetByName(customerGroup.CustomerGroupName);
        //        switch (customerGroupByName.Count)
        //        {
        //            // không có bản ghi nào trùng
        //            case 0:
        //                serviceResult.Success = true;
        //                break;
        //            case 1:
        //                // có 1 bản ghi trùng mã
        //                if (customerGroupByName[0].CustomerGroupId == customerGroup.CustomerGroupId)
        //                {
        //                    serviceResult.Success = true;
        //                }
        //                else
        //                {
        //                    throw new MISAValidateException(Resources.MISAResource.CustomerGroupNameIsDuplicated);
        //                }
        //                break;
        //            default:
        //                throw new MISAValidateException(Resources.MISAResource.CustomerGroupNameIsDuplicated);
        //                break;
        //        }

        //    }
        //    return serviceResult;
        //}

        ///// <summary>
        ///// Kiểm tra dữ liệu trước khi update
        ///// </summary>
        ///// <param name="id">id bản ghi cần xóa</param>
        ///// <returns>MISAServiceResult.Success = true - thỏa mãn </returns>
        ///// Created by: PMCHIEN
        //public MISAServiceResult DeleteService(string id)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
