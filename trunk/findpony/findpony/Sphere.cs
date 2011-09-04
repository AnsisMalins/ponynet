using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace findpony
{
	[DebuggerDisplay("{ToString()}")]
	public struct Sphere
	{
		public Vector O { get { return o; } }
		public float R { get { return r; } }
		public float X { get { return o.X; } }
		public float Y { get { return o.Y; } }
		public float Z { get { return o.Z; } }
		Vector o;
		float r;

		public Sphere(Vector o, float r)
		{
			if (r < 0) throw new ArgumentOutOfRangeException("r");
			this.o = o;
			this.r = r;
		}

		public Sphere(IEnumerable<Vector> v)
		{
			if (v == null) throw new ArgumentNullException("v");
			float minx = float.MaxValue;
			float maxx = float.MinValue;
			float miny = float.MaxValue;
			float maxy = float.MinValue;
			float minz = float.MaxValue;
			float maxz = float.MinValue;
			foreach (Vector i in v)
			{
				if (i.X < minx) minx = i.X;
				if (i.X > maxx) maxx = i.X;
				if (i.Y < miny) miny = i.Y;
				if (i.Y > maxy) maxy = i.Y;
				if (i.Z < minz) minz = i.Z;
				if (i.Z > maxz) maxz = i.Z;
			}
			o = new Vector((minx + maxx) / 2, (miny + maxy) / 2, (minz + maxz) / 2);
			r = 0;
			foreach (Vector i in v)
			{
				float l = (o - i).GetLength();
				if (r < l) r = l;
			}
		}

		public Sphere(float x, float y, float z, float r)
		{
			if (r < 0) throw new ArgumentOutOfRangeException("r");
			o = new Vector(x, y, z);
			this.r = r;
		}

		public bool Contains(Vector v)
		{
			return (o - v).GetLength() < r;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Sphere)) return false;
			return this == (Sphere)obj;
		}

		public override int GetHashCode()
		{
			int result = 17;
			result = result * 23 + o.GetHashCode();
			result = result * 23 + r.GetHashCode();
			return result;
		}

		public override string ToString()
		{
			return "(" + o.X + ", " + o.Y + ", " + o.Z + ", " + r + ")";
		}

		public static bool operator ==(Sphere a, Sphere b)
		{
			return a.o == b.o && a.r == b.r;
		}

		public static bool operator !=(Sphere a, Sphere b)
		{
			return a.o != b.o || a.r != b.r;
		}
	}
}