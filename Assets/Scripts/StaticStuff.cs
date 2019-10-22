using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticStuff
{    
    static public void FlowDebug(string s)
    {
        Debug.Log(s);
    }
    static public void FlowDebugWarning(string s)
    {
        Debug.LogWarning(s);
    }
}
