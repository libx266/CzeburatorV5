using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using RabbitCrypt;
using System.IO;
using System.Threading.Tasks;
using RabbitCrypt.Extensions;
using System.Linq;
using System;
using Avalonia.Threading;

namespace CzeburatorV5
{
    public partial class TextWindow : Window
    {
        public TextWindow()
        {
            InitializeComponent();

            BtEncode.Click += new System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () => 
            {
                try { TbRight.Text = await TextEngine.EncodeAsync(TbLeft.Text, TbPassword.Text); }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));

            BtDecode.Click += new System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () => 
            {
                try { TbRight.Text = await TextEngine.DecodeAsync(TbLeft.Text, TbPassword.Text); }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));

            BtParse.Click += new System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    string path = (await new OpenFileDialog().ShowAsync(this))[0];
                    string format = path.Split('.').Last();
                    TbLeft.Text = AdmiralFyyar.EncodeFormat(format) + File.ReadAllBytes(path).ToBase256();
                }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));
            
            BtAss.Click += new System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    string format = AdmiralFyyar.DetectFormat(TbRight.Text.First());
                    string path = await new SaveFileDialog().ShowAsync(this) + '.' + format;
                    File.WriteAllBytes(path, String.Join("", TbRight.Text.Skip(1)).FromBase256());
                }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));
        }

       
    }
}
