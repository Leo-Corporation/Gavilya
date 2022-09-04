using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gavilya.Fps
{
	class Program
	{
		internal static double Opacity { get; set; } = 1;
		//event codes (https://github.com/GameTechDev/PresentMon/blob/40ee99f437bc1061a27a2fc16a8993ee8ce4ebb5/PresentData/PresentMonTraceConsumer.cpp)
		public const int EventID_D3D9PresentStart = 1;
		public const int EventID_DxgiPresentStart = 42;

		//ETW provider codes
		public static readonly Guid DXGI_provider = Guid.Parse("{CA11C036-0102-4A2D-A6AD-F03CFED5D3C9}");
		public static readonly Guid D3D9_provider = Guid.Parse("{783ACA0A-790E-4D7F-8451-AA850511C6B9}");

		static TraceEventSession m_EtwSession;
		public static Dictionary<int, TimestampCollection> frames = new Dictionary<int, TimestampCollection>();
		private static Queue<int> avgFpsQueue = new Queue<int>();
		static Stopwatch watch = null;
		static object sync = new object();
		public static bool avgFpsCheck = true;
		public static bool avgFpsQueueUnlimited = true;
		public static int avgFpsQueueLimit = 21;

		static void EtwThreadProc()
		{
			//start tracing
			m_EtwSession.Source.Process();
		}

		static void OutputThreadProc()
		{
			while (true)
			{
				double to, from;
				lock (sync)
				{
					to = watch.Elapsed.TotalMilliseconds;
					from = to - 1000;// 1 Seconds
					foreach (var x in frames.Values)
					{
						if (x.Name == GetWindowDetails().ToString())
						{
							//get the number of frames
							int count = x.QueryCount(from, to);
							if (avgFpsCheck)
							{
								if (avgFpsQueueUnlimited)
								{
									avgFpsQueue.Enqueue(count);
								}
								else if (avgFpsQueue.Count <= avgFpsQueueLimit)
								{
									avgFpsQueue.Enqueue(count);
								}
								else
								{
									for (int i = 0; i < avgFpsQueue.Count - avgFpsQueueLimit; i++)
									{
										avgFpsQueue.Dequeue();
									}
									avgFpsQueue.Enqueue(count);
								}
							}
							else
							{
								fpsCalculate = count;
							}
						}
					}
					if (avgFpsCheck)
					{
						fpsCalculate = avgFpsQueue.Count > 0 ? avgFpsQueue.Average() : 0.0;
					}
				}
				Thread.Sleep(1000);
			}
		}

		[STAThread]
		public static void Main(string[] argv)
		{
			try
			{
				if (argv != null && argv.Length >= 1)
				{
					Opacity = double.Parse(argv[0]);
				}

				//create ETW session and register providers
				m_EtwSession = new TraceEventSession("mysess");
				m_EtwSession.StopOnDispose = true;
				m_EtwSession.EnableProvider("Microsoft-Windows-D3D9");
				m_EtwSession.EnableProvider("Microsoft-Windows-DXGI");

				//handle event
				m_EtwSession.Source.AllEvents += data =>
				{
					// filter out frame presentation events
					if (((int)data.ID == EventID_D3D9PresentStart && data.ProviderGuid == D3D9_provider) ||
					((int)data.ID == EventID_DxgiPresentStart && data.ProviderGuid == DXGI_provider))
					{
						int pid = data.ProcessID;
						double t;

						t = watch.Elapsed.TotalMilliseconds;
						lock (sync)
						{
							//if process is not yet in Dictionary, add it
							if (!frames.ContainsKey(pid))
							{
								frames[pid] = new TimestampCollection();

								string name = "";
								var proc = Process.GetProcessById(pid);
								if (proc != null)
								{
									using (proc)
									{
										name = proc.ProcessName;
									}
								}
								else name = pid.ToString();
								frames[pid].Name = pid.ToString();
							}
						}
						//store frame timestamp in collection
						frames[pid].Add(t);

					}
				};

				watch = new Stopwatch();
				watch.Start();

				Thread thETW = new Thread(EtwThreadProc);
				thETW.IsBackground = true;
				thETW.SetApartmentState(ApartmentState.STA);
				thETW.Start();

				Thread thOutput = new Thread(OutputThreadProc);
				thOutput.IsBackground = true;
				thOutput.SetApartmentState(ApartmentState.STA);
				thOutput.Start();
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FpsCounter());
				m_EtwSession.Dispose();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
			}
		}

		[DllImport("user32.dll")]
		static extern IntPtr GetForegroundWindow();

		[DllImport("User32.dll")]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		public static uint GetWindowDetails()
		{
			uint processId = 0;
			const int intCharCount = 256;

			IntPtr intWindowHandle = IntPtr.Zero; //Window Handle

			StringBuilder strWindowText = new StringBuilder(intCharCount);

			intWindowHandle = GetForegroundWindow(); //get Current Active Window 
			GetWindowThreadProcessId(intWindowHandle, out processId);
			return processId;
		}

		static double fpsCalculate = 0;
		public static string Fps()
		{
			return Math.Round(fpsCalculate).ToString(); // Rounds the Fps
		}
	}
}
