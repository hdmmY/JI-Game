using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLighting : MonoBehaviour
{
    public Shader m_brightnessMaskShader;

    public RenderTexture m_maskTexture;

    [Range(0, 1)]
    public float m_brightnessScale = 1;

    [Range(0, 1)]
    public float m_backgroundFactor = 0.9f;

    private Material _hideLightingMaterial;
    private Material HideLightingMaterial
    {
        get
        {
            if(_hideLightingMaterial == null || _hideLightingMaterial.shader != m_brightnessMaskShader)
            {
                return _hideLightingMaterial = new Material(m_brightnessMaskShader);
            }
            return _hideLightingMaterial;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(m_brightnessMaskShader != null)
        {
            HideLightingMaterial.SetFloat("_BrightnessScale", m_brightnessScale);
            HideLightingMaterial.SetFloat("_BackgroundFactor", m_backgroundFactor);
            HideLightingMaterial.SetTexture("_MaskTex", m_maskTexture);

            Graphics.Blit(source, destination, HideLightingMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
