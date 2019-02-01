using System;
using AppKit;
using CoreGraphics;
using Foundation;
using Surf.Views;

namespace Surf
{
	public class MainWindow : NSWindow
	{
		private const string DefaultTitle = "Surf";

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
			Title = DefaultTitle;
			MinSize = MinimumSize;
			CollectionBehavior = NSWindowCollectionBehavior.FullScreenPrimary;

			AllowsToolTipsWhenApplicationIsInactive = false;
			AutorecalculatesKeyViewLoop = false;
			ReleasedWhenClosed = false;
			AnimationBehavior = NSWindowAnimationBehavior.Default;
			
			ContentView = new NSView (Frame);
			
			// Add a web view to the window
			WebView = new WebView(ContentView.Frame)
			{
				AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable
			};
			WebView.TitleChanged += (_, title) =>
				Title = title.Length > 0
					? $"{title} - {DefaultTitle}"
					: DefaultTitle;

			ContentView.AddSubview(WebView);
		}

		#endregion
		
		public WebView WebView { get; private set; }
	}
}
