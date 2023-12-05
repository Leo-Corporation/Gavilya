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

using System.Windows.Input;
using System.Windows;

namespace Gavilya.Commands
{
	public static class DropBehavior
	{
		/// <summary>
		/// The Dependency property. To allow for Binding, a dependency
		/// property must be used.
		/// </summary>
		private static readonly DependencyProperty PreviewDropCommandProperty =
			DependencyProperty.RegisterAttached
			(
				"PreviewDropCommand",
				typeof(ICommand),
				typeof(DropBehavior),
				new PropertyMetadata(PreviewDropCommandPropertyChangedCallBack)
			);

		public static void SetPreviewDropCommand(this UIElement inUIElement, ICommand inCommand)
		{
			inUIElement.SetValue(PreviewDropCommandProperty, inCommand);
		}

		private static ICommand GetPreviewDropCommand(UIElement inUIElement)
		{
			return (ICommand)inUIElement.GetValue(PreviewDropCommandProperty);
		}

		private static void PreviewDropCommandPropertyChangedCallBack(DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
		{
			UIElement uiElement = inDependencyObject as UIElement;
			if (null == uiElement) return;

			uiElement.Drop += (sender, args) =>
			{
				GetPreviewDropCommand(uiElement).Execute(args.Data);
				args.Handled = true;
			};
		}
	}
}
