using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using MovementEffects.Extensions;
using UnityEngine;

public class PlayerLifeMonitor : MonoBehaviour
{
    public Texture PlayerLife0;

    public Texture PlayerLife1;

    public Texture PlayerLife2;

    public Texture PlayerLife3;

    public Texture PlayerLife4;

    public Texture PlayerLife5;

    private PlayerProperty _player;

    private Material _fadeMaterial;

    private void OnEnable ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
        _player.TakeDamage += ChangeLifeUI;

        _fadeMaterial = GetComponent<MeshRenderer> ().material;
    }

    private void OnDisable ()
    {
        _player.TakeDamage -= ChangeLifeUI;
    }

    private void ChangeLifeUI (int curLife, int curHealth)
    {
        _fadeMaterial.SetTexture ("_UpperTex", _fadeMaterial.GetTexture ("_UnderTex"));

        switch (curLife)
        {
            case 0:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerLife0);
                break;
            case 1:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerLife1);
                break;
            case 2:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerLife2);
                break;
            case 3:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerLife3);
                break;
            case 4:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerLife4);
                break;
            case 5:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerLife5);
                break;
            default:
                return;
        }

        _fadeMaterial.SetFloat ("_Fade", 1);

        var fadeEffect = new Effect<Material, float> ();
        fadeEffect.Duration = 1f;
        fadeEffect.OnUpdate = (mat, value) => mat.SetFloat ("_Fade", value);
        fadeEffect.RetrieveStart = (mat, lastValue) => 1;
        fadeEffect.RetrieveEnd = (mat) => 0;
        fadeEffect.CalculatePercentDone = Easing.Pow2Out;

        var fadeSeq = new Sequence<Material, float> ();
        fadeSeq.Add (fadeEffect);
        fadeSeq.Reference = _fadeMaterial;

        Movement.Run (fadeSeq);
    }

}