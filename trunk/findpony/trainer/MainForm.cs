using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using findpony;

namespace trainer
{
	public partial class MainForm : Form
	{
		Rectangle imageRect = new Rectangle(0, 23, 0, 0);
		Bitmap loadedImage;
		VectorMap chromaMap;
		BoolMap fillMap;
		Point fillOrigin;
		List<Vector> colorPoints = new List<Vector>();
		UIForm colorPointForm;
		Sphere boundingSphere;

		public MainForm()
		{
			InitializeComponent();
			colorPointForm = new UIForm(this);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!imageRect.Contains(e.Location)) return;
			Cursor cursor = Cursor;
			Cursor = Cursors.WaitCursor;
			Vector i = ColorToVector(loadedImage.GetPixel(e.Location.X - imageRect.X, e.Location.Y - imageRect.Y));
			float m = Math.Max(Math.Max(i.X, i.Y), i.Z);
			i = m > 0 ? new Vector(i.X / m, i.Y / m, i.Z / m) : new Vector(1, 1, 1);

			colorPoints.Add(i);
			colorPointForm.listBox.Items.Add(i);
			boundingSphere = new Sphere(colorPoints);
			boundingSphere = new Sphere(boundingSphere.O, boundingSphere.R + 0.001953125f);
			resultTextBox.Text = boundingSphere.ToString();
			int area;
			Rectangle bounds;
			FillBlots(chromaMap, boundingSphere, out fillMap, out area, out bounds, out fillOrigin);
			Invalidate();
			Cursor = cursor;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (loadedImage == null || !browseButton.Enabled) return;
			Rectangle clipRectangle = Rectangle.Intersect(e.ClipRectangle, imageRect);
			if (clipRectangle == Rectangle.Empty) return;
			if (!showChromaMap.Checked) e.Graphics.DrawImage(loadedImage, imageRect);
			else using (Bitmap cm = chromaMap.ToBitmap()) e.Graphics.DrawImage(cm, imageRect);

			var queue = new Queue<Point>(chromaMap.Width + chromaMap.Height);
			var fillMap = new BoolMap(chromaMap.Width, chromaMap.Height);
			queue.Enqueue(fillOrigin);
			while (queue.Count > 0)
			{
				Point p = queue.Dequeue();
				if (fillMap[p.X, p.Y] || !boundingSphere.Contains(chromaMap[p])) continue;
				fillMap[p.X, p.Y] = true;
				Point q = new Point(imageRect.X + p.X, imageRect.Y + p.Y);
				e.Graphics.FillRectangle(Brushes.Blue, q.X, q.Y, 1, 1);
				if (p.X > 0) queue.Enqueue(new Point(p.X - 1, p.Y));
				if (p.Y > 0) queue.Enqueue(new Point(p.X, p.Y - 1));
				if (p.X < chromaMap.Width - 1) queue.Enqueue(new Point(p.X + 1, p.Y));
				if (p.Y < chromaMap.Height - 1) queue.Enqueue(new Point(p.X, p.Y + 1));
			}
		}

		void browseButton_Click(object sender, EventArgs e)
		{
			using (var dialog = new OpenFileDialog())
			{
				if (dialog.ShowDialog() == DialogResult.Cancel) return;
				browseButton.Enabled = false;
				Cursor = Cursors.WaitCursor;
				string fileName = dialog.FileName;
				Text = "Loading: " + fileName;
				ThreadPool.UnsafeQueueUserWorkItem(delegate
				{
					try
					{
						if (loadedImage != null)
						{
							loadedImage.Dispose();
							loadedImage = null;
							chromaMap = null;
						}
						GC.Collect();
						loadedImage = new Bitmap(fileName);
						chromaMap = new VectorMap(loadedImage);
						chromaMap.ForEach(delegate(Vector i)
						{
							float m = Math.Max(Math.Max(i.X, i.Y), i.Z);
							return m > 0 ? new Vector(i.X / m, i.Y / m, i.Z / m) : new Vector(1, 1, 1);
						});
						imageRect.Size = loadedImage.Size;
						if (colorPoints.Count > 0)
						{
							int area;
							Rectangle bounds;
							FillBlots(chromaMap, boundingSphere, out fillMap, out area, out bounds, out fillOrigin);
						}
						BeginInvoke(new Action(delegate { Text = fileName; }));
					}
					catch (Exception ex)
					{
						imageRect.Size = Size.Empty;
						BeginInvoke(new Action(delegate { MessageBox.Show(this, ex.Message); }));
					}
					finally
					{
						BeginInvoke(new Action(delegate { Cursor = Cursors.Default; browseButton.Enabled = true; Invalidate(); }));
					}
				}, null);
			}
		}

		static Vector ColorToVector(Color color)
		{
			float a = color.A / 255f;
			return new Vector(
				color.R * a / 255,
				color.G * a / 255,
				color.B * a / 255);
		}

		static void FloodFill(VectorMap map, Point origin, Sphere target, BoolMap fillMap, out int area, out Rectangle bounds)
		{
			var queue = new Queue<Point>(map.Width + map.Height);
			queue.Enqueue(origin);
			bounds = new Rectangle(int.MaxValue, int.MaxValue, 0, 0);
			area = 0;
			while (queue.Count > 0)
			{
				Point p = queue.Dequeue();
				if (fillMap[p.X, p.Y] || !target.Contains(map[p])) continue;
				fillMap[p.X, p.Y] = true;
				if (bounds.X > p.X) bounds.X = p.X;
				if (bounds.Y > p.Y) bounds.Y = p.Y;
				if (bounds.Right <= p.X) bounds.Width = p.X - bounds.X + 1;
				if (bounds.Bottom <= p.Y) bounds.Height = p.Y - bounds.Y + 1;
				area++;
				if (p.X > 0) queue.Enqueue(new Point(p.X - 1, p.Y));
				if (p.Y > 0) queue.Enqueue(new Point(p.X, p.Y - 1));
				if (p.X < map.Width - 1) queue.Enqueue(new Point(p.X + 1, p.Y));
				if (p.Y < map.Height - 1) queue.Enqueue(new Point(p.X, p.Y + 1));
			}
			if (bounds.Size == Size.Empty) bounds.Location = Point.Empty;
		}

		static void FillBlots(VectorMap map, Sphere target, out BoolMap fillMap, out int area, out Rectangle bounds, out Point origin)
		{
			fillMap = new BoolMap(map.Width, map.Height);
			origin = Point.Empty;
			area = 0;
			bounds = Rectangle.Empty;
			for (int y = 0; y < map.Height; y++)
			{
				for (int x = 0; x < map.Width; x++)
				{
					if (fillMap[x, y]) continue;
					var xy = new Point(x, y);
					int xyArea;
					Rectangle xyBounds;
					FloodFill(map, xy, target, fillMap, out xyArea, out xyBounds);
					if (xyArea > area)
					{
						area = xyArea;
						bounds = xyBounds;
						origin = xy;
					}
				}
			}
		}

		void showColorPoints_CheckedChanged(object sender, EventArgs e)
		{
			colorPointForm.Visible = showColorPoints.Checked;
		}

		void showChromaMap_CheckedChanged(object sender, EventArgs e)
		{
			Invalidate();
		}
	}

	public delegate void Action();
}