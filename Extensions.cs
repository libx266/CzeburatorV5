using Avalonia.Controls;
using Avalonia.Media.Imaging;
using RabbitCrypt.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CzeburatorV5
{
    internal static class Extensions
    {
        internal static Bitmap ToBitmap(this byte[] data) =>
            new Bitmap(new MemoryStream(data));


        private static void ShowMBox(string title, string msg) =>
            MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(title, msg).Show();


        /// <summary>
        /// Сохраняет информацию об ошибке в файл
        /// </summary>
        /// <param name="ex">Ошибка</param>
        /// <param name="from">Адрес ошибки</param>
        private static void SaveIntoFile(Exception ex, string from = "")
        {
            try
            {
                string date = DateTime.Now.ToString();
                string info = $"[[{from}]|[{date}]]:\n";

                string message = ex.InnerException == null ?
                    ex.Message : ex.InnerException.Message;

                Console.WriteLine(info + message);
                
                File.AppendAllText
                (
                    "./log.txt", info
                    + message + "\n\n"
                );
            }
            catch { File.Create("./log.txt"); SaveIntoFile(ex, from); }
        }

        private static string From(string file, int row) => $"{file}:  {row}";

        internal static void Error(this Exception ex, string userMessage, [CallerFilePath] string file = "", [CallerLineNumber] int row = 0)
        {
            string from = From(file, row);
            ShowMBox("Ошибка!", userMessage);
            SaveIntoFile(ex, from);
        }

        internal static void Info(this string message) => ShowMBox("Info", message);

        /// <summary>
        /// Добавить фильтры расширения
        /// </summary>
        /// <param name="filters">Изображения,.png;Текст,.txt</param>
        /// <returns></returns>
        internal static T AddFilters<T>(this FileDialog dialog, string filters) where T : FileDialog
        {
            foreach(string filterPair in filters.Split(';'))
            {
                string[] F = filterPair.Split(',');
                dialog.Filters.Add(new FileDialogFilter
                {
                    Name = F[0],
                    Extensions = F.Skip(1).ToList()
                });
            }
            return dialog as T;
        }


        internal static TextBox ConfigureWithPassword(this TextBox textBox)
        {
            string pw = "Введите пароль:"; textBox.Text = pw;
            
            textBox.GotFocus += new EventHandler<Avalonia.Input.GotFocusEventArgs>((s, e) =>
            {
                var TbPassword = s as TextBox;
                 
                if (TbPassword.Text == pw)
                {
                    TbPassword.Text = "";
                    TbPassword.PasswordChar = '*';
                }
            });

            textBox.LostFocus += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) =>
            {
                var TbPassword = s as TextBox;
                
                if (TbPassword.Text == "")
                {
                    TbPassword.Text = pw;
                    TbPassword.PasswordChar = '\0';
                }
            });

            return textBox;
        }

        internal static TextBox ConfigureContextMenu(this TextBox TbMain, Window parent)
        {
            TbMain.ContextMenu = new ContextMenu();
            string menuItems = "Копировать;Вставить;Вырезать;Сохранить;Открыть;Очистить";

            TbMain.ContextMenu.Items = menuItems.Split(';').Select(item =>
            {
                var menu = new MenuItem { Header = item };
                menu.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(async (s, e) =>
                {
                    try
                    {
                        string header = (s as MenuItem).Header.ToString();
                        switch (header)
                        {
                            case "Копировать": TbMain.Copy(); break;
                            case "Вставить": TbMain.Paste(); break;
                            case "Вырезать": TbMain.Cut(); break;
                            case "Сохранить": File.WriteAllText((await new SaveFileDialog().AddFilters<SaveFileDialog>("Текстовые файлы,txt").ShowAsync(parent)), TbMain.Text); break;
                            case "Открыть": TbMain.Text = File.ReadAllText((await new OpenFileDialog().AddFilters<OpenFileDialog>("Текстовые файлы,txt").ShowAsync(parent))[0]); break;
                            case "Очистить": TbMain.Clear(); break;
                        }
                    }
                    catch (Exception ex) { ex.Error("Действие отменено"); }
                });
                return menu;
            });

            TbMain.AcceptsReturn = true;
            TbMain.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
            return TbMain;
        }

        private static HashAlgorithm Encryptor = SHA512.Create();
        internal static string Hash(this SixLabors.ImageSharp.Image<Rgb24> img)
        {
            var result = new StringBuilder();
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Rgb24 px = img[x, y];
                    result.Append(String.Format("{0},{1},{2};", px.R, px.G, px.B));
                }
            }
            return Encryptor.ComputeHash(Encoding.ASCII.GetBytes(result.ToString())).ToBase256();
        }

    }
}
