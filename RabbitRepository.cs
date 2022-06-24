using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzeburatorV5
{
    internal static class RabbitRepository
    {
        internal static byte[] SelectByte(int id)
        {
            switch(id)
            {
                case 0: return Properties.Resources.B0;
                case 1: return Properties.Resources.B1;
                case 2: return Properties.Resources.B2;
                case 3: return Properties.Resources.B3;
                case 4: return Properties.Resources.B4;
                case 5: return Properties.Resources.B5;
                case 6: return Properties.Resources.B6;
                case 7: return Properties.Resources.B7;
                case 8: return Properties.Resources.B8;
                case 9: return Properties.Resources.B9;
                default: return SelectByte(Random.Shared.Next(10));
            }
        }

        internal static Image<Rgb24> SelectImage(int id) =>
            Image.Load<Rgb24>(SelectByte(id));

        internal static Image<Rgb24>? Detect(Image<Rgb24> compare)
        {
            Rgb24 pxCompare = compare[0, 0];
            for(int i = 0; i < 10; i++)
            {
                Image<Rgb24> orig = SelectImage(i);
                Rgb24 pxOrig = orig[0, 0];

                if
                (
                       Math.Abs(pxCompare.R - pxOrig.R) < 3
                    && Math.Abs(pxCompare.G - pxOrig.G) < 3
                    && Math.Abs(pxCompare.B - pxOrig.B) < 3
                )
                return orig;
            }
            return null;
        }
    }
}
