using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Reports.Dto
{
    public class BM13Dto
    {
        public int STT { get; set; }
        public string FullName { get; set; }
        public string DonDuTuyen { get; set; }
        public string LyLichCoXacNhan { get; set; }
        public string BanSaoKhaiSinh { get; set; }
        public string GiayChungNhanSucKhoe { get; set; }
        public string ChungNhanDoiTuongUuTien { get; set; }
        public string KhongBiKyLuat { get; set; }
        public string ChuyenMon { get; set; }
        public string NgoaiNgu { get; set; }
        public string TinHoc { get; set; }
        public string ThoiGianThongBaoKetQua { get; set; }
        public string ThoiGianRaQuyetDinh { get; set; }
        public string DonViVaViTriCongTac { get; set; }
        public string ThoiGianDenNhanViec { get; set; }
        public string QuyetDinhHuongDanTapSu { get; set; }
        public string TapSuCheDoCuaNguoiTapSu { get; set; }
        public string CheDoCuaNguoiHuongDan { get; set; }
        public string MienTapSu { get; set; }
    }
}
