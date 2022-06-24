using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzeburatorV5
{
    public partial class TextWindow
    {

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            TbPassword = this.FindControl<global::Avalonia.Controls.TextBox>("TbPassword");
            TbLeft = this.FindControl<global::Avalonia.Controls.TextBox>("TbLeft");
            TbRight = this.FindControl<global::Avalonia.Controls.TextBox>("TbRight");

            TbPassword.ConfigureWithPassword();
            TbLeft.ConfigureContextMenu(this);
            TbRight.ConfigureContextMenu(this);

            BtEncode = this.FindControl<global::Avalonia.Controls.Button>("BtEncode");
            BtDecode = this.FindControl<global::Avalonia.Controls.Button>("BtDecode");
            BtParse = this.FindControl<global::Avalonia.Controls.Button>("BtParse");
            BtAss = this.FindControl<global::Avalonia.Controls.Button>("BtAss");
        }
        
    }
}
