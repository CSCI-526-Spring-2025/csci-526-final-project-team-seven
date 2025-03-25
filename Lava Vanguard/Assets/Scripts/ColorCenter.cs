using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCenter
{
    public static Dictionary<string, Color> CardColors = new Dictionary<string, Color>()
    {
        {"Async", new Color(0,0,0) },
        {"Empty", new Color(0,0,0) },
        {"Bullet", new Color(238f/255f,255f/255f,52f/255f) },
        {"Functional", new Color(22f/255f,196f/255f,255f/255f) },
        {"Health",new Color(1f,0f,0f) }
    };
}
