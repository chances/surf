using AppKit;
using Foundation;

namespace Surf
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);
        }

        [Export("openDocument:")]
        void OpenDialog(NSObject sender)
        {
            var openPanel = NSOpenPanel.OpenPanel;
            openPanel.ShowsHiddenFiles = false;
            openPanel.CanChooseFiles = true;
            openPanel.CanChooseDirectories = false;
            openPanel.CanCreateDirectories = true;
            openPanel.AllowsMultipleSelection = false;
            openPanel.AllowedFileTypes = new[] { "htm", "html" };

            // TODO: Show open file sheet
            //openPanel.BeginSheet(_mainWindow, _ =>
            //{
            //    var filepath = openPanel.Url?.Path;

            //    if (filepath != null && File.Exists(filepath))
            //        _mainWindow.WebView.LoadHtml(filepath);
            //});
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }
    }
}
