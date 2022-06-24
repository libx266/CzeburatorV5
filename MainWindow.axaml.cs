using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using RabbitCrypt;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;

namespace CzeburatorV5
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            TbPassword.ConfigureWithPassword();
            TbMain.ConfigureContextMenu(this);


            List<Bitmap> comboRabbits = new();
            comboRabbits.Add(Properties.Resources.question.ToBitmap());
            comboRabbits.AddRange(Enumerable.Range(0, 10).Select(i => RabbitRepository.SelectByte(i).ToBitmap()));

            CbRabbit.Items = comboRabbits;
            CbRabbitString.Items = Enumerable.Range(-1, 10).Select(i => i >= 0 ?  $"Кролик №{i + 1}" : "Случайный");

            CbRabbit.SelectionChanged += new EventHandler<SelectionChangedEventArgs>((s, e) => 
            {
                CbRabbitString.SelectedIndex = CbRabbit.SelectedIndex;
                CbRabbit.IsVisible = false;
                CbRabbitString.IsVisible = true;
            });

            CbRabbitString.GotFocus += new EventHandler<Avalonia.Input.GotFocusEventArgs>((s, e) =>
            {
                CbRabbitString.IsVisible = false;
                CbRabbit.IsVisible = true;
                CbRabbit.Focus();
                CbRabbit.IsDropDownOpen = true;
            });

            CbRabbit.SelectedIndex = 0;

            BtEncode.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () => 
            {
                if (TbMain.Text.Length < 9)
                {
                    new Exception().Error("Нужно больше 9 символов");
                    return;
                }

                if (TbMain.Text.Length > Batch.MaxLength)
                {
                    new Exception().Error("Слишком большой текст");
                    return;
                }

                try
                {
                    Image<Rgb24> orig = RabbitRepository.SelectImage(CbRabbit.SelectedIndex - 1);
                    Image<Rgb24> img = await ImageEngine.EncodeAsync(orig, TbMain.Text, TbPassword.Text);
                    img.SaveAsPng((await new SaveFileDialog().ShowAsync(this)) + ".png");
                }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));

            BtDecode.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () => 
            {
                try
                {
                    Image<Rgb24> compare = Image.Load<Rgb24>((await new OpenFileDialog { AllowMultiple = false }.ShowAsync(this))[0]);
                    TbMain.Text = await ImageEngine.DecodeAsync(RabbitRepository.Detect(compare), compare, TbPassword.Text);
                }
                catch (Exception ex) { ex.Error("Это не кролик"); }
            }));

            BtTextEngine.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) =>  new TextWindow().Show());
            BtBatch.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => { new BatchWindow { Topmost = true }.Show(); Topmost = false; });
        }

    }
}
