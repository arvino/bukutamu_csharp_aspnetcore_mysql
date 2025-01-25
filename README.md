# Aplikasi Buku Tamu

Aplikasi Buku Tamu berbasis web menggunakan ASP.NET Core dan MySQL.

## Fitur

### Halaman Publik
- Melihat daftar buku tamu
- Melihat detail buku tamu
- Login
- Registrasi

### REST API
- Endpoint untuk CRUD buku tamu
- Autentikasi berbasis cookie
- Otorisasi berbasis role (member/admin)

### Halaman Member
- Menulis pesan buku tamu
- Upload gambar/foto
- Edit profil (password, nama, phone)
- Melihat daftar dan detail buku tamu

### Halaman Admin
- Manajemen member (CRUD)
- Manajemen buku tamu (CRUD)
- Melihat semua data

## Teknologi

- ASP.NET Core 8.0 (LTS)
- Entity Framework Core
- MySQL (XAMPP)
- Bootstrap 5
- jQuery

## Persyaratan Sistem

- .NET 8.0 SDK
- XAMPP (dengan MySQL)
- Visual Studio 2022 atau VS Code

## Persiapan Development

1. Install .NET SDK
   - Download .NET 8.0 SDK dari https://dotnet.microsoft.com/download/dotnet/8.0
   - Jalankan installer
   - Buka Command Prompt atau PowerShell baru
   - Verifikasi instalasi dengan menjalankan:
   ```bash
   dotnet --version
   ```

2. Install Visual Studio Code (opsional)
   - Download VS Code dari https://code.visualstudio.com/
   - Install extension C# Dev Kit
   - Install extension C# untuk syntax highlighting

## Instalasi

1. Clone repository

2. Buka terminal/command prompt, masuk ke direktori proyek:
   ```bash
   cd path/to/bukutamu_csharp_aspnetcore_mysql
   ```

3. Install dependencies:
   ```bash
   dotnet restore
   ```

4. Install dan jalankan XAMPP
   - Download XAMPP terbaru dari https://www.apachefriends.org/
   - Install XAMPP
   - Start Apache dan MySQL dari XAMPP Control Panel

5. Buat database dan tabel
   - Buka phpMyAdmin (http://localhost/phpmyadmin)
   - Buat database baru:
   ```sql
   CREATE DATABASE bukutamu_simple;
   ```
   
   - Buat tabel member:
   ```sql
   CREATE TABLE member (
       Id INT AUTO_INCREMENT PRIMARY KEY,
       Nama VARCHAR(100) NOT NULL,
       Phone VARCHAR(20),
       Email VARCHAR(100) NOT NULL UNIQUE,
       Password VARCHAR(256) NOT NULL,
       Role VARCHAR(20) NOT NULL
   );
   ```
   
   - Buat tabel bukutamu:
   ```sql
   CREATE TABLE bukutamu (
       Id INT AUTO_INCREMENT PRIMARY KEY,
       MemberId INT NOT NULL,
       Messages TEXT NOT NULL,
       Gambar VARCHAR(255),
       Timestamp DATETIME NOT NULL,
       FOREIGN KEY (MemberId) REFERENCES member(Id)
   );
   ```

6. Konfigurasi koneksi database di appsettings.json:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=bukutamu_simple;User=root;Password=;"
     }
   }
   ```
   Note: Secara default XAMPP MySQL menggunakan username "root" tanpa password

7. Jalankan migrasi database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

8. Buat folder untuk upload gambar:
   ```bash
   mkdir wwwroot/uploads
   ```

9. Jalankan aplikasi:
   ```bash
   dotnet run
   ```

## Struktur Database

### Tabel Member
- Id (int, primary key)
- Nama (string)
- Phone (string)
- Email (string, unique)
- Password (string)
- Role (string)

### Tabel BukuTamu
- Id (int, primary key)
- MemberId (int, foreign key)
- Messages (string)
- Gambar (string)
- Timestamp (datetime)

## Role User

1. Member
   - Dapat menulis pesan
   - Upload gambar
   - Edit profil sendiri
   - Tidak dapat menghapus pesan

2. Admin
   - Manajemen penuh member
   - Manajemen penuh buku tamu
   - Akses ke semua fitur

## Penggunaan

1. Register sebagai member baru
2. Login dengan email dan password
3. Member dapat menulis pesan dan upload gambar
4. Admin dapat mengelola semua data

## Keamanan

- Password di-hash menggunakan SHA256
- Autentikasi menggunakan Cookie
- Otorisasi berbasis Role
- Validasi email saat registrasi

## Membuat User Admin

Untuk membuat user admin pertama kali, jalankan SQL berikut:

```sql
INSERT INTO member (Nama, Phone, Email, Password, Role) 
VALUES (
    'Admin',
    '08123456789',
    'admin@admin.com',
    'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', -- password: 123456
    'admin'
);
```

## API Endpoints

### GET /api/BukuTamuApi
Mengambil semua data buku tamu
- Method: GET
- Access: Public
- Response: Array of BukuTamuDTO

### GET /api/BukuTamuApi/{id}
Mengambil detail buku tamu berdasarkan ID
- Method: GET
- Access: Public
- Response: BukuTamuDTO

### POST /api/BukuTamuApi
Membuat buku tamu baru
- Method: POST
- Access: Member only
- Request: Form-data
  - messages: string (required)
  - gambar: file (optional)

### PUT /api/BukuTamuApi/{id}
Mengupdate buku tamu
- Method: PUT
- Access: Member (pemilik) atau Admin
- Request: Form-data
  - messages: string (required)
  - gambar: file (optional)

### DELETE /api/BukuTamuApi/{id}
Menghapus buku tamu
- Method: DELETE
- Access: Admin only

### Response Format
```json
{
    "id": 1,
    "memberId": 1,
    "messages": "Pesan buku tamu",
    "gambar": "filename.jpg",
    "timestamp": "2024-03-14T10:30:00",
    "memberNama": "Nama Member"
}
```

