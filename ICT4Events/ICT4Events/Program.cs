using System;
using System.Windows.Forms;
using ICT4Events;

namespace ICT4Events
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ICT4EventsForm());
        }
    }
}