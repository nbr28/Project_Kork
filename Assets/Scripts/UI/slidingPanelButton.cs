using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class slidingPanelButton : MonoBehaviour
{
    public RectTransform panel;
    private bool plusTog;

    public void togglePanel()
    {
        if (!plusTog)
        {
            panel.DOAnchorPosX(6.0f, -85.0f);
            plusTog = true;
        }
        else
        {
            panel.DOAnchorPosX(-161.0f, -85.0f);
            plusTog = false;
        }
    }

}
