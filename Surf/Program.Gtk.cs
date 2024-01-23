namespace Surf
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            Gtk.Module.Initialize();

            ﻿var application = Adw.Application.New("me.chancesnow.surf", Gio.ApplicationFlags.FlagsNone);
            application.OnActivate += (sender, args) =>
            {
                var window = Gtk.ApplicationWindow.New((Adw.Application) sender);
                window.Title = "Surf";
                window.SetDefaultSize(300, 300);
                window.SetChild(Gtk.Label.New("Hello world"));
                window.Show();
            };

            return application.RunWithSynchronizationContext(null);
        }
    }
}
