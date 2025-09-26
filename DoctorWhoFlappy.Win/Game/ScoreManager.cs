using System;

namespace DoctorWhoFlappy.Win.Game
{
    /// <summary>
    /// Oyun skoru yöneticisi
    /// </summary>
    public class ScoreManager
    {
        #region Fields

        private int _currentScore;
        private int _highScore;

        #endregion

        #region Properties

        /// <summary>
        /// Mevcut oyun skoru
        /// </summary>
        public int CurrentScore { get { return _currentScore; } }

        /// <summary>
        /// En yüksek skor
        /// </summary>
        public int HighScore { get { return _highScore; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Skor yöneticisini başlatır
        /// </summary>
        public ScoreManager()
        {
            _currentScore = 0;
            _highScore = LoadHighScore();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Skoru bir artırır (angel geçildiğinde)
        /// </summary>
        public void IncrementScore()
        {
            _currentScore++;
            
            // Yeni rekor kontrolü
            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
                SaveHighScore();
            }
        }

        /// <summary>
        /// Mevcut skoru sıfırlar (yeni oyun başlarken)
        /// </summary>
        public void ResetCurrentScore()
        {
            _currentScore = 0;
        }

        /// <summary>
        /// Skor bilgilerini formatlı string olarak döner
        /// </summary>
        /// <returns>Formatlanmış skor metni</returns>
        public string GetScoreText()
        {
            return "Score: " + _currentScore;
        }

        /// <summary>
        /// Yüksek skor bilgisini formatlı string olarak döner
        /// </summary>
        /// <returns>Formatlanmış yüksek skor metni</returns>
        public string GetHighScoreText()
        {
            return "High Score: " + _highScore;
        }

        /// <summary>
        /// Oyun bittiğinde gösterilecek özet bilgi
        /// </summary>
        /// <returns>Oyun sonu özet metni</returns>
        public string GetGameOverSummary()
        {
            string summary = "Final Score: " + _currentScore + "\nHigh Score: " + _highScore;
            
            if (_currentScore == _highScore && _currentScore > 0)
            {
                summary += "\n🎉 NEW RECORD! 🎉";
            }
            
            return summary;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// En yüksek skoru dosyadan yükler
        /// </summary>
        /// <returns>Yüklenen yüksek skor</returns>
        private int LoadHighScore()
        {
            try
            {
                // Properties.Settings kullanarak yüksek skoru yükle
                var settings = Properties.Settings.Default;
                if (settings.Properties["HighScore"] != null)
                {
                    return (int)settings["HighScore"];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("High score loading failed: " + ex.Message);
            }
            
            return 0; // Varsayılan değer
        }

        /// <summary>
        /// En yüksek skoru dosyaya kaydeder
        /// </summary>
        private void SaveHighScore()
        {
            try
            {
                // Properties.Settings kullanarak yüksek skoru kaydet
                var settings = Properties.Settings.Default;
                
                // Eğer property yoksa, runtime'da ekle
                if (settings.Properties["HighScore"] == null)
                {
                    settings.Properties.Add(new System.Configuration.SettingsProperty("HighScore")
                    {
                        DefaultValue = 0,
                        PropertyType = typeof(int),
                        IsReadOnly = false
                    });
                }
                
                settings["HighScore"] = _highScore;
                settings.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("High score saving failed: " + ex.Message);
            }
        }

        #endregion
    }
}
