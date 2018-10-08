using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AssetStudio.Utils
{ 
	public struct CopyDataStruct
	{
		public IntPtr dwData;
		public int cbData;

		[MarshalAs(UnmanagedType.LPStr)]
		public string lpData;
	}

	//REF http://www.cnblogs.com/chencong/archive/2011/11/03/2234891.html

	public class WinMsgUtil
	{
		public const int WM_COPYDATA = 0x004A;
		
		[DllImport("User32.dll", EntryPoint = "FindWindow")]
		private static extern int FindWindow(string lpClassName, string lpWindowName);
		
		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage
			(
			int hWnd,   
			int Msg,    
			int wParam, 
			ref CopyDataStruct lParam  
			);
		

		public static bool SendMessage(Process p, string[] data)
		{
			int hWnd = p.MainWindowHandle.ToInt32();
			if (hWnd == 0) return false;

			const int MESSAGE_ID = 8099;
			var strMsg = String.Join("\n", data);

			var copydata = new CopyDataStruct
			{
				dwData = (IntPtr)MESSAGE_ID,
				lpData = strMsg,
				cbData = Encoding.Default.GetBytes(strMsg).Length + 1,
			};

			var result = SendMessage(hWnd, WM_COPYDATA, 0, ref copydata);
			return result == 200;
		}

		public static string[] ParseMessage(ref System.Windows.Forms.Message m)
		{
			CopyDataStruct data = (CopyDataStruct)m.GetLParam(typeof(CopyDataStruct));
			m.Result = (IntPtr)200;
			return data.lpData.Split('\n');
		}
		
	}


}