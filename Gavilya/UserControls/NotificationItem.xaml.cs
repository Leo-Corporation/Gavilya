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

using Gavilya.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gavilya.UserControls;
/// <summary>
/// Interaction logic for NotificationItem.xaml
/// </summary>
public partial class NotificationItem : UserControl
{
	internal string Title { get; init; }
	internal string Message { get; init; }
	internal string Icon { get; init; }
	internal string OkText { get; init; }
	internal string CancelText { get; init; }
	public NotificationItem(string title, string msg, string icon, string okText, string cancelText, Action okAction, Action closeAction)
	{
		InitializeComponent();
		Title = title; // Set property
		Message = msg; // Set property
		Icon = icon; // Set property
		OkText = okText; // Set property
		CancelText = cancelText; // Set property

		// Actions
		OKBtn.Click += (o, e) => okAction();
		if (closeAction is not null)
		{
			CancelBtn.Click += (o, e) => closeAction();
		}
		else
		{
			CancelBtn.Click += (o, e) => CloseBtn_Click(o, e);
		}

		Definitions.MainWindow.BadgeTxt.Visibility = Visibility.Visible; // Show
		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		// Text
		TitleTxt.Text = Title; // Set text
		DescTxt.Text = Message; // Set text
		IconTxt.Text = Icon; // Set text

		// Buttons
		OKBtn.Content = OkText; // Set the content of the button
		CancelBtn.Content = CancelText; // Set the content of the button
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Definitions.MainWindow.NotificationPanel.Children.Remove(this); // Remove the notification
		Definitions.MainWindow.NotificationStatusTxt.Text = 
			$"{Properties.Resources.YouHave} {Definitions.MainWindow.NotificationPanel.Children.Count - 1} " +
			$"{((Definitions.MainWindow.NotificationPanel.Children.Count - 1 > 1) ? Properties.Resources.NotificationsLower : Properties.Resources.NotificationLower)}";
		// Refresh

		if (Definitions.MainWindow.NotificationPanel.Children.Count - 1 < 1)
		{
			Definitions.MainWindow.NotificationPlaceholder.Visibility = Visibility.Visible; // Show
			Definitions.MainWindow.BadgeTxt.Visibility = Visibility.Hidden; // Hide
		}
		else
		{
			Definitions.MainWindow.NotificationPlaceholder.Visibility = Visibility.Collapsed; // Hide
		}
	}
}
