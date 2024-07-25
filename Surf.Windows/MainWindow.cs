using System;
using System.Collections;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace Surf.Windows
{
    public partial class MainWindow : Form
    {
        static string[] cultureNames = { "en-US", "fr-FR", "es-ES" };

        public MainWindow()
        {
            var reader = new ResXResourceReader("Resources.resx", new[] { typeof(MainWindow).Assembly.GetName() });
            Console.WriteLine("Resources:");
            foreach (DictionaryEntry d in reader) Console.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());

            var icon = reader.Cast<DictionaryEntry>().FirstOrDefault((DictionaryEntry entry) => entry.Key == "Icon")
                .Value;
            Icon = icon as Icon;

            reader.Close();
        }
    }
}
