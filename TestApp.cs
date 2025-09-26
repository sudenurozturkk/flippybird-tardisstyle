using System;
using System.Drawing;
using System.Windows.Forms;

public class TestForm : Form
{
    public TestForm()
    {
        this.Text = "Test - Doctor Who Flappy";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        
        Label label = new Label();
        label.Text = "OYUN ÇALIŞIYOR!\n\nTARDIS vs Crying Angels\n\nSpace tuşuna basın!";
        label.Font = new Font("Arial", 16, FontStyle.Bold);
        label.ForeColor = Color.White;
        label.BackColor = Color.DarkBlue;
        label.TextAlign = ContentAlignment.MiddleCenter;
        label.Dock = DockStyle.Fill;
        
        this.Controls.Add(label);
        this.BackColor = Color.Black;
        
        this.KeyPreview = true;
        this.KeyDown += (s, e) => {
            if (e.KeyCode == Keys.Space)
            {
                label.Text = "SPACE BASILDI!\n\nTARDIS uçuyor!\n\nGeronimo! 🌟";
                label.BackColor = Color.DarkGreen;
            }
        };
    }
}

public class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TestForm());
    }
}
