using System.Drawing;
internal class Program
{
    private static void Main(string[] args)
    {
        string text = "Text:file.txt(6B);Some string content\r\nImage:img.bmp(19MB);1920x1080\r\nText:data.txt(12B);Another string\r\nText:data1.txt(7B);Yet another string\r\nMovie:logan.2017.mkv(19GB);1920x1080;2h12m";


        ConsoleApp1.File.GetDataAboutFilesByPaths(text);
        
    }
}