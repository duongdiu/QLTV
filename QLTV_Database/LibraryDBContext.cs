using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTV_DTO;

namespace QLTV_Database
{
    public class LibraryDBContext : DbContext
    {
        public LibraryDBContext() : base("name=QuanLyThuVienEntities") { }

        public DbSet<NguoiDung> NguoiDungDatabase { get; set; }
        public DbSet<Sach> SachDatabase { get; set; }
        public DbSet<DocGia> DocGiaDatabase { get; set; }
        public DbSet<PhieuMuon> PhieuMuonDatabase { get; set; }
        public DbSet<ChiTietPhieuMuon> ChiTietPhieuMuonDatabase { get; set; }
        public DbSet<PhanHoi> PhanHoiDatabase { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBanDatabase { get; set; }
        public DbSet<TacGia> TacGiaDatabase { get; set; }
        public DbSet<TheLoai> TheLoaiDatabase { get; set; }
        public DbSet<LienHe> LienHeDatabase { get; set; }
        public DbSet<DatLichMuon> DatLichMuonDatabase { get; set; }
        public DbSet<ChiTietDatLichMuon> ChiTietDatLichMuonDatabase { get; set; }
        public DbSet<ChucNang> ChucNangDatabase { get; set; }
        public DbSet<PhanQuyen> PhanQuyenDatabase { get; set; }
        public DbSet<ViTri> ViTriDatabase { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}