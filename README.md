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

- ASP.NET Core 7.0 (LTS)
- Entity Framework Core
- MySQL (XAMPP)
- Bootstrap 5
- jQuery

## Persyaratan Sistem

- .NET 7.0 SDK
- XAMPP (dengan MySQL)
- Visual Studio 2022 atau VS Code

## Persiapan Development

### Mengelola Versi .NET SDK

#### Melihat Versi Terinstal
```bash
# Lihat daftar SDK
dotnet --list-sdks

# Lihat daftar runtime
dotnet --list-runtimes
```

#### Uninstall .NET SDK
1. Menggunakan Windows Settings:
   - Buka Settings > Apps & Features
   - Cari "Microsoft .NET SDK"
   - Pilih versi yang ingin dihapus
   - Klik Uninstall

2. Menggunakan PowerShell (Administrator):
   ```powershell
   # Uninstall SDK versi spesifik
   winget uninstall Microsoft.DotNet.SDK.9
   ```

3. Menggunakan dotnet-core-uninstall tool:
   ```bash
   # Install tool
   dotnet tool install -g dotnet-core-uninstall

   # Lihat versi terinstal
   dotnet-core-uninstall list

   # Hapus SDK spesifik
   dotnet-core-uninstall remove 9.0.100
   ```

#### Downgrade/Upgrade Proyek
1. Edit file .csproj:
   ```xml
   <PropertyGroup>
       <TargetFramework>net7.0</TargetFramework>
   </PropertyGroup>
   ```

2. Update package versions di .csproj:
   ```xml
   <PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.2" />
   <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="7.0.0-silver.1" />
   ```

3. Restore dan build ulang:
   ```bash
   dotnet restore
   dotnet build
   ```

> Note: Setelah uninstall/install SDK, restart terminal atau PowerShell untuk memperbarui PATH

1. Install .NET SDK
   - Jika ada versi .NET lain yang terinstal, uninstall terlebih dahulu:
     - Buka Settings > Apps & Features, atau
     - Jalankan di PowerShell: `winget uninstall Microsoft.DotNet.SDK.9`
   - Download .NET 7.0 SDK dari https://dotnet.microsoft.com/download/dotnet/7.0
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

2. Konfigurasi HTTPS Development Certificate:
   ```bash
   dotnet dev-certs https --trust
   ```

3. Buka terminal/command prompt, masuk ke direktori proyek:
   ```bash
   cd path/to/bukutamu_csharp_aspnetcore_mysql
   ```

4. Install dependencies:
   ```bash
   dotnet restore
   ```

5. Install dan jalankan XAMPP
   - Download XAMPP terbaru dari https://www.apachefriends.org/
   - Install XAMPP
   - Start Apache dan MySQL dari XAMPP Control Panel

6. Buat database dan tabel
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

7. Konfigurasi aplikasi di appsettings.json:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=bukutamu_simple;User=root;Password=;"
     },
     "Kestrel": {
       "Endpoints": {
         "Http": {
           "Url": "http://localhost:5000"
         },
         "Https": {
           "Url": "https://localhost:5001"
         }
       }
     }
   }
   ```
   Note: 
   - Secara default XAMPP MySQL menggunakan username "root" tanpa password
   - Aplikasi akan berjalan di http://localhost:5000 dan https://localhost:5001

8. Jalankan migrasi database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

9. Buat folder untuk upload gambar:
   ```bash
   mkdir wwwroot/uploads
   ```

10. Jalankan aplikasi:
    ```bash
    dotnet run
    ```

11. Akses aplikasi:
    - Setelah menjalankan `dotnet run`, akan muncul output seperti ini:
    ```bash
    info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
    info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
    ```
    - Buka browser dan akses salah satu URL tersebut (disarankan menggunakan HTTPS)
    - Jika muncul peringatan keamanan di browser, klik "Advanced" dan "Proceed"
    - Untuk mempercayai sertifikat development secara permanen, jalankan:
    ```bash
    dotnet dev-certs https --trust
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
   - Maksimal 1 pesan per hari
   - Upload gambar
   - Edit profil sendiri
   - Tidak dapat menghapus pesan

2. Admin
   - Manajemen penuh member
   - Manajemen penuh buku tamu
   - Dapat menulis dan edit pesan
   - Maksimal 1 pesan per hari
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

## Error Handling

Aplikasi memiliki beberapa mekanisme error handling:

1. Development Environment
   - Menampilkan detailed error page
   - Swagger UI tersedia di /swagger

2. Production Environment
   - Custom error page (/Views/Shared/Error.cshtml)
   - HTTPS enforcement dengan HSTS
   - Logging ke file system

3. API Responses
   - 400 Bad Request - Invalid input
   - 401 Unauthorized - Not authenticated
   - 403 Forbidden - Not authorized
   - 404 Not Found - Resource not found
   - 500 Internal Server Error - Server errors

