using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public static class Util
{
    #region Core
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go,name,recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static T Parse<T>(string stringData)
    {
        return (T)Enum.Parse(typeof(T), stringData);
    }

    public static string RemovePrefix(string str, string prefix)
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

    public static float CalculateRatio(int left, int right)
    {
        if (left > right)
            return (float)right / (float)left;
        else
            return (float)left / (float)right;
    }

    public static string SummaryOfNumbers(int number)
    {
        return SummaryOfNumbers($"{number}");
    }

    public static string SummaryOfNumbers(ulong number)
    {
        return SummaryOfNumbers($"{number}");
    }

    public static string SummaryOfNumbers(string number)
    {
        char[] unitAlphabet = new char[6] {'A','B','C','D','E','F'};
        int unit = 0;
        while (number.Length > 6)
        {
            unit++;
            number = number.Substring(0, number.Length - 3);
        }
        if (number.Length > 3)
        {
            int newInt = int.Parse(number);

            float retF = newInt/1000f;
            retF = Mathf.Floor(retF * 10) * 0.1f;
            return retF.ToString("0.0") + unitAlphabet[unit];
        }
        else
        {
            return number;
        }
    }

    public static async UniTask DelayedAction(float delay, Action callback,CancellationTokenSource token = null,bool ignoreTileScale = true)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay),ignoreTimeScale: ignoreTileScale,cancellationToken: token != null ? token.Token : default);

        callback?.Invoke();
    }

    [System.Serializable]
    public class WeightedItem<T>
    {
        public T item;
        public float weight;
    }

    public class WeightedRandomUtility
    {
        public static T GetWeightedRandom<T>(List<WeightedItem<T>> weightedItems)
        {
            // 만약 항목이 없으면 기본값 반환
            if (weightedItems.Count == 0)
                return default(T);

            // 모든 가중치를 더하여 총합을 구함
            float totalWeight = 0f;
            foreach (var weightedItem in weightedItems)
            {
                totalWeight += weightedItem.weight;
            }

            // 0부터 총합 사이의 랜덤 값을 생성
            float randomValue = UnityEngine.Random.value * totalWeight;

            // 랜덤 값이 어느 범위에 속하는지 확인하여 항목 선택
            foreach (var weightedItem in weightedItems)
            {
                randomValue -= weightedItem.weight;
                if (randomValue <= 0)
                    return weightedItem.item;
            }

            // 여기까지 왔다면 뭔가 잘못된 것이 아니므로 마지막 항목 반환
            return weightedItems[weightedItems.Count - 1].item;
        }
    }

    #endregion

    #region Contents
    /// <summary>
    /// Transform의 z회전값을 이용하여 2D 평면 상의 바라보고 있는 방향 벡터를 구합니다.
    /// </summary>
    public static Vector2 GetDirection(Transform tf)
    {
        float z = tf.eulerAngles.z;

        // Z축 회전 각도를 반대로 적용하여 반시계 방향 회전 처리
        return new Vector2(Mathf.Sin(-z * Mathf.Deg2Rad), Mathf.Cos(-z * Mathf.Deg2Rad));
    }

    public static Quaternion GetTargetRotation(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        angle = -angle;

        if (angle < -180)
        {
            angle += 360f;
        }

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Quaternion GetTargetRotation(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        angle = -angle;

        if (angle < -180)
        {
            angle += 360f;
        }

        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Quaternion GetTargetRotation(float angle)
    {
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Vector2 GetDirrectionFromTo(Vector2 from,Vector2 to)
    {
        return (to - from).normalized;
    }

    public static Vector2 GetDirrectionFromTo(Vector3 from, Vector2 to)
    {
        Vector2 fromVec2 = new Vector2(from.x,from.y);
        return (to - fromVec2).normalized;
    }

    public static Vector2 GetDirrectionFromTo(Vector3 from, Vector3 to)
    {
        Vector2 fromVec2 = new Vector2(from.x,from.y);
        Vector2 toVec2 = new Vector2(to.x,to.y);
        return (toVec2 - fromVec2).normalized;
    }

    #endregion
}
