using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    #region Core
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static bool TryGetChild(this Transform parent, int index, out Transform child)
    {
        child = null;
        if (parent.childCount <= index)
            return false;

        child = parent.GetChild(index);
        return true;
    }

    public static bool TryGetChild<T>(this Transform parent, int index, out T childComp) where T : Component
    {
        childComp = null;
        if (parent.childCount <= index)
            return false;

        Transform tempChild;
        tempChild = parent.GetChild(index);
        childComp = tempChild.GetComponent<T>();
        if (childComp != null)
            return true;
        return false;
    }

    public static GameObject FindChild(this GameObject go, string name = null, bool recursive = false)
    {
        return Util.FindChild(go, name, recursive);
    }

    public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : Component
    {
        return Util.FindChild<T>(go, name, recursive);
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static T GetOrAddComponent<T>(this Transform tf) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(tf.gameObject);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }


    /// <summary>
    /// Dictionary의 Value 값으로 Key값을 찾습니다.
    /// </summary>
    public static bool FindKeyByValueInDictionary<K, V>(this Dictionary<K, V> dict, V value, out K key)
    {
        foreach (KeyValuePair<K, V> pair in dict)
        {
            if (EqualityComparer<V>.Default.Equals(pair.Value, value))
            {
                key = pair.Key;
                return true;
            }
        }

        key = default(K);
        return false;
    }

    /// <summary>
    /// str 문자열에서 prefix 문자열을 제거한 문자열을 반환
    /// </summary>
    public static string RemovePrefix(this string str, string prefix)
    {
        // 문자열이 null이거나 비어있을 경우 그대로 반환
        if (string.IsNullOrEmpty(str))
            return str;

        // 접두사가 문자열의 시작과 일치하는지 확인하고, 일치하면 해당 부분을 제거한 문자열 반환
        if (str.StartsWith(prefix))
            return str.Substring(prefix.Length);

        // 일치하지 않으면 원래 문자열 그대로 반환
        return str;
    }

    /// <summary>
    /// string 을 Enum으로 변환
    /// </summary>
    public static T Parse<T>(this string stringData) where T : Enum
    {
        return Util.Parse<T>(stringData);
    }
    #endregion

    #region Contents
    /// <summary>
    /// Transform의 z회전값을 이용하여 2D평면상의 바라보고 있는 방향벡터를 구합니다
    /// </summary>
    public static Vector2 GetDirection(this Transform transform)
    {
        return Util.GetDirection(transform);
    }

    /// <summary>
    /// Vector2 방향을 통해 Z축 회전한 Quaternion을 구합니다
    /// </summary>
    public static Quaternion GetTargetRotation(this Vector2 dir)
    {
        return Util.GetTargetRotation(dir);
    }

    /// <summary>
    /// Vector3 방향을 통해 Z축 회전한 Quaternion을 구합니다
    /// </summary>
    public static Quaternion GetTargetRotation(this Vector3 dir)
    {
        return Util.GetTargetRotation(dir);
    }

    /// <summary>
    /// Z축 angle을 통해 Z축 회전한 Quaternion을 구합니다
    /// </summary>
    public static Quaternion GetTargetRotation(this float angle)
    {
        return Util.GetTargetRotation(angle);
    }

    public static Vector2 GetDirectionFromTo(this Vector2 from, Vector2 to)
    {
        return Util.GetDirrectionFromTo(from, to);
    }

    public static Vector2 GetDirectionFromTo(this Vector3 from, Vector2 to)
    {
        return Util.GetDirrectionFromTo(from, to);
    }

    public static Vector2 GetDirectionFromTo(this Vector3 from, Vector3 to)
    {
        return Util.GetDirrectionFromTo(from, to);
    }

    #endregion
}
