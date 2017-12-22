using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessBlur : MonoBehaviour
{
    public Shader m_guessBlurShader;

    private Material _guessBlurMaterial;
    private Material GuessBlurMaterial
    {
        get
        {
            if(_guessBlurMaterial == null)
            {
                _guessBlurMaterial = new Material(m_guessBlurShader);
            }
            return _guessBlurMaterial;
        }
    }

    // Blur iterations => large number means more blur
    [Range(0, 5)]
    public int m_iterations = 1;

    // Blur speed for each iteration => large number means more blur
    [Range(0.2f, 3.0f)]
    public float m_blueSpeed = 0.6f;

    [Range(1, 8)]
    public int m_downSamples = 1;


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(m_guessBlurShader != null)
        {
            int resWidth = source.width / m_downSamples;
            int resHeight = source.height / m_downSamples;

            RenderTexture buffer0 = RenderTexture.GetTemporary(resWidth, resHeight);
            buffer0.filterMode = FilterMode.Bilinear;

            RenderTexture buffer1 = RenderTexture.GetTemporary(resWidth, resHeight);
            buffer1.filterMode = FilterMode.Bilinear;

            Graphics.Blit(source, buffer0);

            for(int i = 0; i < m_iterations; i++)
            {
                GuessBlurMaterial.SetFloat("_BlurSize", 1.0f + i * m_blueSpeed);

                // Render vertical
                Graphics.Blit(buffer0, buffer1, GuessBlurMaterial, 0);
                // Render horizontal
                Graphics.Blit(buffer1, buffer0, GuessBlurMaterial, 1);
            }

            Graphics.Blit(buffer0, destination);

            RenderTexture.ReleaseTemporary(buffer0);
            RenderTexture.ReleaseTemporary(buffer1);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

}
