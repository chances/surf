using System.Collections.Generic;

namespace xavierHTML.CSS
{
    public class Stylesheet
    {
        public Stylesheet(List<Rule> rules)
        {
            Rules = rules;
        }

        public List<Rule> Rules { get; }
    }
}
