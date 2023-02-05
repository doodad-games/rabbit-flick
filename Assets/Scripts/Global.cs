using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    [RuntimeInitializeOnLoadMethod]
    public static void Init() =>
        Application.targetFrameRate = 60;
}
