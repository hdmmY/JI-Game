using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuButtonController : MonoBehaviour
{
    public List<Transform> m_buttons;
                
    private int _curIndex;

    private void Start()
    {
        if (m_buttons == null || m_buttons.Count == 0) return;

        HighLightButton(0);
        for(int i = 1; i < m_buttons.Count; i++)
        {
            HideButton(i);
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            HideButton(_curIndex);
            _curIndex = (_curIndex + 1) % m_buttons.Count;
            HighLightButton(_curIndex);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            HideButton(_curIndex);
            _curIndex = _curIndex - 1 < 0 ? m_buttons.Count - 1 : _curIndex - 1;
            HighLightButton(_curIndex);
        }


        // Hard code  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if(_curIndex == 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Stage1", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else if(_curIndex == 1)
            {
                
            }
            else if(_curIndex == 2)
            {
                Application.Quit();
            }
        }     
    }              

    private void HighLightButton(int index)
    {
        var text = m_buttons[index].GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.fontStyle = TMPro.FontStyles.Bold;

        var image = m_buttons[index].GetComponentInChildren<Image>(true);
        image.gameObject.SetActive(true);
    }

    private void HideButton(int index)
    {
        var text = m_buttons[index].GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.fontStyle = TMPro.FontStyles.Normal;

        var image = m_buttons[index].GetComponentInChildren<Image>(true);
        image.gameObject.SetActive(false);
    }         
}
