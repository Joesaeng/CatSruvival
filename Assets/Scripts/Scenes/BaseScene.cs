using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    [SerializeField] AudioClip bgm;

    private void Awake()
    {
        Init();
    }

    protected virtual void Start()
    {
        InitAfterLoadScene();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindFirstObjectByType(typeof(EventSystem));
        if (obj == null)
        {
            Managers.Resource.Instantiate("EventSystem").name = "@EventSystem";
        }

        Managers.Scene.CurrentScene = this;
    }

    protected virtual void InitAfterLoadScene()
    {
        if (bgm != null)
            Managers.Sound.Play(bgm, Define.Sound.Bgm);
    }

    public abstract void Clear();
}
