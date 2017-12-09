using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessSaturationAndContrast : PostEffectsBase {

    public Shader m_BriSatConShader;

    [Range(0, 3f)]
    public float m_brightness;

    [Range(0, 3f)]
    public float m_saturation;

    [Range(0, 3f)]
    public float m_contrast;

    private Material _briSatConMaterial;


    public Material material
    {
        get
        {
            _briSatConMaterial = CheckShaderAndCreateMaterial(m_BriSatConShader, _briSatConMaterial);
            return _briSatConMaterial;
        }
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material != null)
        {
            material.SetFloat("_Brightness", m_brightness);
            material.SetFloat("_Saturation", m_saturation);
            material.SetFloat("_Contrast", m_contrast);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
