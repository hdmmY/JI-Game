using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JIUI
{
    public class MenuButtonController : MonoBehaviour
    {
        public List<Transform> m_buttons;

        [Space]
        [Header("Option Button")]
        public GuessBlur m_guessBlueEffect;
        public Canvas m_UICanvas;
        public GameObject m_keyboardSprite;

        private bool _showingOption;
        private int _curIndex;

        private void Start()
        {
            if (m_buttons == null || m_buttons.Count == 0) return;

            HighLightButton(0);
            for (int i = 1; i < m_buttons.Count; i++)
            {
                HideButton(i);
            }
        }


        private void Update()
        {
            if (_showingOption)
            {
                if (Input.anyKeyDown)
                {
                    _showingOption = false;
                    m_keyboardSprite.SetActive(false);
                    m_guessBlueEffect.enabled = false;
                    m_UICanvas.enabled = true;
                }

                return;
            }


            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                HideButton(_curIndex);
                _curIndex = (_curIndex + 1) % m_buttons.Count;
                HighLightButton(_curIndex);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                HideButton(_curIndex);
                _curIndex = _curIndex - 1 < 0 ? m_buttons.Count - 1 : _curIndex - 1;
                HighLightButton(_curIndex);
            }


            // Hard code  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  Will be fixed other day
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                if (_curIndex == 0)
                {
                    LoadStage1();
                }
                else if (_curIndex == 1)
                {
                    ShowOptions();
                }
                else if (_curIndex == 2)
                {
                    Exit();
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


        #region Button Event
        private void LoadStage1()
        {
            SceneUtil.LoadSceneAsync("Stage1");
        }

        private void ShowOptions()
        {
            _showingOption = true;

            m_UICanvas.enabled = false;
            m_keyboardSprite.SetActive(true);
            m_guessBlueEffect.enabled = true;
        }

        private void Exit()
        {
            SceneUtil.Exit();
        }
        #endregion
    }
}


