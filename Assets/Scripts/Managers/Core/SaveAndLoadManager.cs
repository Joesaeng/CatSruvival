using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngineInternal;

// 플레이어의 데이터 저장 및 로드 담당
public class SaveAndLoadManager
{
    private Data.PlayerData _playerData;

    string _path;

    public void Init()
    {
        _path = Application.persistentDataPath + "/PlayerData";
        LoadFromJson();
    }

    // 플레이어 데이터를 json형태로 로컬에 저장
    public void SaveToJson()
    {
        if (File.Exists(_path))
            File.Delete(_path);

        string json = JsonConvert.SerializeObject(_playerData, Formatting.Indented);

        File.WriteAllText(_path, json);

    }

    // 로컬에 저장된 json 플레이어 데이터를 불러옴
    public void LoadFromJson()
    {
        if (!File.Exists(_path))
        {
            _playerData = new Data.PlayerData();

            SaveToJson();
        }

        string json = File.ReadAllText(_path);

        _playerData = JsonConvert.DeserializeObject<Data.PlayerData>(json);
    }
}
