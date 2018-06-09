using xavierHTML.DOM.Elements;

namespace xavierHTML.DOM
{
    public class Document
    {
        private string domain;
        private string referrer;
        private string lastModified;
        private DocumentReadyState readyState = DocumentReadyState.Loading;

        private string title;
        private Element _document;
        private Element body;
        private Element head;

        public Document(Element document)
        {
            _document = document;
        }

        public Element DocumentElement
        {
            get => _document;
            set => _document = value;
        }
    }

    public enum DocumentReadyState
    {
        Loading,
        Interactive,
        Complete
    }
}