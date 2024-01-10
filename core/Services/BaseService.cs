using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        #region Declaration
        protected IBaseRepository<T> repository;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<T> repository)
        {
            this.repository = repository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra nghiệp vụ trước khi xóa
        /// </summary>
        /// <param name="id">Id đối tượng cần kiểm tra</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Created by: PMCHIEN(08/01/2024)
        public MISAServiceResult DeleteService(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kiểm tra nghiệp vụ trước khi Insert và thêm dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// <returns>
        /// MISAServiceResult {
        /// Success = true - thỏa mãn, false - không thỏa mãn,
        /// Data = res (dữ liệu trả về) - thỏa mãn, null - không thỏa mãn
        /// }
        /// </returns>
        /// Created by: PMCHIEN (08/01/2024)
        public MISAServiceResult InsertService(T entity)
        {
            // xử lý nghiệp vụ trước khi insert
            ValidateObject(entity);
            // Insert dữ liệu
            var res = repository.Insert(entity);
            if (res is int)
            {
                return new MISAServiceResult
                {
                    Success = true,
                    Data = res
                };
            }
            else
            {
                return new MISAServiceResult
                {
                    Success = false,
                    Data = null
                };
            }

        }

        /// <summary>
        /// Kiểm tra nghiệp vụ trước khi Update và cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// <returns>
        /// MISAServiceResult {
        /// Success = true - thỏa mãn, false - không thỏa mãn,
        /// Data = res (dữ liệu trả về) - thỏa mãn, null - không thỏa mãn
        /// }
        /// </returns>
        /// Created by: PMCHIEN (08/01/2024)
        public MISAServiceResult UpdateService(T entity)
        {
            // xử lý nghiệp vụ trước khi update
            ValidateUpdate(entity);
            // Update dữ liệu
            var res = repository.Update(entity);
            if (res is int)
            {
                return new MISAServiceResult
                {
                    Success = true,
                    Data = res
                };
            }
            else
            {
                return new MISAServiceResult
                {
                    Success = false,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Logic kiểm tra nghiệp vụ trước khi insert
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// Created by: PMCHIEN(08/01/2024)
        protected virtual void ValidateObject(T entity) { }

        /// <summary>
        /// Sự kiện cần thực hiện sau khi Insert
        /// </summary>
        /// <param name="entity">Đối tượng vừa thêm</param>
        /// Created by: PMCHIEN (08/01/2024)
        protected virtual void AfterInsert(T entity) { }

        /// <summary>
        /// Logic kiểm tra nghiệp vụ trước khi Update
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// Created by: PMCHIEN (08/01/2024)
        protected virtual void ValidateUpdate(T entity) { }
        #endregion
    }
}
