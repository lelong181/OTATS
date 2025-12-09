using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSPos_Data.Models
{
    public class ChiTietDonHangModels
    {
        public ChiTietDonHangModels()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int idchitietdonhang { get; set; }
        public int iddonhang { get; set; }
        public int idhanghoa { get; set; }
        public double soluong { get; set; }
        public double soLuongTraLai { get; set; }
        public double tongTien { get; set; }
        private string _ghichu = "";
        public string ghichu
        {
            get { return _ghichu; }
            set { _ghichu = value; }
        }
        public int hinhthucban { get; set; }//2 = tinh tien theo gia khac
        public string tenhinhthucban { get; set; }
        public double giakhac { get; set; }
        public double giaban { get; set; }
        public string mota { get; set; }
        public string linkgioithieu { get; set; }
        public string tenKho { get; set; }

        public string tenhienthi { get; set; }
        public double giabuon { get; set; }

        public double giale { get; set; }

        public double chietkhauphantram_banle { get; set; }
        public double chietkhautien_banle { get; set; }
        public double chietkhauphantram_banbuon { get; set; }
        public double chietkhautien_banbuon { get; set; }
        public double phantramhaohut { get; set; }
        public double soluonghaohut { get; set; }

        public int idnhanvien { get; set; }
        public int idLichSuGiaoHang { get; set; }
        public int idKho { get; set; }
        public int idquanly { get; set; }


        private string _ngaysuadoi = "";
        public string ngaysuadoi
        {
            get { return _ngaysuadoi; }
            set { _ngaysuadoi = value; }
        }
        public int IsDichVu { get; set; }

        public double chietkhauphantram { get; set; }
        public double chietkhautien { get; set; }
        public double tongtienchietkhau { get; set; }
        public int idctkm { get; set; }

        private string _tenctkm = "";
        public string tenctkm
        {
            get { return _tenctkm; }
            set { _tenctkm = value; }
        }


        private string _tenhang = "";
        public string tenhang
        {
            get { return _tenhang; }
            set { _tenhang = value; }
        }

        private string _mahang = "";
        public string mahang
        {
            get { return _mahang; }
            set { _mahang = value; }
        }
        private string _tendanhmuc = "";
        public string tendanhmuc
        {
            get { return _tendanhmuc; }
            set { _tendanhmuc = value; }
        }
        private int _iddanhmuc = 0;
        public int iddanhmuc
        {
            get { return _iddanhmuc; }
            set { _iddanhmuc = value; }
        }

        private string _tendonvi = "";
        public string tendonvi
        {
            get { return _tendonvi; }
            set { _tendonvi = value; }
        }

        private string _ghichugia = "";
        public string ghichugia
        {
            get { return _ghichugia; }
            set { _ghichugia = value; }
        }
        public double dagiao { get; set; }
        public double tonthucte { get; set; }
        public double tonorder { get; set; }

        public string tennhacungcap { get; set; }
        public string tennhanhieu { get; set; }
        public int loaihanghoa { get; set; }
        public List<HangHoa_DichVuModel> lstDichVu { get; set; }
        public DateTime Ngay { get; set; }
        public List<ChiTietMatHangDonHangModels> dschitietmathang { get; set; }
        public List<ChiTietDonHangModels> dshangtang { get; set; }

        public int idhaohut { get; set; }

        public int HangKhuyenMai { get; set; }

        public int idkhoxuat { get; set; }
        public string tenkhoxuat { get; set; }
        public int ID_ChiTietDonHang_DatKhuyenMai { get; set; }

        public int isdichvu { get; set; }
        public string sitecode { get; set; }
        public string servicerateid { get; set; }


    }
}