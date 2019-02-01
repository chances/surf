using System;
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

        public override void WillFinishLaunching(NSNotification notification)
        {
            _mainWindowController = new MainWindowController();
            _mainWindow = _mainWindowController.Window;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            _mainWindow.MakeKeyAndOrderFront(this);

            var document = HtmlParser.Parse("<html><head><style>body { background-color: #aaa; height:40px; margin: 0 10em; }</style></head><body>Hello, world!</body></html>");
            Console.WriteLine($"# of stylesheets: {document.Stylesheets.Count()}");
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
