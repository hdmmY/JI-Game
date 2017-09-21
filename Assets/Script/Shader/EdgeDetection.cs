using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetection : PostEffectsBase {

    public Shader m_edgeDetectionShader;

    public RenderTexture m_postRenderTexture;

    private Material _edgeDetectionMaterial = null;
    public Material EdgeDetectionMaterial
    {
        get
        {
            _edgeDetectionMaterial = CheckShaderAndCreateMaterial(m_edgeDetectionShader, _edgeDetectionMaterial);
            return _edgeDetectionMaterial;
        }
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(EdgeDetectionMaterial != null)
        {
            Graphics.Blit(m_postRenderTexture, destination, EdgeDetectionMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

}
