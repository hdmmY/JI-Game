namespace DanmakU.Fireables
{
    [System.Serializable]
    public class Fireable : UnityEngine.ScriptableObject, IFireable
    {

        public IFireable Child { get; set; }

        public virtual void Fire (DanmakuConfig state)
        {

        }

        protected void Subfire (DanmakuConfig state)
        {
            if (Child == null)
                return;
            Child.Fire (state);
        }

    }
}