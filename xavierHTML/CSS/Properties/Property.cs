using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS.Values;

namespace xavierHTML.CSS.Properties
{
    public static class Property
    {
        public static Value GetValue(IReadOnlyDictionary<string, List<Value>> specifiedValues, string name) =>
            specifiedValues.ContainsKey(name)
                ? specifiedValues[name]?.FirstOrDefault()
                : null;

        public static List<Value> GetValues(IReadOnlyDictionary<string, List<Value>> specifiedValues, string key)
        {
            return specifiedValues.ContainsKey(key) ? specifiedValues[key] : null;
        }
    }
}
