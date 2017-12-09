using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveEffect : PostEffectsBase
{

    public Shader m_ShockWaveShader;

    public Camera m_mainCamera;

    [Range(0, 1)]
    public float m_maxRadius;

    [Range(0, 1)]
    public float m_startWidth;

    [Range(0, 1)]
    public float m_endWidth;

    [Range(0.1f, 5f)]
    public float m_totalTime;

    [Range(0, 1)]
    public float m_timeScaleWhenSlowDown;

    private float _curRadius = 0;
    private float _curWidth = 0.0001f;
    private float _timer = 0;


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
        if (material != null)
        {
            material.SetFloat("_Radius", _curRadius);
            material.SetFloat("_Width", _curWidth);
            material.SetFloat("_Aspect", m_mainCamera.aspect);

            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseScreenPos = m_mainCamera.ScreenToViewportPoint(Input.mousePosition);
            material.SetVector("_Centre", mouseScreenPos);

            ResetMaterial();
            //StopCoroutine(StartShockWave());
            StartCoroutine(StartShockWave());
        }
    }


    private void ResetMaterial()
    {
        _curRadius = 0;
    }

    private void SlowDownTime()
    {
        JITimer.Instance.TimeScale = m_timeScaleWhenSlowDown;
    }

    private void ResetTimeScale()
    {
        JITimer.Instance.TimeScale = 1;
    }

    IEnumerator StartShockWave()
    {
        SlowDownTime();

        _timer = 0;
        _curWidth = m_startWidth;

        float deltWidth = (m_endWidth - m_startWidth) / m_totalTime;
        float deltRadius = m_maxRadius / m_totalTime;

        Debug.Log(deltWidth);
        Debug.Log(deltRadius);

        while (_timer < m_totalTime)
        {
            _timer += JITimer.Instance.RealDeltTime;

            _curRadius += deltRadius * JITimer.Instance.RealDeltTime;
            _curWidth += deltWidth * JITimer.Instance.RealDeltTime;

            yield return null;
        }

        ResetTimeScale();
    }

}
