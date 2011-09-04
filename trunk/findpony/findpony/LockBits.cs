using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace findpony
{
	public class LockBits : IDisposable
	{
		Bitmap bitmap;
		BitmapData bitmapData;

		public LockBits(Bitmap bitmap)
		{
			if (bitmap == null) throw new ArgumentNullException("bitmap");
			this.bitmap = bitmap;
			bitmapData = bitmap.LockBits(
				new Rectangle(Point.Empty, bitmap.Size),
				ImageLockMode.ReadWrite,
				PixelFormat.Format32bppArgb);
		}

		public void Dispose()
		{
			if (bitmapData == null) return;
			bitmap.UnlockBits(bitmapData);
			bitmap = null;
			bitmapData = null;
		}

		public unsafe void ForEach(Converter<Color, Color> converter)
		{
			if (bitmapData == null) throw new ObjectDisposedException("LockBits");
			System.Diagnostics.Debug.Assert(bitmapData.Width * 4 == bitmapData.Stride);
			var s0 = (int*)bitmapData.Scan0;
			int l = bitmapData.Width * bitmap.Height;
			for (int i = 0; i < l; i++) s0[i] = converter(Color.FromArgb(s0[i])).ToArgb();
		}

		public unsafe Color GetPixel(int x, int y)
		{
#if DEBUG
			if (bitmapData == null) throw new ObjectDisposedException("LockBits");
			if (x < 0 || x >= bitmapData.Width) throw new ArgumentOutOfRangeException("x");
			if (y < 0 || y >= bitmapData.Height) throw new ArgumentOutOfRangeException("y");
#endif
			return Color.FromArgb(((int*)bitmapData.Scan0)[x + y * bitmapData.Stride / 4]);
		}

		public unsafe void SetPixel(int x, int y, Color color)
		{
#if DEBUG
			if (bitmapData == null) throw new ObjectDisposedException("LockBits");
			if (x < 0 || x >= bitmapData.Width) throw new ArgumentOutOfRangeException("x");
			if (y < 0 || y >= bitmapData.Height) throw new ArgumentOutOfRangeException("y");
#endif
			((int*)bitmapData.Scan0)[x + y * bitmapData.Stride / 4] = color.ToArgb();
		}
	}
}