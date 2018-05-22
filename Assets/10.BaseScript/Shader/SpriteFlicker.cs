using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SpriteFlicker : MonoBehaviour {

    public Color m_flickerColor;

    [Range(0, 1)]
    public float m_bindFactor;

    private MaterialPropertyBlock _materialProperty;
    private SpriteRenderer _spriteRender;

    private void Start()
    {
        _spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateShaderProperty();
    }

    void UpdateShaderProperty()
    {
        if(_materialProperty == null)
        {
            _materialProperty = new MaterialPropertyBlock();
        }

        _materialProperty.SetTexture("_MainTex", _spriteRender.sprite.texture);
        _materialProperty.SetColor("_FlickColor", m_flickerColor);
        _materialProperty.SetFloat("_BindFactor", m_bindFactor);

        _spriteRender.SetPropertyBlock(_materialProperty);
    }

}
