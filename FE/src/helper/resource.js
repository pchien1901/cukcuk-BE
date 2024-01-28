const MResource = {
  VN: {
    // validate form
    FieldIsNotEmpty: "Thông tin không được trống",
    CustomerCodeIsNotEmpty: "Mã khách hàng không được để trống",
    CustomerCodeIsExist: "Mã khách hàng đã tồn tại",
    FullNameIsNotEmpty: "Họ và tên không được để trống",
    DoBInvalid: "Ngày sinh không hợp lệ",
    MemberCardCodeIsNotFormat: "Số CMTND không đúng định dạng", 
    DoRInvalid: "Ngày không hợp lệ",
    PhoneNumberIsNotFormat: "Số điện thoại không đúng định dạng",
    EmailIsNotEmpty: "Email không được để trống",
    EmailIsNotFormat: "Email không đúng định dạng",
    DebitAmountHaveToNumber: "Vui lòng chỉ nhập số",

    // form employee
    EmployeeCodeIsNotEmpty: "Mã nhân viên không được để trống",
    EmployeeCodeIsExist: "Mã nhân viên đã tồn tại",
    DepartmentIdIsNotEmpty: "Đơn vị không được để trống",

    // dialog
    DeleteCustomerTitle: "Xóa khách hàng",
    ValidateDialogTitle: "Cảnh báo",
    DeleteCustomerMessage: "Người dùng bị xóa sẽ không thể khôi phục, bạn vẫn sẽ xóa người dùng ?",
    DeleteEmployeeTitle: "Xóa nhân viên",
    DeleteEmployeeMessage: "Nhân viên bị xóa sẽ không thể khôi phục, bạn vẫn sẽ xóa nhân viên ?",
    EmployeeCodeDuplicate: "Mã nhân viên đã tồn tại trong hệ thống",
    UpdateEmployeeTitle: "Xác nhận thay đổi",
    UpdateEmployeeMessage: "Xữ liệu đã bị thay đổi, bạn có muốn cất không?",
    PrimaryBtnText: "Đồng ý",
    PrimaryBtnTextConfirm: "Có",
    SecondBtnTextConfirm: "Không",

    // toast
    AddCustomerSuccess: "Thêm khách hàng thành công.",
    EditCustomerSuccess: "Cập nhật thông tin thành công",
    AddEmployeeSuccess: "Thêm nhân viên thành công",
    EditEmployeeSuccess: "Cập nhật thông tin thành công",
    DeleteEmployeeSuccess: "Xóa nhân viên thành công",
    ToastTypeSuccess: "success",
    ToastTypeWarning: "warning",
    ToastTypeInfo: "info",
    ToastTypeError: "error",
    
    // axios error
    BadRequest: "Thông tin chưa hợp lệ, vui lòng kiểm tra lại",
    Forbidden: "Bạn không được cấp, quyền truy cập, vui lòng liên hệ lại quản trị",
    NotFound: "Không tìm thấy thông tin",
    ServerError: "Máy chủ không phản hồi, vui lòng quay lại sau",

    // error
    Error: "Không thành công, đã có lỗi xảy ra",
  },
  EN: {
    CustomerNotEmpty: "Mã khách hàng không được để trống",
  },
};

export default MResource;
