using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface ICustomerRepository: IBaseRepository<Customer>
    {
        /// <summary>
        /// Kiểm tra Code đã tồn tại chưa
        /// </summary>
        /// <returns>true - code đã tồn tại, false - code chưa tồn tại</returns>
        /// Created By: PMCHIEN
        bool CheckCodeIsExist(string code);

        /// <summary>
        /// Lấy tất cả người dùng theo code
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        /// Created By: PMCHIEN
        List<Customer> GetByCode(string code);

    }
}
