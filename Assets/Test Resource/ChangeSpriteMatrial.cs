using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ChangeSpriteMatrial : MonoBehaviour
{
    public GameObject m_parent;

    public Material m_targetMaterial;

    public Color m_targetColor;

    public float m_targetBindFactor;

    public void ChangeMaterial()
    {
        var sprites = m_parent.GetComponentsInChildren<SpriteRenderer>(true);

        foreach(var sprite in sprites)
        {
            sprite.material = m_targetMaterial;

            var spritFlickScript = sprite.GetComponent<SpriteFlicker>();
            if(spritFlickScript == null)
            {
                spritFlickScript = sprite.gameObject.AddComponent<SpriteFlicker>();
            }
            spritFlickScript.m_bindFactor = m_targetBindFactor;
            spritFlickScript.m_flickerColor = m_targetColor;

            Debug.Log(sprite);
        }
    }
}
