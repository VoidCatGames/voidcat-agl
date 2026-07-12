using VoidCat.Agl.Core.GraphAlgorithms;

namespace VoidCat.Agl.Layout.Layered {
    /// <summary>
    /// the basis class for layering algorithms
    /// </summary>
    public interface LayerCalculator
    {
		/// <summary>
		/// the main method
		/// </summary>
		int[] GetLayers();
    }
}
