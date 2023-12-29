using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface ICustomerGroupRepository: IBaseRepository<CustomerGroup>
    {
        /// <summary>
        /// Kiểm tra CustomerGroupName đã tồn tại hay chưa
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// Created By: PMCHIEN
        public bool CheckNameIsExist(string name);

        /// <summary>
        /// Lấy các bản ghi theo tên
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// Created by: PMCHIEN
        public List<CustomerGroup> GetByName(string name);
    }
}
