using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ZXing;

class Program
{
    static void Main()
    {
        Console.Write("Kaç adet barkod oluşturmak istiyorsunuz: ");
        if (!int.TryParse(Console.ReadLine(), out int barkodAdet) || barkodAdet <= 0)
        {
            Console.WriteLine("Geçerli bir değer girmediniz...");
            return;
        }

        string barkodDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Barkodlar");

        if (!Directory.Exists(barkodDirectory))
            Directory.CreateDirectory(barkodDirectory);

        for (int i = 0; i < barkodAdet; i++)
        {
            string barkodMetin = RastgeleBarkodMetinOlustur(8, 12);
            string barkodPath = Path.Combine(barkodDirectory, $"{Guid.NewGuid()}.png");

            BarkodOlustur(barkodMetin, barkodPath);
            Console.WriteLine($"Oluşturuldu: {barkodMetin} -> {barkodPath}");
        }

        Console.WriteLine("Tüm barkodlar başarıyla oluşturuldu!");
    }

    /// <summary>
    /// Bir barkod görüntüsü oluşturur ve bunu bir dosyaya kaydeder.
    /// </summary
    static void BarkodOlustur(string metin, string filePath)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.CODE_128,
            Options = new ZXing.Common.EncodingOptions
            {
                Width = 300,
                Height = 100
            }
        };

        using (Bitmap bitmap = writer.Write(metin))
        {
            bitmap.Save(filePath, ImageFormat.Png);
        }
    }

    /// <summary>
    /// Belirtilen uzunluk aralığında rastgele bir alfanümerik barkod metni oluşturur.
    /// </summary>
    static string RastgeleBarkodMetinOlustur(int min, int max)
    {
        const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        int uzunluk = random.Next(min, max + 1);

        return new string(Enumerable.Range(0, uzunluk)
            .Select(_ => karakterler[random.Next(karakterler.Length)])
            .ToArray());
    }
}