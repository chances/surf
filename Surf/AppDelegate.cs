﻿using System;
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
            Console.WriteLine($"Document text content: {textContent}");
            var document = HtmlParser.Parse("<html><head><style>body { background-color: gray; height:40px; column-span: 4forks; }</style></head><body>Hello, world!</body></html>");
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
