using System;

public static class DelegateUtils
{
    public static void ClearDelegate (Delegate del)
    {
        if (del == null) return;

        var cloneDel = del.GetInvocationList ();

        for (int i = 0; i < cloneDel.Length; i++)
        {
            del = System.Delegate.Remove (del, cloneDel[i]);
        }
    }
}