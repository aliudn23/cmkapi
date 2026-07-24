# cmkapi

API sederhana untuk autentikasi dan manajemen user menggunakan ASP.NET Core.

## Persyaratan

Sebelum memulai, pastikan sudah terinstall:

- .NET SDK 10
- PostgreSQL
- Git

## 1. Siapkan database PostgreSQL

Buat database PostgreSQL lokal, misalnya:

```bash
createdb cmkdb
```

Lalu isi koneksi database di file `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=cmkdb;Username=postgres;Password=your_password"
  }
}
```

Pastikan username dan password sesuai dengan instalasi PostgreSQL Anda.

## 2. Install dependency

Jalankan perintah berikut di folder project:

```bash
dotnet restore
```

## 3. Jalankan migrasi database

Jika belum ada database schema, jalankan:

```bash
dotnet ef database update
```

Jika `dotnet ef` belum tersedia, install dulu:

```bash
dotnet tool install --global dotnet-ef
```

## 4. Jalankan project

```bash
dotnet run
```

Setelah berjalan, aplikasi biasanya tersedia di:

- http://localhost:5000
- atau https://localhost:5001

Atau sesuaikan dengan port kalian

## Catatan

- Untuk development, project ini memakai file `appsettings.Development.json`.
- Jika ingin mengubah JWT, email, atau koneksi database, edit file tersebut.
- Dan rename menajadi appsettings.json
