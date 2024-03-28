const MResource = {
  VN: {
    // validate form
    FieldIsNotEmpty: "Thông tin không được trống.",
    CustomerCodeIsNotEmpty: "Mã khách hàng không được để trống.",
    CustomerCodeIsExist: "Mã khách hàng đã tồn tại.",
    FullNameIsNotEmpty: "Họ và tên không được để trống.",
    DoBInvalid: "Ngày sinh không hợp lệ.",
    MemberCardCodeIsNotFormat: "Số CMTND không đúng định dạng.", 
    DoRInvalid: "Ngày không hợp lệ.",
    PhoneNumberIsNotFormat: "Số điện thoại không đúng định dạng.",
    EmailIsNotEmpty: "Email không được để trống.",
    EmailIsNotFormat: "Email không đúng định dạng.",
    DebitAmountHaveToNumber: "Vui lòng chỉ nhập số.",

    // form employee
    EmployeeCodeIsNotEmpty: "Mã nhân viên không được để trống.",
    EmployeeCodeIsExist: "Mã nhân viên đã tồn tại.",
    DepartmentIdIsNotEmpty: "Đơn vị không được để trống.",
    InformationNotFound: "Không tìm thấy thông tin",

    // dialog
    DeleteCustomerTitle: "Xóa Khách hàng",
    ValidateDialogTitle: "Cảnh báo",
    DeleteCustomerMessage: "Người dùng bị xóa sẽ không thể khôi phục, bạn vẫn sẽ xóa người dùng ?",
    DeleteEmployeeTitle: "Xóa nhân viên",
    DeleteEmployeeMessage: "Nhân viên bị xóa sẽ không thể khôi phục, bạn vẫn sẽ xóa nhân viên ?",
    EmployeeCodeDuplicate: "Mã nhân viên đã tồn tại trong hệ thống.",
    UpdateEmployeeTitle: "Xác nhận thay đổi.",
    UpdateEmployeeMessage: "Dữ liệu đã bị thay đổi, bạn có muốn cất không?",
    PrimaryBtnText: "Đồng ý",
    PrimaryBtnTextConfirm: "Có",
    SecondBtnTextConfirm: "Không",

    // toast
    AddCustomerSuccess: "Thêm khách hàng thành công.",
    EditCustomerSuccess: "Cập nhật thông tin thành công.",
    AddEmployeeSuccess: "Thêm nhân viên thành công.",
    EditEmployeeSuccess: "Cập nhật thông tin thành công.",
    DeleteEmployeeSuccess: "Xóa nhân viên thành công.",
    ToastTypeSuccess: "success",
    ToastTypeWarning: "warning",
    ToastTypeInfo: "info",
    ToastTypeError: "error",
    
    // axios error
    BadRequest: "Thông tin chưa hợp lệ, vui lòng kiểm tra lại.",
    Unauthorized: "Vui lòng đăng nhập lại.",
    Forbidden: "Truy cập bị từ chối.",
    NotFound: "Không tìm thấy thông tin.",
    ServerError: "Máy chủ không phản hồi, vui lòng quay lại sau.",

    // error
    Error: "Không thành công, đã có lỗi xảy ra.",

    // employee table
    SearchEmployeeTitle: "Nhập để tìm kiếm",

    Input: {
      TypeFile: {
        FileNameDefault: "Không có tệp nào được chọn",
        ButtonText: "Chọn tệp",
      }
    },

    File: {
      EmployeeFile: "Danh_sach_nhan_vien",
      ChooseFileToImport: "Chọn tệp nguồn",
      ValidateFileToImport: "Kiểm tra dữ liệu",
      ImportResult: "Kết quả nhập khẩu"
    },

    // thông báo khi import
    Toast: {
      Import: {
        FileIsNotChosen: "Vui lòng chọn tệp Excel để thực hiện.",
      },
      Auth: {
        TokenExpiration: "Hết phiên đăng nhập"
      }
    },

    // Thông báo cho màn hình login
    Login: {
      Username: {
        UsernameIsRequired: "Tên đăng nhập không được để trống",
        UsernameIsNotFormat: "Tên đăng nhập không đúng định dạng",
      },
      Password: {
        PasswordIsRequired: "Mật khẩu không được để trống",
        PasswordMustBeInFormat: "Mật khẩu cần có chữ hoa, chữ thường, số và kí tự đặc biệt"
      },
      Invalid: {
        InvalidMsg: "Tên đăng nhập hoặc mật khẩu không hợp lệ",
      }
    },

    // Các sự kiện (emit?)
    Event: {
      Toast: {
        openMainToast: "openMainToast",
      }
    }
  },
  EN: {
    CustomerNotEmpty: "Mã khách hàng không được để trống",
  },
};

export default MResource;
