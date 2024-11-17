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

        TextAsset jsonFile = Resources.Load<TextAsset>("YangyangData"); // Resources �������� YangyangData.json�� �ҷ�����

        if (jsonFile != null)
        {
            string dataAsJson = jsonFile.text; // TextAsset�� �ؽ�Ʈ ���� ��������
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
