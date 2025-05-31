 
-- Tạo CSDL mới
CREATE DATABASE QuanLyThuVien;
GO

USE QuanLyThuVien;

-- Bảng Sách
CREATE TABLE Sach (
    MaSach NVARCHAR(50) PRIMARY KEY,  -- Dùng MaSach làm khóa chính
    TenSach NVARCHAR(255) NOT NULL,
    MaTacGia NVARCHAR(50) NOT NULL,
    MaTheLoai NVARCHAR(50) NOT NULL,
    MaNXB NVARCHAR(50) NOT NULL,
    NamXuatBan INT CHECK (NamXuatBan >= 1900 AND NamXuatBan <= YEAR(GETDATE())),
	GiaTien DECIMAL(18, 2) CHECK (GiaTien >= 0),
    SoLuong INT NOT NULL CHECK (SoLuong >= 0),
    AnhBia NVARCHAR(255),
    GhiChu NVARCHAR(MAX),
	MaViTri NVARCHAR(10),
	SoLanTaiBan INT DEFAULT 1 CHECK (SoLanTaiBan >= 1),
    FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia),
    FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai),
    FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan(MaNXB),
	FOREIGN KEY (MaViTri) REFERENCES ViTri(MaViTri)
);

CREATE TABLE ViTri (
    MaViTri NVARCHAR(10) PRIMARY KEY,
    Tu NVARCHAR(20),
    Ke NVARCHAR(20),
    Tang NVARCHAR(20),
	Kho NVARCHAR(50),
    MoTa NVARCHAR(100) -- Mô tả bổ sung nếu cần
);

-- Bảng Nhà Xuất Bản
CREATE TABLE NhaXuatBan (
    MaNXB NVARCHAR(50) PRIMARY KEY,  
    TenNXB NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    Email NVARCHAR(255) UNIQUE
);

-- Bảng Tác Giả
CREATE TABLE TacGia (
    MaTacGia NVARCHAR(50) PRIMARY KEY,  
    TenTacGia NVARCHAR(255) NOT NULL,
    NamSinh INT CHECK (NamSinh >= 1800 AND NamSinh <= YEAR(GETDATE())),
    QueQuan NVARCHAR(255)
);

-- Bảng Thể Loại
CREATE TABLE TheLoai (
    MaTheLoai NVARCHAR(50) PRIMARY KEY,  
    TenTheLoai NVARCHAR(255) NOT NULL UNIQUE
);

-- Bảng Phiếu Mượn
CREATE TABLE PhieuMuon (
    MaPhieuMuon NVARCHAR(50) PRIMARY KEY,   -- Dùng MaPhieuMuon làm khóa chính
    MaDocGia NVARCHAR(50) NOT NULL,
    NgayMuon DATE DEFAULT GETDATE(),
    NgayTraDuKien DATE,
	TienPhat DECIMAL(10, 2),
    TinhTrang NVARCHAR(50) CHECK (TinhTrang IN (N'Đang mượn', N'Đã trả', N'Quá hạn')),
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia)
);

-- Bảng Chi Tiết Phiếu Mượn
CREATE TABLE ChiTietPhieuMuon (
    MaPhieuMuon NVARCHAR(50),
    MaSach NVARCHAR(50),
    SoLuongMuon INT CHECK (SoLuongMuon > 0),
    TinhTrangSach NVARCHAR(100),
	NgayTra DATE NULL,
    PRIMARY KEY (MaPhieuMuon, MaSach),
    FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon) ON DELETE CASCADE,
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach) ON DELETE CASCADE
);

-- Bảng Phản Hồi & Đánh Giá
CREATE TABLE PhanHoi (
    MaPhanHoi NVARCHAR(50) PRIMARY KEY,     -- Dùng MaPhanHoi làm khóa chính
    MaDocGia NVARCHAR(50) NOT NULL,
    MaSach NVARCHAR(50) NOT NULL,
    NoiDung NVARCHAR(MAX) NOT NULL,
    NgayGui DATE DEFAULT GETDATE(),
	TrangThai NVARCHAR(50) DEFAULT N'Chờ duyệt',
    PhanHoiAdmin NVARCHAR(MAX) NULL,
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia) ON DELETE CASCADE,
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach) ON DELETE CASCADE
);

-- Bảng Độc Giả
CREATE TABLE DocGia (
    MaDocGia NVARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(255) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN ('Nam', N'Nữ')),
    DiaChi NVARCHAR(255),
    Email NVARCHAR(255) UNIQUE,
	SoDienThoai NVARCHAR(15),
    NgayDangKy DATE DEFAULT GETDATE()
);

-- Bảng Người Dùng (Tài khoản đăng nhập)
CREATE TABLE NguoiDung (
    TenDangNhap NVARCHAR(50) PRIMARY KEY,
    MatKhau NVARCHAR(255) NOT NULL, -- TODO: Hash mật khẩu
    HoTen NVARCHAR(255) NOT NULL,
    VaiTro NVARCHAR(50) CHECK (VaiTro IN ('Admin', 'Librarian', 'Reader')),
    MaDocGia NVARCHAR(50) NULL, -- Nếu là độc giả, liên kết với bảng DocGia
	IsActive BIT DEFAULT 1
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia) ON DELETE SET NULL
);
CREATE TABLE LienHe (
    MaLienHe NVARCHAR(50) PRIMARY KEY,      -- Mã liên hệ (khóa chính)
    HoTen NVARCHAR(255) NOT NULL,           -- Họ tên người liên hệ
    Email NVARCHAR(255) NOT NULL,           -- Địa chỉ email
    Sdt NVARCHAR(15) NULL,                  -- Số điện thoại (tùy chọn)
    Chude NVARCHAR(255) NOT NULL,           -- Chủ đề phản hồi
     NoiDung NVARCHAR(MAX) NOT NULL,                  -- Nội dung phản hồi
    FileDinhKem NVARCHAR(255) NULL,         -- Đường dẫn tệp đính kèm (tùy chọn)
    ThoiGianGui DATETIME DEFAULT GETDATE()  -- Thời gian gửi (mặc định là thời gian hiện tại)
);
CREATE TABLE DatLichMuon (
    MaLichMuon NVARCHAR(10) PRIMARY KEY, -- Ví dụ: 'LM01'
    MaDocGia NVARCHAR(50) NOT NULL,
    NgayDenMuon DATE NOT NULL,
    GioDenMuon TIME NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Chờ xử lý',
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia)
);
CREATE TABLE ChiTietDatLichMuon (
    MaChiTiet NVARCHAR(50) PRIMARY KEY,     -- ví dụ: 'CTLM01'
    MaLichMuon NVARCHAR(10) NOT NULL,
    MaSach NVARCHAR(50) NOT NULL,
    SoLuong INT CHECK (SoLuong > 0),
    FOREIGN KEY (MaLichMuon) REFERENCES DatLichMuon(MaLichMuon) ON DELETE CASCADE,
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);
-- Bảng Chức năng (danh sách các form)
CREATE TABLE ChucNang (
    MaChucNang NVARCHAR(50) PRIMARY KEY,
    TenChucNang NVARCHAR(255) NOT NULL, -- Ví dụ: "Quản lý mượn sách"
    TenForm NVARCHAR(255) NOT NULL      -- Ví dụ: "FormPhieuMuonSach"
);


CREATE TABLE PhanQuyen (
    TenDangNhap NVARCHAR(50),                -- Tên đăng nhập của người dùng
    MaChucNang NVARCHAR(50),                  -- Mã chức năng (ví dụ: QuanLySanPham, QuanLyDonHang)
    DuocTruyCap BIT DEFAULT 0,                -- Quyền truy cập (1: có quyền, 0: không có quyền)
    PRIMARY KEY (TenDangNhap, MaChucNang),    -- Khóa chính kết hợp giữa tên đăng nhập và mã chức năng
    FOREIGN KEY (TenDangNhap) REFERENCES NguoiDung(TenDangNhap),  -- Liên kết với bảng NguoiDung
    FOREIGN KEY (MaChucNang) REFERENCES ChucNang(MaChucNang)  -- Liên kết với bảng ChucNang (bảng chứa các chức năng của hệ thống)
);
SELECT 
    f.name AS ForeignKeyName,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc 
    ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Sach';
ALTER TABLE Sach
ADD CONSTRAINT FK_Sach_TacGia
FOREIGN KEY (MaTacGia) REFERENCES TacGia(MaTacGia);

ALTER TABLE Sach
ADD CONSTRAINT FK_Sach_TheLoai
FOREIGN KEY (MaTheLoai) REFERENCES TheLoai(MaTheLoai);

ALTER TABLE Sach
ADD CONSTRAINT FK_Sach_NhaXuatBan
FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan(MaNXB);

ALTER TABLE Sach DROP CONSTRAINT FK__Sach__MaNXB__3B40CD36;
ALTER TABLE Sach DROP CONSTRAINT FK__Sach__MaTacGia__395884C4;
ALTER TABLE Sach DROP CONSTRAINT FK__Sach__MaTheLoai__3A4CA8FD;
SELECT*FROM PhanQuyen;
SELECT*FROM ChucNang;
INSERT INTO ViTri (MaViTri, Tu, Ke, Tang, MoTa)  
VALUES   
('VT01', N'Tủ Văn Học', N'Kệ 1', N'Tầng 1', N'Góc bên trái cửa vào, chứa sách văn học cổ điển'),  
('VT02', N'Tủ Truyện Tranh', N'Kệ 2', N'Tầng 1', N'Gần khu truyện tranh, có nhiều thể loại khác nhau'),  
('VT03', N'Tủ Tham Khảo', N'Kệ 1', N'Tầng 2', N'Khu tham khảo nâng cao, sách phục vụ nghiên cứu'),  
('VT04', N'Tủ Khoa Học', N'Kệ 3', N'Tầng 1', N'Khu sách khoa học tự nhiên, xã hội'),  
('VT05', N'Tủ Giáo Khoa', N'Kệ 1', N'Tầng 3', N'Chứa sách giáo khoa và tài liệu học tập'),  
('VT06', N'Tủ Thiếu Nhi', N'Kệ 2', N'Tầng 2', N'Góc yên tĩnh, đầy đủ sách thiếu nhi');  

DROP TABLE IF EXISTS PhanQuyen;
ALTER TABLE PhieuMuon
DROP CONSTRAINT DF__PhieuMuon__TienP__73852659;
ALTER TABLE PhieuMuon
ALTER COLUMN TienPhat DECIMAL(10, 2)
ALTER TABLE PhieuMuon
ADD CONSTRAINT DF_TienPhat DEFAULT 0.00 FOR TienPhat;
ALTER TABLE Sach
ADD GiaTien DECIMAL(18, 2) CHECK (GiaTien >= 0);
ALTER TABLE PhieuMuon
ADD TienPhat INT DEFAULT 0;
ALTER TABLE ChiTietPhieuMuon
ADD TinhTrangSach NVARCHAR(100);
ALTER TABLE LienHe
ALTER COLUMN NoiDung NVARCHAR(MAX) NOT NULL;
Select * from PhieuMuon
UPDATE NguoiDung  
SET MatKhau = 'admin123' 
WHERE TenDangNhap = 'admin';
SELECT*FROM NguoiDung
UPDATE NguoiDung
SET IsActive = 1;
ALTER TABLE DocGia  
ADD SoDienThoai NVARCHAR(15);  
-- Bảng Phiếu Mượn
ALTER TABLE PhanHoi
ADD TrangThai NVARCHAR(50) DEFAULT N'Chờ duyệt',
    PhanHoiAdmin NVARCHAR(MAX) NULL;

	ALTER TABLE NguoiDung ADD IsActive BIT DEFAULT 1;
ALTER TABLE PhieuMuon
DROP COLUMN NgayTra;
ALTER TABLE ChiTietPhieuMuon
ADD NgayTra DATE NULL;
DELETE FROM ChiTietPhieuMuon;
DELETE FROM PhieuMuon;
DROP TABLE IF EXISTS ChiTietPhieuMuon;
DROP TABLE IF EXISTS PhieuMuon;
DROP TABLE IF EXISTS PhanHoi;
DROP TABLE IF EXISTS Sach;
DROP TABLE IF EXISTS TacGia;
DROP TABLE IF EXISTS NhaXuatBan;
DROP TABLE IF EXISTS TheLoai;
INSERT INTO TacGia (TenTacGia, NamSinh, QueQuan) VALUES (N'J.K. Rowling', 1965, N'Anh');
INSERT INTO NhaXuatBan (TenNXB, DiaChi, Email) VALUES (N'NXB Kim Đồng', N'Hà Nội', 'contact@kimdong.vn');
INSERT INTO TheLoai (TenTheLoai) VALUES (N'Tiểu thuyết');

SELECT * FROM TacGia;
SELECT * FROM NhaXuatBan;
SELECT * FROM ChiTietDatLichMuon;
SELECT * FROM Sach;
INSERT INTO Sach (MaSach, TenSach, MaTacGia, MaTheLoai, MaNXB, NamXuatBan, SoLuong)
VALUES (
    'S' + RIGHT('00' + CAST((SELECT COUNT(*) + 1 FROM Sach) AS NVARCHAR), 2), 
    N'Harry Potter', 'TG01', 'TL01', 'NXB01', 1997, 5
);
INSERT INTO PhieuMuon (MaDocGia, NgayMuon, TinhTrang)
VALUES ('DG4999886817', GETDATE(), N'Đang mượn');
SELECT * FROM DocGia 
SELECT * FROM PhieuMuon
INSERT INTO NhaXuatBan (MaNXB, TenNXB, DiaChi, Email)
VALUES 
('NXB01', N'Nhà xuất bản giáo dục Việt Nam', N'123 Đường Mai dịch, Hà Nội', 'nxbgd@gmail.com'),
INSERT INTO TacGia (MaTacGia, TenTacGia, NamSinh, QueQuan)
VALUES 
('TG01', N'Phạm Văn Ất', 1985, N'Hà Nội'),
INSERT INTO TheLoai (MaTheLoai, TenTheLoai)
VALUES 
('TL01', 'Lập trình')
INSERT INTO Sach (MaSach, TenSach, MaTacGia, MaTheLoai, MaNXB, NamXuatBan, SoLuong, AnhBia, GhiChu)
VALUES 
('S0001', N'Lập trình C#', 'TG01', 'TL01', 'NXB01', 2022, 50, 'D:\BTL_Quanlythuvien\images\c.jpg', N'Sách học lập trình C# cơ bản'),
-- Dữ liệu mẫu
INSERT INTO NguoiDung (TenDangNhap, MatKhau, HoTen, VaiTro)
VALUES 
('giang', '19112004', N'Lê Đình Giang', 'Librarian');
('thuthu', 'thuthu123', 'Thủ thư A', 'Librarian');

SELECT S.MaSach, S.TenSach, TG.TenTacGia, TL.TenTheLoai, NXB.TenNXB
FROM Sach S
JOIN TacGia TG ON S.MaTacGia = TG.MaTacGia
JOIN TheLoai TL ON S.MaTheLoai = TL.MaTheLoai
JOIN NhaXuatBan NXB ON S.MaNXB = NXB.MaNXB

Select*from NguoiDung;

SELECT * FROM NguoiDung
WHERE TenDangNhap = 'admin'
DELETE FROM NguoiDung;
-- 1. Thêm sách vào bảng Sach
INSERT INTO Sach (MaSach, TenSach, TacGia, TheLoai, NhaXuatBan, NamXuatBan, SoLuong, GhiChu)
VALUES 
    ('S001', N'Đắc Nhân Tâm', N'Dale Carnegie', N'Kỹ năng sống', N'NXB Trẻ', 2019, 10, NULL),
    ('S002', N'Tư Duy Làm Giàu', N'Napoleon Hill', N'Kinh tế', N'NXB Lao Động', 2020, 5, NULL);

-- 2. Thêm độc giả vào bảng DocGia
INSERT INTO DocGia (MaDocGia, HoTen, NgaySinh, GioiTinh, DiaChi, Email, NgayDangKy)
VALUES 
    ('DG001', N'Nguyễn Văn A', '2000-05-20', N'Nam', N'Hà Nội', 'nguyenvana@gmail.com', GETDATE());

-- 3. Thêm phiếu mượn vào bảng PhieuMuon
INSERT INTO PhieuMuon (MaPhieuMuon, MaDocGia, NgayMuon, NgayTra, TinhTrang)
VALUES 
    ('PM001', 'DG4999886817', '2025-03-22', '2025-03-27', N'Đang mượn');

-- 4. Thêm chi tiết phiếu mượn vào bảng ChiTietPhieuMuon
INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, SoLuongMuon)
VALUES 
    ('PM001', 'S001', 1),
    ('PM001', 'S002', 1);
	SELECT*FROM PhieuMuon;
ALTER TABLE Sach ADD AnhBia NVARCHAR(255);
UPDATE Sach
SET AnhBia = N'D:\BTL_Quanlythuvien\images\dacnhantam.jpg'
WHERE MaSach = 'S001';

UPDATE Sach
SET AnhBia = N'D:\BTL_Quanlythuvien\images\sach.png'
WHERE MaSach = 'S002';
SELECT*FROM ViTri;
DELETE FROM ViTri;  
DELETE FROM PhieuMuon;
SELECT*FROM PhieuMuon;
SELECT * FROM DatLichMuon;
SELECT * FROM Sach;

DELETE FROM ChiTietPhieuMuon
WHERE MaChiTiet = 'CT003'
SELECT * FROM DocGia WHERE MaDocGia IS NULL; 
UPDATE DocGia
SET Email = 'thao@gmail.com'
WHERE MaDocGia = 'DG02';

 DELETE FROM ChiTietPhieuMuon
WHERE MaPhieuMuon IN (
    SELECT MaPhieuMuon FROM PhieuMuon WHERE MaDocGia = 'DG4999886817'
);
SELECT*FROM PhieuMuon
SELECT*FROM ChiTietPhieuMuon
-- Bước 2: Xóa trong PhieuMuon
DELETE FROM PhieuMuon
DELETE FROM ChiTietPhieuMuon
WHERE MaDocGia = 'DG4999886817';
DELETE FROM PhieuMuon  
WHERE MaPhieuMuon = 'PM03';  
DELETE FROM ChiTietPhieuMuon  
WHERE MaPhieuMuon = 'PM03';  


