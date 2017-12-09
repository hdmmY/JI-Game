using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour
{
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
            Debug.Log(0);
            return null;
        }

        if(shader.isSupported && material && material.shader == shader)
        {
            Debug.Log(123);
            return material;
        }

        if(!shader.isSupported)
        {
            Debug.Log(456);
            return null;
        }
        else
        {
            Debug.Log(789);
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;

            if (material)
                return material;

            return null;               
        }
    }

}
