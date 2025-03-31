using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCenter
{
    public static Dictionary<string, Color> CardColors = new Dictionary<string, Color>()
    {
        {"Async", new Color(0,0,0) },
        {"Empty", new Color(0,0,0) },
        {"Functional", new Color(22/255f,196/255f,255/255f) },
        {"Health",new Color(1f,0f,0f) },
        
        {"Bullet1", new Color(1f, 1f, 1f)},
        {"Bullet2", new Color(0.9f, 0.9f, 1f)},
        {"Bullet3", new Color(0.8f, 0.8f, 1f)},
        {"Bullet4", new Color(0.7f, 0.6f, 1f)},
        {"Bullet5", new Color(0.6f, 0.4f, 1f)},
        {"Bullet6", new Color(0.5f, 0.3f, 1f)},
        {"Bullet7", new Color(0.6f, 0.2f, 0.9f)},
        {"Bullet8", new Color(0.8f, 0.3f, 0.6f)},
        {"Bullet9", new Color(1f, 0.4f, 0.3f)},
        
        {"Boss1", new Color(1f, 1f, 1f)},
        {"Boss2", new Color(0.9f, 0.9f, 1f)},
        {"Boss3", new Color(0.8f, 0.8f, 1f)},
        {"Boss4", new Color(0.7f, 0.6f, 1f)},
        {"Boss5", new Color(0.6f, 0.4f, 1f)},
        {"Boss6", new Color(0.5f, 0.3f, 1f)},
        {"Boss7", new Color(0.6f, 0.2f, 0.9f)},
        {"Boss8", new Color(0.8f, 0.3f, 0.6f)},
        {"Boss9", new Color(1f, 0.4f, 0.3f)}

    };
    public static Dictionary<string, Color> SelectorPanelColors = new Dictionary<string, Color>()
    {
        {"Green",new Color(89/255f,160/255f,49/255f) },
        {"Red",new Color(209/255f,43/255f,28/255f) }
    };
    public static Color platformFadeColor = new Color(1, 1, 1, 0);
}
