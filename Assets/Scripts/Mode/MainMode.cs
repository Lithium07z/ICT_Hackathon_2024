using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;


public class MainMode : MonoBehaviour
{
    public static MainMode instance;

    public LocationDictionary locationImages;
    [SerializeField] ARTrackedImageManager imageManager;
    [SerializeField] Toggle infoButton;

    [SerializeField] GameObject prefabs;
    [SerializeField] GameObject markerPanel;

    [SerializeField] Image locationImage;
    [SerializeField] TMP_Text detailText;
    [SerializeField] Image detailImage;
    private LocationData currentLocation;

    public LocationManager locationManager;

    Camera camera;
    int layerMask;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        camera = Camera.main;
        layerMask = 1 << LayerMask.NameToLayer("PlaceObjects");
        StartCoroutine(TurnOnImageTracker());
    }

    private void Update()
    {
        if (imageManager.trackables.count == 0)
        {
            InteractionController.EnableMode("Scan");
        }
        else
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                detailText.text = currentLocation.text;
                detailImage.sprite = locationImages[currentLocation.id];
                infoButton.interactable = true;
            }
            else
            {
                infoButton.interactable = false;
            }
        }
    }

    void OnEnable()
    {
        UIController.ShowUI("Main");    
        infoButton.interactable = false;
        foreach (ARTrackedImage image in imageManager.trackables)
        {
            InstantiatePrefab(image);
        }
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    
    void InstantiatePrefab(ARTrackedImage image)
    {
        for (int i = 0; i < locationManager.locations.Count; i++)
        {
            LocationData location = locationManager.locations[i];
            string name = image.referenceImage.name;

            if (location.id.Equals(name) && (Mathf.Abs(GPS_Manager.instance.latitude - location.lat) < 0.002) && (Mathf.Abs(GPS_Manager.instance.longitude - location.lng) < 0.002))
            {
                if (image.transform.childCount == 0)
                {
                    StartCoroutine(PartialScreenshotCaptureAndUpload.instance.CaptureAndUpload(i));

                    GameObject prefab = Instantiate(prefabs);
                    prefab.transform.SetParent(image.transform, false);
                    markerPanel.SetActive(false);
                    currentLocation = location;
                }
                else
                {
                    Debug.Log("${name} already instantiated");
                }

                break;
            }
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage newImage in eventArgs.added)
        {

            Vector2 myPos = new Vector2(GPS_Manager.instance.latitude, GPS_Manager.instance.longitude);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.transform.childCount > 0)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
            }
        }
    }

    IEnumerator TurnOnImageTracker()
    {
        imageManager.enabled = false;

        while (!GPS_Manager.instance.receiveGPS)
        {
            yield return null;
        }
        
        imageManager.enabled = true;

        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    [System.Serializable]
    public class LocationData
    {
        public string id;
        public string place;
        public string address;
        public string phone;
        public string type;
        public float lat;
        public float lng;
        public string text;

        public LocationData(string id, string place, string address, string phone, string type, float lat, float lng, string text)
        {
            this.id = id;
            this.place = place;
            this.address = address;
            this.phone = phone;
            this.type = type;
            this.lat = lat;
            this.lng = lng;
            this.text = text;
        }
    }
}