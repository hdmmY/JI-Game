using UnityEngine;
using System.Collections;

public class UbhBackground : UbhMonoBehaviour
{
    const string TEX_OFFSET_PROPERTY = "_MainTex";
    [SerializeField]
    float _Speed = 0.1f;
    Vector2 _Offset = Vector2.zero;

    void Start ()
    {
        UbhManager manager = FindObjectOfType<UbhManager>();
        if (manager != null && manager._ScaleToFit) {
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1f, 1f));
            Vector2 scale = max * 2f;
            transform.localScale = scale;
        }
    }

    void Update ()
    {
        float y = Mathf.Repeat(Time.time * _Speed, 1f);
        _Offset.x = 0f;
        _Offset.y = y;
        renderer.sharedMaterial.SetTextureOffset(TEX_OFFSET_PROPERTY, _Offset);
    }
}
