//using core.Entities;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Services
{
    public class PositionService: BaseService<Position>, IPositionService
    {
        #region Declaration
        IPositionRepository positionRepository;
        #endregion

        #region Constructor
        public PositionService(IPositionRepository positionRepository): base(positionRepository)
        {
            this.positionRepository = positionRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra Position trước khi thêm vào database
        /// </summary>
        /// <param name="position">Position cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created By: PMCHIEN (09/01/2024)
        protected override void ValidateObject(Position position)
        {
            var isDuplicate = positionRepository.CheckNameIsExist(position.PositionName);
            if(isDuplicate)
            {
                throw new MISAValidateException(MISAResource.PositionNameIsDuplicated); 
            }
        }

        /// <summary>
        /// Kiểm tra Position trước khi cập nhật (có tồn tại không?, có bị trùng tên không?)
        /// </summary>
        /// <param name="position">Position cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN (09/01/2024)
        protected override void ValidateUpdate(Position position)
        {
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = positionRepository.Get(position.PositionId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra customerCode có bị trùng với khách hàng khác không
                var positionName = positionRepository.GetByName(position.PositionName);
                switch (positionName.Count)
                {
                    // không có bản ghi nào trùng mã
                    case 0:
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (positionName[0].PositionId == position.PositionId)
                        {
                            break;
                        }
                        else
                        {
                            throw new MISAValidateException(Resources.MISAResource.PositionNameIsDuplicated);
                        }
                    default:
                        throw new MISAValidateException(Resources.MISAResource.PositionNameIsDuplicated);
                }

            }
        }
        #endregion
    }
}
