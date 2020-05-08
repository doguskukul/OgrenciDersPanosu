# OgrenciDersPanosu
OgrenciOgretmenDersPanosuSistemi

Proje Çalıştırılırken: 
-Visual Studio 2019 kullanıldı. (2017'de de çalışıyor)

-SQL server management studio 2018 kullanıldı. Kurmanız gerekiyor. Sürüm farkı problem olabilir.

-Projenin çalışması için asp.net'in kurulu olması gerekiyor. (Eğer kurulu değilse projeyi açtığınızda yüklenmesi gerektiğine dair uyarı geliyor, ordan da yükleyebilirsiniz.)

-Proje açıldığında Çözüm Gezginine sağ tıklayıp NuGet paket yöneticisini açın. Üstte "Bu çözümde bazı NuGet paketleri eksik. Çevrimiçi paket kaynaklarınızdan geri yüklemek için tıklayın -> Geri Yükle butonuna tıklayarak eksik paketleri yükleyin. Daha sonra NuGet -> Güncelleştirmeler -> Tüm paketleri seç -> Güncelleştir butonuna tıklayarak paket güncelleştirmelerini yapın. 

-SQL server management studiodaki "Server name" inizi projenin en altındaki Web.config dosyasındaki connection string içerisindekilerle değiştirin. 
![image1](https://i.imgyukle.com/2020/04/01/JlbL0Q.png)
![image2](https://i.imgyukle.com/2020/04/01/JlbHKs.png)
(Sarı yerler ile değiştireceksiniz)

-Projeyi Çalıştırın(İlk açılış uzun sürebilir, bekleyin)

Uygulama kullanılırken:
-İlk olarak veritabanına rol eklemeniz gerekiyor. "Ogretmen", "Ogrenci", "admin" rollerini oluşturun. (Küçük harf büyük harf önemli) ("https://localhost:44351/admin/home/create" adresinden rol oluşturabilir, "https://localhost:44351/admin/home/roles" adresinden oluşturduğunuz rolleri görebilirsiniz. Bu adreslere butonlar aracılığıyla ulaşılmayıp ilk aşamada sistemin doğru şekilde çalışması için oluşturulmuş modüller, sonradan rollere müdahale edilmesi gerektiğinde kullanılabilir.)

-Roller eklendikten sonra, "https://localhost:44351/home" adresindeki navbardan register butonuna basarak oluşturmak istediğiniz rolü seçip kullanıcı oluşturabilirsiniz. Admin kullanıcısı oluşturmak için "https://localhost:44351/admin/home/register" kullanmalısınız. (Rol oluşturma ve admin register işlemlerini gerçekleştirdikten sonra admin areasındaki authentication role satırını aktif hale getirin. Bu sayede url üzerinden farklı arealara erişilmesi engellenmiş olacak.)

![image3](https://i.imgyukle.com/2020/04/01/JlbjDc.png)
![image4](https://i.imgyukle.com/2020/04/01/JlbOxt.png)
