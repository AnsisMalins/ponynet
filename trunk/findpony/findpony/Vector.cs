using System;
using System.Diagnostics;

namespace findpony
{
	[DebuggerDisplay("{ToString()}")]
	public struct Vector
	{
		public float X { get { return x; } }
		public float Y { get { return y; } }
		public float Z { get { return z; } }
		public static Vector Zero = new Vector(0, 0, 0);
		float x, y, z;

		public Vector(float x, float y, float z)
		{
			this.x = x; this.y = y; this.z = z;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector)) return false;
			return this == (Vector)obj;
		}

		public override int GetHashCode()
		{
			int result = 17;
			result = result * 23 + x.GetHashCode();
			result = result * 23 + y.GetHashCode();
			result = result * 23 + z.GetHashCode();
			return result;
		}

		public float GetLength()
		{
			return (float)Math.Sqrt(x * x + y * y + z * z);
		}

		public override string ToString()
		{
			return "(" + x + ", " + y + ", " + z + ")";
		}

		public static Vector operator -(Vector a)
		{
			return new Vector(-a.x, -a.y, -a.z);
		}

		public static Vector operator +(Vector a, Vector b)
		{
			return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vector operator -(Vector a, Vector b)
		{
			return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static bool operator ==(Vector a, Vector b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		public static bool operator !=(Vector a, Vector b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}

		/*public static Vector operator *(Matrix a, Vector b)
		{
			return new Vector(
				a.A * b.x + a.B * b.y + a.C * b.z,
				a.D * b.x + a.E * b.y + a.F * b.z,
				a.G * b.x + a.H * b.y + a.I * b.z);
		}

		public static Vector operator *(Vector a, Matrix b)
		{
			return new Vector(
				a.x * b.A + a.y * b.D + a.z * b.G,
				a.x * b.B + a.y * b.E + a.z * b.H,
				a.x * b.C + a.y * b.F + a.y * b.I);
		}*/
	}
}