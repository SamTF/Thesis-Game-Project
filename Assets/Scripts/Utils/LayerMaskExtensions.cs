using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extra Helper methods to use with LayerMasks
/// </summary>
public static class LayerMaskExtensions
{
    /// <summary>
    /// Checks if a given Layer int exists in the layer mask.
    /// </summary>
    /// <param name="layerMask">LayerMask value to analyse.</param>
    /// <param name="layer">Layer ID to look for.</param>
    /// <returns></returns>
    public static bool HasLayer(this LayerMask layerMask, int layer)
    {
        if (layerMask == (layerMask | (1 << layer))) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Extracts all layer IDs found in a LayerMask
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns>List of Layer IDs</returns>
    public static int[] Layers(this LayerMask layerMask)
    {
        var hasLayers = new bool[32];
        List<int> layers = new List<int>();

        for (int i = 0; i < 32; i++)
        {
            if (layerMask == (layerMask | (1 << i))) {
                hasLayers[i] = true;
                layers.Add(i);
            }
        }

        return layers.ToArray();
    }

    public static int? FirstLayer(LayerMask layerMask) {
        int[] layers = Layers(layerMask);

        if (layers.Length > 0)
            return layers[0];
        else
            return null;
    }
}
