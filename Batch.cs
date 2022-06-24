using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitCrypt;
using RabbitCrypt.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CzeburatorV5
{

    public class ProgressEventArgs : EventArgs
    {
        public int Count { get; }

        public string Log { get; }

        public int MaxCount { get;  }

        public ProgressEventArgs(int count, string log, int maxCount)
        {
            Count = count;
            Log = log;
            MaxCount = maxCount;
        }
    }


    internal abstract class Batch
    {
        internal const int MaxLength = 33264;
        protected int MaxCount = 0;
        
        protected string password;
        protected string inputPath;
        protected string outputPath;
        protected string serialName;

        internal bool IsRunning;

        internal abstract Task Job();
        internal abstract event EventHandler<ProgressEventArgs> ProgressChanged;

        protected string hash = "";

        internal Batch(string password, string inputPath, string outputPath, string serialName)
        {
            this.password = password;
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.serialName = serialName;
        }
    }


    internal sealed class BatchEncode : Batch
    {
       internal BatchEncode(string password, string inputPath, string outputPath, string serialName) 
            : base(password, inputPath, outputPath, serialName) { }

        internal override event EventHandler<ProgressEventArgs> ProgressChanged;

        internal override async Task Job()
        {
            IsRunning = true;
            byte[] data = File.ReadAllBytes(inputPath);
            string ext = inputPath.Split('.').Last();
            string inputText = AdmiralFyyar.EncodeFormat(ext) + data.ToBase256();
            MaxCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(data.Length) / MaxLength));
            
            for (int i = 0; i < MaxCount; i++)
            {
                try
                {
                    string text;
                    try { text = inputText.Substring(i * MaxLength, MaxLength); }
                    catch { text = inputText.Substring(i * MaxLength, inputText.Length - i * MaxLength); }

                    Image<Rgb24> enc = await ImageEngine.EncodeAsync(RabbitRepository.SelectImage(-1), text, password + hash);

                    string digit = String.Empty;
                    string count = i.ToString();
                    for (int j = 0; j < 6 - count.Length; j++)
                        digit += "0";
                    string path = $"{outputPath}/{serialName}_{digit + count}.png";

                    enc.SaveAsPng(path);
                    hash = enc.Hash();
                    ProgressChanged.Invoke(this, new ProgressEventArgs(i, $"{path}\n", MaxCount-1));

                }
                catch (Exception ex) { ProgressChanged.Invoke(this, new ProgressEventArgs(i, ex.Message + "\n", MaxCount-1)); }
                finally { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); }
            }

            IsRunning = false;
        }

        

    }

    internal sealed class BatchDecode : Batch
    {
        internal BatchDecode(string password, string inputPath, string outputPath, string serialName) 
            : base(password, inputPath, outputPath, serialName) { }

        internal override event EventHandler<ProgressEventArgs> ProgressChanged;

        internal override async Task Job()
        {
            IsRunning = true;
            string[] files = System.IO.Directory.GetFiles(inputPath).OrderBy(s => s).ToArray();
            MaxCount = files.Length;
            var text = new StringBuilder();
            
            for (int i = 0; i < MaxCount; i++)
            {
                try
                {
                    string path = files[i];
                    if (!path.Contains(".png") || !path.Contains(serialName)) continue;

                    Image<Rgb24> compare = Image.Load<Rgb24>(path);
                    Image<Rgb24> orig = RabbitRepository.Detect(compare);

                    text.Append(await ImageEngine.DecodeAsync(orig, compare, password + hash));

                    hash = compare.Hash();

                    ProgressChanged.Invoke(this, new ProgressEventArgs(i, $"{path}\n", MaxCount-1));

                }
                catch (Exception ex) { ProgressChanged.Invoke(this, new ProgressEventArgs(i, ex.Message + "\n", MaxCount-1)); }
                finally { GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); }
            }

            string output = text.ToString();
            string ext = AdmiralFyyar.DetectFormat(output.First());
            string write = String.Join("", output.Skip(1));

            File.WriteAllBytes($"{outputPath}.{ext}", write.FromBase256());
            IsRunning = false;
        }
    }
}
