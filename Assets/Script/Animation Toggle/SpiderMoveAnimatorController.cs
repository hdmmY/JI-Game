using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationToggle
{
    public class SpiderMoveAnimatorController : MonoBehaviour
    {
        public Animator m_spiderAnimator;

        private Vector3 _prevPosition;

        private void Start()
        {
            _prevPosition = m_spiderAnimator.transform.position;
        }

        private void Update()
        {
            Vector3 curPosition = m_spiderAnimator.transform.position;
            Vector3 deltPosition = curPosition - _prevPosition;
            _prevPosition = curPosition;


            Vector2 velocity;
            if(JITimer.Instance.Pause)
            {
                velocity = Vector2.zero;
            }
            else
            {
                velocity = deltPosition / JITimer.Instance.DeltTime;
                velocity.x = Mathf.Clamp(velocity.x, -1, 1);
                velocity.y = Mathf.Clamp(velocity.y, -1, 1);
            }

            if(Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.y) < 0.1f)
            {
                m_spiderAnimator.SetBool("Moving", false);
            }
            else
            {
                m_spiderAnimator.SetBool("Moving", true);
            }
            Debug.Log(velocity);

            m_spiderAnimator.SetFloat("Horizontal", velocity.x);
            m_spiderAnimator.SetFloat("Vertical", velocity.y);
        }


    }
}



