using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iTweenExample
{
    public class MoveAdd : MonoBehaviour
    {
        public string m_itweenName;
        public float m_speed;
        public string m_pathName;
        public iTween.LoopType m_loopType;
        public iTween.EaseType m_easeType;

        private Hashtable _itweenArgs;

        private void Start()
        {
            iTween.Init(this.gameObject);

            _itweenArgs = new Hashtable();
            SetHashArgs(_itweenArgs);

            iTween.MoveTo(this.gameObject, _itweenArgs);
        }

        private void SetHashArgs(Hashtable args)
        {
            args.Add("name", m_itweenName);
            args.Add("path", iTweenPath.GetPath(m_pathName));
            args.Add("speed", m_speed);
            args.Add("looptype", m_loopType);
            args.Add("easetype", m_easeType);

            args.Add("movetopath", false);
        }
    }
}