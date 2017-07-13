using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iTweenExample
{
    public class CameraFade : MonoBehaviour
    {

        public Texture2D m_texture;


        void Start()
        {

            Texture2D texture = iTween.CameraTexture(Color.blue);

            iTween.CameraFadeAdd(texture);

            Hashtable args = new Hashtable();
            SetHashArgs(args);

            iTween.CameraFadeTo(args);

        }


        private void SetHashArgs(Hashtable args)
        {
            args.Add("amount", 1f);

            args.Add("time", 10f);

            args.Add("easetype", iTween.EaseType.easeInSine);

            args.Add("looptype", iTween.LoopType.pingPong);

            args.Add("onstart", "OnStart");
            args.Add("onstartparams", this.gameObject.ToString());
            args.Add("onstarttarget", this.gameObject);

            args.Add("oncomplete", "OnComplete");
            args.Add("oncompleteparams", this.gameObject.ToString());
            args.Add("oncompletetarget", this.gameObject);
        }


        private void OnStart(string name)
        {
            Debug.Log("start: " + name);
        }

        private void OnComplete(string name)
        {
            Debug.Log("end: " + name);
        }


    }
}


