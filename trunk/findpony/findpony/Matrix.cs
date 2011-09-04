namespace findpony
{
	public struct Matrix
	{
		public float A { get { return a; } }
		public float B { get { return b; } }
		public float C { get { return c; } }
		public float D { get { return d; } }
		public float E { get { return e; } }
		public float F { get { return f; } }
		public float G { get { return g; } }
		public float H { get { return h; } }
		public float I { get { return i; } }
		float a, b, c, d, e, f, g, h, i;

		public Matrix(
			float a, float b, float c,
			float d, float e, float f,
			float g, float h, float i)
		{
			this.a = a; this.b = b; this.c = c;
			this.d = d; this.e = e; this.f = f;
			this.g = g; this.h = h; this.i = i;
		}

		public static Matrix Identity = new Matrix(
			1, 0, 0,
			0, 1, 0,
			0, 0, 1);
	}
}