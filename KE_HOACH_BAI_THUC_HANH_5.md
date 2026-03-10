# Kế hoạch thực hiện Bài thực hành 5

## Tài liệu đầu vào

- File đề bài: `Bai thuc hanh 5_Study case_Cong ty bao hiem_Cai dat 2_phần 1.pdf`
- Phạm vi hiện tại của bài: `Task #1` và `Task #2`

## Mục tiêu của bài 5

1. Bổ sung code cho microservice `PricingService`.
2. Cài đặt và cấu hình Eureka.
3. Đăng ký `PricingService` vào Eureka.
4. Kiểm tra `PricingService` hiển thị trên dashboard Eureka tại `http://localhost:8761`.

## Hiện trạng repo sau khi rà soát

### Đã có

- `eureka-server` đã tồn tại trong repo.
- File cấu hình Eureka đã có tại:
  - `eureka-server/src/main/resources/application.yml`
- Bộ `PricingService*` hiện đã có code thật, không còn là project khung:
  - `PricingService`
  - `PricingService.Api`
  - `PricingService.Test`
  - `PricingService.IntegrationTest`
- `PricingService` hiện đã reference `PricingService.Api`.
- Solution build thành công.

### Đã có một phần của Task #2

- `PricingService` đã có `Startup.cs`.
- `Startup.cs` đã có:
  - `using Steeltoe.Discovery.Client;`
  - `services.AddDiscoveryClient(Configuration);`
- `PricingService.csproj` đã có package Steeltoe/Eureka.
- `PricingService/appsettings.json` đã có một phần cấu hình `Spring` và `Eureka`.
- `PricingService/appsettings.docker.json` đã có cấu hình Eureka cho môi trường Docker.

### Còn thiếu hoặc chưa khớp đề bài

- `Startup.Configure(...)` của `PricingService` hiện chưa có `app.UseDiscoveryClient();`
- `eureka-server/src/main/resources/application.yml` hiện chưa thấy:
  - `eureka.server.waitTimeInMsWhenSyncEmpty = 0`
- `PricingService/appsettings.json` chưa đủ các khóa mà đề bài yêu cầu rõ:
  - `Eureka:Client:ShouldRegisterWithEureka`
  - `Eureka:Client:ServiceUrl`
  - `Eureka:Client:ValidateCertificates`
  - `Eureka:Instance:AppName`
- Cần chạy thật `eureka-server` và `PricingService` để xác nhận đăng ký thành công, chưa chỉ dừng ở build.

## Đánh giá nhanh theo từng task

### Task #1: Bổ sung code cho microservice PricingService

- Trạng thái hiện tại: gần như đã hoàn thành.
- Cơ sở đánh giá:
  - Có controller, handler, domain, data access, init data, API contracts, unit test, integration test.
  - Không còn là solution khung.
- Việc còn lại:
  - Chỉ cần rà thêm nếu muốn đối chiếu tuyệt đối với source mẫu thầy cung cấp.

### Task #2: Cài đặt và cấu hình liên quan đến Eureka

- Trạng thái hiện tại: chưa hoàn thành.
- Đây là phần cần làm tiếp để hoàn chỉnh bài 5 phần 1.

## Kế hoạch thực hiện chi tiết

## Giai đoạn 1: Chốt Task #1

1. Rà lại 4 project `PricingService*` với source mẫu của thầy nếu có file nén gốc.
2. Xác nhận tên project, namespace, file, cấu trúc thư mục đã khớp source mẫu.
3. Build lại solution.
4. Chạy unit test của `PricingService.Test`.
5. Ghi nhận:
   - `PricingService.IntegrationTest` có thể fail nếu Docker chưa chạy, đây là vấn đề môi trường test chứ không phải thiếu code nghiệp vụ.

## Giai đoạn 2: Chuẩn hóa Eureka Server

1. Mở file:
   - `eureka-server/src/main/resources/application.yml`
2. Đối chiếu với đề bài, bảo đảm có đầy đủ:
   - `server.port: 8761`
   - `eureka.client.registerWithEureka: false`
   - `eureka.client.fetchRegistry: false`
   - `eureka.server.waitTimeInMsWhenSyncEmpty: 0`
3. Nếu thiếu `waitTimeInMsWhenSyncEmpty`, bổ sung vào file.
4. Chọn cách chạy Eureka:
   - Ưu tiên chạy bằng Docker nếu dễ dùng hơn trong repo hiện tại.
   - Nếu không, chạy theo cách của project Java/Gradle.
5. Sau khi chạy, mở:
   - `http://localhost:8761`
6. Xác nhận dashboard Eureka hiển thị bình thường.

## Giai đoạn 3: Hoàn thiện cấu hình Eureka Client trong PricingService

### Việc cần kiểm tra trong mã nguồn

1. Mở:
   - `PricingService/Startup.cs`
2. Kiểm tra `ConfigureServices(...)` đã có:
   - `services.AddDiscoveryClient(Configuration);`
3. Kiểm tra `Configure(...)` phải có thêm:
   - `app.UseDiscoveryClient();`
4. Nếu chưa có, bổ sung theo đúng hướng dẫn PDF.

### Việc cần kiểm tra trong package

1. Mở:
   - `PricingService/PricingService.csproj`
2. Xác nhận package Eureka client đã có.
3. Nếu package đang khác tên với PDF nhưng cùng họ Steeltoe Discovery/Eureka, cần đánh giá:
   - có tương thích với cách cấu hình hiện tại hay không
   - có dùng được với `AddDiscoveryClient` và `UseDiscoveryClient` hay không

## Giai đoạn 4: Hoàn thiện appsettings của PricingService

1. Mở:
   - `PricingService/appsettings.json`
2. Đối chiếu với đề bài và bổ sung đầy đủ các khóa sau:
   - `Spring:Application:Name = PricingService`
   - `Eureka:Client:ShouldRegisterWithEureka = true`
   - `Eureka:Client:ServiceUrl = http://localhost:8761/eureka`
   - `Eureka:Client:ValidateCertificates = false`
   - `Eureka:Instance:AppName = PricingService`
   - `Eureka:Instance:HostName = localhost`
   - `Eureka:Instance:Port = 5040`
3. Kiểm tra lại:
   - `PricingService/Properties/launchSettings.json`
   - port thực tế của app
4. Nếu port chạy local khác `5040`, phải chỉnh lại để đồng bộ giữa:
   - appsettings
   - launch settings
   - tài liệu kiểm thử

## Giai đoạn 5: Chạy thử end-to-end Task #2

1. Chạy Eureka server trước.
2. Kiểm tra bằng trình duyệt:
   - `http://localhost:8761`
3. Từ thư mục gốc repo, chạy:
   - `dotnet run --project ./PricingService`
4. Theo dõi log:
   - không lỗi config
   - không lỗi kết nối Eureka
   - không lỗi package
5. Mở lại dashboard Eureka.
6. Xác nhận có service:
   - `PricingService`
7. Trạng thái mong đợi:
   - `UP`

## Giai đoạn 6: Kiểm tra sau khi hoàn tất Task #2

1. Build lại solution:
   - `dotnet build InsuranceCompany.Microservices.sln`
2. Chạy unit test:
   - `dotnet test PricingService.Test/PricingService.Test.csproj --no-build`
3. Nếu Docker đang chạy, có thể chạy thêm:
   - `dotnet test PricingService.IntegrationTest/PricingService.IntegrationTest.csproj --no-build`
4. Ghi lại kết quả:
   - Eureka có chạy được không
   - `PricingService` có đăng ký được không
   - Có xuất hiện trên dashboard không

## Checklist riêng cho Task #2

### Cần sửa trong file

- `eureka-server/src/main/resources/application.yml`
- `PricingService/Startup.cs`
- `PricingService/appsettings.json`
- Có thể cần kiểm tra thêm:
  - `PricingService/Properties/launchSettings.json`
  - `PricingService/appsettings.docker.json`

### Cần xác nhận bằng chạy thật

- Eureka mở được tại `localhost:8761`
- `PricingService` chạy được bằng `dotnet run`
- Dashboard Eureka hiển thị `PricingService`

## Thứ tự ưu tiên thực hiện từ bây giờ

1. Chuẩn hóa `application.yml` của Eureka.
2. Bổ sung `app.UseDiscoveryClient()` trong `PricingService/Startup.cs`.
3. Hoàn thiện `PricingService/appsettings.json` theo đúng đề.
4. Chạy Eureka.
5. Chạy `PricingService`.
6. Kiểm tra dashboard Eureka.
7. Build và test lại.

## Kết quả mong đợi cuối cùng

- `Task #1`: hoàn tất về mặt code của `PricingService`
- `Task #2`: hoàn tất về mặt cấu hình và chạy thật
- `PricingService` đăng ký thành công lên Eureka
- Bài 5 phần 1 có thể xem là hoàn chỉnh trong phạm vi PDF hiện tại

## Ghi chú

- `Task #3` trong PDF ghi “Còn tiếp”, nên hiện tại chỉ cần tập trung hoàn tất `Task #1` và `Task #2`.
- Trọng tâm thực tế lúc này là hoàn tất `Task #2`, vì `Task #1` đã ở mức gần xong sau khi rà riêng bộ `PricingService*`.

