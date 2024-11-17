using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using static MainMode;

[System.Serializable]
public class LocationDictionary : SerializableDictionaryBase<string, Sprite> { }

public class ScanMode : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    [SerializeField] GameObject markerPanel;
    [SerializeField] Image locationImage;

    public LocationManager locationManager;

    private void Start()
    {
        StartCoroutine(LocationFind());
    }

    void OnEnable()
    {
        UIController.ShowUI("Scan");
    }

    void Update()
    {
        if (imageManager.trackables.count > 0)
        {
            InteractionController.EnableMode("Main");
        }
    }

    IEnumerator LocationFind()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            LocationData currentLocation = null;

            for (int i = 0; i < locationManager.locations.Count; i++)
            {
                LocationData location = locationManager.locations[i];

                if ((Mathf.Abs(GPS_Manager.instance.latitude - location.lat) <= 0.002) && (Mathf.Abs(GPS_Manager.instance.longitude - location.lng) <= 0.002))
                {
                    currentLocation = location;
                    TTSManager.instance.currentLocation = currentLocation;
                    markerPanel.SetActive(true);

                    locationImage.sprite = instance.locationImages[location.id];
                    break;
                }
            }

            if (currentLocation != null && (Mathf.Abs(GPS_Manager.instance.latitude - currentLocation.lat) > 0.002) || (Mathf.Abs(GPS_Manager.instance.longitude - currentLocation.lng) > 0.002))
            {
                markerPanel.SetActive(false);
            }
        }
    }
}