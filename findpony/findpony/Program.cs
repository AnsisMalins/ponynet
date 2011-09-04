using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace findpony
{
	class Program
	{
		static int Main(string[] args)
		{
			if (Debugger.IsAttached) args = new[] { "255", "0", "0", "15", @"C:\Users\Ansis\Downloads\RGB_24bits_palette_color_test_chart.png" };

			Vector targetColor;
			float threshold;
			VectorMap map;

			try
			{
				targetColor = new Vector(byte.Parse(args[0]) / 255f, byte.Parse(args[1]) / 255f, byte.Parse(args[2]) / 255f);
				threshold = byte.Parse(args[3]);
				using (var bitmap = new Bitmap(args[4]))
				{
					map = new VectorMap(bitmap);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 0;
			}

			map.ForEach(delegate(Vector i)
			{
				float m = Math.Max(Math.Max(i.X, i.Y), i.Z);
				return m > 0 ? new Vector(i.X / m, i.Y / m, i.Z / m) : new Vector(1, 1, 1);
			});

			var mark = new Vector(-1, 0, 0);
			int max = 0;
			for (int y = 0; y < map.Height; y++)
			{
				for (int x = 0; x < map.Width; x++)
				{
					if (map[x, y] == mark) continue;
					int count = FloodFill(map, new Point(x, y), new Sphere(targetColor, threshold), mark);
					if (count > max) max = count;
				}
			}

			if (Debugger.IsAttached)
			{
				Console.WriteLine(max.ToString());
				Console.ReadKey(true);
			}

			return max;
		}

		static int FloodFill(VectorMap map, Point origin, Sphere target, Vector fillColor)
		{
			var queue = new Queue<Point>(map.Width + map.Height);
			queue.Enqueue(origin);
			int count = 0;
			while (queue.Count > 0)
			{
				Point p = queue.Dequeue();
				Vector v = map[p];
				if (v == fillColor || !target.Contains(v)) continue;
				map[p] = fillColor;
				count++;
				if (p.X > 0) queue.Enqueue(new Point(p.X - 1, p.Y));
				if (p.Y > 0) queue.Enqueue(new Point(p.X, p.Y - 1));
				if (p.X < map.Width - 1) queue.Enqueue(new Point(p.X + 1, p.Y));
				if (p.Y < map.Height - 1) queue.Enqueue(new Point(p.X, p.Y + 1));
			}
			return count;
		}
	}
}