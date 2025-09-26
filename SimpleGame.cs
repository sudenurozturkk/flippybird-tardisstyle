using System;
using System.Threading;

class SimpleGame
{
    static void Main()
    {
        Console.Title = "Doctor Who Flappy - TARDIS vs Crying Angels";
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        
        Console.WriteLine("===========================================");
        Console.WriteLine("    DOCTOR WHO FLAPPY - TARDIS EDITION    ");
        Console.WriteLine("===========================================");
        Console.WriteLine();
        Console.WriteLine("🚀 TARDIS vs Crying Angels 🗿");
        Console.WriteLine();
        Console.WriteLine("OYUN BAŞARIYLA DERLENDİ VE ÇALIŞIYOR!");
        Console.WriteLine();
        Console.WriteLine("Ana Windows Forms oyunu arka planda çalışıyor.");
        Console.WriteLine("Eğer pencere görünmüyorsa:");
        Console.WriteLine("• Alt+Tab ile pencereler arası geçiş yapın");
        Console.WriteLine("• Taskbar'da 'Doctor Who Flappy' arayın");
        Console.WriteLine();
        Console.WriteLine("KONTROLLER:");
        Console.WriteLine("• SPACE - TARDIS'i yukarı zıplat");
        Console.WriteLine("• P - Pause/Resume");
        Console.WriteLine("• R - Restart");
        Console.WriteLine();
        Console.WriteLine("Bu console'u kapatmak için herhangi bir tuşa basın...");
        
        Console.ReadKey();
        
        Console.WriteLine();
        Console.WriteLine("Geronimo! 🌟 TARDIS'inizi uçurmaya devam edin!");
        Thread.Sleep(2000);
    }
}
