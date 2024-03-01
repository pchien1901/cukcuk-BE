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

        ///<summary>
        ///Kiểm tra đối tượng và thêm vào database
        ///</summary>
        ///<param name="entity">Đối tượng cần kiểm tra trước khi thêm vào Database</param>
        ///<returns>
        ///MISAResponse.Success = true nếu thỏa mãn, false - không thỏa mãn
        ///</returns>
        ///Created by: PMChien (29/02/2024)
        //MISAResponse InsertService(T entity);

        ///<summary>
        ///Kiểm tra đối tượng và cập nhật vào database
        ///</summary>
        ///<param name="entity">Đối tượng cần kiểm tra trước khi cập nhật vào Database</param>
        ///<returns>
        ///MISAResponse.Success = true nếu thỏa mãn, false - không thỏa mãn
        ///</returns>
        ///Created by: PMChien (29/02/2024)
        //MISAResponse UpdateService(T entity);

        ///<summary>
        ///Kiểm tra đối tượng và xóa database
        ///</summary>
        ///<param name="entity">Đối tượng cần kiểm tra trước khi xóa vào Database</param>
        ///<returns>
        ///MISAResponse.Success = true nếu thỏa mãn, false - không thỏa mãn
        ///</returns>
        ///Created by: PMChien (29/02/2024)
        //MISAResponse DeleteService(string id);
    }
}
