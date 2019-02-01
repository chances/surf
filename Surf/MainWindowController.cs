using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Surf
{
	public class MainWindowController : NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}

		// Shared initialization code
		private void Initialize ()
		{
			var contentRect = new CGRect (0, 0, MainWindow.MinimumSize.Width, MainWindow.MinimumSize.Height);
			base.Window = new MainWindow(
				contentRect,
				NSWindowStyle.Titled |
				NSWindowStyle.Closable |
				NSWindowStyle.Miniaturizable |
				NSWindowStyle.Resizable,
				NSBackingStore.Buffered,
				false
			);

			// Center window on screen, inside screen's visible frame
			var screen = Window.Screen.Frame;
			var visibleFrame = Window.Screen.VisibleFrame;
			var x = (screen.Width / 2.0) - (Window.Frame.Width / 2.0);
			var y = (screen.Height / 2.0) - (Window.Frame.Height / 2.0);
			y += (screen.Height - visibleFrame.Height) / 2.0;
			contentRect = new CGRect(x, y, MainWindow.MinimumSize.Width, MainWindow.MinimumSize.Height);
			Window.SetFrame(contentRect, true);;

			// Simulate Awaking from Nib
			Window.AwakeFromNib();
		}

		#endregion

		public new MainWindow Window => (MainWindow)base.Window;
	}
}
