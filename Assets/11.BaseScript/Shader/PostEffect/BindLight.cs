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
            Vector2 viewPortPosition = Camera.WorldToViewportPoint(new Vector3(2.25f, 2.5f, 0f));   

            material.SetTexture("_LightTex", m_bulletLightTexture);
            material.SetFloat("_Brightness", m_birghtness);
            material.SetFloat("_EdgeXMax", viewPortPosition.x);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }


}
