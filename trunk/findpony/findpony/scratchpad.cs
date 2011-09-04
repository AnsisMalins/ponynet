using System;
using System.Diagnostics;
using findpony;
using System.Drawing;

namespace scratchpad
{
	class scratchpad
	{
		static void Form()
		{
			/*Form form = new Form();
			form.ClientSize = new Size(bOriginal.Width * 2, bOriginal.Height);
			form.FormBorderStyle = FormBorderStyle.FixedSingle;
			form.Text = max.ToString();
			form.Click += delegate { form.Invalidate(); };
			form.Paint += delegate(object sender, PaintEventArgs e)
			{
				e.Graphics.DrawImage(bOriginal, 0, 0, bOriginal.Width, bOriginal.Height);
				using (var bitmap = vMarks.ToBitmap())
				{
					e.Graphics.DrawImage(bitmap, bOriginal.Width, 0);
				}
			};
			form.ShowDialog();*/
		}

		static void XYZ(VectorMap vOriginal)
		{
			// Note to self: sRGB white point = (0.3127, 0.329, 0.3583)

			/*string s = "";
e.Graphics.DrawString(s, form.Font, Brushes.Black, form.PointToClient(Form.MousePosition) - e.Graphics.MeasureString(s, form.Font).ToSize());*/
			
			VectorMap vXyyChroma = vOriginal
				.ConvertAll(i => RemoveGamma(i))
				.ConvertAll(i => RgbToXyz(i))
				.ConvertAll(i => XyzToXyy(i))
				.ConvertAll(i => new Vector(i.X, i.Y, 0.5f));

			Bitmap bXyzChroma = vXyyChroma
				.ConvertAll(i => XyyToXyz(i))
				.ConvertAll(i => XyzToRgb(i))
				.ConvertAll(i => ApplyGamma(i))
				.ToBitmap();
		}

		static void HSV(VectorMap vOriginal)
		{
			VectorMap vHsvChroma = vOriginal
	.ConvertAll(delegate(Vector i)
			{
				float max = Math.Max(Math.Max(i.X, i.Y), i.Z);
				float min = Math.Min(Math.Min(i.X, i.Y), i.Z);
				float c = max - min;
				float h = (float)Math.PI / 3 * (c == 0 ? 0
					: max == i.X ? ((i.Y - i.Z) / c + 6) % 6
					: max == i.Y ? (i.Z - i.X) / c + 2
					: (i.X - i.Y) / c + 4);
				return new Vector(h, c == 0 ? 0 : c / max, max);
			})
	.ConvertAll(i => new Vector(i.X, i.Y, 1));

			Bitmap bHsvChroma = vHsvChroma
				.ConvertAll(delegate(Vector i)
			{
				float c = i.Z * i.Y;
				float h = i.X * 3 / (float)Math.PI;
				float x = c * (1 - Math.Abs(h % 2 - 1));
				Vector j = h < 1 ? new Vector(c, x, 0)
					: h < 2 ? new Vector(x, c, 0)
					: h < 3 ? new Vector(0, c, x)
					: h < 4 ? new Vector(0, x, c)
					: h < 5 ? new Vector(x, 0, c)
					: new Vector(c, 0, x);
				float m = i.Z - c;
				return new Vector(j.X + m, j.Y + m, j.Z + m);
			})
				.ToBitmap();
		}

		static bool ZeroToOne(Vector i)
		{
			return i.X >= 0 && i.X <= 1 && i.Y >= 0 && i.Y <= 1 && i.Z >= 0 && i.Z <= 1;
		}

		static Vector RemoveGamma(Vector i)
		{
			Debug.Assert(ZeroToOne(i));
			Vector j = new Vector(
				i.X > 0.04045f ? (float)Math.Pow((i.X + 0.055) / 1.055, 2.4) : i.X / 12.92f,
				i.Y > 0.04045f ? (float)Math.Pow((i.Y + 0.055) / 1.055, 2.4) : i.Y / 12.92f,
				i.Z > 0.04045f ? (float)Math.Pow((i.Z + 0.055) / 1.055, 2.4) : i.Z / 12.92f);
			Debug.Assert(ZeroToOne(j));
			return j;
		}

		static Vector ApplyGamma(Vector i)
		{
			Debug.Assert(ZeroToOne(i));
			Vector j = new Vector(
				i.X > 0.0031308f ? (float)(1.055 * Math.Pow(i.X, 1 / 2.4) - 0.055) : 12.92f * i.X,
				i.Y > 0.0031308f ? (float)(1.055 * Math.Pow(i.Y, 1 / 2.4) - 0.055) : 12.92f * i.Y,
				i.Z > 0.0031308f ? (float)(1.055 * Math.Pow(i.Z, 1 / 2.4) - 0.055) : 12.92f * i.Z);
			Debug.Assert(ZeroToOne(j));
			return j;
		}

		static Vector RgbToXyz(Vector i)
		{
			Debug.Assert(ZeroToOne(i));
			Matrix m = new Matrix(
				0.4360747f, 0.3850649f, 0.1430804f,
				0.2225045f, 0.7168786f, 0.0606169f,
				0.0139322f, 0.0971045f, 0.7141733f);
			/*0.4124f, 0.3576f, 0.1805f,
			0.2126f, 0.7152f, 0.0722f,
			0.0193f, 0.1192f, 0.9505f);*/
			Vector j = m * i;
			Debug.Assert(ZeroToOne(j));
			return j;
		}

		static Vector XyzToRgb(Vector i)
		{
			Debug.Assert(ZeroToOne(i));
			Matrix m = new Matrix(
				3.1338561f, -1.6168667f, -0.4906146f,
				-0.9787684f, 1.9161415f, 0.033454f,
				0.0719453f, -0.2289914f, 1.4052427f);
			/*3.2406f, -1.5372f, -0.4976f,
			-0.9689f, 1.8758f, 0.0415f,
			0.0557f, -0.204f, 1.057f);*/
			Vector j = m * i;
			Debug.Assert(ZeroToOne(j));
			return j;
		}

		static Vector XyzToXyy(Vector i)
		{
			Debug.Assert(ZeroToOne(i));
			float s = i.X + i.Y + i.Z;
			if (s == 0) return i;
			Vector j = new Vector(i.X / s, i.Y / s, i.Y);
			Debug.Assert(ZeroToOne(j));
			return j;
		}

		static Vector XyyToXyz(Vector i)
		{
			Debug.Assert(ZeroToOne(i));
			float z = 1 - i.X - i.Y;
			if (i.X == 0 || i.Y == 0 || z == 0) return i;
			Vector j = new Vector(i.Z / i.Y * i.X, i.Z, i.Z / i.Y * z);
			Debug.Assert(ZeroToOne(j));
			return j;
		}
	}
}