using VoidCat.Agl.Core.GraphAlgorithms;

namespace VoidCat.Agl.Layout.Layered {
    internal class RecoveryLayerCalculator : LayerCalculator {
        LayerArrays layers;

        public RecoveryLayerCalculator(LayerArrays recoveredLayerArrays) {
            layers=recoveredLayerArrays;
        }
        public int[] GetLayers() {
            return layers.Y;
        }
    }
}