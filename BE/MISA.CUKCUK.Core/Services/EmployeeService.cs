//using core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
using MISA.CUKCUK.Core.DTOs.ImportDTOs;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Resources;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Services
{
    public class EmployeeService: BaseService<Employee>, IEmployeeService
    {
        #region Declaration
        IEmployeeRepository _employeeRepository;
        IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra Employee trước khi xóa
        /// </summary>
        /// <param name="id">id của Employee cần xóa</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override bool ValidateDelete(string id)
        {
            var isExist = _unitOfWork.Employees.Get(id);
            if(isExist == null)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Kiểm tra Employee trươc Insert
        /// </summary>
        /// <param name="employee">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN(08/01/2024)
        protected override void ValidateObject(Employee employee)
        {
            var isDuplicate = _unitOfWork.Employees.CheckCodeIsExist(employee.EmployeeCode);
            if (isDuplicate)
            {
                throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
            }
        }

        /// <summary>
        /// Kiểm tra Employee trươc Insert
        /// </summary>
        /// <param name="employee">Đối tượng cần kiểm tra</param>
        /// <exception cref="MISAValidateException">Ngoại lệ trả về nếu không thỏa mãn</exception>
        /// Created by: PMCHIEN (08/01/2024)
        protected override void ValidateUpdate(Employee employee)
        {
            // Kiểm tra bản ghi đã tồn tại chưa
            var isExist = _unitOfWork.Employees.Get(employee.EmployeeId.ToString());
            if (isExist == null)
            {
                throw new MISAValidateException(Resources.MISAResource.RecordsDoesNotExist);
            }
            else
            {
                // Kiểm tra employeeCode có bị trùng với nhân viên khác không
                var employeeByCode = _unitOfWork.Employees.GetByCode(employee.EmployeeCode);
                switch (employeeByCode.Count)
                {
                    // không có bản ghi nào trùng mã
                    case 0:
                        break;
                    case 1:
                        // có 1 bản ghi trùng mã
                        if (employeeByCode[0].EmployeeId == employee.EmployeeId)
                        {
                            break;
                        }
                        else
                        {
                            throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
                        }
                    default:
                        throw new MISAValidateException(Resources.MISAResource.EmployeeCodeIsDuplicated);
                }

            }
        }

        /// <summary>
        /// Lấy mã code lớn nhất trong Db trả về mã mới + 1
        /// </summary>
        /// <returns></returns>
        public string MaxCode()
        {
            var maxEmployeeCodeInDb = _unitOfWork.Employees.GetMaxCode();
            string prefix = maxEmployeeCodeInDb.Substring(0, 2);
            int number = int.Parse(maxEmployeeCodeInDb.Substring(3)) + 1;
            return $"{prefix}-{number}";
        }

        /// <summary>
        /// Kiểm tra EmployeeCode trước khi thực hiện Insert hoặc Update tại Frontend
        /// </summary>
        /// <param name="checkEmployeeCode">Đối tượng cần kiểm tra</param>
        /// <returns>true - đã tồn tại, false - chưa tồn tại</returns>
        /// Created by: PMChien
        public bool CheckEmployeeCodeBeforeCU(CheckEmployeeCode checkEmployeeCode)
        {
            var isDuplicate = false;
            // lấy tất cả bản ghi cùng code
            var employeeByCode = _unitOfWork.Employees.GetByCode(checkEmployeeCode.EmployeeCode);
            switch (employeeByCode.Count)
            {
                // không có bản ghi nào trùng => không bị trùng
                case 0:
                    isDuplicate = false;
                    break;
                /* Có  1 bản ghi trùng:
                 *      + Nếu đang create (IsCreate = true) => bị trùng
                 *      + Nếu đang update (IsUpdate = true) 
                 *          - Nếu bản ghi Db trùng bản ghi truyền vào => không trùng
                 *          - Không trùng id => trả về true
                */
                case 1:
                    if(checkEmployeeCode.IsCreate == true)
                    {
                        isDuplicate = true;
                    }
                    if(checkEmployeeCode.IsUpdate == true)
                    {
                        if(checkEmployeeCode.EmployeeId == employeeByCode[0].EmployeeId.ToString())
                        {
                            isDuplicate = false;
                        }
                        else
                        {
                            isDuplicate = true;
                        }
                    }
                    break;
                default:
                    isDuplicate = true;
                    break;
            }
            return isDuplicate;
        }

        /// <summary>
        /// Validate file trước khi import
        /// </summary>
        /// <param name="fileImport">file excel</param>
        /// <returns>Danh sách thông tin nhân viên từ file và trạng thái có thể thêm hay không</returns>
        public IEnumerable<EmployeeImport> ValidateImportService(IFormFile fileImport)
        {
            CheckFileImport(fileImport);
            var employeeInfoList = new List<EmployeeImport>();
            using (var stream = new MemoryStream())
            {
                // Copy tệp vào stream
                fileImport.CopyTo(stream);
                // thực hiện đọc dữ liệu
                using(var package = new ExcelPackage(stream))
                {
                    // Sheet đọc dữ liệu
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    if(worksheet != null)
                    {
                        // tổng số dòng dữ liệu
                        var rowCount = worksheet.Dimension.Rows;
                        // bắt đầu đọc dữ liệu từ dòng thứ 3
                        for(int row = 3; row <= rowCount; row++)
                        {
                            // nếu ô STT trống thì bỏ qua dòng đó
                            var index = worksheet.Cells[row, 1]?.Value?.ToString()?.Trim();
                            if(String.IsNullOrEmpty(index))
                            {
                                continue;
                            }
                            // Các trường cần được format lại
                            var dateOfBirth = worksheet.Cells[row, 9]?.Value?.ToString()?.Trim();
                            var identityDate = worksheet.Cells[row, 8]?.Value?.ToString()?.Trim();
                            var positionName = worksheet.Cells[row, 4]?.Value?.ToString()?.Trim();
                            var departmentName = worksheet.Cells[row, 5]?.Value?.ToString()?.Trim();
                            var gender = worksheet.Cells[row, 10]?.Value?.ToString()?.Trim();
                            var employeeCode = worksheet.Cells[row, 2]?.Value?.ToString()?.Trim();

                            // kết quả validate date, nếu thất bại thì success bằng false 
                            var dobString = ValidateDate(dateOfBirth);
                            var iDateString = ValidateDate(identityDate);
                            var checkDepartment = CheckDepartmentName(departmentName);
                            var checkPosition = CheckPositionName(positionName);
                            var checkEmployeeCode = _unitOfWork.Employees.CheckCodeIsExist(employeeCode);

                            var dobInExcel = worksheet.Cells[row, 9]?.Value?.ToString()?.Trim();
                            var identityDateInExcel = worksheet.Cells[row, 8]?.Value?.ToString()?.Trim();
                            if (dobString.DataObject is ExcelDate excelDate )
                            {
                                dobInExcel = excelDate.DateString;
                            }

                            if(iDateString.DataObject is ExcelDate identityExcel)
                            {
                                identityDateInExcel = identityExcel.DateString;
                            }

                            // Tạo đối tượng EmployeeImport
                            var validateEmployee = new EmployeeImport
                            {
                                EmployeeCode = employeeCode,
                                FullName = worksheet.Cells[row, 3]?.Value?.ToString()?.Trim(),
                                PositionName = positionName,
                                Departmentname = departmentName,
                                IdentityNumber = worksheet.Cells[row, 6]?.Value?.ToString()?.Trim(),
                                IdentityPlace = worksheet.Cells[row, 7]?.Value?.ToString()?.Trim(),
                                IdentityDateString = identityDateInExcel,
                                DateOfBirthString = dobInExcel,
                                GenderString = worksheet.Cells[row, 10]?.Value?.ToString()?.Trim(),
                                Nationality = worksheet.Cells[row, 11]?.Value?.ToString()?.Trim(),
                                Ethnicity = worksheet.Cells[row, 12]?.Value?.ToString()?.Trim(),
                                MobilePhoneNumber = worksheet.Cells[row, 13]?.Value?.ToString()?.Trim(),
                                Email = worksheet.Cells[row, 14]?.Value?.ToString()?.Trim(),
                                ContractNumber = worksheet.Cells[row, 15]?.Value?.ToString()?.Trim(),
                                ContractType = worksheet.Cells[row, 16]?.Value?.ToString()?.Trim(),
                                Salary = decimal.TryParse(worksheet.Cells[row, 17]?.Value?.ToString()?.Trim(), out decimal salaryNumber) ? 
                                        salaryNumber : null,
                                Address = worksheet.Cells[row, 18]?.Value?.ToString()?.Trim(),
                            };

                            // khởi tạo mặc định có thể import
                            validateEmployee.CanImported = true;

                            // kiểm tra employeeCode đã có trong Db chưa
                            if(checkEmployeeCode)
                            {
                                validateEmployee.ImportInvalidErrors.Add(MISAEmployeeImportResource.EmployeeCodeDuplicateInDB);
                                validateEmployee.CanImported = false;
                            }

                            // kiểm tra phòng ban department name
                            if(checkDepartment.Success)
                            {
                                // Nếu hợp lệ thì thêm DepartmentId
                                if(checkDepartment.DataObject is EmployeeImport ex)
                                {
                                    validateEmployee.DepartmentId = ex.DepartmentId;
                                }
                            }
                            else
                            {
                                validateEmployee.ImportInvalidErrors.Add(MISAEmployeeImportResource.DepartmentNotExist);
                                validateEmployee.CanImported = false;

                            }

                            // kiểm tra chức vụ Position
                            if (checkPosition.Success)
                            {
                                // Nếu hợp lệ thì thêm PositionId
                                if(checkPosition.DataObject is EmployeeImport ex)
                                {
                                    validateEmployee.PositionId = ex.PositionId;
                                }
                            }
                            else
                            {
                                validateEmployee.ImportInvalidErrors.Add(MISAEmployeeImportResource.PositionNotExist);
                                validateEmployee.CanImported = false;

                            }

                            //Thêm ngày sinh nếu ngày sinh hợp lệ
                            if (dobString.Success)
                            {
                                if(dobString.DataObject is ExcelDate ex)
                                {
                                    validateEmployee.DateOfBirth = ex.Date;
                                }
                            }

                            // Thêm ngày cấp CMTND nếu hợp lệ
                            if(iDateString.Success)
                            {
                                if(iDateString.DataObject is ExcelDate ex)
                                {
                                    validateEmployee.IdentityDate = ex.Date;
                                }
                            }

                            employeeInfoList.Add(validateEmployee);
                        }

                        // Kiểm tra mã bị trùng trong tệp chưa
                        // Sử dụng Dictionary để theo dõi số lần xuất hiện của từng mã EmployeeCode
                        Dictionary<string, int> employeeCodeCounts = new Dictionary<string, int>();

                        // Duyệt qua mảng và đếm số lần xuất hiện của từng mã EmployeeCode
                        foreach (EmployeeImport employeeImport in employeeInfoList)
                        {
                            if(string.IsNullOrEmpty(employeeImport.EmployeeCode))
                            {
                                employeeImport.CanImported = false;
                                employeeImport.ImportInvalidErrors.Add(MISAEmployeeImportResource.EmployeeCodeIsRequired);
                            }
                            else
                            {
                                if (employeeCodeCounts.ContainsKey(employeeImport.EmployeeCode))
                                {
                                    employeeCodeCounts[employeeImport.EmployeeCode]++;
                                }
                                else
                                {
                                    employeeCodeCounts[employeeImport.EmployeeCode] = 1;
                                }
                            }
                            
                        }

                        // Duyệt qua mảng và cập nhật CanImport dựa trên số lần xuất hiện
                        foreach (EmployeeImport employeeImport in employeeInfoList)
                        {
                            if(employeeCodeCounts[employeeImport.EmployeeCode] > 1)
                            {
                                employeeImport.CanImported = false;
                                employeeImport.ImportInvalidErrors.Add(MISAEmployeeImportResource.EmployeeCodeDuplicateInFile);
                            }
                            
                        }
                    }
                }
            }
            return employeeInfoList;
        }

        /// <summary>
        /// Thực hiện thêm các thông tin từ file vào database
        /// </summary>
        /// <param name="employeeList">Danh sách EmployeeImport đã gửi từ trước cho Frontend</param>
        /// <returns>
        /// MISAServiceResult { Success = true nếu thêm thành công, Data = số bản ghi thay đổi }
        /// </returns>
        /// Created by: PMChien
        public MISAServiceResult ImportEmployee(List<EmployeeImport> employeeList)
        {
            var result = 0;
            foreach (var employeeImport in employeeList)
            {
                if(employeeImport.CanImported == true)
                {
                    var employee = mapper.Map<Employee>(employeeImport);
                    var res = _unitOfWork.Employees.Insert(employee);
                    result += res;
                }
            }
            if (result == 0)
            {
                return new MISAServiceResult
                {
                    Success = false,
                    Data = result
                };
            }
            else
            {
                return new MISAServiceResult
                {
                    Success = true,
                    Data = result
                };
            }
        }

        /// <summary>
        /// Kiểm tra ngày có đúng định dạng không
        /// </summary>
        /// <param name="date">Chuỗi ngày được đọc từ excel</param>
        /// <returns>MISAServiceResult 
        /// {   Success = true - nếu thỏa mãn, 
        ///     DataObject = { date= giá trị ngày thỏa mãn}
        ///  }
        /// </returns>
        /// Created by: PMChien
        private MISAServiceResult ValidateDate(string? dateInput)
        {
            if (String.IsNullOrEmpty(dateInput))
            {
                return new MISAServiceResult
                {
                    Success = true,
                    DataObject = new ExcelDate { DateString = dateInput },
                };
            }
            else
            {
                if(dateInput.Length > 10)
                {
                    dateInput = dateInput.Substring(0, 10);
                }
                switch (dateInput.Length)
                {
                    case 4:
                        Regex regex = new Regex(@"^\d{4}$");
                        if (regex.IsMatch(dateInput))
                        {
                            var year = int.Parse(dateInput);
                            return new MISAServiceResult
                            {
                                Success = true,
                                DataObject = new ExcelDate { 
                                    Date = new DateTime(year, 1, 1),
                                    DateString = $"01/01/{year}"
                                }
                            };
                        }
                        else
                        {
                            return new MISAServiceResult
                            {
                                Success = false,
                                DataObject = new ExcelDate{ DateString = dateInput }
                            };
                        }
                    case 7:
                        // Biểu thức chính quy kiểm tra định dạng mm/yyyy
                        Regex regexMM_YYYY = new Regex(@"^(0[1-9]|1[0-2])/\d{4}$");
                        if (regexMM_YYYY.IsMatch(dateInput))
                        {
                            int slashIndex = dateInput.IndexOf('/');
                            if (slashIndex != -1 && slashIndex < dateInput.Length - 1)
                            {
                                string monthString = dateInput.Substring(0, slashIndex);
                                string yearString = dateInput.Substring(slashIndex + 1);

                                var year = int.Parse(yearString);
                                var month = int.Parse(monthString);
                                return new MISAServiceResult
                                {
                                    Success = true,
                                    DataObject = new ExcelDate { 
                                        Date = new DateTime(year, month, 1),
                                        DateString = $"01/{month}/{year}"
                                    }
                                };
                            }
                            else
                            {
                                return new MISAServiceResult
                                {
                                    Success = false,
                                    DataObject = new ExcelDate { DateString = dateInput }
                                };
                            }
                        }
                        else
                        {
                            return new MISAServiceResult
                            {
                                Success = false,
                                DataObject = new ExcelDate { DateString = dateInput }
                            };
                        }
                    case 10:
                        // Biểu thức chính quy kiểm tra định dạng dd/mm/yyyy
                        Regex regexDDMMYYYY = new Regex(@"^(\d{2})/(\d{2})/(\d{4})$");
                        Match match = regexDDMMYYYY.Match(dateInput);

                        if (match.Success)
                        {
                            int day = int.Parse(match.Groups[1].Value);
                            int month = int.Parse(match.Groups[2].Value);
                            int year = int.Parse(match.Groups[3].Value);

                            if (day >= 1 && day <= 31 && month >= 1 && month <= 12)
                            {
                                // Kiểm tra xem ngày, tháng, năm có hợp lệ không (ví dụ: không phải ngày 31 tháng 02)
                                if (day <= DateTime.DaysInMonth(year, month))
                                {
                                    var tempDate = new DateTime(year, month, day);
                                    return new MISAServiceResult
                                    {
                                        Success = true,
                                        DataObject = new ExcelDate { 
                                            Date = tempDate,
                                            DateString = tempDate.ToString("dd/MM/yyyy")
                                        }
                                    };
                                }
                                else
                                {
                                    return new MISAServiceResult
                                    {
                                        Success = false,
                                        DataObject = new ExcelDate { DateString = dateInput }
                                    };
                                }
                            }
                            else
                            {
                                return new MISAServiceResult
                                {
                                    Success = false,
                                    DataObject = new ExcelDate { DateString = dateInput }
                                };
                            }
                        }
                        else
                        {
                            return new MISAServiceResult
                            {
                                Success = false,
                                DataObject = new ExcelDate { DateString = dateInput }
                            };
                        }
                    default:
                        return new MISAServiceResult
                        {
                            Success = false,
                            DataObject = new ExcelDate { DateString = dateInput }
                        };
                        break;
                }
            }
        }

        /// <summary>
        /// Kiểm tra PositionName có trong hệ thống không
        /// </summary>
        /// <param name="name">Tên cần kiểm tra</param>
        /// <returns>MISAServiceResult Success = true - có tồn tại</returns>
        /// Created by: PMChien
        private MISAServiceResult CheckPositionName(string name)
        {
            List<Position> positions = _unitOfWork.Positions.Get().ToList();
            foreach (var position in positions)
            {
                if(position.PositionName == name)
                {
                    return new MISAServiceResult
                    {
                        Success = true,
                        DataObject = new EmployeeImport{ PositionId = position.PositionId }
                    };
                }
                else
                {
                    continue;
                }
            }

            return new MISAServiceResult
            {
                Success = false,
                DataObject = new EmployeeImport{ PositionId = null }
            };
        }

        /// <summary>
        /// Kiểm tra PositionName có trong hệ thống không
        /// </summary>
        /// <param name="name">Tên cần kiểm tra</param>
        /// <returns>MISAServiceResult</returns>
        /// Created by: PMChien
        private MISAServiceResult CheckDepartmentName(string name)
        {
            List<Department> departments = _unitOfWork.Departments.Get().ToList();
            foreach (var department in departments)
            {
                if (department.DepartmentName == name)
                {
                    return new MISAServiceResult
                    {
                        Success = true,
                        DataObject = new EmployeeImport{ DepartmentId = department.DepartmentId }
                    };
                }
                else
                {
                    continue;
                }
            }

            return new MISAServiceResult
            {
                Success = false,
                DataObject = new EmployeeImport{ DepartmentId = null }
            };
        }

        /// <summary>
        /// Xử lí phân trang
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi / trang</param>
        /// <param name="text">chuỗi tìm kiếm</param>
        /// <returns>
        /// MISAServiceResult 
        /// {    
        ///     Success: true - nếu phân trang thành công, 
        ///     DataObject: object chưa thông tin phân trang Page 
        ///     { CurrentPage - trang hiện tại, ListRecord: danh sách bản ghi, TotalPage: tổng số trang}
        /// }
        /// </returns>
        /// Created by: PMChien
        public MISAServiceResult PageService(int page, int pageSize, string? text)
        {
            if(page < 1)
            {
                //throw new MISAValidateException(Resources.MISAResource.PageError);
                page = 1;
            }
            List<EmployeeInfo> employeeInfoByPage = _unitOfWork.Employees.GetEmployeeInfoByPage(
               (page - 1)*pageSize, pageSize, text );
            if(employeeInfoByPage.Count >= 0)
            {
                var employeeInfos = new Page<EmployeeInfo>
                {
                    ListRecord = employeeInfoByPage,
                    CurrentPage = page,
                    TotalPage = _unitOfWork.Employees.GetPageCount<EmployeeInfo>(pageSize,text),
                    TotalRecord = _unitOfWork.Employees.CountSearchRecord(text)
                };
                return new MISAServiceResult
                {
                    Success = true,
                    DataObject = employeeInfos
                };
            }
            else
            {
                return new MISAServiceResult
                {
                    DataObject = null,
                    Success = false,
                };
            }
        }

        /// <summary>
        /// Kiểm tra file nhập có đúng định dạng excel khong
        /// </summary>
        /// <param name="fileImport"></param>
        /// <exception cref="MISAValidateException"></exception>
        private void CheckFileImport(IFormFile fileImport)
        {
            if (fileImport == null || fileImport.Length <= 0)
            {
                throw new MISAValidateException("File nhập khẩu không được để trống");
            }
            if(!Path.GetExtension(fileImport.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new MISAValidateException("File nhập khẩu phải là file Excel.");
            }
        }

       
        #endregion
    }
}
