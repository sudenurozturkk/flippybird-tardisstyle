# Doctor Who Flappy - TARDIS vs Crying Angels

🚀 **Doctor Who** temalı **Flappy Bird** klonu! TARDIS'i kullanarak Crying Angel'lardan kaçın ve uzayda hayatta kalmaya çalışın.

## 🎮 Oyun Özellikleri

- **TARDIS** kuş yerine oynanabilir karakter
- **Crying Angels** engel olarak borular yerine
- **Uzay arkaplanı** yıldızlı nebula ile
- **Skor sistemi** ve yüksek skor kaydetme
- **60 FPS** akıcı oyun deneyimi
- **Klavye kontrolü** (Space, P, R tuşları)

## 🛠️ Teknik Özellikler

- **.NET Framework 4.8** WinForms uygulaması
- **DevExpress** bileşenleri (opsiyonel)
- **Timer tabanlı** oyun döngüsü (16ms tick)
- **Çarpışma tespiti** Rectangle.IntersectsWith ile
- **Embedded Resources** resim yönetimi
- **Settings** ile yüksek skor saklama

## 🎯 Nasıl Oynanır

### Kontroller
- **SPACE** - TARDIS'i yukarı zıplat / Oyunu başlat
- **P** - Oyunu duraklat/devam ettir
- **R** - Oyunu yeniden başlat

### Amaç
- TARDIS'i Crying Angel'lar arasından geçirin
- Zemine veya angel'lara çarpmayın
- Mümkün olduğunca yüksek skor elde edin

## 🏗️ Kurulum

### Gereksinimler
- **Visual Studio 2019/2022**
- **.NET Framework 4.8 SDK**
- **Windows 10/11** (DirectX desteği için)

### Adımlar
1. Repository'yi klonlayın:
   ```bash
   git clone https://github.com/yourusername/flippybird-tardisstyle.git
   cd flippybird-tardisstyle
   ```

2. Visual Studio'da `DoctorWhoFlappy.sln` dosyasını açın

3. Solution'ı **Build** edin:
   - Build → Build Solution (Ctrl+Shift+B)

4. Uygulamayı çalıştırın:
   - Debug → Start Debugging (F5)

## 📁 Proje Yapısı

```
DoctorWhoFlappy.Win/
├── Game/                    # Oyun mantığı
│   ├── GameLoop.cs         # Ana oyun döngüsü
│   ├── Tardis.cs           # TARDIS karakter sınıfı
│   ├── AngelPair.cs        # Crying Angel çiftleri
│   └── ScoreManager.cs     # Skor yönetimi
├── UI/                     # Kullanıcı arayüzü
│   ├── MainForm.cs         # Ana form
│   └── MainForm.Designer.cs
├── Properties/             # Proje ayarları
│   ├── Resources.resx      # Embedded kaynaklar
│   └── Settings.settings   # Uygulama ayarları
├── Resources/              # Oyun varlıkları
│   ├── background.jpg      # Uzay arkaplanı
│   ├── tardis.png          # TARDIS resmi
│   └── cryingangel.png     # Crying Angel resmi
└── Program.cs              # Giriş noktası
```

## 🎨 Görseller

### Oyun Ekran Görüntüsü
*[Ekran görüntüsü buraya eklenecek]*

### Oyun İçi Elementler
- **TARDIS**: Mavi polis kutusu, 60x90 piksel
- **Crying Angels**: Gri taş heykel, kanatları olan
- **Arkaplan**: Koyu mavi uzay, yıldızlar ve nebula

## 🚀 Geliştirme Notları

### Performans
- **16ms Timer** (~60 FPS) için optimize edildi
- **Double buffering** titreşimi önlemek için
- **Efficient collision detection** Rectangle tabanlı

### Genişletilebilirlik
- Yeni temalar eklenebilir (Resources değiştirerek)
- Ses efektleri eklenebilir
- PowerUps sistemi eklenebilir
- Multiplayer desteği eklenebilir

## 🐛 Bilinen Sorunlar

- DevExpress bileşenleri şu anda kullanılmıyor (gelecek sürümde)
- Placeholder görseller gerçek asset'ler yerine kullanılıyor
- Ses efektleri henüz eklenmedi

## 📝 Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje **MIT License** altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 🎬 Doctor Who Referansları

- **TARDIS**: Time And Relative Dimension In Space - Doctor'ın zaman makinesi
- **Crying Angels**: "Don't Blink" bölümündeki ikonik düşmanlar
- **Uzay Teması**: Doctor Who'nun uzay maceralarından ilham

---

**Geronimo!** 🌟 TARDIS'inizi uçurmaya hazır mısınız?
