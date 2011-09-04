using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace findpony
{
	public class VectorMap
	{
		public int Height { get { return Size.Height; } }
		public Size Size { get; private set; }
		public int Width { get { return Size.Width; } }
		Vector[] pixels;

		public unsafe VectorMap(Bitmap bitmap)
			: this(bitmap.Size)
		{
			BitmapData bitmapData = bitmap.LockBits(
				new Rectangle(Point.Empty, Size),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);
			System.Diagnostics.Debug.Assert(bitmapData.Width * 4 == bitmapData.Stride);
			var ptr = (byte*)bitmapData.Scan0;
			for (int i = 0; i < pixels.Length; i++)
			{
				float a = ptr[3] / 255f;
				pixels[i] = new Vector(
					ptr[2] * a / 255, 
					ptr[1] * a / 255, 
					ptr[0] * a / 255);
				ptr += 4;
			}
			bitmap.UnlockBits(bitmapData);
		}

		public VectorMap(Size size)
		{
			Size = size;
			pixels = new Vector[Width * Height];
		}

		public VectorMap(VectorMap vectorMap)
			: this(vectorMap.Size)
		{
			Buffer.BlockCopy(vectorMap.pixels, 0, pixels, 0, pixels.Length);
		}

		public Vector this[Point p]
		{
			get
			{
#if DEBUG
				if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height) throw new ArgumentOutOfRangeException("p");
#endif
				return pixels[p.X + p.Y * Width];
			}
			set
			{
#if DEBUG
				if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height) throw new ArgumentOutOfRangeException("p");
#endif
				pixels[p.X + p.Y * Width] = value;
			}
		}

		public void ForEach(Converter<Vector, Vector> converter)
		{
			for (int i = 0; i < pixels.Length; i++) pixels[i] = converter(pixels[i]);
		}

		public Vector this[int x, int y]
		{
			get
			{
#if DEBUG
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");
#endif
				return pixels[x + y * Width];
			}
			set
			{
#if DEBUG
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");
#endif
				pixels[x + y * Width] = value;
			}
		}

		public unsafe Bitmap ToBitmap()
		{
			var bitmap = new Bitmap(Width, Height);
			BitmapData bitmapData = bitmap.LockBits(
				new Rectangle(Point.Empty, Size),
				ImageLockMode.WriteOnly,
				PixelFormat.Format32bppArgb);
			System.Diagnostics.Debug.Assert(bitmapData.Width * 4 == bitmapData.Stride);
			var ptr = (byte*)bitmapData.Scan0;
			for (int i = 0; i < pixels.Length; i++)
			{
				Vector j = pixels[i];
				ptr[3] = 255;
				ptr[2] = (byte)(j.X * 255 + 0.5f);
				ptr[1] = (byte)(j.Y * 255 + 0.5f);
				ptr[0] = (byte)(j.Z * 255 + 0.5f);
				ptr += 4;
			}
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}
	}
}