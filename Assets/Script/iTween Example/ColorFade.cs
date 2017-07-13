using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iTweenExample
{
    public class ColorFade : MonoBehaviour
    {
        private void OnEnable()
        {
            Hashtable itweenArgs = new Hashtable();
            SetHashArgs(itweenArgs);

            iTween.ColorTo(this.gameObject, itweenArgs);

        }




        private void SetHashArgs(Hashtable args)
        {
            args.Add("amount", 1f);

            args.Add("time", 5f);

            args.Add("easetype", iTween.EaseType.easeInSine);

            args.Add("looptype", iTween.LoopType.pingPong);

            args.Add("color", Color.blue);

            args.Add("NamedColorValue", iTween.NamedValueColor._Diffuse);

            args.Add("includechildren", false);

            //args.Add("onstart", "OnStart");
            //args.Add("onstartparams", this.gameObject.ToString());
            //args.Add("onstarttarget", this.gameObject);

            //args.Add("oncomplete", "OnComplete");
            //args.Add("oncompleteparams", this.gameObject.ToString());
            //args.Add("oncompletetarget", this.gameObject);
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


