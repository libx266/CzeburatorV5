using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzeburatorV5
{
    internal static  class AdmiralFyyar
    {
        internal const int P = 40960; //U+A000 bytecode alphabet
        internal static string DetectFormat(char u9)
        {
            string format = "bruh";
            switch ((ushort)u9 - P)
            {
                case 10: format = "txt"; break;
                case 11: format = "bmp"; break;
                case 12: format = "awb"; break;
                case 13: format = "png"; break;
                case 14: format = "jpg"; break;
                case 16: format = "tar.gz"; break;
                case 20: format = "tif"; break;
                case 22: format = "psd"; break;
                case 24: format = "tga"; break;
                case 26: format = "jfif"; break;
                case 28: format = "aac"; break;
                case 30: format = "amr"; break;
                case 32: format = "3gpp"; break;
                case 34: format = "ogg"; break;
                case 36: format = "flac"; break;
                case 38: format = "wav"; break;
                case 40: format = "mp3"; break;
                case 42: format = "mp2"; break;
                case 44: format = "opus"; break;
                case 46: format = "wma"; break;
                case 48: format = "mov"; break;
                case 50: format = "3gp"; break;
                case 52: format = "mp4"; break;
                case 54: format = "mkv"; break;
                case 56: format = "webm"; break;
                case 58: format = "wmv"; break;
                case 60: format = "avi"; break;
                case 62: format = "zip"; break;
                case 64: format = "7z"; break;
                case 66: format = "lzh"; break;
                case 68: format = "rar"; break;
                case 70: format = "tar"; break;
                case 72: format = "tgz"; break;
                case 74: format = "fb2"; break;
                case 76: format = "doc"; break;
                case 78: format = "docx"; break;
                case 80: format = "xlsx"; break;
                case 82: format = "xlsm"; break;
                case 84: format = "xml"; break;
                case 86: format = "html"; break;
                case 88: format = "mhtml"; break;
                case 90: format = "csv"; break;
                case 92: format = "ppt"; break;
                case 94: format = "odt"; break;
                case 96: format = "pdf"; break;
                case 98: format = "djvu"; break;
                case 100: format = "mpg"; break;
                case 144: format = "odf"; break;
                case 145: format = "xls"; break;
                case 146: format = "rtf"; break;
                case 147: format = "pptx"; break;
                case 148: format = "ods"; break;
                case 149: format = "sql"; break;
                case 150: format = "accdb"; break;
            }
            return format;
        }

        internal static char EncodeFormat(string format)
        {
            byte fmt = 0;
            switch (format.ToLower())
            {
                case "txt": fmt = 10; break;
                case "bmp": fmt = 11; break;
                case "awb": fmt = 12; break;
                case "png": fmt = 13; break;
                case "jpg": fmt = 14; break;
                case "gif": fmt = 15; break;
                case "gz": fmt = 16; break;
                case "tif": fmt = 20; break;
                case "psd": fmt = 22; break;
                case "tga": fmt = 24; break;
                case "jfif": fmt = 26; break;
                case "aac": fmt = 28; break;
                case "amr": fmt = 30; break;
                case "3gpp": fmt = 32; break;
                case "ogg": fmt = 34; break;
                case "cdw": fmt = 35; break;
                case "flac": fmt = 36; break;
                case "wav": fmt = 38; break;
                case "mp3": fmt = 40; break;
                case "mp2": fmt = 42; break;
                case "opus": fmt = 44; break;
                case "wma": fmt = 46; break;
                case "mov": fmt = 48; break;
                case "3gp": fmt = 50; break;
                case "mp4": fmt = 52; break;
                case "mkv": fmt = 54; break;
                case "webm": fmt = 56; break;
                case "wmv": fmt = 58; break;
                case "avi": fmt = 60; break;
                case "zip": fmt = 62; break;
                case "7z": fmt = 64; break;
                case "lzh": fmt = 66; break;
                case "rar": fmt = 68; break;
                case "tar": fmt = 70; break;
                case "tgz": fmt = 72; break;
                case "fb2": fmt = 74; break;
                case "doc": fmt = 76; break;
                case "docx": fmt = 78; break;
                case "xlsx": fmt = 80; break;
                case "xlsm": fmt = 82; break;
                case "xm1": fmt = 84; break;
                case "html": fmt = 86; break;
                case "mhtml": fmt = 88; break;
                case "csv": fmt = 90; break;
                case "ppt": fmt = 92; break;
                case "odt": fmt = 94; break;
                case "pdf": fmt = 96; break;
                case "djvu": fmt = 98; break;
                case "mpg": fmt = 100; break;
                case "odf": fmt = 144; break;
                case "xls": fmt = 145; break;
                case "rtf": fmt = 146; break;
                case "pptx": fmt = 147; break;
                case "ods": fmt = 148; break;
                case "sql": fmt = 149; break;
                case "accdb": fmt = 150; break;
                default: fmt = 0; break;
            }
            return Convert.ToChar(P + fmt);
        }
    }
}
