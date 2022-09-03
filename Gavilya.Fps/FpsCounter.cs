using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Gavilya.Fps
{
	public partial class FpsCounter : Form
	{
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
		public string positionForm;

		bool drag = false;
		Point startPoint;
		public FpsCounter()
		{
			InitializeComponent();

			FpsLabel.MouseDown += (o, e) =>
			{
				drag = true;
				startPoint = e.Location;
			};
			FpsLabel.MouseUp += (o, e) => drag = false;
			FpsLabel.MouseMove += (o, e) =>
			{
				if (drag)
				{ // if we should be dragging it, we need to figure out some movement
					Point p1 = new Point(e.X, e.Y);
					Point p2 = PointToScreen(p1);
					Point p3 = new Point(p2.X - startPoint.X,
										 p2.Y - startPoint.Y);
					Location = p3;
				}
			};

			BackColor = Color.FromArgb(255, 1, 1, 1);
			TransparencyKey = Color.FromArgb(255, 1, 1, 1);
			//FormClickThrough();
		}

		private void WindowHandle_Tick(object sender, EventArgs e)
		{

			FpsLabel.Text = Program.Fps();
			Size = FpsLabel.Size;
			switch (positionForm)
			{
				case "Top Right":
					if (Location.X + Width != (Screen.PrimaryScreen.Bounds.Width))
					{
						Location = new Point(Screen.PrimaryScreen.Bounds.Width - Size.Width);
					}
					break;
				case "Bottom Right":
					if (Location.X + Width != (Screen.PrimaryScreen.Bounds.Width))
					{
						Location = new Point(Screen.PrimaryScreen.Bounds.Width - Size.Width);
					}
					if (Location.Y + Height != (Screen.PrimaryScreen.Bounds.Height))
					{
						Location = new Point(Screen.PrimaryScreen.Bounds.Width - Size.Width, Screen.PrimaryScreen.Bounds.Height - Size.Height);
					}
					break;
			}

		}

		/// <summary>
		/// Hotkey Registering
		/// </summary>
		/// <param name="key"></param>
		/// <param name="modifier"></param>
		public void GlobalHotKeyRegister(int key, int modifier)
		{
			RegisterHotKey(Handle, 1, modifier, key);
		}

		/// <summary>
		/// Hotkey Unregistering
		/// </summary>
		public void GlobalHotKeyUnRegister()
		{
			UnregisterHotKey(Handle, 1);
		}

		private void FormClickThrough()
		{
			int initialStyle = GetWindowLong(Handle, -20);
			SetWindowLong(Handle, -20, initialStyle | 0x80000 | 0x20);
		}
	}
}
