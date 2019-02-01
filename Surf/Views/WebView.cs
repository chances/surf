using System;
using System.Linq;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using xavierHTML.DOM;

namespace Surf.Views
{
    public sealed class WebView : UrlDropView
    {
        private bool _isDirty;

        private readonly string[] _dropTargetFileExtensions = { "htm", "html" };

        public WebView(CGRect frame) : base(frame)
        {
            WantsLayer = true;
            LayerContentsRedrawPolicy = NSViewLayerContentsRedrawPolicy.OnSetNeedsDisplay;

            ShouldAcceptDropSubject = ShouldAcceptDropFile;
            DragOperationPerformed += OnDragOperationPerformed;

            UpdateLayer(frame);
            
            // TODO: Use this.MarkDirty somewhere when the webview needs updating
            // TODO: Something with this.InLiveResize ?
            // TODO:
            // Use this.Display() or this.DisplayIfNeeded() to redraw on demand
        }

        public Document Document { get; }

        public override bool WantsUpdateLayer => NeedsDisplay;

        public override void ViewWillDraw()
        {
            NeedsDisplay = _isDirty;
            _isDirty = false;
            
            base.ViewWillDraw();
        }
        
        public override void DraggingEnded(NSDraggingInfo sender)
        {
            Layer.BorderWidth = 0;
            MarkDirty();
        }

        public override void DraggingExited(NSDraggingInfo sender)
        {
            Layer.BorderWidth = 0;
            MarkDirty();
        }

        public override void ResizeWithOldSuperviewSize(CGSize oldSize)
        {
            UpdateLayer(Frame);
            
            base.ResizeWithOldSuperviewSize(oldSize);
        }

        private void OnDragOperationPerformed(object sender, string droppedPath)
        {
            Console.WriteLine($"Dropped a HTML file: {droppedPath}");
        }

        private bool ShouldAcceptDropFile(string path)
        {
            var accept = _dropTargetFileExtensions.Any(path.EndsWith);
            if (accept)
            {
                Layer.BorderWidth = 2;
                Layer.BorderColor = NSColor.SelectedControl.CGColor;
                MarkDirty();
            }

            return accept;
        }

        private void UpdateLayer(CGRect frame)
        {
            var shapeLayer = new CAShapeLayer();
            var path = new CGPath ();
            var radius = Math.Min(
                (frame.Width / 2) - 10,
                (frame.Height / 2) - 10
            );
            path.AddArc (
                0,
                0,
                new nfloat(radius),
                (float)(-(Math.PI / 2)),
                (float)(3 * Math.PI / 2),
                false
            );
            shapeLayer.Path = path;
            shapeLayer.Position = new CGPoint (Bounds.Width / 2, Bounds.Height / 2);
            shapeLayer.FillColor = NSColor.LightGray.CGColor;
            shapeLayer.StrokeColor = NSColor.Blue.CGColor;
            shapeLayer.LineWidth = 2;

            if ((Layer.Sublayers?.Length ?? 0) == 0)
            {
                Layer.AddSublayer(shapeLayer);
            }
            else
            {
                Layer.ReplaceSublayer(Layer.Sublayers.First(), shapeLayer);
            }

            _isDirty = true;
        }
    }
}
