using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get; set; }

    /// <summary>
    /// 현재 Scene을 클리어하고 type에 맞는 Scene을 동기적으로 로드합니다.
    /// </summary>
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    /// <summary>
    /// 현재 Scene을 클리어하고 type에 맞는 Scene을 비동기적으로 로드합니다.
    /// </summary>
    public void LoadSceneWithLoadingScene(Define.Scene type, System.Action callback = null)
    {
        Managers.Clear();
    }

    private async UniTask LoadGameAsync(Define.Scene type)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(GetSceneName(type));
        ao.allowSceneActivation = false;

        while (ao.progress < 0.9f)
        {
            await UniTask.Yield();
        }


        ao.allowSceneActivation = true;

        await UniTask.WaitUntil(() => ao.isDone);
    }

    string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
