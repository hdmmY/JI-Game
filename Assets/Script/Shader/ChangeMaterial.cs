    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class ChangeMaterial : MonoBehaviour {

    public Camera m_Camera;

    public Material m_OutlineMaterial;
    public Material m_OriginMaterial;

    private Material _originMaterial;
    private SpriteRenderer _spriteRender;

    private void Start()
    {
        _spriteRender = GetComponent<SpriteRenderer>();

        Camera.onPreRender += ChangeToNewMaterial;
        Camera.onPostRender += ChangeToOriginMaterial;
    }


    private void ChangeToNewMaterial(Camera cam)
    {
        if(cam == m_Camera)
            _spriteRender.material = m_OutlineMaterial;
    }

    private void ChangeToOriginMaterial(Camera cam)
    {
        if(cam == m_Camera)
            _spriteRender.material = m_OriginMaterial;
    }

}
