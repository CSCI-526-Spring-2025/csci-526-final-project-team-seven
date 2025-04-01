using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCenter
{
    public static Dictionary<string, Color> CardColors = new Dictionary<string, Color>()
    {
        {"Async", new Color(0,0,0) },
        {"Empty", new Color(0,0,0) },
        {"Bullet", new Color(238/255f,255f/255f,52/255f) },
        {"Functional", new Color(22/255f,196/255f,255/255f) },
        {"Health",new Color(1f,0f,0f) }
    };
    public static Dictionary<string, Color> SelectorPanelColors = new Dictionary<string, Color>()
    {
        {"Green",new Color(89/255f,160/255f,49/255f) },
        {"Red",new Color(209/255f,43/255f,28/255f) }
    };
    public static Color platformFadeColor = new Color(1, 1, 1, 0);
}
