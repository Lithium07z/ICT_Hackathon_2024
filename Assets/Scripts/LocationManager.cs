using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static MainMode;

public class LocationManager : MonoBehaviour
{
    public List<LocationData> locations;

    public static LocationManager instance;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        TextAsset jsonFile = Resources.Load<TextAsset>("YangyangData"); // Resources 폴더에서 YangyangData.json을 불러오기

        if (jsonFile != null)
        {
            string dataAsJson = jsonFile.text; // TextAsset의 텍스트 내용 가져오기
            locations = JsonConvert.DeserializeObject<List<LocationData>>(dataAsJson);
            Debug.Log(locations[54].id + "json success");
            Debug.Log("Data loaded successfully! Total locations: " + locations.Count);
        }
        else
        {
            Debug.LogError("JSON file not found in Resources folder.");
        }
    }

    void Start()
    {
        
    }
}
