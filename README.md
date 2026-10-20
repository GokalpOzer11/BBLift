# BBLift API

BBLift, antrenman planlama ve takip uygulamasıdır. Kullanıcılar egzersiz kütüphanesinden rutin oluşturur, antrenmanlarını set set kaydeder, geçmişini ve toplam hacmini görür; üyelik paketlerini sepete ekleyip ödeme ve sipariş akışını tamamlar. Yöneticiler panelden içerikleri ve kullanıcı rollerini yönetir.

Bu depo **API** katmanıdır. Arayüz (MVC) için `BBLiftClient` deposuna bakınız.

## Mimari

Katmanlı mimari, generic repository pattern, FluentValidation, ASP.NET Core Identity + JWT.

- `BBLift.Domain`: Entity'ler, value object'ler, enum'lar
- `BBLift.Application`: Servisler, DTO'lar, doğrulayıcılar, `Result` yapısı
- `BBLift.Infrastructure`: EF Core, DbContext, repository'ler, migration'lar
- `BBLift.WebApi`: Controller'lar, kimlik doğrulama, Scalar (OpenAPI) dokümantasyonu
- `BBLift.Mvc`: Eski deneme arayüzü, kullanılmıyor

## Özellikler

- Üyelik sistemi (kayıt, giriş, JWT, rol bazlı yetkilendirme: SuperAdmin, Admin, User)
- Egzersiz, kas grubu, ekipman yönetimi
- Rutin oluşturma ve rutine egzersiz ekleme
- Antrenman seansı ve set kaydı, toplam hacim hesabı
- Üyelik paketleri, sepet, sipariş ve ödeme (demo)
- Kullanıcılara rol ekleme ve çıkarma (SuperAdmin)

## Kurulum

Gereksinimler: .NET SDK (10), SQL Server.

1. Gizli ayarları girin. Değerler depoda tutulmaz, kullanıcı kasasına yazılır:

```bash
cd BBLift.WebApi
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DbConn" "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True"
dotnet user-secrets set "JWT:Secret" "en-az-32-karakterlik-gizli-bir-anahtar-yazin"
```

2. Veritabanını oluşturun:

```bash
dotnet ef database update --project BBLift.Infrastructure --startup-project BBLift.WebApi
```

3. API'yi çalıştırın:

```bash
cd BBLift.WebApi
dotnet run
```

API varsayılan olarak `https://localhost:7273` adresinde çalışır. Önce API, sonra MVC (BBLiftClient) başlatılmalıdır.

## Varsayılan yönetici hesabı

Uygulama ilk açılışta rolleri ve bir yönetici hesabı oluşturur:

- E-posta: `super.admin@bblift.com`
- Şifre: `Admin@123`

Şifreyi değiştirmek için hesap oluşmadan önce `dotnet user-secrets set "Seed:SuperAdminPassword" "yeni-sifre"` komutunu çalıştırın.

## Notlar

- Ödeme formu demodur, kart bilgileri kaydedilmez ve gönderilmez.
- Rol değişiklikleri, kullanıcının bir sonraki girişinde geçerli olur.
