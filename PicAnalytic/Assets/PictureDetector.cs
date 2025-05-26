using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PictureDetector : MonoBehaviour
{
    ARTrackedImageManager m_imageManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_imageManager = GetComponent<ARTrackedImageManager>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        m_imageManager.trackablesChanged.AddListener(OnTrackableChanged);
    }
    private void OnTrackableChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var newItem in args.added)
        {
            Debug.Log("Add new image :"+newItem.referenceImage.name);
            
        }
        foreach (var updatedImage in args.updated)
        {
            Debug.Log("une update a ete realiser : " + updatedImage.referenceImage.name);
        }
        foreach (var removed in args.removed)
        {
            // Handle removed event
            TrackableId removedImageTrackableId = removed.Key;
            ARTrackedImage removedImage = removed.Value;
        }
    }
    private void OnDisable()
    {
        m_imageManager.trackablesChanged.RemoveListener(OnTrackableChanged);
    }
    
}
