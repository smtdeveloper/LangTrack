# LangTrack - Günlük İngilizce Öğrenme API'si

LangTrack, kullanıcıların günlük İngilizce kelime öğrenme süreçlerini takip etmelerine yardımcı olan modern bir web API'sidir. Clean Architecture prensiplerine uygun olarak geliştirilmiş, JWT tabanlı kimlik doğrulama ve yetkilendirme sistemi içerir.

## 🎯 Proje Özellikleri

### 👤 Kullanıcı Yönetimi
- **Kayıt Olma**: Yeni kullanıcı hesapları oluşturma
- **Giriş Yapma**: JWT token tabanlı kimlik doğrulama
- **Rol Tabanlı Yetkilendirme**: Student, Admin, SuperAdmin rolleri
- **Kullanıcı Profili**: Kişisel bilgileri görüntüleme ve güncelleme

### 📚 Kelime Yönetimi
- **Kelime Ekleme**: Kişisel kelime koleksiyonu oluşturma
- **Kelime Listeleme**: Kendi kelimelerini sayfalama ve arama ile görüntüleme
- **Kelime Silme**: Soft delete ile güvenli kelime silme
- **Kelime Arama**: Metin ve anlam bazlı arama

### 🎲 Rastgele Öğrenme
- **Kendi Kelimelerinden Rastgele**: Kişisel koleksiyondan rastgele kelime seçimi
- **Global Rastgele**: Tüm sistemdeki kelimelerden rastgele seçim
- **Çeşitli Öğrenme**: Farklı kullanıcıların kelimelerini keşfetme

### 🌐 Sosyal Öğrenme
- **Tüm Kelimeleri Görme**: Sistemdeki tüm kullanıcıların kelimelerini görüntüleme
- **Kelime Havuzu**: Geniş kelime koleksiyonundan faydalanma
- **Sosyal Keşif**: Diğer kullanıcıların kelime seçimlerini inceleme

### 📊 İstatistik ve Takip
- **Çalışma Kayıtları**: Kelime çalışma geçmişi
- **İstatistikler**: Öğrenme performansı analizi
- **İlerleme Takibi**: Günlük çalışma takibi

## 🏗️ Teknik Mimari

### Clean Architecture
- **Domain Layer**: İş mantığı ve varlıklar
- **Application Layer**: Servisler ve DTO'lar
- **Infrastructure Layer**: Veritabanı ve harici servisler
- **API Layer**: Controllers ve middleware

### Teknolojiler
- **.NET 9.0**: Modern C# framework
- **Entity Framework Core**: ORM ve veritabanı yönetimi
- **SQLite**: Hafif ve taşınabilir veritabanı
- **JWT Authentication**: Güvenli kimlik doğrulama
- **Swagger/OpenAPI**: API dokümantasyonu
- **AutoMapper**: Obje dönüşümleri

### Güvenlik
- **JWT Bearer Token**: Stateless kimlik doğrulama
- **Role-Based Authorization**: Rol tabanlı yetkilendirme
- **Permission-Based Access**: İzin tabanlı erişim kontrolü
- **Password Hashing**: Güvenli şifre saklama

## 📋 API Endpoints

### 🔐 Kimlik Doğrulama
- `POST /api/v1/auth/register` - Kullanıcı kaydı
- `POST /api/v1/auth/login` - Kullanıcı girişi
- `GET /api/v1/auth/me` - Kullanıcı bilgileri

### 📚 Kelime Yönetimi
- `POST /api/v1/words` - Yeni kelime ekleme
- `GET /api/v1/words` - Kendi kelimelerini listeleme (sayfalama + arama)
- `GET /api/v1/words/{id}` - Belirli kelimeyi görüntüleme
- `DELETE /api/v1/words/{id}` - Kelime silme

### 🎲 Rastgele Öğrenme
- `GET /api/v1/words/random` - Kendi kelimelerinden rastgele
- `GET /api/v1/words/random/global` - Tüm kelimelerden rastgele

### 🌐 Sosyal Öğrenme
- `GET /api/v1/words/all` - Tüm kullanıcıların kelimelerini listeleme (sayfalama + arama)

### 📊 İstatistik ve Takip
- `GET /api/v1/study` - Çalışma kayıtları
- `POST /api/v1/study` - Çalışma kaydı oluşturma
- `GET /api/v1/stats` - İstatistikler

### 👥 Kullanıcı Yönetimi (Admin)
- `GET /api/v1/roles` - Rolleri listeleme
- `GET /api/v1/roles/{id}` - Rol detayları

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- **.NET 9.0 SDK** veya üzeri
- **Git** (projeyi klonlamak için)
- **Visual Studio 2022** veya **VS Code** (önerilen)

### 1. Projeyi İndirme
```bash
git clone <repository-url>
cd LangTrack
```

### 2. Bağımlılıkları Yükleme
```bash
dotnet restore
```

### 3. Veritabanını Hazırlama
```bash
# Veritabanı otomatik olarak oluşturulur ve seed veriler eklenir
# Ekstra migration gerekmez
```

### 4. Projeyi Çalıştırma
```bash
dotnet run --project LangTrack.Api
```

### 5. API'ye Erişim
- **Ana URL**: `http://localhost:5183`
- **Swagger UI**: `http://localhost:5183` (otomatik yönlendirme)
- **API Base URL**: `http://localhost:5183/api/v1/`

## 🧪 Test Kullanıcısı

Proje ilk çalıştırıldığında otomatik olarak test kullanıcısı oluşturulur:

- **Email**: `test@example.com`
- **Password**: `Test123!`
- **Rol**: Student
- **Test Kelimeleri**: 10 adet A1 seviyesinde kelime

## 📖 Kullanım Örnekleri

### 1. Kullanıcı Kaydı
```bash
curl -X POST "http://localhost:5183/api/v1/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "Password123!",
    "firstName": "John",
    "lastName": "Doe"
  }'
```

### 2. Giriş Yapma
```bash
curl -X POST "http://localhost:5183/api/v1/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

### 3. Kelime Ekleme
```bash
curl -X POST "http://localhost:5183/api/v1/words" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "text": "beautiful",
    "meaning": "güzel",
    "example": "She is a beautiful person.",
    "tags": "adjective,a2"
  }'
```

### 4. Rastgele Kelime Alma
```bash
# Kendi kelimelerinden rastgele
curl -X GET "http://localhost:5183/api/v1/words/random" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Tüm sistemden rastgele
curl -X GET "http://localhost:5183/api/v1/words/random/global" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## 🔧 Geliştirme

### Proje Yapısı
```
LangTrack/
├── LangTrack.Api/           # API katmanı
├── LangTrack.Application/   # Uygulama katmanı
├── LangTrack.Domain/       # Domain katmanı
├── LangTrack.Infrastructure/ # Altyapı katmanı
└── LangTrack.sln           # Solution dosyası
```

### Build ve Test
```bash
# Tüm projeyi build etme
dotnet build

# Test çalıştırma (test projeleri varsa)
dotnet test

# Clean build
dotnet clean
dotnet build
```

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📞 İletişim

https://www.instagram.com/smtcoder/
https://www.linkedin.com/in/bensametakca/

## ☕ Destek

Eğer bu projeyi beğendiyseniz ve geliştiricisini desteklemek istiyorsanız:

[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-☕-yellow.svg)](https://buymeacoffee.com/bensametakb)

Proje hakkında sorularınız için issue açabilir veya iletişime geçebilirsiniz.

---

**LangTrack** ile İngilizce öğrenme yolculuğunuzda başarılar! 🚀📚
