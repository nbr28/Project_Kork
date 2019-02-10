using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnippetDetailFader : MonoBehaviour {

    public float fadeTime = 0.1f;

    public delegate void setActiveDelegate(bool a);

    public void FadeIn(CanvasGroup target)
    {
        StartCoroutine(FadeCanvasGroup(target, target.alpha, 1, fadeTime));
    }

    public void FadeOut(CanvasGroup target)
    {
        StartCoroutine(FadeCanvasGroup(target, target.alpha, 0, fadeTime, target.gameObject.SetActive));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup target, float start, float end, float lerpTime = 0.5f, setActiveDelegate setActiveFalse = null)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            target.alpha = currentValue;

            if (percentageComplete >= 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        if (setActiveFalse != null)
        {
            setActiveFalse(false);
        }
    }
}
