using System;
using AppKit;
using Foundation;
using xavierHTML.Parsers.HTML;

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
            // Insert code here to initialize your application
            var document = HtmlParser.Parse("<html><body>Hello, world!</body></html>");
            var nodes = document.Children.Count;
            Console.Write($"Nodes: {nodes}");
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
