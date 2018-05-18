using Sirenix.OdinInspector;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    /// <summary>
    /// Input controller
    /// </summary>
    public IPlayerInputCtrl InputCtrl
    {
        get
        {
            return _inputCtrl ?? (_inputCtrl = new PlayerGeneralInputCtrl ());
        }
        set
        {
            _inputCtrl = value ?? new PlayerGeneralInputCtrl ();
        }
    }

    [ShowInInspector, ReadOnly]
    private IPlayerInputCtrl _inputCtrl;

    private void Update ()
    {
        InputCtrl.Update ();
    }
}