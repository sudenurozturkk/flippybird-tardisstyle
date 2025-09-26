namespace DoctorWhoFlappy.Win.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gamePanel = new System.Windows.Forms.Panel();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblHighScore = new System.Windows.Forms.Label();
            this.lblGameState = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.pipeTop = new System.Windows.Forms.PictureBox();
            this.pipeBottom = new System.Windows.Forms.PictureBox();
            this.controlPanel.SuspendLayout();
            this.tardis = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tardis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pipeTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pipeBottom)).BeginInit();
            this.SuspendLayout();
            // 
            // gamePanel
            // 
            this.gamePanel.BackColor = System.Drawing.Color.Black;
            this.gamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gamePanel.Location = new System.Drawing.Point(0, 60);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new System.Drawing.Size(1280, 660);
            this.gamePanel.TabIndex = 0;
            this.gamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GamePanel_Paint);
            this.gamePanel.Controls.Add(this.tardis);
            this.gamePanel.Controls.Add(this.pipeTop);
            this.gamePanel.Controls.Add(this.pipeBottom);
            // 
            // tardis
            // 
            this.tardis.Location = new System.Drawing.Point(150, 250);
            this.tardis.Name = "tardis";
            this.tardis.Size = new System.Drawing.Size(55, 55);
            this.tardis.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.tardis.TabStop = false;
            // 
            // lblScore
            // 
            this.lblScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.ForeColor = System.Drawing.Color.White;
            this.lblScore.Location = new System.Drawing.Point(1100, 15);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(78, 25);
            this.lblScore.TabIndex = 1;
            this.lblScore.Text = "Score: 0";
            // 
            // lblHighScore
            // 
            this.lblHighScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHighScore.AutoSize = true;
            this.lblHighScore.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighScore.ForeColor = System.Drawing.Color.LightGray;
            this.lblHighScore.Location = new System.Drawing.Point(1080, 40);
            this.lblHighScore.Name = "lblHighScore";
            this.lblHighScore.Size = new System.Drawing.Size(98, 17);
            this.lblHighScore.TabIndex = 2;
            this.lblHighScore.Text = "High Score: 0";
            // 
            // lblGameState
            // 
            this.lblGameState.AutoSize = true;
            this.lblGameState.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameState.ForeColor = System.Drawing.Color.Gold;
            this.lblGameState.Location = new System.Drawing.Point(12, 18);
            this.lblGameState.Name = "lblGameState";
            this.lblGameState.Size = new System.Drawing.Size(186, 21);
            this.lblGameState.TabIndex = 3;
            this.lblGameState.Text = "Press SPACE to start!";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.DarkBlue;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(220, 15);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 30);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.DarkOrange;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.ForeColor = System.Drawing.Color.White;
            this.btnPause.Location = new System.Drawing.Point(301, 15);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 30);
            this.btnPause.TabIndex = 5;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.BtnPause_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.DarkRed;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestart.ForeColor = System.Drawing.Color.White;
            this.btnRestart.Location = new System.Drawing.Point(382, 15);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 30);
            this.btnRestart.TabIndex = 6;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.BtnRestart_Click);
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.controlPanel.Controls.Add(this.lblInstructions);
            this.controlPanel.Controls.Add(this.lblGameState);
            this.controlPanel.Controls.Add(this.btnRestart);
            this.controlPanel.Controls.Add(this.lblScore);
            this.controlPanel.Controls.Add(this.btnPause);
            this.controlPanel.Controls.Add(this.lblHighScore);
            this.controlPanel.Controls.Add(this.btnStart);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1280, 60);
            this.controlPanel.TabIndex = 7;
            // 
            // pipeTop
            // 
            this.pipeTop.Location = new System.Drawing.Point(1000, 0);
            this.pipeTop.Name = "pipeTop";
            this.pipeTop.Size = new System.Drawing.Size(80, 300);
            this.pipeTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pipeTop.TabStop = false;
            // 
            // pipeBottom
            // 
            this.pipeBottom.Location = new System.Drawing.Point(1000, 360);
            this.pipeBottom.Name = "pipeBottom";
            this.pipeBottom.Size = new System.Drawing.Size(80, 300);
            this.pipeBottom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pipeBottom.TabStop = false;
            // 
            // lblInstructions
            // 
            this.lblInstructions.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.ForeColor = System.Drawing.Color.LightBlue;
            this.lblInstructions.Location = new System.Drawing.Point(550, 20);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(180, 15);
            this.lblInstructions.TabIndex = 7;
            this.lblInstructions.Text = "Use SPACE to make TARDIS fly!";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.controlPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doctor Who Flappy - TARDIS vs Crying Angels";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tardis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pipeTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pipeBottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblHighScore;
        private System.Windows.Forms.Label lblGameState;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.PictureBox pipeTop;
        private System.Windows.Forms.PictureBox pipeBottom;
        private System.Windows.Forms.PictureBox tardis;
    }
}
