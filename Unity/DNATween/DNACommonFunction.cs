using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;//[Conditional]需要
using UnityEngine;

public static class DNACommonFunction
{
	public static float minValue = 0.00001f;
    private static StringBuilder _customSB = new StringBuilder();
    [Conditional("UNITY_EDITOR")]
    public static void LogEditor(string s)
    {
        UnityEngine.Debug.Log(s);
    }
    [Conditional("UNITY_EDITOR")]
    public static void LogWarningEditor(string s)
    {
        UnityEngine.Debug.LogWarning(s);
    }
    public static void LogError(string s)
    {
        UnityEngine.Debug.LogError(s);
    }
    [Conditional("UNITY_EDITOR")]
    public static void WarningDialog(string s)
    {
        UnityEditor.EditorUtility.DisplayDialog("⚠️警告", s, "好的");
    }

    public static bool IsApproximately(double a, double b)
	{
		return IsApproximately(a, b, minValue);
	}
	public static bool IsApproximately(double a, double b, double dvalue)
	{
		double delta = a-b;
		return delta >= -dvalue && delta <= dvalue;
	}

    public static string Format(string format, params object[] args)
    {
        try
        {
            _customSB.Remove(0, _customSB.Length);
            _customSB.AppendFormat(format, args);
            return _customSB.ToString();
        }
        catch (Exception e)
        {
            LogError(e.Message);
            return format;
        }
    }

    public static string ToStringUS(this int i)
    {
        return i.ToString("n0").Replace(',', ' ');
    }

    public static string ToStringUS(this long i)
    {
        return i.ToString("n0").Replace(',', ' ');
    }

    public static void FillWithTempItem<T>(Transform parent, MonoBehaviour goTmp, int needCount, List<T> goList,
        Action<T> initCallback = null) where T : MonoBehaviour
    {
        if (goList == null)
        {
            return;
        }
        if (parent == null || goTmp == null)
        {
            return;
        }

        for (int i = goList.Count - 1; i >= 0; --i)
        {
            if (null == goList[i])
            {
                goList.RemoveAt(i);
            }
        }

        goTmp.gameObject.SetActive(false);
        int GoListCount = goList.Count;
        for (int i = GoListCount; i < needCount; i++)
        {
            T item = GameObject.Instantiate((T)goTmp, parent, false) as T;
            if (initCallback != null)
            {
                initCallback(item);
            }
            goList.Add(item);
        }
        GoListCount = goList.Count;
        for (int i = 0; i < GoListCount; i++)
        {
            T item = goList[i];
            bool active = i < needCount;
            if (active)
            {
                Transform itemTra = item.transform;
                if (itemTra.parent != parent)
                {
                    itemTra.SetParent(parent, false);
                    itemTra.localPosition = Vector3.zero;
                    itemTra.rotation = Quaternion.identity;
                }
            }
            item.gameObject.SetActive(active);
        }
    }
}