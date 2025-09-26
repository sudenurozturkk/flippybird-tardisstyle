using System;
using System.Drawing;

namespace DoctorWhoFlappy.Win.Game
{
    /// <summary>
    /// Cyberman çifti - Flappy Bird'deki borular yerine kullanıyoruz
    /// </summary>
    public class AngelPair
    {
        #region Fields

        private float _x;
        private const int AngelWidth = 80;   // pipe genişliği
        private const int AngelHeight = 300; // pipe yüksekliği (dinamik alt/üstte kesilir)
        // Varsayılan boşluk 200 px. Oyun ilerledikçe küçülecek ama 150'nin altına inmeyecek
        private const int DefaultGap = 180; // istenen varsayılan gap
        private const int MinGap = 150;
        private int _currentGap = DefaultGap;
        private float _speed = 9f; // 60 FPS için uygun başlangıç pipeSpeed (8–10 arası)
        private bool _passed = false;

        #endregion

        #region Properties

        /// <summary>
        /// Angel çiftinin X konumu
        /// </summary>
        public float X { get { return _x; } }

        /// <summary>
        /// Angel genişliği
        /// </summary>
        public int Width { get { return AngelWidth; } }

        /// <summary>
        /// Üst angel yüksekliği
        /// </summary>
        public int TopHeight { get; private set; }

        /// <summary>
        /// Alt angel yüksekliği
        /// </summary>
        public int BottomHeight { get; private set; }

        /// <summary>
        /// Alt angel Y başlangıç konumu
        /// </summary>
        public int BottomY { get; private set; }

        /// <summary>
        /// TARDIS bu angel çiftini geçti mi?
        /// </summary>
        public bool HasPassed { get { return _passed; } }

        /// <summary>
        /// Angel çifti ekranın solundan çıktı mı?
        /// </summary>
        public bool IsOffScreen { get { return _x + AngelWidth < 0; } }

        /// <summary>
        /// Üst angel için çarpışma alanı
        /// </summary>
        public Rectangle TopBounds { get { return new Rectangle((int)_x, 0, AngelWidth, TopHeight); } }

        /// <summary>
        /// Alt angel için çarpışma alanı
        /// </summary>
        public Rectangle BottomBounds { get { return new Rectangle((int)_x, BottomY, AngelWidth, BottomHeight); } }

        #endregion

        #region Constructor

        /// <summary>
        /// Angel çiftini oluşturur
        /// </summary>
        /// <param name="startX">Başlangıç X konumu</param>
        /// <param name="screenHeight">Ekran yüksekliği</param>
        public AngelPair(float startX, int screenHeight)
        {
            _x = startX;
            _passed = false;
            GenerateRandomGap(screenHeight);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Angel çiftinin hareketini günceller
        /// </summary>
        public void Update()
        {
            _x -= _speed;
        }

        /// <summary>
        /// TARDIS bu angel çiftini geçti mi kontrol eder
        /// </summary>
        /// <param name="tardisX">TARDIS X konumu</param>
        public void CheckPassed(float tardisX)
        {
            if (!_passed && tardisX > _x + AngelWidth)
            {
                _passed = true;
            }
        }

        /// <summary>
        /// Angel çiftini yeni konuma sıfırlar
        /// </summary>
        /// <param name="newX">Yeni X konumu</param>
        /// <param name="screenHeight">Ekran yüksekliği</param>
        public void Reset(float newX, int screenHeight)
        {
            _x = newX;
            _passed = false;
            GenerateRandomGap(screenHeight);
        }

        /// <summary>
        /// TARDIS ile çarpışma kontrolü
        /// </summary>
        /// <param name="tardis">TARDIS nesnesi</param>
        /// <returns>Çarpışma varsa true</returns>
        public bool CheckCollision(Tardis tardis)
        {
            Rectangle tardisBounds = tardis.Bounds;
            return tardisBounds.IntersectsWith(TopBounds) || tardisBounds.IntersectsWith(BottomBounds);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Rastgele boşluk konumu oluşturur
        /// </summary>
        /// <param name="screenHeight">Ekran yüksekliği</param>
        private void GenerateRandomGap(int screenHeight)
        {
            Random rand = new Random();
            
            // Boşluğun merkezi için rastgele Y konumu
            int minGapCenter = _currentGap / 2 + 50;
            int maxGapCenter = screenHeight - _currentGap / 2 - 50;
            int gapCenter = rand.Next(minGapCenter, maxGapCenter);
            
            // Üst ve alt angel boyutlarını hesapla
            TopHeight = gapCenter - _currentGap / 2;
            BottomY = gapCenter + _currentGap / 2;
            BottomHeight = screenHeight - BottomY;
        }

        /// <summary>
        /// Zorluk arttıkça boşluğu biraz daraltır. Minimum 150 px.
        /// </summary>
        /// <param name="difficultyStep">Artış sayısı (ör: skor/5)</param>
        public void UpdateDifficulty(int difficultyStep)
        {
            int target = DefaultGap - (difficultyStep * 5); // her adımda 5 piksel daralsın
            if (target < MinGap) target = MinGap;
            _currentGap = target;
        }

        /// <summary>
        /// Hızı ayarlamak için
        /// </summary>
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        #endregion
    }
}
