using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DoctorWhoFlappy.Win.Game
{
    /// <summary>
    /// Ana oyun döngüsü ve durum yöneticisi
    /// </summary>
    public class GameLoop
    {
        #region Enums

        /// <summary>
        /// Oyun durumları
        /// </summary>
        public enum GameState
        {
            Ready,      // Oyun başlamaya hazır
            Playing,    // Oyun oynaniyor
            GameOver,   // Oyun bitti
            Paused      // Oyun duraklatıldı
        }

        #endregion

        #region Fields

        private readonly Timer _gameTimer;
        private readonly Tardis _tardis;
        private readonly List<AngelPair> _angelPairs;
        private readonly ScoreManager _scoreManager;
        private GameState _currentState;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private const int AngelSpacing = 350; // Angel çiftleri arası mesafe (daha sık)
        private DateTime _gameStartTime; // Oyun başlama zamanı

        #endregion

        #region Properties

        /// <summary>
        /// Mevcut oyun durumu
        /// </summary>
        public GameState CurrentState { get { return _currentState; } }

        /// <summary>
        /// TARDIS nesnesi
        /// </summary>
        public Tardis GameTardis { get { return _tardis; } }

        /// <summary>
        /// Angel çiftleri listesi
        /// </summary>
        public List<AngelPair> AngelPairs { get { return _angelPairs; } }

        /// <summary>
        /// Skor yöneticisi
        /// </summary>
        public ScoreManager ScoreManager { get { return _scoreManager; } }

        #endregion

        #region Events

        /// <summary>
        /// Oyun durumu değiştiğinde tetiklenir
        /// </summary>
        public event EventHandler<GameState> GameStateChanged;

        /// <summary>
        /// Skor değiştiğinde tetiklenir
        /// </summary>
        public event EventHandler ScoreChanged;

        /// <summary>
        /// Oyun çizimi için tetiklenir
        /// </summary>
        public event EventHandler GameTick;

        #endregion

        #region Constructor

        /// <summary>
        /// Oyun döngüsünü başlatır
        /// </summary>
        /// <param name="screenWidth">Ekran genişliği</param>
        /// <param name="screenHeight">Ekran yüksekliği</param>
        public GameLoop(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            
            // Timer kurulumu (60 FPS için ~16ms)
            _gameTimer = new Timer();
            _gameTimer.Interval = 16; // ~60 FPS
            _gameTimer.Tick += OnGameTimerTick;
            
            // Oyun nesnelerini oluştur
            _tardis = new Tardis(100, screenHeight / 2);
            _angelPairs = new List<AngelPair>();
            _scoreManager = new ScoreManager();
            
            // Başlangıç durumu
            _currentState = GameState.Ready;
            
            // İlk angel çiftlerini oluştur
            InitializeAngelPairs();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Oyunu başlatır
        /// </summary>
        public void StartGame()
        {
            if (_currentState == GameState.Ready || _currentState == GameState.GameOver)
            {
                ResetGame();
                _gameStartTime = DateTime.Now; // Başlama zamanını kaydet
                SetGameState(GameState.Playing);
                _gameTimer.Start();
            }
        }

        /// <summary>
        /// Oyunu duraklatır veya devam ettirir
        /// </summary>
        public void TogglePause()
        {
            if (_currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
                _gameTimer.Stop();
            }
            else if (_currentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
                _gameTimer.Start();
            }
        }

        /// <summary>
        /// Oyunu yeniden başlatır
        /// </summary>
        public void RestartGame()
        {
            _gameTimer.Stop();
            ResetGame();
            SetGameState(GameState.Ready);
        }

        /// <summary>
        /// TARDIS'i zıplatır (Space tuşu)
        /// </summary>
        public void Jump()
        {
            if (_currentState == GameState.Playing)
            {
                _tardis.Jump();
            }
            else if (_currentState == GameState.Ready)
            {
                StartGame();
            }
        }

        /// <summary>
        /// Oyunu kapatır
        /// </summary>
        public void Dispose()
        {
            if (_gameTimer != null)
            {
                _gameTimer.Stop();
                _gameTimer.Dispose();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Oyun timer'ının tick eventi
        /// </summary>
        private void OnGameTimerTick(object sender, EventArgs e)
        {
            if (_currentState == GameState.Playing)
            {
                UpdateGame();
            }
            
            // UI güncellemesi için event tetikle
            if (GameTick != null)
                GameTick(this, EventArgs.Empty);
        }

        /// <summary>
        /// Oyun mantığını günceller
        /// </summary>
        private void UpdateGame()
        {
            // TARDIS'i güncelle
            _tardis.Update();
            
            // Zemin çarpışması kontrolü
            if (_tardis.IsHittingGround(_screenHeight - 50))
            {
                GameOver();
                return;
            }
            
            // 2 saniye sonra angellar hareket etmeye başlasın
            var timeSinceStart = DateTime.Now - _gameStartTime;
            if (timeSinceStart.TotalSeconds >= 2.0)
            {
                // Angel çiftlerini güncelle
                int difficultyStep = _scoreManager.CurrentScore / 5; // her 5 skorda zorluk artar
                float speed = 9f + (difficultyStep * 0.5f); // hız çok az artsın
                
                foreach (var angelPair in _angelPairs)
                {
                    angelPair.SetSpeed(speed);
                    angelPair.UpdateDifficulty(difficultyStep);
                    angelPair.Update();
                    
                    // Çarpışma kontrolü
                    if (angelPair.CheckCollision(_tardis))
                    {
                        GameOver();
                        return;
                    }
                    
                    // Skor kontrolü
                    angelPair.CheckPassed(_tardis.X);
                    if (angelPair.HasPassed)
                    {
                        _scoreManager.IncrementScore();
                        if (ScoreChanged != null)
                            ScoreChanged(this, EventArgs.Empty);
                    }
                }
                
                // Ekrandan çıkan angel çiftlerini yeniden konumlandır
                ManageAngelPairs();
            }
        }

        /// <summary>
        /// Angel çiftlerini yönetir (yeniden konumlandırma)
        /// </summary>
        private void ManageAngelPairs()
        {
            for (int i = 0; i < _angelPairs.Count; i++)
            {
                if (_angelPairs[i].IsOffScreen)
                {
                    // En sağdaki angel çiftinin konumunu bul
                    float rightmostX = 0;
                    foreach (var ap in _angelPairs)
                    {
                        if (ap.X > rightmostX)
                            rightmostX = ap.X;
                    }
                    _angelPairs[i].Reset(rightmostX + AngelSpacing, _screenHeight);
                }
            }
        }

        /// <summary>
        /// Oyunu sıfırlar
        /// </summary>
        private void ResetGame()
        {
            _tardis.Reset(100, _screenHeight / 2);
            _scoreManager.ResetCurrentScore();
            InitializeAngelPairs();
        }

        /// <summary>
        /// İlk angel çiftlerini oluşturur
        /// </summary>
        private void InitializeAngelPairs()
        {
            _angelPairs.Clear();
            
            // 3 angel çifti oluştur - ekranda yeterli aralıklarla spawnlansın
            for (int i = 0; i < 3; i++)
            {
                float startX = _screenWidth + 400 + (i * AngelSpacing);
                _angelPairs.Add(new AngelPair(startX, _screenHeight));
            }
        }

        /// <summary>
        /// Oyun biter
        /// </summary>
        private void GameOver()
        {
            _gameTimer.Stop();
            SetGameState(GameState.GameOver);
        }

        /// <summary>
        /// Oyun durumunu değiştirir
        /// </summary>
        /// <param name="newState">Yeni oyun durumu</param>
        private void SetGameState(GameState newState)
        {
            if (_currentState != newState)
            {
                _currentState = newState;
                if (GameStateChanged != null)
                    GameStateChanged(this, newState);
            }
        }

        #endregion
    }
}
