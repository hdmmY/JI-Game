using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindLight : PostEffectsBase
{
    public Shader m_bindLightShader;

    [Range(1, 10)]
    public float m_birghtness;
    public RenderTexture m_bulletLightTexture;

    private Material _lightBindMaterial;

    public Material material
    {
        get
        {
            _lightBindMaterial = CheckShaderAndCreateMaterial(m_bindLightShader, _lightBindMaterial);
            return _lightBindMaterial;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetTexture("_LightTex", m_bulletLightTexture);
            material.SetFloat("_Brightness", m_birghtness);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }


}
