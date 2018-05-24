using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SpriteFade : MonoBehaviour
{ 
    [Range(0, 1)]
    public float m_fadeFactor;

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
        if (_materialProperty == null)
        {
            _materialProperty = new MaterialPropertyBlock();
        }

        _materialProperty.SetTexture("_MainTex", _spriteRender.sprite.texture);
        _materialProperty.SetFloat("_FadeFactor", m_fadeFactor);

        _spriteRender.SetPropertyBlock(_materialProperty);
    }

}
