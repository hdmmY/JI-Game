using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SetSpriteShaderProperty : MonoBehaviour {

    public Texture m_DissolveMap;

    [Range(0, 1f)]
    public float m_DissolveAmount;

    public Color m_EdgeColor;

    [Range(1f, 5)]
    public float m_EdgeWidth;

    private MaterialPropertyBlock materialProperty;
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
        materialProperty = new MaterialPropertyBlock();

        materialProperty.SetTexture("_MainTex", _spriteRender.sprite.texture);
        materialProperty.SetTexture("_DissolveMap", m_DissolveMap);
        materialProperty.SetFloat("_DissolveAmount", m_DissolveAmount);
        materialProperty.SetColor("_EdgeColor", m_EdgeColor);
        materialProperty.SetFloat("_EdgeWidth", m_EdgeWidth);

        GetComponent<SpriteRenderer>().SetPropertyBlock(materialProperty);
    }

}
