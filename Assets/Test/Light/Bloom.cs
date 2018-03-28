using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : PostEffectsBase
{
    /// <summary>
    /// Down sample the render texture to reduce draw cost.
    /// </summary>
    public int m_downSamples;
    
    /// <summary>
    /// Blur iteration times for blur render texture.
    /// </summary>
    /// <remarks>
    /// Higher iteration times will make draw cost higher but a good blur effect.
    /// </remarks>
    public int m_blurIteration;
}