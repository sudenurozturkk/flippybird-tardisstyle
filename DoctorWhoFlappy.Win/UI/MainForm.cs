using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DoctorWhoFlappy.Win.Game;
using DoctorWhoFlappy.Win.Properties;
using System.Reflection;
using System.Collections.Generic;

namespace DoctorWhoFlappy.Win.UI
{
    /// <summary>
    /// Ana oyun formu - UI ve oyun döngüsünü yönetir
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        private GameLoop _gameLoop;
        private Image _backgroundImage;
        private Image _tardisImage;
        private Image _cybermanImage;
        
        // Flappy Bird fizik ve tempo sabitleri
        const int TARDIS_START_X = 150;
        const int TARDIS_START_Y = 250;
        float _velY = 0f;
        // Daha hızlı ve tepkisel fizik (düşüş daha da hızlandı)
        const float GRAVITY = 1.25f;        // düşüş daha hızlı
        const float JUMP_VELOCITY = -14.5f; // zıplama güçlü
        const float MAX_FALL = 20f;         // terminal hız daha yüksek
        const int PIPE_SPEED = 12;          // borular daha hızlı
        const int PIPE_WIDTH = 80;
        int _currentGap = 220; // başlangıç boşluk
        const int GAP_MIN = 180; // minimum boşluk
        const int PIPE_SPACING = 230; // ardışık pipe'lar arası sabit mesafe (daha sık)
        const int MIN_GAP_CENTER = 140;
        const int BOTTOM_MARGIN = 60;
        
        // Designer'da tanımlı PictureBox referansları (MainForm.Designer.cs)

        // Panel için DoubleBuffer helper
        // WinForms'ta Panel.DoubleBuffered internal olduğundan extension ile açıyoruz
        #endregion

        #region DoubleBuffer Helper
        
        private static void EnableDoubleBuffer(Control control, bool setting)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop != null)
            {
                prop.SetValue(control, setting, null);
            }
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Ana formu başlatır
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeGame();
            LoadGameAssets();
            // Flicker (siyah çizgi) engelleme
            this.DoubleBuffered = true;
            EnableDoubleBuffer(gamePanel, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            this.KeyPreview = true;
            // Panel arkaplanını resimle doldur ki PNG şeffaflık düzgün çalışsın
            try
            {
                string bgPath = System.IO.Path.Combine(Application.StartupPath, "Resources", "background.jpg");
                if (System.IO.File.Exists(bgPath))
                    _backgroundImage = Image.FromFile(bgPath);
            }
            catch { }
            gamePanel.BackgroundImage = _backgroundImage;
            gamePanel.BackgroundImageLayout = ImageLayout.Stretch;
            
            // Cyberman görselini pipe'lara bağla (dosyadan)
            pipeTop.Image = _cybermanImage;
            pipeBottom.Image = _cybermanImage;
            pipeTop.SizeMode = PictureBoxSizeMode.StretchImage; // boru gibi doldur, kenarlık bırakma
            pipeBottom.SizeMode = PictureBoxSizeMode.StretchImage;
            pipeTop.BorderStyle = BorderStyle.None;
            pipeBottom.BorderStyle = BorderStyle.None;
            pipeTop.BackColor = Color.Transparent;
            pipeBottom.BackColor = Color.Transparent;
            tardis.BackColor = Color.Transparent;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Oyun döngüsünü ve event'leri başlatır
        /// </summary>
        private void InitializeGame()
        {
            // Oyun döngüsünü oluştur
            _gameLoop = new GameLoop(gamePanel.Width, gamePanel.Height);
            
            // Event'leri bağla
            _gameLoop.GameStateChanged += OnGameStateChanged;
            _gameLoop.ScoreChanged += OnScoreChanged;
            _gameLoop.GameTick += OnGameTick;
            
            // Pipe başlangıç konumlarını ayarla (ekranda gözüksün)
            ResetPipePair(pipeTop, pipeBottom, this.ClientSize.Width + 150);
            
            // İlk UI güncellemesi
            UpdateUI();
        }

        /// <summary>
        /// Oyun varlıklarını yükler (resimler)
        /// </summary>
        private void LoadGameAssets()
        {
            try
            {
                // Önce gerçek dosyalardan yüklemeyi dene
                string resourcePath = System.IO.Path.Combine(Application.StartupPath, "Resources");
                
                if (System.IO.Directory.Exists(resourcePath))
                {
                    string backgroundPath = System.IO.Path.Combine(resourcePath, "background.jpg");
                    string tardisPath = System.IO.Path.Combine(resourcePath, "tardis1.png");
                    string angelPath = System.IO.Path.Combine(resourcePath, "cyberman.png");
                    
                    if (System.IO.File.Exists(backgroundPath))
                        _backgroundImage = Image.FromFile(backgroundPath);
                    if (System.IO.File.Exists(tardisPath))
                        _tardisImage = Image.FromFile(tardisPath);
                    if (System.IO.File.Exists(angelPath))
                        _cybermanImage = Image.FromFile(angelPath);
                }
                
                // Eğer dosyalar yüklenemediyse sade placeholder kullan
                if (_backgroundImage == null || _tardisImage == null || _cybermanImage == null)
                {
                    CreatePlaceholderAssets();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Asset loading failed: " + ex.Message + "\nUsing placeholder graphics.", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // Placeholder renkli dikdörtgenler oluştur
                CreatePlaceholderAssets();
            }
        }

        /// <summary>
        /// Asset yüklenemezse placeholder grafikler oluşturur
        /// </summary>
        private void CreatePlaceholderAssets()
        {
            // Arkaplan - koyu mavi uzay
            _backgroundImage = new Bitmap(gamePanel.Width, gamePanel.Height);
            using (var g = Graphics.FromImage(_backgroundImage))
            {
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, gamePanel.Width, gamePanel.Height),
                    Color.FromArgb(8, 12, 32), Color.FromArgb(0, 0, 16),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, gamePanel.Width, gamePanel.Height);
                }
                
                // Yıldızlar ekle
                var rand = new Random();
                using (var starBrush = new SolidBrush(Color.White))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        int x = rand.Next(gamePanel.Width);
                        int y = rand.Next(gamePanel.Height);
                        g.FillEllipse(starBrush, x, y, 2, 2);
                    }
                }
            }

            // TARDIS - mavi kutu
            _tardisImage = new Bitmap(60, 90);
            using (var g = Graphics.FromImage(_tardisImage))
            {
                using (var brush = new SolidBrush(Color.FromArgb(18, 60, 150)))
                {
                    g.FillRectangle(brush, 0, 0, 60, 90);
                }
                using (var pen = new Pen(Color.White, 2))
                {
                    g.DrawRectangle(pen, 2, 2, 56, 86);
                    g.DrawLine(pen, 2, 20, 58, 20);
                    g.DrawLine(pen, 2, 40, 58, 40);
                    g.DrawLine(pen, 2, 60, 58, 60);
                    g.DrawLine(pen, 20, 2, 20, 88);
                    g.DrawLine(pen, 40, 2, 40, 88);
                }
            }

            // Crying Angel - gri taş
            _cybermanImage = new Bitmap(80, 300);
            using (var g = Graphics.FromImage(_cybermanImage))
            {
                using (var brush = new SolidBrush(Color.FromArgb(180, 170, 160)))
                {
                    g.FillRectangle(brush, 20, 10, 40, 60);  // Baş
                    g.FillRectangle(brush, 10, 70, 60, 100); // Gövde
                    g.FillRectangle(brush, 30, 170, 20, 30); // Ayaklar
                }
                using (var wingBrush = new SolidBrush(Color.FromArgb(160, 150, 140)))
                {
                    g.FillEllipse(wingBrush, 0, 30, 30, 120);  // Sol kanat
                    g.FillEllipse(wingBrush, 50, 30, 30, 120); // Sağ kanat
                }
            }
        }

        /// <summary>
        /// UI elementlerini günceller
        /// </summary>
        private void UpdateUI()
        {
            // Skor bilgilerini güncelle
            lblScore.Text = _gameLoop.ScoreManager.GetScoreText();
            lblHighScore.Text = _gameLoop.ScoreManager.GetHighScoreText();
            
            // Oyun durumuna göre mesaj güncelle
            switch (_gameLoop.CurrentState)
            {
                case GameLoop.GameState.Ready:
                    lblGameState.Text = "Press SPACE to start!";
                    lblGameState.ForeColor = Color.Gold;
                    break;
                    
                case GameLoop.GameState.Playing:
                    lblGameState.Text = string.Empty; // oyun başladıktan sonra mesaj kaybolsun
                    lblGameState.ForeColor = Color.LightGreen;
                    break;
                    
                case GameLoop.GameState.Paused:
                    lblGameState.Text = "PAUSED - Press SPACE to continue";
                    lblGameState.ForeColor = Color.Orange;
                    break;
                    
                case GameLoop.GameState.GameOver:
                    lblGameState.Text = "GAME OVER! Press SPACE to restart";
                    lblGameState.ForeColor = Color.Red;
                    break;
            }
            
            // Buton durumlarını güncelle
            btnStart.Enabled = _gameLoop.CurrentState == GameLoop.GameState.Ready || 
                              _gameLoop.CurrentState == GameLoop.GameState.GameOver;
            btnPause.Enabled = _gameLoop.CurrentState == GameLoop.GameState.Playing || 
                              _gameLoop.CurrentState == GameLoop.GameState.Paused;
            btnPause.Text = _gameLoop.CurrentState == GameLoop.GameState.Paused ? "Resume" : "Pause";
        }

        #endregion

        #region Simple FB loop (UI-only)

        private bool running = false;
        private int score = 0;
        private Timer gameTimer = new Timer();
        private readonly Random _rng = new Random();
        
        // Çoklu pipe yönetimi
        private class PipePair
        {
            public PictureBox Top;
            public PictureBox Bottom;
        }
        private readonly List<PipePair> _pipes = new List<PipePair>();

        private void StartGame()
        {
            score = 0; UpdateScoreLabel();
            _velY = 0f;
            tardis.Image = _tardisImage ?? Resources.tardis;
            tardis.Left = TARDIS_START_X;
            tardis.Top = TARDIS_START_Y;
            pipeTop.Image = _cybermanImage;
            pipeBottom.Image = _cybermanImage;
            InitPipes();
            running = true;
            gameTimer.Stop();
            gameTimer.Interval = 20; // 50 FPS
            gameTimer.Tick -= GameTick;
            gameTimer.Tick += GameTick;
            gameTimer.Start();
            lblGameState.Text = string.Empty;
        }

        private void GameTick(object s, EventArgs e)
        {
            // fizik
            // İvmeyi biraz daha "snappy" hissettirmek için iki aşamalı uygulayalım
            _velY += GRAVITY;
            if (_velY > MAX_FALL) _velY = MAX_FALL;
            tardis.Top += (int)Math.Round(_velY);
            if (tardis.Top < 0) { tardis.Top = 0; _velY = 0; }
            MovePipes();
            CheckCollision();
        }

        private void InitPipes()
        {
            // Öncekileri temizle (dinamik eklenenleri panelden de kaldır)
            for (int i = 0; i < _pipes.Count; i++)
            {
                if (_pipes[i].Top != pipeTop) gamePanel.Controls.Remove(_pipes[i].Top);
                if (_pipes[i].Bottom != pipeBottom) gamePanel.Controls.Remove(_pipes[i].Bottom);
            }
            _pipes.Clear();

            // Mevcut designer pipe'ını ilk çift olarak kullan
            ConfigurePipePictureBox(pipeTop);
            ConfigurePipePictureBox(pipeBottom);
            _pipes.Add(new PipePair { Top = pipeTop, Bottom = pipeBottom });

            // En az 4 çift olacak şekilde 3 ek çift oluştur (toplam 4)
            for (int i = 0; i < 3; i++)
            {
                var top = new PictureBox();
                var bottom = new PictureBox();
                top.Parent = gamePanel; bottom.Parent = gamePanel;
                top.Image = _cybermanImage; bottom.Image = _cybermanImage;
                ConfigurePipePictureBox(top); ConfigurePipePictureBox(bottom);
                gamePanel.Controls.Add(top); gamePanel.Controls.Add(bottom);
                _pipes.Add(new PipePair { Top = top, Bottom = bottom });
            }

            // Başlangıç konumları
            for (int i = 0; i < _pipes.Count; i++)
            {
                int spawnX = this.ClientSize.Width + 150 + (i * PIPE_SPACING);
                ResetPipePair(_pipes[i].Top, _pipes[i].Bottom, spawnX);
            }
        }

        private static void ConfigurePipePictureBox(PictureBox pb)
        {
            pb.SizeMode = PictureBoxSizeMode.StretchImage; // Boru alanını tam doldur
            pb.BackColor = Color.Transparent;
            pb.BorderStyle = BorderStyle.None;
        }

        private void ResetPipePair(Control top, Control bottom, int xSpawn)
        {
            // Panel yüksekliğini baz al: ground altında taşma olmasın
            int panelH = gamePanel.Height;
            int groundTop = panelH - BOTTOM_MARGIN; // zeminin üst sınırı – alt pipe burayı aşamaz

            int gap = _currentGap; if (gap < GAP_MIN) gap = GAP_MIN;
            // Merkez, tepe ile ground arasında güvenli aralıkta seçilir
            int minCenter = (gap / 2) + 30;                       // tepede daha geniş tampon – üstte boşluk azalır
            int maxCenter = groundTop - (gap / 2) - 10;           // ground'dan tampon
            if (maxCenter <= minCenter) maxCenter = minCenter + 1;
            int center = _rng.Next(minCenter, maxCenter);

            // Üst pipe tepeye yapışık, 0'dan başlar
            int topH = center - (gap / 2);
            if (topH < 10) topH = 10;
            if (topH > groundTop - 20) topH = groundTop - 20;     // aşırı uzamayı engelle
            top.SetBounds(xSpawn, 0, PIPE_WIDTH, topH);

            // Alt pipe ground üzerine taşmasın
            int bottomY = center + (gap / 2);
            int bottomH = groundTop - bottomY; // alt pipe yüksekliği zemini aşmayacak
            if (bottomH < 10)
            {
                // Çok sıkıştıysa merkez yukarı kaydırılmış olur; minimum bir yükseklik bırak
                bottomH = 10;
                bottomY = groundTop - bottomH;
            }
            bottom.SetBounds(xSpawn, bottomY, PIPE_WIDTH, bottomH);
        }

        private void MovePipes()
        {
            // Tüm pipe çiftlerini sola kaydır
            foreach (var pair in _pipes)
            {
                pair.Top.Left -= PIPE_SPEED;
                pair.Bottom.Left -= PIPE_SPEED;
            }

            // Ekranı terk edenleri arkaya taşı
            foreach (var pair in _pipes)
            {
                if (pair.Top.Right < 0)
                {
                    int maxX = 0;
                    for (int i = 0; i < _pipes.Count; i++)
                    {
                        if (_pipes[i].Top.Right > maxX) maxX = _pipes[i].Top.Right;
                    }
                    int spawnX = maxX + PIPE_SPACING;
                    ResetPipePair(pair.Top, pair.Bottom, spawnX);
                    score++;
                    UpdateScoreLabel();
                    if (_currentGap > GAP_MIN)
                    {
                        _currentGap -= 2;
                        if (_currentGap < GAP_MIN) _currentGap = GAP_MIN;
                    }
                }
            }
        }

        private void CheckCollision()
        {
            // Zemin – panel koordinatlarında kontrol
            Rectangle ground = new Rectangle(0, gamePanel.Height - BOTTOM_MARGIN,
                                             gamePanel.Width, BOTTOM_MARGIN);
            if (tardis.Bounds.IntersectsWith(ground))
            {
                GameOver();
                return;
            }

            // Pipe çarpışmaları – önce dikdörtgen, sonra piksel
            for (int i = 0; i < _pipes.Count; i++)
            {
                if (tardis.Bounds.IntersectsWith(_pipes[i].Top.Bounds))
                {
                    if (PixelPerfectHit(tardis, _pipes[i].Top, 128)) { GameOver(); return; }
                }
                if (tardis.Bounds.IntersectsWith(_pipes[i].Bottom.Bounds))
                {
                    if (PixelPerfectHit(tardis, _pipes[i].Bottom, 128)) { GameOver(); return; }
                }
            }
        }

        private static bool PixelPerfectHit(PictureBox a, PictureBox b, byte alphaThreshold)
        {
            if (a.Image == null || b.Image == null) return false;
            Bitmap bmpA = a.Image as Bitmap;
            Bitmap bmpB = b.Image as Bitmap;
            if (bmpA == null || bmpB == null) return false;

            RectangleF destA = GetImageDestRect(a);
            RectangleF destB = GetImageDestRect(b);
            RectangleF interF = RectangleF.Intersect(destA, destB);
            if (interF.Width <= 0 || interF.Height <= 0) return false;

            // Ölçekler (Zoom veya StretchImage)
            GetScaleAndOffset(a, bmpA, out float scaleAx, out float scaleAy, out float offAx, out float offAy);
            GetScaleAndOffset(b, bmpB, out float scaleBx, out float scaleBy, out float offBx, out float offBy);

            int left = (int)Math.Floor(interF.Left);
            int top = (int)Math.Floor(interF.Top);
            int right = (int)Math.Ceiling(interF.Right);
            int bottom = (int)Math.Ceiling(interF.Bottom);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    int srcAx = (int)((x - offAx) / scaleAx);
                    int srcAy = (int)((y - offAy) / scaleAy);
                    if (srcAx < 0 || srcAy < 0 || srcAx >= bmpA.Width || srcAy >= bmpA.Height) continue;

                    int srcBx = (int)((x - offBx) / scaleBx);
                    int srcBy = (int)((y - offBy) / scaleBy);
                    if (srcBx < 0 || srcBy < 0 || srcBx >= bmpB.Width || srcBy >= bmpB.Height) continue;

                    if (bmpA.GetPixel(srcAx, srcAy).A > alphaThreshold &&
                        bmpB.GetPixel(srcBx, srcBy).A > alphaThreshold)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static RectangleF GetImageDestRect(PictureBox pb)
        {
            if (pb.Image == null) return RectangleF.Empty;
            var img = pb.Image;
            if (pb.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                return new RectangleF(pb.Left, pb.Top, pb.Width, pb.Height);
            }
            // Zoom (aspect koru)
            float sx = pb.Width / (float)img.Width;
            float sy = pb.Height / (float)img.Height;
            float scale = Math.Min(sx, sy);
            float drawW = img.Width * scale;
            float drawH = img.Height * scale;
            float ox = pb.Left + (pb.Width - drawW) / 2f;
            float oy = pb.Top + (pb.Height - drawH) / 2f;
            return new RectangleF(ox, oy, drawW, drawH);
        }

        private static void GetScaleAndOffset(PictureBox pb, Bitmap bmp,
            out float scaleX, out float scaleY, out float offX, out float offY)
        {
            if (pb.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                scaleX = pb.Width / (float)bmp.Width;
                scaleY = pb.Height / (float)bmp.Height;
                offX = pb.Left;
                offY = pb.Top;
                return;
            }
            // Zoom
            float sx = pb.Width / (float)bmp.Width;
            float sy = pb.Height / (float)bmp.Height;
            float scale = Math.Min(sx, sy);
            float drawW = bmp.Width * scale;
            float drawH = bmp.Height * scale;
            offX = pb.Left + (pb.Width - drawW) / 2f;
            offY = pb.Top + (pb.Height - drawH) / 2f;
            scaleX = scaleY = scale;
        }

        private void GameOver()
        {
            gameTimer.Stop();
            running = false;
            lblGameState.Text = "GAME OVER! Press SPACE to restart.";
        }

        private void UpdateScoreLabel()
        {
            lblScore.Text = "Score: " + score;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Oyun durumu değiştiğinde çağrılır
        /// </summary>
        private void OnGameStateChanged(object sender, GameLoop.GameState newState)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnGameStateChanged(sender, newState)));
                return;
            }
            
            UpdateUI();
            
            // Game over durumunda özet göster
            if (newState == GameLoop.GameState.GameOver)
            {
                var summary = _gameLoop.ScoreManager.GetGameOverSummary();
                MessageBox.Show(summary, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Skor değiştiğinde çağrılır
        /// </summary>
        private void OnScoreChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnScoreChanged(sender, e)));
                return;
            }
            
            UpdateUI();
        }

        /// <summary>
        /// Oyun tick'inde çağrılır (çizim için)
        /// </summary>
        private void OnGameTick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnGameTick(sender, e)));
                return;
            }
            // GameTick içinde çizim zorunlu değil; pipes hareketi tek noktadan yapılacak
            MovePipes();
        }

        /// <summary>
        /// Oyun paneli çizim eventi
        /// </summary>
        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            
            // Arkaplanı çiz
            if (_backgroundImage != null)
            {
                g.DrawImage(_backgroundImage, 0, 0, gamePanel.Width, gamePanel.Height);
            }
            
            // TARDIS ve pipes artık PictureBox ile çiziliyor; Paint'te manuel çizime gerek yok
            
            // Zemin çizgisi tam BOTTOM_MARGIN ile hizalansın
            using (var pen = new Pen(Color.Gray, 3))
            {
                int groundY = gamePanel.Height - BOTTOM_MARGIN;
                g.DrawLine(pen, 0, groundY, gamePanel.Width, groundY);
            }
        }

        /// <summary>
        /// Klavye input eventi
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (!running) { StartGame(); }
                _velY = JUMP_VELOCITY; // anlık zıplama hissi
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.P)
            {
                _gameLoop.TogglePause();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.R)
            {
                _gameLoop.RestartGame();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Start butonu click eventi
        /// </summary>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        /// <summary>
        /// Pause butonu click eventi
        /// </summary>
        private void BtnPause_Click(object sender, EventArgs e)
        {
            _gameLoop.TogglePause();
        }

        /// <summary>
        /// Restart butonu click eventi
        /// </summary>
        private void BtnRestart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        /// <summary>
        /// Form kapanırken cleanup
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_gameLoop != null)
                _gameLoop.Dispose();
            if (_backgroundImage != null)
                _backgroundImage.Dispose();
            if (_tardisImage != null)
                _tardisImage.Dispose();
            if (_cybermanImage != null)
                _cybermanImage.Dispose();
            base.OnFormClosed(e);
        }

        #endregion
    }
}
