using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace trainer
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}