using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utility
{
    public static T GetEnum<T>(string enumName)
    {
        return (T)Enum.Parse(typeof(T), enumName);
    }

    public static T SelectOne<T>(this List<T> ts)
    {
        return ts[UnityEngine.Random.Range(0, ts.Count)];
    }

    public static int RandomSign()
    {
        return UnityEngine.Random.value < 0.5f ? 1 : -1;
    }

    public static Color GetChangeAlpha(this Color color, float fadeValue)
    {
        return new Color(color.r, color.g, color.b, fadeValue);
    }

    public static Vector2 GetChangeX(this Vector2 vector, float changeValue)
    {
        return new Vector2(changeValue, vector.y);
    }

    public static Vector2 GetChangeY(this Vector2 vector, float changeValue)
    {
        return new Vector2(vector.x, changeValue);
    }

    public static void Invoke(this MonoBehaviour monoBehaviour, Action action, float duration, bool isRealtime = false)
    {
        monoBehaviour.StartCoroutine(InvokeAction(action, duration, isRealtime));
    }

    private static IEnumerator InvokeAction(Action action, float duration, bool isRealtime)
    {
        yield return isRealtime ? new WaitForSecondsRealtime(duration) : new WaitForSeconds(duration);
        action?.Invoke();
    }

    public static void RemoveAndAddListener(this Button button, Action action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action?.Invoke());
    }

    public static void AddListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<PointerEventData> callBack)
    {
        var triggerEntry = new EventTrigger.Entry
        {
            eventID = type
        };
        triggerEntry.callback.AddListener((data) => callBack(data as PointerEventData));
        eventTrigger.triggers.Add(triggerEntry);
    }
}