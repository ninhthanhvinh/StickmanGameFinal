using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenUI : MonoBehaviour
{
    private void Start()
    {
        //Tween(gameObject, 1f);
    }
    // Start is called before the first frame update
    public void Tween(GameObject obj, float time)
    {
        Debug.Log("Before Tween" + obj.transform.localScale);
        LeanTween.scale(obj, new Vector3(1.5f, 1.5f, 1.5f), time).setEase(LeanTweenType.once);
        Debug.Log("Tween" + obj.transform.localScale);
        Invoke(nameof(DelayTime), time);
    }

    private void DelayTime()
    {
        Time.timeScale = 0f;
    }

    public void TweenIn(float time)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), time).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
        Invoke(nameof(DelayTime), time);
    }

    public void TweenOut(float time)
    {
        Time.timeScale = 1f;
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), time).setDelay(.5f).setEase(LeanTweenType.easeOutElastic);
        Debug.Log("TweenOut" + gameObject.transform.localScale);
        Time.timeScale = 1f;
        Debug.Log(Time.timeScale);
    }
}
