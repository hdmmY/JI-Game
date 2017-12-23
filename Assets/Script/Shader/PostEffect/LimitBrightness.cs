using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBrightness : PostEffectsBase
{

    public Shader m_limitBrightnessShader;

    [Range(0, 1f)]
    public float m_threshold;

    private Material _limitBrightnessMaterial;


    public Material material
    {
        get
        {
            _limitBrightnessMaterial = CheckShaderAndCreateMaterial(m_limitBrightnessShader, _limitBrightnessMaterial);
            return _limitBrightnessMaterial;
        }
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetFloat("_Threshold", m_threshold);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
