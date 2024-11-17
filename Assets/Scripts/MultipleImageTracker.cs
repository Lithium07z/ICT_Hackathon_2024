using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultipleImageTracker : MonoBehaviour
{
    ARTrackedImageManager imageManager;

    void Start()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
        imageManager.trackedImagesChanged += OnTrackedImages;
    }

    void Update()
    {
        
    }

    void OnTrackedImages(ARTrackedImagesChangedEventArgs args)
    {

        foreach (ARTrackedImage trackedImage in args.added)
        {
            /*string imageName = trackedImage.referenceImage.name;

            GameObject imagePrefab = Resources.Load<GameObject>(imageName);
            if (imagePrefab != null)
            {
                if (trackedImage.transform.childCount < 1)
                {
                    GameObject go = Instantiate(imagePrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    go.transform.SetParent(trackedImage.transform);
                }
            }*/

            // 현재 나의 위치를 Vector2 형태로 저장한다.
            Vector2 myPos = new Vector2(GPS_Manager.instance.latitude, GPS_Manager.instance.longitude);

            //StartCoroutine(DB_Manager.instance.LoadData(myPos, trackedImage.transform));
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            if (trackedImage.transform.childCount > 0)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
            }
        }
    }
}
