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
        /// <param name="entity"></param>
        /// <returns></returns>
        /// Created by: PMCHIEN
        MISAServiceResult InsertService(T entity);

        /// <summary>
        /// Kiểm tra đầu vào khi Put
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// Created by: PMCHIEN
        MISAServiceResult UpdateService(T entity);

        /// <summary>
        /// Kiểm tra đầu vào khi Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created by: PMCHIEN
        MISAServiceResult DeleteService(string id);
    }
}
