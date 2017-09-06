using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletDestoryOutBound : MonoBehaviour
{
    private BulletPool _bulletPool;

    private Vector2 _screenBorder;

    private Camera _mainCamera;

    private void OnEnable()
    {
        _bulletPool = GetComponent<BulletReference>().m_BulletPool;
        _mainCamera = Camera.main;
    }


    private void Update()
    {
        _screenBorder = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        Vector3 position = transform.position;

        if(position.x >= _screenBorder.x || position.x <= -_screenBorder.x)
        {
            DestroyBullet();
            return;
        }


        if(position.y >= _screenBorder.y || position.y <= -_screenBorder.y)
        {
            DestroyBullet();
            return;
        }
    }
   

    private void DestroyBullet()
    {
        _bulletPool.delete(this.gameObject);
    }
}
