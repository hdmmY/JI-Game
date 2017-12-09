using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveEffect : PostEffectsBase {

    public Shader m_ShockWaveShader;

    public Camera m_mainCamera;

    [Range(0, 1)]
    public float m_radius;

    [Range(0, 1)]
    public float m_width;

    private Material _shockWaveMaterial;
    public Material material
    {
        get
        {
            _shockWaveMaterial = CheckShaderAndCreateMaterial(m_ShockWaveShader, _shockWaveMaterial);
            return _shockWaveMaterial;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {          
        if (material != null/* && Input.GetMouseButtonDown(0)*/)
        {
            Debug.Log("asdfasdf");

            Vector2 mouseScreenPos = m_mainCamera.ScreenToViewportPoint(Input.mousePosition);

            material.SetVector("_Centre", new Vector4(0.5f, 0.5f, 0, 0));
            material.SetFloat("_Radius", m_radius);
            material.SetFloat("_Width", m_width);
            material.SetFloat("_Aspect", m_mainCamera.aspect);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

}
