using System;
using AppKit;
using CoreGraphics;
using Foundation;
using Surf.Views;

namespace Surf
{
	public class MainWindow : NSWindow
	{
		public static CGSize MinimumSize = new CGSize(640, 480);
		
		#region Constructors

		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) :
			base(contentRect, aStyle, bufferingType, deferCreation)
		{
			Initialize();
		}

		// Shared initialization code
		private void Initialize ()
		{
			Title = "Surf";
			MinSize = MinimumSize;
			CollectionBehavior = NSWindowCollectionBehavior.FullScreenPrimary;

			AllowsToolTipsWhenApplicationIsInactive = false;
			AutorecalculatesKeyViewLoop = false;
			ReleasedWhenClosed = false;
			AnimationBehavior = NSWindowAnimationBehavior.Default;
			
			ContentView = new NSView (Frame);
			
			// Add a web view to the window
			ContentView.AddSubview(new WebView(ContentView.Frame)
			{
				AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable
			});
		}

		#endregion
	}
}
