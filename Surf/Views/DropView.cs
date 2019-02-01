using System;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Surf.Views
{
    public class UrlDropView : NSView
    {
        protected UrlDropView(CGRect frameRect) : base(frameRect)
        {
            // Register as a drop target for URLs and files
            RegisterForDraggedTypes(new string[]
            {
                NSPasteboard.NSPasteboardTypeUrl,
                NSPasteboard.NSPasteboardTypeFileUrl
            });
        }

        public event EventHandler<string> DragOperationPerformed;

        protected Func<string, bool> ShouldAcceptDropSubject { private get; set; }

        public override NSDragOperation DraggingEntered(NSDraggingInfo sender)
        {
            if (ShouldAcceptDropSubject == null) return base.DraggingEntered(sender);

            try
            {
                var path = GetFilePathFromPasteboard(sender.DraggingPasteboard);

                return path != null && ShouldAcceptDropSubject(path)
                    ? NSDragOperation.Copy
                    : base.DraggingEntered(sender);
            }
            catch
            {
                // ignored
            }

            return base.DraggingEntered(sender);
        }

        public override bool PrepareForDragOperation(NSDraggingInfo sender)
        {
            if (ShouldAcceptDropSubject == null) return base.PrepareForDragOperation(sender);

            try
            {
                var path = GetFilePathFromPasteboard(sender.DraggingPasteboard);
                if (path != null && ShouldAcceptDropSubject(path))
                    return true;
            }
            catch
            {
                // ignored
            }

            return base.PrepareForDragOperation(sender);
        }

        public override bool PerformDragOperation(NSDraggingInfo sender)
        {
            try
            {
                var path = GetFilePathFromPasteboard(sender.DraggingPasteboard);

                if (path != null && DragOperationPerformed != null)
                {
                    DragOperationPerformed?.Invoke(this, path);
                    return true;
                }
            }
            catch
            {
                // ignored
            }

            return base.PerformDragOperation(sender);
        }

        private static string GetFilePathFromPasteboard(NSPasteboard draggingPasteboard)
        {
            var filenames = NSArray.FromArrayNative<NSString>(draggingPasteboard
                .GetPropertyListForType(NSPasteboard.NSFilenamesType) as NSArray);
            return filenames.FirstOrDefault();
        }
    }
}
