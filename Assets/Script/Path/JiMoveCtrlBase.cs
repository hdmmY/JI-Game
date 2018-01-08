using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class JiMoveCtrlBase : MonoBehaviour
{
    // GameObject that move on the path
    [SceneObjectsOnly]
    [InlineEditor(InlineEditorModes.LargePreview)]
    public GameObject m_targetGameObject;

    // Destroy the m_targetGameObject when at the end of the last m_Paths
    public bool m_distroyWhenEndOfPaths = true;

    public virtual void Start()
    {
        if (m_targetGameObject == null) return;
    }

    private void OnDisable()
    {
        if(m_targetGameObject != null)
        {
            StopMove();
        }
    }

    private void OnDestroy()
    {
        if(m_targetGameObject != null)
        {
            StopMove();
        }
    }

    // Stop the target move
    public abstract void StopMove();

    // Lauch iTween args
    public abstract Hashtable LauchArgs(int index);

    // Start move
    public void StartMove(int index)
    {
        iTween.MoveTo(m_targetGameObject, LauchArgs(index));
    }

}
