using System;

namespace findpony
{
	public class BoolMap
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		int[] bits;

		public BoolMap(int width, int height)
		{
			Width = width;
			Height = height;
			bits = new int[(width * height + 31) / 32];
		}

		public bool this[int x, int y]
		{
			get
			{
#if DEBUG
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");
#endif
				int i = x + y * Width;
				return (bits[i / 32] & 1 << i % 32) != 0;
			}
			set
			{
#if DEBUG
				if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException("y");
#endif
				int i = x + y * Width;
				int j = i / 32;
				bits[j] = value ? bits[j] | 1 << i % 32 : bits[j] & ~(1 << i % 32);
			}
		}
	}
}