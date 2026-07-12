using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace VoidCat.Agl.DebugHelpers{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"), Serializable]
    public class DebugCurveCollection{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="debugCurves"></param>
        public DebugCurveCollection(IEnumerable<DebugCurve> debugCurves){
            DebugCurvesArray = debugCurves.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public DebugCurve[] DebugCurvesArray;

        ///<summary>
        ///</summary>
        ///<param name="debugCurves"></param>
        ///<param name="fileName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static void WriteToFile(IEnumerable<DebugCurve> debugCurves, string fileName) {
            // VoidCat fork: System.Text.Json dependency removed — debug-curve dumps
            // are disabled (Unity editor has no System.Text.Json by default).
            try
            {
                System.Diagnostics.Debug.WriteLine($"DebugCurveCollection.WriteToFile disabled in VoidCat fork ({fileName}).");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="fileName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static IEnumerable<DebugCurve> ReadFromFile(string fileName) {
            try
            {
                System.Diagnostics.Debug.WriteLine($"DebugCurveCollection.ReadFromFile disabled in VoidCat fork ({fileName}).");
                return new List<DebugCurve>();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return new List<DebugCurve>();
            }
        }
    }
}
