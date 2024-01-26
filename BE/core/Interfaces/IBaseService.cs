using MISA.CUKCUK.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        /// <summary>
        /// Kiểm tra đầu vào khi post
        /// </summary>
        /// <param name="entity">Đói tượng cần kiểm tra trước khi insert</param>
        /// <returns>
        /// MISAServiceResult 
        /// (MISAServiceResult.Success = true nếu thỏa mãn, false nếu không thỏa mãn)
        /// </returns>
        /// Created by: PMCHIEN (31/12/2023)
        MISAServiceResult InsertService(T entity);

        /// <summary>
        /// Kiểm tra đầu vào khi Put
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra trước khi update</param>
        /// <returns>
        /// MISAServiceResult
        /// (MISAServiceResult.Success = true nếu thỏa mãn, false nếu không thỏa mãn)
        /// </returns>
        /// Created by: PMCHIEN (31/12/2023)
        MISAServiceResult UpdateService(T entity);

        /// <summary>
        /// Kiểm tra đầu vào khi Delete
        /// </summary>
        /// <param name="id">id của đối tượng cần kiểm tra trước khi xóa</param>
        /// <returns>
        /// MISAServiceResult 
        /// (MISAServiceResult.Success = true nếu thỏa mãn, false nếu không thỏa mãn)
        /// </returns>
        /// Created by: PMCHIEN (31/12/2023)
        MISAServiceResult DeleteService(string id);
    }
}
