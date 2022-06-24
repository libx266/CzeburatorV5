using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;

namespace CzeburatorV5
{
    public partial class BatchWindow : Window
    {
        Batch? Task;
        public BatchWindow()
        {
            MinWidth = 400; MinHeight = 300;
            InitializeComponent();
            Width = 400; Height = 300;
            
            this.AttachDevTools();

            var progress = new EventHandler<ProgressEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(() =>
            {
                PbProgress.Maximum = e.MaxCount;
                PbProgress.Value = e.Count;
                TbLog.Text += e.Log;
            }));

            BtEncode.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    if (Task is null || !Task.IsRunning)
                    {
                        PbProgress.Value = 0;
                        PbProgress.Maximum = 1;

                        string[]? file = await new OpenFileDialog { AllowMultiple = false }.ShowAsync(this);
                        string? folder = await new OpenFolderDialog().ShowAsync(this);

                        if (file is null || folder is null)
                        {
                            new Exception().Error("Задача отменена");
                            return;
                        }

                        (Task = new BatchEncode
                        (
                            TbPassword.Text,
                            file[0],
                            folder,
                            TbName.Text
                        ))
                        .ProgressChanged += progress;

                        await Task.Job();
                    }
                    else new Exception().Error("Задача уже запущена");
                }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));

            BtDecode.Click += new EventHandler<Avalonia.Interactivity.RoutedEventArgs>((s, e) => Dispatcher.UIThread.InvokeAsync(async () => 
            {
                try
                {
                    if (Task is null || !Task.IsRunning)
                    {
                        PbProgress.Value = 0;
                        PbProgress.Maximum = 1;

                        string? folder = await new OpenFolderDialog().ShowAsync(this);
                        string? file = await new SaveFileDialog().ShowAsync(this);

                        if (file is null || folder is null)
                        {
                            new Exception().Error("Задача отменена");
                            return;
                        }

                        (Task = new BatchDecode
                        (
                            TbPassword.Text,
                            folder,
                            file,
                            TbName.Text
                        )).ProgressChanged += progress;

                        await Task.Job();
                    }
                    else new Exception().Error("Задача уже запущена");
                }
                catch (Exception ex) { ex.Error("Кролик впал в транс"); }
            }));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = Task != null && Task.IsRunning;
            base.OnClosing(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            TbName = this.FindControl<global::Avalonia.Controls.TextBox>("TbName");
            TbLog = this.FindControl<global::Avalonia.Controls.TextBox>("TbLog");
            TbPassword = this.FindControl<global::Avalonia.Controls.TextBox>("TbPassword").ConfigureWithPassword();
            
            PbProgress = this.FindControl<global::Avalonia.Controls.ProgressBar>("PbProgress");
            
            BtEncode = this.FindControl<global::Avalonia.Controls.Button>("BtEncode");
            BtDecode = this.FindControl<global::Avalonia.Controls.Button>("BtDecode");
            
        }
    }
}
