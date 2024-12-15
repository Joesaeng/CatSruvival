using System;
using System.Collections.Generic;

// Json 데이터 저장 형식
namespace Data
{
    #region PlayerData

    // 플레이어의 정보 저장 객체
    [Serializable]
    public class PlayerData
    {
        public bool     beginner        = true;
        public GameSettingData settingData = new();
    }

    [Serializable]
    public class GameSettingData
    {
        public int gameLanguage = (int)Define.GameLanguage.Korean;
        public bool bgmOn = true;
        public bool sfxOn = true;
        public float bgmVolume = 1f;
        public float sfxVolume = 1f;
    }

    #endregion

    [Serializable]
    public class Data
    {
        public int ID;
    }

    [Serializable]
    public class Datas<T> : ILoader<int, T> where T : Data
    {
        public List<T> datas = new();

        public Dictionary<int, T> MakeDict()
        {
            Dictionary<int, T> dict = new();
            foreach (T data in datas)
            {
                dict.Add(data.ID, data);
            }
            return dict;
        }
    }
}