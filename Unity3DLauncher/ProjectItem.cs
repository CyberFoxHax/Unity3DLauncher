using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3DLauncher {
	public class ProjectItem {
		public ProjectItem() { }
		public ProjectItem(string folderPath) {
			FolderPath = folderPath;

			ProjectName = Path.GetFileName(Path.GetDirectoryName(folderPath.EndsWith("\\")? FolderPath : FolderPath+"\\"));

			CreatedDate = Directory.GetCreationTime(FolderPath);
			ChangedDate = Directory.GetLastWriteTime(FolderPath);
		}
		public string FolderPath { get; set; }

		public string ProjectName { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ChangedDate { get; set; }

	    public bool IsTag => FolderPath.ToLower().Contains("\\tags\\");

	    public string CreatedDateString => CreatedDate.ToString("dd-MM-yyyy hh:mm");
		public string ChangedDateString => ChangedDate.ToString("dd-MM-yyyy hh:mm");
	}
}
