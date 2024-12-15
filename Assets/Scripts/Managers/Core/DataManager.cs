using System.Collections.Generic;
using UnityEngine;
using Data;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.IO;
using Unity.VisualScripting;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    private string sheetID = ""; // 실제 Google Sheets 문서 ID

    //public Dictionary<int, EnemyData>               EnemyDataDict           { get; private set; } = new();

    // 데이터 초기화
    public async UniTask InitFromGoogleCheets(System.Action callback)
    {
        try
        {
            var tasks = UniTask.WhenAll(
            //MakeDictFromGoogleSheetData<EnemyData>("EnemyData"),    // 실제 시트의 이름
            );

            //var results = await tasks;

            //EnemyDataDict = results.Item1;

            // 모든 데이터 로드 완료 후 다음 작업 처리
            Debug.Log("모든 데이터가 성공적으로 로드되었습니다.");
            callback?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"데이터 로드 중 오류가 발생했습니다: {ex.Message}");
            // 로드 실패 시 기본 처리 로직 추가 가능
        }
    }

    private async UniTask<Dictionary<int,T>> MakeDictFromGoogleSheetData<T>(string sheetName) where T : Data.Data
    {
        var sheet = await DownloadCSV(sheetName);
        Dictionary<int,T> retDict = ProcessCSV<Datas<T>, int, T>(sheet,sheetName).MakeDict();
        return retDict;
    }

    private async UniTask<string> DownloadCSV(string sheetName)
    {
        string csvURL = $"https://docs.google.com/spreadsheets/d/{sheetID}/gviz/tq?tqx=out:csv&sheet={sheetName}";

        UnityWebRequest request = UnityWebRequest.Get(csvURL);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            return request.downloadHandler.text;
        }
        else
        {
            Debug.LogError($"CSV 다운로드 실패: {request.error}");
            return null;
        }
    }

    private Loader ProcessCSV<Loader, Key, Value>(string csv,string sheetName = "") where Loader : ILoader<Key, Value>
    {
        try
        {
            var jsonObjects = new List<Dictionary<string, string>>();
            string replaceCSV = csv.Replace("\"","");

            using(StringReader reader = new StringReader(replaceCSV))
            {
                string line;
                string[] headers = null;

                while((line = reader.ReadLine()) != null)
                {
                    // 첫번째 줄 헤더 처리
                    if(headers == null)
                    {
                        headers = line.Split(',');
                        continue;
                    }

                    // 데이터 라인을 읽어서 Dictionary<string,string>으로 변환
                    string[] values = line.Split(",");
                    var rowDict = new Dictionary<string, string>();

                    for(int i = 0; i < headers.Length; ++i)
                    {
                        // 헤더를 키, 데이터를 값으로 설정
                        rowDict[headers[i]] = values.Length > i ? values[i] : string.Empty;
                    }

                    jsonObjects.Add(rowDict);
                }
            }


            // 리스트를 JSON 문자열로 변환
            string jsonData = JsonConvert.SerializeObject(jsonObjects);
            Debug.Log("Converted CSV Data to JSON: " + jsonData);
            string dataFormat = $@"{{""datas"":{jsonData}}}";

//#if UNITY_EDITOR
//            if (sheetName != "")
//                SaveJsonToFile(dataFormat, sheetName);
//#endif

            // JSON 데이터를 파싱하여 Loader로 변환
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,  // 이스케이프 처리를 최소화
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };

            return JsonConvert.DeserializeObject<Loader>(dataFormat, settings);
        }
        catch (Exception ex)
        {
            Debug.LogError($"CSV 데이터를 처리하는 중 오류 발생: {ex.Message}");
            throw;
        }
    }

    public void InitFromLocalData()
    {
        //EnemyDataDict = LoadJson<Datas<EnemyData>, int, Data.EnemyData>("EnemyData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        if (textAsset == null)
        {
            Debug.LogError($"Failed to load JSON file at path: Data/{path}");
            return default;
        }

        JsonSerializerSettings settings = new()
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };

        Debug.Log($"{path} Load Completed");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text, settings);
    }

    private void SaveJsonToFile(string jsonData, string sheetName)
    {
        // 저장할 경로 설정
        string folderPath = Path.Combine(Application.dataPath, "Resources/Data");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, $"{sheetName}.json");

        // JSON 데이터를 파일로 저장
        File.WriteAllText(filePath, jsonData);
        Debug.Log($"JSON 파일이 {filePath}에 저장되었습니다.");

#if UNITY_EDITOR
        // 에디터에서 저장된 파일이 Resources에 적용되도록 Refresh
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}