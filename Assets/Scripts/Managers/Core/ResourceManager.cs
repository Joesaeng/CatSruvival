using Cysharp.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(GameObject origin, Transform parent = null)
    {
        if (origin.TryGetComponent<Poolable>(out Poolable poolable))
            return Managers.Pool.Pop(origin, parent).gameObject;

        return Object.Instantiate(origin,parent);
    }

    public GameObject Instantiate(GameObject origin, Vector3 pos)
    {
        GameObject go;
        if (origin.TryGetComponent<Poolable>(out _))
        {
            go = Managers.Pool.Pop(origin).gameObject;
            go.transform.SetPositionAndRotation(pos, Quaternion.identity);
            return go;
        }
        go = Object.Instantiate(origin, pos, Quaternion.identity);

        return go;
    }

    public GameObject Instantiate(GameObject origin, Vector3 pos, Quaternion rot)
    {
        GameObject go;
        if (origin.TryGetComponent<Poolable>(out _))
        {
            go = Managers.Pool.Pop(origin).gameObject;
            go.transform.SetPositionAndRotation(pos, rot);
            return go;
        }
        go = Object.Instantiate(origin, pos, rot);

        return go;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.TryGetComponent<Poolable>(out Poolable poolable))
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go;
        go = Object.Instantiate(original, parent);

        go.name = original.name;

        return go;
    }

    public GameObject Instantiate(string path, Vector3 pos)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go;

        if (original.TryGetComponent<Poolable>(out _))
        {
            go = Managers.Pool.Pop(original).gameObject;
            go.transform.SetPositionAndRotation(pos, Quaternion.identity);
            return go;
        }
        go = Object.Instantiate(original, pos, Quaternion.identity);

        return go;
    }

    public GameObject Instantiate(string path, Vector3 pos, Quaternion rot)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go;

        if (original.TryGetComponent<Poolable>(out _))
        {
            go = Managers.Pool.Pop(original).gameObject;
            go.transform.SetPositionAndRotation(pos, rot);
            return go;
        }
        go = Object.Instantiate(original, pos, rot);

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (go.TryGetComponent<Poolable>(out Poolable poolable))
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

    public async void DestroyDelay(GameObject go, float time)
    {
        if (go == null)
            return;

        if (go.TryGetComponent<Poolable>(out Poolable poolable))
        {
            // Managers.Timer.StartTimer(time, () => Managers.Pool.Push(poolable));
            await UniTask.Delay(System.TimeSpan.FromSeconds(time));
            if(poolable != null)
                Managers.Pool.Push(poolable);
            return;
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(time));
        Object.Destroy(go);
        // Managers.Timer.StartTimer(time, () => Object.Destroy(go));
    }

    public async void DestroyDelayUnscaled(GameObject go, float time)
    {
        if (go == null)
            return;

        if (go.TryGetComponent<Poolable>(out Poolable poolable))
        {
            // Managers.Timer.StartTimerUnscaled(time, () => Managers.Pool.Push(poolable));
            await UniTask.Delay(System.TimeSpan.FromSeconds(time),DelayType.UnscaledDeltaTime);
            Managers.Pool.Push(poolable);
            return;
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(time), DelayType.UnscaledDeltaTime);
        Object.Destroy(go);
        // Managers.Timer.StartTimerUnscaled(time, () => Object.Destroy(go));
    }
}