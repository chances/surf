using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using Surf.Rasterization;
using xavierHTML.CSS;
using xavierHTML.CSS.Selectors;
using xavierHTML.CSS.Values;
using xavierHTML.DOM;
using xavierHTML.Layout;
using xavierHTML.Parsers;
using xavierHTML.Parsers.HTML;

namespace Surf.Views
{
    public sealed class WebView : UrlDropView
    {
        private bool _isDirty;
        private readonly string[] _dropTargetFileExtensions = { "htm", "html" };
        private readonly DisplayList _displayList = DisplayList.Empty;
        private Document _document;
        private string _workingDirectory;

        public WebView(CGRect frame) : base(frame)
        {
            WantsLayer = true;
            LayerContentsRedrawPolicy = NSViewLayerContentsRedrawPolicy.OnSetNeedsDisplay;

            ShouldAcceptDropSubject = ShouldAcceptDropFile;
            DragOperationPerformed += OnDragOperationPerformed;
            
            // TODO: Use this.MarkDirty somewhere when the webview needs updating
            // TODO: Something with this.InLiveResize ?
            // TODO:
            // Use this.Display() or this.DisplayIfNeeded() to redraw on demand
        }

        public event EventHandler<string> TitleChanged;

        public Document Document
        {
            get => _document;
            private set
            {
                _document = value;
                UpdateViewport();
                _displayList.Render(_document);
            }
        }

        public void LoadHtml(string htmlSourcePath)
        {
            _workingDirectory = Directory.GetParent(htmlSourcePath).FullName;
            var htmlSource = File.ReadAllText(htmlSourcePath);

            try
            {
                Document = HtmlParser.Parse(htmlSource);
                Document.Stylesheets.Insert(0, UserAgentStylesheet());
                TitleChanged?.Invoke(this, Document.Title?.Trim() ?? "");

                Render();
            }
            catch (ParserException ex)
            {
                var alert = new NSAlert {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = ex.ToString(),
                    MessageText = "Parser Error"
                };
                alert.RunModal();
            }
            catch (Exception ex)
            {
                var alert = new NSAlert {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = ex.Message,
                    MessageText = "Error Loading Document"
                };
                alert.RunModal();
            }
        }

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
            Render();

            base.ResizeWithOldSuperviewSize(oldSize);
        }

        private void LoadExternalAssets()
        {
            // TODO: Load external stylesheets
        }

        private void Render()
        {
            UpdateViewport();
            UpdateLayer(_displayList.Viewport.ToCgRect());
        }

        private void UpdateViewport()
        {
            _displayList.Viewport = new Rectangle(
                (float) Frame.Width, (float) Frame.Height
            );
        }

        private void OnDragOperationPerformed(object sender, string droppedPath)
        {
            if (File.Exists(droppedPath))
            {
                LoadHtml(droppedPath);
                
                // TODO: Put documents in the NSDocumentController.SharedDocumentController
            }
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
            
            _isDirty = true;

            foreach (var layer in Layer.Sublayers?.ToList() ?? new List<CALayer>())
            {
                layer.RemoveFromSuperLayer();
                layer.Dispose();
            }

            foreach (var command in _displayList)
            {
                var shapeBounds = command.OpaqueBounds.ToCgRect();
                var shape = new CAShapeLayer
                {
                    Position = ConvertPointToLayer(new CGPoint(
                        shapeBounds.X + shapeBounds.Width / 2.0,
                        Frame.Height - shapeBounds.Bottom + shapeBounds.Height / 2.0
                    )),
                    Bounds = shapeBounds,
                };

                Console.WriteLine($"{{X: {shapeBounds.X}, Y: {shapeBounds.Y}, Width: {shapeBounds.Width}, Height: {shapeBounds.Height}}}");

                if (command is SolidColor solidShape)
                {
                    shape.BackgroundColor = solidShape.Color;

                    if (solidShape.BorderWidth > 0)
                    {
                        shape.Path = CGPath.FromRect(shapeBounds);
                        shape.StrokeColor = solidShape.BorderColor;
                        shape.LineWidth = solidShape.BorderWidth;
                    }
                }

                Layer.AddSublayer(shape);
            }
        }

        private static Stylesheet UserAgentStylesheet()
        {
            var head = SimpleSelector.FromTagName("head");
            var displayNone = new Declaration(
                "display",
                new List<Value>(new[] {new Keyword("none")})
            );

            var rules = new List<Rule>
            {
                new Rule(
                    new List<Selector>(new []{ head }),
                    new List<Declaration>(new []{ displayNone })
                )
            };

            return new Stylesheet(rules);
        }
    }
}
