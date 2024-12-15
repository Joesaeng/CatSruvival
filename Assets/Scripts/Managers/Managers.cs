using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region Core
    SaveAndLoadManager _saveLoad = new SaveAndLoadManager();
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    UIManager _UI = new UIManager();
    ComponentCacheManager _compCache = new ComponentCacheManager();
    SceneManagerEx _scene = new SceneManagerEx();

    public static DataManager Data { get => Instance._data;  }
    public static PoolManager Pool { get => Instance._pool;  }
    public static ResourceManager Resource { get => Instance._resource;  }
    public static SoundManager Sound { get => Instance._sound;  }
    public static UIManager UI { get => Instance._UI;  }
    public static ComponentCacheManager CompCache { get => Instance._compCache;  }
    public static SaveAndLoadManager SaveLoad { get => Instance._saveLoad;  }
    public static SceneManagerEx Scene { get => Instance._scene;  }

    #endregion

    void Start()
    {
        Init();

        DOTween.Init(true, true,LogBehaviour.ErrorsOnly);
        DOTween.SetTweensCapacity(500, 125);

        Application.targetFrameRate = 60;
    }

    void Update()
    {
    }

    public static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Scene.Clear();
        Sound.Clear();

        CompCache.Clear();
        Pool.Clear();
    }
}
