using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.UnitTests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper Mapper { get; set; }
        public IMemoryCache MemoryCache { get; set; }
        public EmployeeService EmployeeService { get; set; }


        [SetUp]
        public void SetUp()
        {
            EmployeeRepository = Substitute.For<IEmployeeRepository>();
            UnitOfWork = Substitute.For<IUnitOfWork>();
            Mapper = Substitute.For<IMapper>();
            EmployeeService = Substitute.For<EmployeeService>(EmployeeRepository, UnitOfWork, MemoryCache);
        }

        [Test]     
        ///<summary>
        ///Test hàm MaxCode trả về mã code đúng
        /// </summary>
        /// Created by: PMChien
        public void MaxCode_ReturnCorrectCode()
        {
            // Arrange
            var maxEmployeeCodeInDb = "NV-9000"; // Example
            var expectEmployeeCode = "NV-9001";
            UnitOfWork.Employees.GetMaxCode().Returns(maxEmployeeCodeInDb);

            // Act
            var result = EmployeeService.MaxCode();

            // Assert
            Assert.That(expectEmployeeCode, Is.EqualTo(result));
        }

        [Test]
        /// <summary>
        /// kiểm tra hàm CheckEmployeeCodeBeforeCU trường hợp Create trả về false
        /// </summary>
        /// Created by: PMChien
        public void CheckEmployeeCodeBeforeCU_IsCreate_NoDuplicate_ReturnsFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var checkEmployeeCode = new CheckEmployeeCode
            {
                EmployeeId = id.ToString(),
                EmployeeCode = "NV-0001",
                IsCreate = true,
                IsUpdate = false
            };
            var expectOutput = false;

            UnitOfWork.Employees.GetByCode("NV-0001").Returns(new List<Employee>());

            // Act
            var result = EmployeeService.CheckEmployeeCodeBeforeCU(checkEmployeeCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectOutput));
        }

        [Test]
        /// <summary>
        /// kiểm tra hàm CheckEmployeeCodeBeforeCU trường hợp Create trả về true
        /// </summary>
        /// Created by: PMChien
        public void CheckEmployeeCodeBeforeCU_IsCreate_HaveDuplicate_ReturnsTrue()
        {
            // Arrange
            var expectOutput = true;

            // tạo id 
            var id = Guid.NewGuid();
            // Đối tượng chứa Id gửi lên
            var checkEmployeeCode = new CheckEmployeeCode
            {
                EmployeeId = id.ToString(),
                EmployeeCode = "NV-0001",
                IsCreate = true,
                IsUpdate = false
            };

            // kết quả trả về của hàm GetByCode là một list có ít nhất 1 Employee cùng EmployeeId
            var duplicateEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                EmployeeCode = "NV-0001"
            };

            UnitOfWork.Employees.GetByCode("NV-0001").Returns(new List<Employee> { duplicateEmployee });

            // Act
            var result = EmployeeService.CheckEmployeeCodeBeforeCU(checkEmployeeCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectOutput));
        }

        [Test]
        /// <summary>
        ///  Kiểm tra hàm CheckEmployeeCodeBeforeCU trường hợp Update và không có bản ghi trùng code
        /// </summary>
        /// Created by: PMChien
        public void CheckEmployeeCodeBeforeCU_IsUpdate_NoDuplicate_ReturnsFalse()
        {
            // Arrange
            var expectOutput = false;
            var id = Guid.NewGuid();
            var checkEmployeeCode = new CheckEmployeeCode
            {
                EmployeeId = id.ToString(),
                EmployeeCode = "NV-1234",
                IsCreate = false,
                IsUpdate = true
            };
            UnitOfWork.Employees.GetByCode("NV-1234").Returns(new List<Employee>());

            // Act
            var result = EmployeeService.CheckEmployeeCodeBeforeCU(checkEmployeeCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectOutput));
        }

        [Test]
        /// <summary>
        ///  Kiểm tra hàm CheckEmployeeCodeBeforeCU trường hợp Update cho chính EmployeeId đó
        /// </summary>
        /// Created by: PMChien
        public void CheckEmployeeCodeBeforeCU_IsUpdate_UpdateForThisEmployeeId_ReturnsFalse()
        {
            // Arrange
            var expectOutput = false;
            var id = Guid.NewGuid();
            var checkEmployeeCode = new CheckEmployeeCode
            {
                EmployeeId = id.ToString(),
                EmployeeCode = "NV-1234",
                IsCreate = false,
                IsUpdate = true
            };
            var employeeInDb = new Employee
            {
                EmployeeId = id,
                EmployeeCode = "NV-1234"
            };
            UnitOfWork.Employees.GetByCode("NV-1234").Returns(new List<Employee> { employeeInDb });

            // Act
            var result = EmployeeService.CheckEmployeeCodeBeforeCU(checkEmployeeCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectOutput));
        }

        [Test]
        /// <summary>
        ///  Kiểm tra hàm CheckEmployeeCodeBeforeCU trường hợp Update có bản ghi trùng 
        /// </summary>
        /// Created by: PMChien
        public void CheckEmployeeCodeBeforeCU_IsUpdate_HaveDuplicateRecord_ReturnsTrue()
        {
            // Arrange
            var expectOutput = true;
            var id = Guid.NewGuid();
            var checkEmployeeCode = new CheckEmployeeCode
            {
                EmployeeId = id.ToString(),
                EmployeeCode = "NV-1234",
                IsCreate = false,
                IsUpdate = true
            };
            var employeeInDb = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                EmployeeCode = "NV-1234"
            };
            UnitOfWork.Employees.GetByCode("NV-1234").Returns(new List<Employee> { employeeInDb });

            // Act
            var result = EmployeeService.CheckEmployeeCodeBeforeCU(checkEmployeeCode);

            // Assert
            Assert.That(result, Is.EqualTo(expectOutput));
        }

        [Test]
        public void PageService_ReturnsSuccessWithValidData()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string text = "search text";

            var employeeInfoList = new List<EmployeeInfo>
        {
            new EmployeeInfo { /* properties */ },
            new EmployeeInfo { /* properties */ },
            // Add more employee info objects as needed
        };

            var employeeInfoPage = new Page<EmployeeInfo>
            {
                ListRecord = employeeInfoList,
                CurrentPage = 1,
                TotalPage = 3,
                TotalRecord = 25,
            };

            UnitOfWork.Employees.GetEmployeeInfoByPage(Arg.Any<int>(), Arg.Any<int>(), text)
                .Returns(employeeInfoPage);


            // Act
            var result = EmployeeService.PageService(page, pageSize, text);

            // Assert
            Assert.IsTrue(result.Success); // Kiểm tra rằng phân trang thành công
            Assert.IsNotNull(result.DataObject); // Kiểm tra rằng dữ liệu phân trang được trả về không null
            var pageObject = (Page<EmployeeInfo>)result.DataObject;
            Assert.AreEqual(page, pageObject.CurrentPage); // Kiểm tra rằng trang hiện tại được đặt đúng
            Assert.AreEqual(employeeInfoList, pageObject.ListRecord); // Kiểm tra rằng danh sách bản ghi được trả về đúng
            Assert.AreEqual(3, pageObject.TotalPage); // Kiểm tra rằng tổng số trang được đặt đúng
            Assert.AreEqual(25, pageObject.TotalRecord); // Kiểm tra rằng tổng số bản ghi được đặt đúng
        }

        [Test]
        public void PageService_ReturnsFailureIfNoData()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            string text = "search text";

            UnitOfWork.Employees.GetEmployeeInfoByPage(Arg.Any<int>(), Arg.Any<int>(), text)
                .Returns(new Page<EmployeeInfo>()); // Trả về danh sách rỗng

            // Act
            var result = EmployeeService.PageService(page, pageSize, text);

            // Assert
            Assert.IsTrue(result.Success); // Kiểm tra rằng phân trang không thành công khi không có dữ liệu
             // Kiểm tra rằng không có dữ liệu được trả về khi không có dữ liệu
        }
    }
}

