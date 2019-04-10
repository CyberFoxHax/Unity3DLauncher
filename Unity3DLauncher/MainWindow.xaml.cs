using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Unity3DLauncher {
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
			Loaded += OnLoaded;
			ListView.SelectionChanged += ListViewOnSelectionChanged;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
			var dir = Environment.GetCommandLineArgs().ElementAtOrDefault(1);
			if (dir == null || Directory.Exists(dir) == false)
				dir = Environment.CurrentDirectory;

			ListView.Items.Clear();
			var sw = new Stopwatch();
			sw.Start();
		    var folders = new UnityFolderFinder(dir);
            ListView.ItemsSource = folders.Select(p => new ProjectItem(p)).OrderByDescending(p=>p.ChangedDate).ToArray();
			sw.Stop();
			TimerTextBlock.Text = folders.IterationsCounter + " folders, " + sw.ElapsedMilliseconds + "ms";

		}

        private void ListViewOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) {
			var item = (ProjectItem)selectionChangedEventArgs.AddedItems[0];
			Process.Start(@"C:\Program Files\Unity\Editor\Unity.exe", "-projectPath \"" + item.FolderPath + "\"");
			Close();
		}
	}
}
