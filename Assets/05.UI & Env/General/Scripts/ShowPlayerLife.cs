using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerLife : MonoBehaviour
{
    public GameObject[] HealthSprites;

    private PlayerProperty _player;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
    }

    private void Update ()
    {
        int life = Mathf.Clamp (_player.m_playerLife, 0, 5);

        for (int i = 0; i < 5; i++)
        {
            if (i < life)
            {
                HealthSprites[i].SetActive (true);
            }
            else
            {
                HealthSprites[i].SetActive (false);
            }
        }
    }
}