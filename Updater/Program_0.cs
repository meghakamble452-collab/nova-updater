using System;
using System.Windows.Forms;

namespace Updater;

internal static class Program_0
{
	[STAThread]
	private static void Main(string[] args)
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new Update());
	}
}
