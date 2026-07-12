using System;
using VoidCat.Agl.DebugHelpers;

namespace VoidCat.Agl.Routing.Spline.Bundling {
    /// <summary>
    /// Outputs run time in debug mode 
    /// </summary>
    internal class TimeMeasurer {
        static Timer timer;
        static TimeMeasurer() {
            timer = new Timer();
            timer.Start();
        }


        internal delegate void Task();

        internal static void DebugOutput(string str) {
            timer.Stop();
            System.Diagnostics.Debug.Write("{0}: ", String.Format("{0:0.000}", timer.Duration));
            System.Diagnostics.Debug.WriteLine(str);

        }
    }
}
