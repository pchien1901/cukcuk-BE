﻿using core.Entities;
using Dapper;
using MISA.CUKCUK.Core;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class MISABaseRepository<T>: IBaseRepository<T> where T : class
    {
        // thông tin kết nối
        //string __dbContext.ConnectionString = "Host = 8.222.228.150;" +
        //    "Port = 3306;" +
        //    "Database = W08.PMCHIEN.MF1778;" +
        //    "User Id = nvmanh;" +
        //    "Password = 12345678;";



        //protected IDb_dbContext.Connection _dbContext.Connection;
        protected IMISADbContext _dbContext;
        protected string className;

        public MISABaseRepository(IMISADbContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// Xóa bản ghi theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>số bản ghi bị xóa</returns>
        /// Created By: PMCHIEN (27/12/2023)
        public int Delete(string id)
        {
           var result =  _dbContext.Delete<T>(id);
            return result;
        }

        /// <summary>
        /// Xóa danh sách bản ghi
        /// </summary>
        /// <param name="ids">mảng id cần xóa</param>
        /// <returns>Số bản ghi bị thay đổi</returns>
        /// Created by: PMCHIEN (03/01/2024)
        public int DeleteAny(string[] ids)
        {
            var result = _dbContext.DeleteAny<T>(ids);
            return result;
        }



        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>danh sách bản ghi</returns>
        /// Created By : PMCHIEN (27/12/2023)
        public IEnumerable<T> Get()
        {

            var res = _dbContext.Get<T>();
            return res;
        }

        /// <summary>
        /// Lấy một bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bản ghi cần lấy</returns>
        /// Created By: PMCHIEN (27/12/2023)
        public T? Get(string id)
        {
            var res = _dbContext.Get<T>(id);
            return res;
        }

        /// <summary>
        /// Thêm bản ghi vào bảng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By: PMCHIEN(27/12/2023)
        /// 
        public int Insert(T entity)
        {
            var res = _dbContext.Insert<T>(entity);
            return res;
        }


        /// <summary>
        /// Thay đổi thông tin 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Số bản ghi bị thay đổi</returns>
        /// Created By: PMCHIEN(27/12/2023)
        /// 
        public int Update(T entity)
        {
            var res = _dbContext.Update<T>(entity);
            return res;
        }
    }
}
