using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class Config
    {
        public const string BAO_TRI_HE_THONG = "Hiện tại hệ thống đang bảo trì, vui lòng chờ trong giây lát, hoặc liên hệ số Hotline hỗ trợ : 0906260304 để biết thêm thông tin.";
        public const string CHECK_IN_SAI_VI_TRI = "Vào điểm không hợp lệ, vị trí vào điểm xa hơn so với qui định cho phép.";
        public const string CHUA_CHON_CUA_HANG = "Vui lòng chọn cửa hàng.";
        public const string CHUA_VAO_DIEM = "Bạn chưa vào điểm, vui lòng vào điểm trước khi ra điểm.";
        public const string DINH_DANG_TIEN = "N2";
        public const string DON_HANG_DA_XU_LY = "Đơn hàng đã xử lý, vui lòng liên hệ với quản lý để biết thêm thông tin";
        public const string DON_HANG_DA_XU_LY_KHONG_SUA = "Đơn hàng đã xử lý, bạn không thể thao tác, vui lòng liên hệ với quản lý để biết thêm thông tin ";
        public const string DON_HANG_KHONG_THE_XU_LY_VUOT_QUA_SO_LUONG_GIAO = "Số lượng thay đổi vượt quá số lượng đã giao, vui lòng liên hệ với quản lý để biết thêm thông tin ";
        public const string DON_HANG_KHONG_THE_XU_LY_VUOT_QUA_TIEN_THANH_TOAN = "Số tiền đơn hàng vượt quá số tiền đã thanh toán, vui lòng liên hệ với quản lý để biết thêm thông tin ";
        public const string DON_HANG_KHONG_THE_XU_LY_DA_THANH_TOAN = "Đơn hàng đã thanh toán không thể chỉnh sửa";
        public const string DON_HANG_KHONG_THE_XU_LY_XOA_MAT_HANG_DA_GIAO = "Mặt hàng đã giao, bạn không thể xóa, vui lòng liên hệ với quản lý để biết thêm thông tin ";
        public const string DU_LIEU_QUA_GIO_HAN_CHO_PHEP = "Khoảng thời gian lựa chọn không được vượt quá {NGAY} ngày, vui lòng liên hệ với quản lý để biết thêm thông tin";
        public const string DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG = "Mã bảo mật không đúng, vui lòng liên hệ Administrator.";
        public const string GIAO_HANG_KHONG_NHAP_SO_LUONG = "Bạn chưa nhập vào số lượng cần giao.";
        public const string HET_HAN_MUC = "Số nhân viên khai báo đã vượt quá hạn mức, vui lòng liên hệ với quản lý để loại bỏ bớt nhân viên hoặc cấp thêm hạn mức";
        public const string IMAGENAMEUPOLOAD = "LHIMAGE";
        public const string KHACH_HANG_DA_TON_TAI = "Số điện thoại này đã tồn tại trên hệ thống, vui lòng kiểm tra lại hoặc liên hệ với quản lý.";
        public const string KHONG_CO_CUA_HANG = "Chưa có cửa hàng nào được tạo.";
        public const string KHONG_CO_CUA_HANG_NAO_GAN_DAY = "Không có cửa hàng gần đây trong kế hoạch của bạn.";
        public const string KHONG_CO_DON_HANG = "Chưa có đơn hàng nào cần xử lý.";
        public const string KHONG_CO_DU_LIEU = "Không có dữ liệu";
        public const string KHONG_CO_MAT_HANG = "Vui lòng chọn mặt hàng.";
        public const string KHONG_CO_QUYEN_THAO_TAC = "Bạn không có quyền thao tác chức năng này.";
        public const string KHONG_DUOC_PHEP_CAI_DAT_LAI_PHAN_MEM = "Bạn đã cài đặt lại phần mềm, vui lòng liên hệ quản trị viên để được hỗ trợ.";
        public const string KHUYEN_MAI_HET_HIEU_LUC = "Chương trình chiết khấu không có hiệu lực, vui lòng liên hệ với quản lý để biết thêm thông tin";
        public const string KHUYEN_MAI_TONGTIENKM_LONHONTONGTIEN = "Tổng tiền chiết khấu lơn hơn giá trị đơn hàng, vui lòng kiểm tra lại thông tin";
        public const string LIEN_HE_ADMINISTRATOR = "Có vấn đề dữ liệu, vui lòng liên hệ Administrator.";
        public const string MAT_KHAU_CU_KHONG_DUNG = "Mật khẩu cũ không đúng.";
        public const string MA_CONG_TY_KHONG_DUNG = "Mã công ty không tồn tại, vui lòng thử lại hoặc liên hệ quản trị viên.";
        public const string MA_CONG_TY_KHONG_DUNG_DINH_DANG = "Mã công ty sai định dạng (có chứa ký tự đặc biệt), vui lòng thử lại hoặc liên hệ quản trị viên.";
        public const string MA_DON_HANG_DA_TON_TAI = "Mã đơn hàng đã tồn tại, vui lòng nhập lại.";
        public const string MA_KHACH_HANG_DA_TON_TAI = "Mã khách hàng này đã tồn tại trên hệ thống, vui lòng kiểm tra lại hoặc liên hệ với quản lý.";
        public const string MA_TIN_NHAN_KHONG_DUNG = "Mã tin nhắn không chính xác, vui lòng liên hệ với quản lý để biết thêm thông tin";
        public const string MSG_BAT_FAKEGPS = "Thiết bị đang cài đặt Ứng dụng Fake GPS hoặc ở chế độ cho phép giả lập vị trí vui lòng truy cập Cài đặt->Cài đặt cho người phát triển->Cho phép giả lập vị trí";
        public const string MSG_THOI_GIAN_CAU_HINH_SAI = "Thời gian trên máy của bạn lệch múi giờ so với hệ thống, liên hệ với quản trị để được hỗ trợ";
        public const int PAGING = 15;
        public const string PHIEN_BAN_KHONG_DUNG = "Bạn đang dùng phiên bản cũ, vui lòng cập nhật phiên bản mới.";
        public const string PREFIX_CHECKIN = "checkin";
        public const string PREFIX_CHECKOUT = "checkout";
        public const string ROUTERFAIL = "Mã công ty không đúng. Nếu bạn chưa có Mã công ty, gọi số Hot line để được cấp Mã dùng thử";
        public const string SAI_VERSION = "Phiên bản không hợp lệ, vui lòng cập nhật phiên bản mới hoặc liên hệ với quản lý để biết thêm thông tin.";
        public const int SO_NGAY_MAC_DINH_BAO_CAO = -30;
        public const string TAI_KHOAN_DANG_SU_DUNG = "Tài khoản này đang được sử dụng, để đăng nhập bạn cần phải đăng xuất trên thiết bị thuộc phiên làm việc gần nhất.";
        public const string TEN_DANG_NHAP_KHONG_DUNG_DINH_DANG = "Tên đăng nhập sai định dạng (có chứa ký tự đặc biệt), vui lòng thử lại hoặc liên hệ quản trị viên.";
        public const string THANH_CONG = "Xử lý thành công.";
        public const string THONG_BAO_PHIEN_BAN_MOI_ANDROID = "Đã có phiên bản mới {VERSION}, vui lòng truy cập CHPlay để cập nhật";
        public const string THONG_BAO_PHIEN_BAN_MOI_IOS = "Đã có phiên bản mới {VERSION}, vui lòng truy cập Appstore để cập nhật";
        public const string THONG_TIN_KHONG_DUNG = "Thông tin yêu cầu không đúng.";
        public const string THONG_TIN_TAI_KHOAN_KHONG_DUNG = "Tài khoản hoặc mật khẩu không đúng.";
        public const string THU_HOI_CONG_NO_CHUA_CHON_KHACH_HANG = "Bạn chưa chọn khách hàng.";
        public const string THU_HOI_CONG_NO_CHUA_CHON_NHAP_TIEN = "Bạn chưa nhập tiền.";
        public const string THU_HOI_CONG_NO_NOI_DUNG_TIN_NHAN = "Cam on quy khach vua thanh toan thanh cong so tien: {TIEN}d cho nhan vien: {TENNHANVIEN}.";
        public const string THU_HOI_CONG_NO_NOI_DUNG_TIN_NHAN_GUI_THONG_BAO_CONG_TY = "Nhan vien: {TENNHANVIEN} vua thu hoi thanh cong so tien: {TIEN}d tu khach hang: {TENKHACHHANG}.";
        public const string UPLOAD_FAIL = "Gửi ảnh bị lỗi, vui lòng thử lại.";
        public const string NODATANOTFOUND = "Không tồn tại dữ liệu theo điều kiện.";

        public Config()
        {

        }

        //public static string GetValue(int ID_QLLH, string ParamName)
        //{
        //    return 
        //}
    }
}