using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace VFrame_Editor
{
	class Config
	{
		private string filePath;
		private string exe = Assembly.GetExecutingAssembly().GetName().Name;

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(
			string section,
			string key,
			string val,
			string filePath
			);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(
			string section,
			string key,
			string def,
			StringBuilder retVal,
			int size,
			string filePath
			);

		public Config()
		{
			filePath = new FileInfo(exe + ".ini").FullName;
		}
		public Config(string filePath)
		{
			this.filePath = filePath;
		}

		public void Write(string key, string val, string section = null)
		{
			WritePrivateProfileString(section ?? exe, key, val, filePath);
		}
		public string Read(string key, string section = null)
		{
			StringBuilder SB = new StringBuilder(255);
			int i = GetPrivateProfileString(section ?? exe, key, "", SB, 255, filePath);
			return SB.ToString();
		}
		public string FilePath
		{
			get => filePath;
			set => filePath = value;
		}
	}
}
