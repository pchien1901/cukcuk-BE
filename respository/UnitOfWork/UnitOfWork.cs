using core.Entities;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Services;
using MISA.CUKCUK.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        IMISADbContext _dbContext;
        public ICustomerRepository Customers { get; set; }
        public ICustomerGroupRepository CustomerGroups { get; set; }
        public IEmployeeRepository Employees { get; set; }
        bool _disposed;

        public UnitOfWork(IMISADbContext dbContext, ICustomerRepository customerRepository, ICustomerGroupRepository customerGroups, IEmployeeRepository employees)
        {
            _dbContext = dbContext;
            Customers = customerRepository;
            CustomerGroups = customerGroups;
            Employees = employees;
        }

        public void BeginTransaction()
        {
            _dbContext.Connection.Open();
            _dbContext.Transaction = _dbContext.Connection.BeginTransaction();
        }

        public void Commit()
        {
            if(_dbContext.Transaction != null)
            {
                _dbContext.Transaction.Commit();
                Dispose();
            }
        }

        public void Dispose()
        {
            if(_dbContext.Connection != null)
            {
                _dbContext.Connection.Dispose();
            }
        }

        public void Rollback()
        {
            if(_dbContext.Transaction != null)
            {
                _dbContext.Transaction.Rollback();
                Dispose();
            }
        }
    }
}
