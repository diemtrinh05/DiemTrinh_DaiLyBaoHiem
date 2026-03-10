# Rà soát task theo file Bai thuc hanh 4_Study case_Cong ty bao hiem_Cai dat 1.pdf

## Phạm vi rà soát

- Tài liệu đối chiếu: `Bai thuc hanh 4_Study case_Cong ty bao hiem_Cai dat 1.pdf`
- Cách đối chiếu:
  - Đọc nội dung PDF và tách các task/thành phần bắt buộc.
  - Rà soát cấu trúc solution, project, file cấu hình, package và khả năng build/test.
  - Kiểm tra thêm các thành phần phụ trợ: `eureka-server`, `postgres`, `scripts`, `Web`, `BlazorWasmClient`, `AgentPortalApiGateway`.

## Kết luận nhanh

- Repo đã đạt phần lớn yêu cầu về cấu trúc source code và có đầy đủ các thư mục/phụ trợ được yêu cầu add vào solution.
- `dotnet build InsuranceCompany.Microservices.sln` thành công, các test project trong solution đều pass.
- Tuy nhiên, hệ thống chưa thể coi là "build thành công toàn bộ" vì:
  - `BlazorWasmClient` không build được.
  - `scripts/app.yml` đang trỏ đến các `Dockerfile` không tồn tại và tham chiếu `DashboardService` không có trong repo.
  - Các `*.Api` hiện tại phần lớn mới ở mức placeholder (`Class1.cs`), chưa cung cấp các contract mà `BlazorWasmClient` đang sử dụng.
  - Thư mục `Web` tồn tại nhưng build local thất bại trên môi trường hiện tại do stack frontend quá cũ.

## Checklist đối chiếu theo task

| Hạng mục | Trạng thái | Ghi chú |
|---|---|---|
| Task 1.1 - Tạo solution .NET Core | Đạt | Có file `InsuranceCompany.Microservices.sln`. |
| Task 1.1 - Mỗi microservice có 3 project (Api, Implementation, Test) | Đạt | Đã có bộ 3 project cho `AuthService`, `ChatService`, `PricingService`, `PolicyService`, `PolicySearchService`, `PaymentService`, `ProductService`, `DocumentService`, `AgentPortalGateway`. |
| Task 1.2/1.3 - Dùng GitHub, add giảng viên vào repo | Đạt một phần | Có remote GitHub `origin`, nhưng không thể xác minh việc đã add email `caotxuan@gmail.com` vào repo. |
| Task 2.1 - Cài MediatR | Đạt | Các project implementation đã tham chiếu `MediatR.Extensions.Microsoft.DependencyInjection` và gọi `AddMediatR(...)`. |
| Task 2.2 - Cài PostgreSQL >= 10 | Đạt một phần | Có `postgres/Dockerfile` dùng `postgres:11.4` và có `createdatabases.sql`. Không xác minh được DB đã được cài/chạy/restore đầy đủ chỉ từ repo. |
| Task 2.3 - Cài RabbitMQ | Đạt một phần | Có khai báo `rabbitmq:3-management` trong `scripts/infra.yml`. Chưa xác minh runtime. |
| Task 2.4 - Cài Elasticsearch >= 6 | Đạt một phần | Có khai báo `docker.elastic.co/elasticsearch/elasticsearch:8.9.2` trong `scripts/infra.yml`. |
| Task 2.5 - Cài Docker và Docker Compose | Đạt | Môi trường hiện tại có `Docker 29.1.5` và `Docker Compose v5.0.1`. |
| Task 2.6 - Add `eureka-server` vào solution | Đạt | Thư mục tồn tại và đã được add vào solution. |
| Task 2.7 - Add `postgres` vào solution và tạo CSDL | Đạt một phần | Thư mục và script tạo DB tồn tại. Không thấy file backup CSDL để nộp kèm. |
| Task 2.8 - Add `scripts` vào solution | Đạt | Thư mục tồn tại và đã được add vào solution. |
| Task 2.9 - Add `Web` vào solution | Đạt một phần | Thư mục tồn tại và đã được add vào solution, nhưng build local thất bại. |
| Task 2.10 - Add `BlazorWasmClient` vào solution | Đạt một phần | Thư mục tồn tại và đã được add vào solution, nhưng project hiện tại không build được. |
| Task 2.11 - Add `AgentPortalApiGateway` vào solution | Đạt | Thư mục tồn tại, đã được add vào solution và build riêng thành công. |
| Yêu cầu "Build hệ thống thành công" | Chưa đạt hoàn toàn | Chỉ phần `.sln` build xanh; `BlazorWasmClient` và compose app chưa build/chạy được. |

## Những điểm đã đúng

### 1. Cấu trúc solution và bộ 3 project cho microservice

Repo đã có cấu trúc đúng hướng bài tập:

- `AuthService`, `AuthService.Api`, `AuthService.Test`
- `ChatService`, `ChatService.Api`, `ChatService.Test`
- `PricingService`, `PricingService.Api`, `PricingService.Test`
- `PolicyService`, `PolicyService.Api`, `PolicyService.Test`
- `PolicySearchService`, `PolicySearchService.Api`, `PolicySearchService.Test`
- `PaymentService`, `PaymentService.Api`, `PaymentService.Test`
- `ProductService`, `ProductService.Api`, `ProductService.Test`
- `DocumentService`, `DocumentService.Api`, `DocumentService.Test`
- `AgentPortalGateway`, `AgentPortalGateway.Api`, `AgentPortalGateway.Test`

### 2. MediatR đã được cài và đăng ký

Đã tìm thấy package MediatR và cấu hình `AddMediatR(...)` trong các project implementation:

- `AuthService`
- `ChatService`
- `PricingService`
- `PolicyService`
- `PolicySearchService`
- `PaymentService`
- `ProductService`
- `DocumentService`
- `AgentPortalGateway`

### 3. Các thư mục phụ trợ được yêu cầu add vào solution đều tồn tại

Đã có đầy đủ:

- `eureka-server`
- `postgres`
- `scripts`
- `Web`
- `BlazorWasmClient`
- `AgentPortalApiGateway`

### 4. Kết quả build/test .NET

- `dotnet build InsuranceCompany.Microservices.sln`: thành công, `0 Error`.
- `dotnet test InsuranceCompany.Microservices.sln --no-build`: tất cả 9 test project đều pass.
- `dotnet build AgentPortalApiGateway\\AgentPortalApiGateway.csproj`: thành công, nhưng có cảnh báo package `Steeltoe.Discovery.Eureka 3.2.7` có low severity vulnerability.

## Những điểm chưa đúng hoặc chưa đầy đủ

### 1. `BlazorWasmClient` đang vỡ build

Lệnh `dotnet build BlazorWasmClient\\BlazorWasmClient.csproj` thất bại với 57 lỗi.

Nguyên nhân chính:

- `BlazorWasmClient.csproj` tham chiếu `..\\DashboardService.Api\\DashboardService.Api.csproj` nhưng repo không có `DashboardService.Api`.
- `BlazorWasmClient` đang sử dụng các namespace/type như `ProductService.Api.Queries`, `PolicyService.Api.Commands`, `PolicySearchService.Api.Queries`...
- Trong khi đó các project `*.Api` hiện tại mới chỉ có file placeholder kiểu `Class1.cs`, ví dụ `ProductService.Api\\Class1.cs`.

Tác động:

- Thư mục `BlazorWasmClient` có tồn tại nhưng chưa đạt yêu cầu build/chạy được.

### 2. `scripts/app.yml` đang lệch với repo thực tế

File `scripts/app.yml` hiện đang có các vấn đề:

- Trỏ đến các `Dockerfile` không tồn tại:
  - `AuthService/Dockerfile`
  - `ChatService/Dockerfile`
  - `PaymentService/Dockerfile`
  - `PolicySearchService/Dockerfile`
  - `PolicyService/Dockerfile`
  - `PricingService/Dockerfile`
  - `ProductService/Dockerfile`
  - `DashboardService/Dockerfile`
- Có tham chiếu `DashboardService`, nhưng repo không có project/thư mục này.
- Không thấy `DocumentService` trong `scripts/app.yml` dù đây là một service trong phụ lục kiến trúc.

Tác động:

- Compose app hiện tại không thể dùng để build/chạy đầy đủ hệ thống.

### 3. `Web` đã được add nhưng chưa xác nhận build thành công

Thư mục `Web` có đầy đủ source và có `Dockerfile`.

Tuy nhiên:

- Thử `cmd /c npm ci` thất bại trên môi trường hiện tại.
- Lỗi tập trung ở stack frontend cũ (`Vue 2`, `node-sass@4.9.2`, `node-gyp`) và yêu cầu `python` khi build local.

Lưu ý:

- `Web/Dockerfile` đang pin môi trường build theo `node:10.24.0-alpine3.11`, nên khả năng frontend được kỳ vọng build trong Docker thay vì build local bằng Node mới.
- Dù vậy vẫn nên test lại `docker build` để xác minh.

### 4. Test project mới ở mức khung

Tất cả test project đều pass, nhưng hiện tại mới thấy `UnitTest1.cs` mặc định và mỗi project chỉ có 1 test placeholder.

Ví dụ:

- `AuthService.Test\\UnitTest1.cs`
- `ChatService.Test\\UnitTest1.cs`
- `PaymentService.Test\\UnitTest1.cs`
- `PricingService.Test\\UnitTest1.cs`
- `ProductService.Test\\UnitTest1.cs`
- `PolicyService.Test\\UnitTest1.cs`
- `PolicySearchService.Test\\UnitTest1.cs`
- `DocumentService.Test\\UnitTest1.cs`
- `AgentPortalGateway.Test\\UnitTest1.cs`

Nhận xét:

- Điều này đáp ứng yêu cầu "có test project".
- Nhưng chưa thể coi là đã có unit test/integration test cho nghiệp vụ.

### 5. Chưa thấy file backup CSDL để nộp kèm

PDF yêu cầu nộp các file backup CSDL PostgreSQL để giảng viên restore và kiểm tra.

Rà soát repo chỉ thấy:

- `postgres\\createdatabases.sql`

Không thấy:

- `.backup`
- `.bak`
- `.dump`

## Nhận xét bổ sung

### 1. Solution build xanh nhưng chưa bao phủ toàn bộ thành phần

`InsuranceCompany.Microservices.sln` build thành công, nhưng cần lưu ý:

- `AgentPortalApiGateway` và `BlazorWasmClient` đang được add vào solution theo dạng folder/nội dung, không phải project .NET build cùng `dotnet solution build`.
- Vì vậy, kết quả `dotnet build InsuranceCompany.Microservices.sln` không chứng minh rằng toàn bộ hệ thống đã build được.

### 2. Repo đã có GitHub remote

Đã tìm thấy remote:

- `origin = https://github.com/diemtrinh05/DiemTrinh_DaiLyBaoHiem.git`

Tuy nhiên, không thể kiểm tra từ local xem giảng viên đã được thêm collaborator hay chưa.

## Đánh giá tổng hợp

### Mức độ hoàn thành

- Đạt khá tốt phần "tạo khung solution và các project".
- Đạt phần lớn phần "add các thư mục phụ trợ vào solution".
- Chưa đạt phần "build hệ thống thành công" nếu tính đầy đủ cả `BlazorWasmClient`, frontend Vue và app compose.

### Mức ưu tiên cần sửa

1. Quyết định rõ có hay không `DashboardService`.
2. Bổ sung contract thật trong các project `*.Api` hoặc sửa `BlazorWasmClient` để khớp với API hiện có.
3. Bổ sung `Dockerfile` cho các service đang được tham chiếu trong `scripts/app.yml`, hoặc sửa `app.yml` cho khớp repo.
4. Xem lại `DocumentService` có cần thêm vào `scripts/app.yml` hay không.
5. Bổ sung test thực tế thay cho `UnitTest1`.
6. Tạo và đưa các file backup CSDL vào bài nộp nếu giảng viên yêu cầu.

