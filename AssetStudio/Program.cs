using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using AssetStudio.Utils;

namespace AssetStudio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
		static void Main(string[] args)
		{
			//Single Instance
			var curProcess = Process.GetCurrentProcess();
			foreach (var p in Process.GetProcessesByName(curProcess.ProcessName))
			{
				if (p.Id != curProcess.Id)
				{
					if (args != null && args.Length > 0
						&& WinMsgUtil.SendMessage(p, args))
					{
						return;
					}
				}
			}
			

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


			//Load files from commondline
			var form = new AssetStudioForm();
			form.Show();
			if (args != null && args.Length > 0)
			{
				if (Directory.Exists(args[0]))
				{
					form.LoadFolder(args[0]);
				}
				else if (File.Exists(args[0]))
				{
					form.LoadFiles(
						args.Where(x => !String.IsNullOrEmpty(x) && File.Exists(x))
							.ToArray());
				}
			}

			Application.Run(form);
        }
    }
}
