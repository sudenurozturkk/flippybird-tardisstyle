using System;
using System.Drawing;

namespace DoctorWhoFlappy.Win.Game
{
    /// <summary>
    /// TARDIS oyuncu karakteri - Flappy Bird'deki kuş yerine TARDIS kullanıyoruz
    /// </summary>
    public class Tardis
    {
        #region Fields

        private float _x;
        private float _y;
        private float _velocity;
        // Oynanış için 60 FPS'e uygun değerler
        // gravity ~5–6, jumpVelocity ~-12 isteniyor
        private const float Gravity = 5f;      // Yerçekimi ivmesi (px/frame)
        private const float JumpForce = -12f;  // Zıplama hızı (px/frame)
        private const int Width = 60;
        private const int Height = 90;

        #endregion

        #region Properties

        /// <summary>
        /// TARDIS'in X konumu
        /// </summary>
        public float X { get { return _x; } }

        /// <summary>
        /// TARDIS'in Y konumu
        /// </summary>
        public float Y { get { return _y; } }

        /// <summary>
        /// TARDIS'in genişliği
        /// </summary>
        public int TardisWidth { get { return Width; } }

        /// <summary>
        /// TARDIS'in yüksekliği
        /// </summary>
        public int TardisHeight { get { return Height; } }

        /// <summary>
        /// Çarpışma tespiti için Rectangle
        /// </summary>
        public Rectangle Bounds { get { return new Rectangle((int)_x, (int)_y, Width, Height); } }

        #endregion

        #region Constructor

        /// <summary>
        /// TARDIS'i başlangıç konumunda oluşturur
        /// </summary>
        /// <param name="startX">Başlangıç X konumu</param>
        /// <param name="startY">Başlangıç Y konumu</param>
        public Tardis(float startX, float startY)
        {
            _x = startX;
            _y = startY;
            _velocity = 0;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// TARDIS'i yukarı zıplatır (Space tuşuna basıldığında)
        /// </summary>
        public void Jump()
        {
            _velocity = JumpForce;
        }

        /// <summary>
        /// TARDIS'in fiziksel hareketini günceller
        /// </summary>
        public void Update()
        {
            // Yerçekimi etkisi
            _velocity += Gravity;
            
            // Konumu güncelle
            _y += _velocity;
            
            // Ekranın üst sınırını kontrol et
            if (_y < 0)
            {
                _y = 0;
                _velocity = 0;
            }
        }

        /// <summary>
        /// TARDIS'i başlangıç konumuna sıfırlar
        /// </summary>
        /// <param name="startX">Başlangıç X konumu</param>
        /// <param name="startY">Başlangıç Y konumu</param>
        public void Reset(float startX, float startY)
        {
            _x = startX;
            _y = startY;
            _velocity = 0;
        }

        /// <summary>
        /// TARDIS'in zemine çarptığını kontrol eder
        /// </summary>
        /// <param name="groundY">Zemin Y koordinatı</param>
        /// <returns>Zemine çarptıysa true</returns>
        public bool IsHittingGround(int groundY)
        {
            return _y + Height >= groundY;
        }

        #endregion
    }
}
