# QlMenuNewSQLServer

Dự án **QlMenuNewSQLServer** là Web API được xây dựng bằng **ASP.NET Core**, sử dụng kiến trúc nhiều tầng theo hướng **Clean Architecture**, kết hợp **CQRS + MediatR**, **Entity Framework Core**, **Repository Pattern**, **Unit of Work**, **FluentValidation** và **SQL Server**.

Hệ thống hỗ trợ quản lý **Menu** và **News** với quan hệ nhiều-nhiều. Mỗi Menu có thể chứa nhiều News, và mỗi News có thể thuộc nhiều Menu.

---

## Công nghệ sử dụng

* ASP.NET Core Web API
* .NET 8
* Entity Framework Core
* SQL Server
* MediatR
* FluentValidation
* Swagger / OpenAPI
* Repository Pattern
* Unit of Work Pattern
* Clean Architecture

---

## Kiến trúc project

Project được chia thành các tầng chính:

```text
QlMenuNewSQLServer
│
├── Api
│   ├── Controller
│   │   ├── MenuController.cs
│   │   └── NewsController.cs
│   └── Program.cs
│
├── Application
│   ├── DTO
│   ├── Requests
│   ├── Usecase
│   ├── Validators
│   └── Common
│
├── Domain
│   ├── entity
│   └── repositories
│
└── Infrastructure
    ├── Configuration
    ├── Data
    ├── Migrations
    └── Repository
```

---

## Chức năng chính

### Menu

* Lấy danh sách Menu
* Lấy Menu theo Id
* Thêm Menu mới
* Cập nhật Menu
* Xóa mềm Menu
* Gắn News vào Menu
* Kiểm tra trùng Slug khi thêm hoặc cập nhật

### News

* Lấy danh sách News
* Lấy News theo Id
* Thêm News mới
* Cập nhật News
* Xóa mềm News
* Gắn Menu vào News
* Kiểm tra trùng Slug khi thêm hoặc cập nhật

---

## Cấu trúc dữ liệu

### Menu

```csharp
public class Menu : BaseId
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public ICollection<News> News { get; set; } = new List<News>();
}
```

### News

```csharp
public class News : BaseId
{
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Content { get; set; }
    public string? Thumbnail { get; set; }
    public ICollection<Menu> Menu { get; set; } = new List<Menu>();
}
```

### BaseId

```csharp
public abstract class BaseId
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? deleted_at { get; set; }
}
```

---

## Quan hệ cơ sở dữ liệu

Hệ thống sử dụng quan hệ **nhiều-nhiều** giữa bảng `menus` và `news`.

Bảng trung gian:

```text
Menu_News
```

Quan hệ:

```text
menus  n-n  news
```

Một Menu có thể chứa nhiều News.

Một News có thể thuộc nhiều Menu.

---

## Cài đặt project

### 1. Clone project

```bash
git clone <repository-url>
cd QlMenuNewSQLServer
```

### 2. Cấu hình chuỗi kết nối

Mở file `appsettings.json` trong project `Api` và cấu hình:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=QlMenuNewSQLServer;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Nếu dùng SQL Server có tài khoản đăng nhập:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=QlMenuNewSQLServer;User Id=sa;Password=your_password;TrustServerCertificate=True;"
  }
}
```

---

## Cài đặt package cần thiết

Nếu project chưa có package, có thể cài bằng các lệnh sau:

```bash
dotnet add Api package Microsoft.EntityFrameworkCore.SqlServer
dotnet add Api package Microsoft.EntityFrameworkCore.Tools
dotnet add Api package MediatR
dotnet add Api package FluentValidation.DependencyInjectionExtensions
dotnet add Api package Swashbuckle.AspNetCore
```

Nếu package được đặt ở tầng `Infrastructure` hoặc `Application`, hãy cài đúng vào project tương ứng.

---

## Migration database

### Tạo migration

```bash
dotnet ef migrations add InitialCreate -p Infrastructure -s Api
```

### Cập nhật database

```bash
dotnet ef database update -p Infrastructure -s Api
```

Trong đó:

* `-p Infrastructure`: project chứa DbContext và Migration
* `-s Api`: project chạy chính

---

## Chạy project

Tại thư mục chứa solution, chạy:

```bash
dotnet run --project Api
```

Sau khi chạy thành công, mở Swagger:

```text
https://localhost:<port>/swagger
```

Hoặc:

```text
http://localhost:<port>/swagger
```

---

## API Endpoints

### Menu API

| Method | Endpoint         | Mô tả              |
| ------ | ---------------- | ------------------ |
| GET    | `/api/Menu`      | Lấy danh sách Menu |
| GET    | `/api/Menu/{id}` | Lấy Menu theo Id   |
| POST   | `/api/Menu`      | Thêm Menu mới      |
| PUT    | `/api/Menu/{id}` | Cập nhật Menu      |
| DELETE | `/api/Menu/{id}` | Xóa mềm Menu       |

### News API

| Method | Endpoint         | Mô tả              |
| ------ | ---------------- | ------------------ |
| GET    | `/api/News`      | Lấy danh sách News |
| GET    | `/api/News/{id}` | Lấy News theo Id   |
| POST   | `/api/News`      | Thêm News mới      |
| PUT    | `/api/News/{id}` | Cập nhật News      |
| DELETE | `/api/News/{id}` | Xóa mềm News       |

---

## Ví dụ request

### Thêm Menu kèm danh sách News

```json
{
  "name": "Tin tức",
  "slug": "tin-tuc",
  "danhSachNews": [
    {
      "title": "Bài viết 1",
      "slug": "bai-viet-1",
      "content": "Nội dung bài viết 1",
      "Thumbnail": "img1.jpg"
    },
    {
      "title": "Bài viết 2",
      "slug": "bai-viet-2",
      "content": "Nội dung bài viết 2",
      "Thumbnail": "img2.jpg"
    }
  ]
}
```

### Thêm News kèm danh sách Menu

```json
{
  "title": "Tin công nghệ mới",
  "slug": "tin-cong-nghe-moi",
  "content": "Nội dung tin công nghệ mới",
  "Thumbnail": "tech.jpg",
  "danhSachMenus": [
    {
      "name": "Công nghệ",
      "slug": "cong-nghe"
    },
    {
      "name": "Tin tức",
      "slug": "tin-tuc"
    }
  ]
}
```

### Cập nhật Menu

```json
{
  "name": "Tin tức mới",
  "slug": "tin-tuc-moi"
}
```

### Cập nhật News

```json
{
  "title": "Bài viết đã cập nhật",
  "slug": "bai-viet-da-cap-nhat",
  "content": "Nội dung đã cập nhật",
  "Thumbnail": "updated.jpg"
}
```

---

## Validation

Project sử dụng **FluentValidation** để kiểm tra dữ liệu đầu vào.

Một số rule chính:

### Menu

* `Name` không được để trống
* `Name` tối đa 255 ký tự
* `Slug` không được để trống
* `Slug` tối đa 255 ký tự
* `Slug` chỉ được chứa chữ, số, dấu gạch ngang hoặc gạch dưới
* Danh sách News không được trùng Slug

### News

* `Title` không được để trống
* `Title` tối đa 255 ký tự
* `Slug` không được để trống
* `Slug` tối đa 255 ký tự
* `Content` không được để trống
* `Thumbnail` tối đa 255 ký tự
* Danh sách Menu không được trùng Slug

---

## Xử lý trùng Slug

Khi thêm hoặc cập nhật Menu/News, hệ thống sẽ kiểm tra Slug đã tồn tại hay chưa.

Ví dụ:

* Nếu thêm Menu có Slug đã tồn tại, hệ thống trả lỗi.
* Nếu thêm News có Slug đã tồn tại nhưng Title khác, hệ thống trả lỗi.
* Nếu Slug đã tồn tại và thông tin khớp, hệ thống có thể tái sử dụng bản ghi cũ để tạo quan hệ.

Điều này giúp hạn chế việc tạo dữ liệu trùng và tránh sai quan hệ giữa Menu và News.

---

## Soft Delete

Project sử dụng cơ chế **xóa mềm**.

Khi xóa Menu hoặc News, dữ liệu không bị xóa khỏi database mà chỉ cập nhật:

```csharp
IsDeleted = true;
deleted_at = DateTime.Now;
```

Các truy vấn lấy danh sách sẽ chỉ lấy dữ liệu có:

```csharp
IsDeleted == false
```

Cách này giúp có thể khôi phục dữ liệu về sau nếu cần.

---

## Repository Pattern

Project sử dụng Repository để tách phần truy cập dữ liệu ra khỏi nghiệp vụ.

Repository dùng chung:

```csharp
public interface IRepository<T> where T : BaseId
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

Repository riêng cho Menu:

```csharp
public interface IMenuRepo : IRepository<Menu>
{
    Task<Menu?> GetBySlugAsync(string slug);
    Task<Menu?> GetByIdWithNewsAsync(int id);
    Task<IEnumerable<Menu>> GetAllWithNewsAsync();
}
```

Repository riêng cho News:

```csharp
public interface INewsRepo : IRepository<News>
{
    Task<News?> GetBySlugAsync(string slug);
    Task<News?> GetByIdWithMenusAsync(int id);
    Task<IEnumerable<News>> GetAllWithMenusAsync();
}
```

---

## Unit of Work

Project sử dụng Unit of Work để quản lý transaction và lưu thay đổi vào database.

```csharp
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
```

Unit of Work giúp đảm bảo các thao tác thêm Menu, News và quan hệ giữa chúng được xử lý đồng bộ trong cùng một transaction.

---

## Swagger

Project có tích hợp Swagger để test API.

Trong `Program.cs`:

```csharp
builder.Services.AddSwaggerGen();
```

Và bật Swagger:

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

Khi chạy project, truy cập:

```text
/swagger
```

để kiểm thử API.

---

## Gợi ý tối ưu thêm

Một số điểm có thể cải thiện thêm:

* Thêm `AsNoTracking()` cho các query chỉ đọc
* Thống nhất dùng `DateTime.UtcNow`
* Thêm phân trang cho API lấy danh sách
* Chuẩn hóa tên property C# theo PascalCase
* Thêm global query filter cho soft delete
* Thêm middleware xử lý lỗi tổng quát
* Không push thư mục `bin` và `obj` lên Git

---

## File `.gitignore` nên có

```gitignore
bin/
obj/
.vs/
*.user
*.suo
appsettings.Development.json
```