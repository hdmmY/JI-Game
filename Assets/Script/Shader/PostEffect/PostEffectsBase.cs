using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
    private Camera _camera;
    protected Camera camera
    {
        get
        {
            if(_camera == null)
            {
                _camera = GetComponent<Camera>();
            }

            return _camera;
        }
    }

    private void Start()
    {
        CheckResources();
    }

    protected void CheckResources()
    {
        bool isSupported = CheckSupport();

        if (!isSupported)
        {
            NotSupport();
        }
    }   

    protected bool CheckSupport()
    {
        if (!SystemInfo.supportsImageEffects )
        {
            Debug.LogWarning("This platform do not support image effects.");
            return false;
        }

        return true;
    } 

    protected void NotSupport()
    {
        enabled = false;
    }      

    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if(shader == null)
        {
            return null;
        }

        if(shader.isSupported && material && material.shader == shader)
        {
            return material;
        }

        if(!shader.isSupported)
        {
            return null;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;

            if (material)
                return material;

            return null;               
        }
    }

}
