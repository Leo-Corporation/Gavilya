/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
using Gavilya.Widget.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Gavilya.Widget.UserControls
{
	public sealed partial class GameCard : UserControl
	{
		private GameInfo GameInfo { get; set; }
		public GameCard(GameInfo gameInfo)
		{
			this.InitializeComponent();
			GameInfo = gameInfo;
			InitUI();
		}

		private void InitUI()
		{
			GameNameTxt.Text = GameInfo.Name; // Set name
		}

		/// <summary>
		/// Launch a process
		/// </summary>
		/// <param name="processName"></param>
		private async void LaunchProcess(string processName)
		{
			if (!File.Exists(Definitions.AppDataPath + @"\Gavilya\LatestGameOpened.txt"))
			{
				// Create sample file; replace if exists.
				StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Definitions.AppDataPath + @"\Gavilya");
				await storageFolder.CreateFileAsync("LatestGameOpened.txt", CreationCollisionOption.ReplaceExisting); 
			}

			StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Definitions.AppDataPath + @"\Gavilya"); // Get the folder
			StorageFile file = await folder.GetFileAsync("LatestGameOpened.txt"); // Get the file object
			await FileIO.WriteTextAsync(file, processName); // Write the path to the latest game opened

			await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync(); // Launch Gavilya.AppLauncher.exe
		}

		private async void PlayBtn_Click(object sender, RoutedEventArgs e)
		{
			LaunchProcess(GameInfo.FileLocation); // Launch
		}
	}
}
