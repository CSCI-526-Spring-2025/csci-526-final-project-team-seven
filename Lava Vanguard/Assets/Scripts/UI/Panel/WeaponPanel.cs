using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPanel : UIPanel
{
    public override void Hide()
    {
        base.Hide();
        Tooltip.Instance.HideTooltip();
    }
}
