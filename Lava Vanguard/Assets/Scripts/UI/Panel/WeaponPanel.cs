using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPanel : UIPanel
{
    public override void Init()
    {
        base.Init();
        canOpen = false;
    }
    public override void Close()
    {
        base.Close();
        Tooltip.Instance.HideTooltip();
    }
}
