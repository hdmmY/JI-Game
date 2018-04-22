using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using MovementEffects.Extensions;
using UnityEngine;

public class PlayerHealthMonitor : MonoBehaviour
{
    public Texture PlayerHealth0;

    public Texture PlayerHealth1;

    public Texture PlayerHealth2;

    public Texture PlayerHealth3;

    public Texture PlayerHealth4;

    public Texture PlayerHealth5;

    private PlayerProperty _player;

    private Material _fadeMaterial;

    private void OnEnable ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
        _player.TakeDamage += ChangeHealthUI;

        _fadeMaterial = GetComponent<MeshRenderer> ().material;
    }

    private void OnDisable ()
    {
        _player.TakeDamage -= ChangeHealthUI;
    }

    private void ChangeHealthUI (int curLife, int curHealth)
    {
        _fadeMaterial.SetTexture ("_UpperTex", _fadeMaterial.GetTexture ("_UnderTex"));

        switch (curHealth)
        {
            case 0:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerHealth0);
                break;
            case 1:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerHealth1);
                break;
            case 2:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerHealth2);
                break;
            case 3:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerHealth3);
                break;
            case 4:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerHealth4);
                break;
            case 5:
                _fadeMaterial.SetTexture ("_UnderTex", PlayerHealth5);
                break;
            default:
                return;
        }

        _fadeMaterial.SetFloat ("_Fade", 1);

        var fadeEffect = new Effect<Material, float> ();
        fadeEffect.Duration = 0.8f;
        fadeEffect.OnUpdate = (mat, value) => mat.SetFloat ("_Fade", value);
        fadeEffect.RetrieveStart = (mat, lastValue) => 1;
        fadeEffect.RetrieveEnd = (mat) => 0;
        fadeEffect.CalculatePercentDone = Easing.GetEase (Easing.EaseType.Pow2Out);

        var fadeSeq = new Sequence<Material, float> ();
        fadeSeq.Add (fadeEffect);
        fadeSeq.Reference = _fadeMaterial;

        Movement.Run (fadeSeq);
    }

}