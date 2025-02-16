# Optik Form Otomasyon Projesi

## Proje Açıklaması
**Optik Form Otomasyon Projesi**, öğrenci cevap kağıtlarını işlemek ve sınav süreçlerini yönetmek için geliştirilmiş bir Windows Forms uygulamasıdır. Proje, öğrenci bilgilerini alarak PDF formatında optik formlar oluşturur ve sınav kağıtlarını otomatik olarak düzenler.

## Kullanılan Teknolojiler
- **C# (.NET Framework)** - Windows Forms uygulaması için
- **iTextSharp** - PDF oluşturma ve düzenleme işlemleri için
- **MySQL** - Veritabanı bağlantıları ve veri saklama için
- **Windows Forms** - Kullanıcı arayüzü geliştirme

## Proje Yapısı
### **Ana Sınıflar ve Dosyalar**

1. **`Program.cs`** → Uygulamanın giriş noktasıdır ve `Form1` adlı formu başlatır.
2. **`Form1.cs`** → Kullanıcı arayüzünü yönetir, PDF oluşturma işlemlerini başlatır.
3. **`DbConnection.cs`** → MySQL veritabanı bağlantısını sağlar.
4. **`DbValuePositioner.cs`** → Öğrenci bilgilerini veritabanından alıp PDF’e konumlandırır.
5. **PDF İşleme İçin Kullanılan Sınıflar:**
   - **`StudentInformationTable.cs`** → Öğrenci bilgilerini içeren tabloları oluşturur.
   - **`AnswersTable.cs`** → Öğrencinin sınav cevaplarını içeren tabloyu oluşturur.
   - **`NumberedTable.cs`** → TC Kimlik ve öğrenci numarası için özel alanları yönetir.
   - **`BookletTypeTable.cs`** → Kitapçık türü ve gözetmen paraf bölümlerini oluşturur.
   - **`NotTakeExam.cs`** → Sınava girmeyen öğrenciler için bir bölüm ekler.
   - **`ExampleMarking.cs`** → Örnek işaretleme yönergeleri içerir.
   - **`Attention.cs`** → Optik form doldurma kurallarını içeren bir uyarı bölümü ekler.

## Kurulum ve Çalıştırma
1. **Bağımlılıkları yükleyin:**
   - iTextSharp ve MySQL kütüphaneleri projenize dahil edilmelidir.
2. **Veritabanı bağlantısını yapılandırın:**
   - `App.config` dosyasındaki `DefaultConnection` bağlantı dizesini güncelleyin.
3. **Projeyi derleyip çalıştırın:**
   - Visual Studio kullanarak `OptikProjeTumSayfa.sln` dosyasını açın ve çalıştırın.

## Kullanım
1. Uygulamayı başlatın.
2. **"PDF Oluştur"** butonuna basarak öğrencilerin sınav optik formunu oluşturun.
3. PDF dosyası belirtilen dizine kaydedilecektir.
4. PDF içerisinde **öğrenci bilgileri, kitapçık türü, cevap alanı, kimlik numarası gibi bölümler otomatik olarak oluşturulacaktır.**

## Katkıda Bulunma
Proje geliştirmelerine katkıda bulunmak isterseniz, pull request gönderebilir veya hata bildiriminde bulunabilirsiniz.

## Lisans
Bu proje açık kaynaklıdır ve MIT lisansı altında dağıtılmaktadır.

