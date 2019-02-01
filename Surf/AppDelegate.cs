using System;
using System.IO;
using System.Linq;
using AppKit;
using Foundation;
using xavierHTML.Parsers.HTML;

namespace Surf
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private MainWindow _mainWindow;
        private MainWindowController _mainWindowController;
        
        public AppDelegate()
        {
        }

        public override bool OpenFile(NSApplication sender, string filename)
        {
            return base.OpenFile(sender, filename);
        }

        public override void WillFinishLaunching(NSNotification notification)
        {
            _mainWindowController = new MainWindowController();
            _mainWindow = _mainWindowController.Window;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);
            _mainWindow.MakeKeyAndOrderFront(this);
        }
        
        [Export ("openDocument:")]
        void OpenDialog (NSObject sender)
        {
            var openPanel = NSOpenPanel.OpenPanel;
            openPanel.ShowsHiddenFiles = false;
            openPanel.CanChooseFiles = true;
            openPanel.CanChooseDirectories = false;
            openPanel.CanCreateDirectories = true;
            openPanel.AllowsMultipleSelection = false;
            openPanel.AllowedFileTypes = new[] {"htm", "html"};
            
            openPanel.BeginSheet(_mainWindow, _ =>
            {
                var filepath = openPanel.Url?.Path;

                if (filepath != null && File.Exists(filepath))
                    _mainWindow.WebView.LoadHtml(File.ReadAllText(filepath));
            });

//            if (openPanel.RunModal() != (long) NSModalResponse.OK) return;
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
