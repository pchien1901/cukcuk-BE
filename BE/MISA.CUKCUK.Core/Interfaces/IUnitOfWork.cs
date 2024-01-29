//using core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ICustomerRepository Customers { get; set; }
        ICustomerGroupRepository CustomerGroups { get; set; }
        IEmployeeRepository Employees { get; set; }
        public IPositionRepository Positions { get; set; }
        public IDepartmentRepository Departments { get; set; }

        /// <summary>
        /// Bắt đầu một transaction
        /// </summary>
        /// Created By: PMChien (15/01/2024)
        void BeginTransaction();

        /// <summary>
        /// Commit nếu các hành động không gặp lỗi
        /// </summary>
        /// Created by: PMChien (15/01/2024)
        void Commit();

        /// <summary>
        /// Quay lại nếu gặp lỗi
        /// </summary>
        /// Created by: PMChien (15/01/2024)
        void Rollback();
    }
}
