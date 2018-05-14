using System.Collections.Generic;
using System.IO;

namespace xavierHTML.Parsers
{
    public static class StringExtensions
    {
        // https://stackoverflow.com/a/41176852/1363247
        public static IEnumerable<string> GetLines(this string str, bool removeEmptyLines = false)
        {
            using (var sr = new StringReader(str))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (removeEmptyLines && string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    yield return line;
                }
            }
        }
    }
}
