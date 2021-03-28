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
using Gavilya.Widget.UserControls;
using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Gavilya.Widget
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Widget1 : Page
	{
		private XboxGameBarWidget widget = null;
		public Widget1()
		{
			this.InitializeComponent();
			InitUI();
		}

		private async void InitUI()
		{
			Windows.Storage.StorageFolder storageFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(Definitions.AppDataPath + @"\Gavilya");
			Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("Games.gav");
			string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GameInfo>));
			using (TextReader reader = new StringReader(text))
			{
				Definitions.Games = (List<GameInfo>)xmlSerializer.Deserialize(reader);
			}

			if (Definitions.Games.Count > 0)
			{
				for (int i = 0; i < Definitions.Games.Count; i++)
				{
					GamePresenter.Children.Add(new GameCard(Definitions.Games[i])); // Display games
				}
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			widget = e.Parameter as XboxGameBarWidget;
			widget.SettingsClicked += Widget_SettingsClicked;
		}

		private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
		{
			await widget.ActivateSettingsAsync();
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ScrollViwr.Height = ContentRow.ActualHeight;
		}
	}
}
