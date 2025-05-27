
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PictureDetector : MonoBehaviour
{
    private ARTrackedImageManager m_imageManager;
    [SerializeField] private GameObject m_prefab;

    // Pour garder une trace des objets instanciés par image
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    void Awake()
    {
        m_imageManager = GetComponent<ARTrackedImageManager>();

    }

    void OnEnable()
    {
        // S’abonner à l’événement trackables.changed (UnityEvent)
        m_imageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);

    }
    void OnDisable()
    {
        // Désabonnement
        m_imageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    // Méthode appelée automatiquement lors de tout changement (ajout, update, suppression)
    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var added in args.added)
        {
            Debug.Log($"📷 Image ajoutée : {added.referenceImage.name}");
            // Tu peux instancier un objet ici
            HandleImage(added);

        }

        foreach (var updated in args.updated)
        {
            Debug.Log($"🔄 Image mise à jour : {updated.referenceImage.name}, état : {updated.trackingState}");

            if (updated.trackingState == TrackingState.Limited)
            {
                Debug.Log($"👋 Image perdue : {updated.referenceImage.name}");
                // Tu peux ici désactiver l’objet lié
                HandleImage(updated);
                
            }
            else if (updated.trackingState == TrackingState.Tracking)
            {
                Debug.Log($"✅ Image suivie : {updated.referenceImage.name}");
                // Réactiver ou mettre à jour ton contenu AR
                HandleImage(updated);
                
            }
        }

        foreach (var removed in args.removed)
        {
            Debug.Log($"❌ Image retirée : ");
            // Nettoyage ou désactivation de contenu lié à l’image
        }
    }
    private void HandleImage(ARTrackedImage trackedImage)
    {
        var imageName = trackedImage.referenceImage.name;

        // Si on n'a pas encore instancié de prefab pour cette image
        if (!spawnedPrefabs.ContainsKey(imageName))
        {
            GameObject newPrefab = Instantiate(m_prefab, trackedImage.transform);
            newPrefab.transform.rotation = Quaternion.identity;
            newPrefab.name = "AR_" + imageName;
            spawnedPrefabs[imageName] = newPrefab;
        }

        // Mise à jour de la position et de la visibilité
        var prefab = spawnedPrefabs[imageName];
        prefab.transform.position = trackedImage.transform.position;
        Debug.Log("La prefab est : "+prefab.name);
        Debug.Log("La prefab size est de : "+prefab.transform.localScale);


        // Activer/désactiver selon trackingState
        //prefab.SetActive(trackedImage.trackingState == TrackingState.Tracking);
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            prefab.SetActive(true);
            var prefabScale = prefab.transform.localScale;
            // Apparition avec scale rebond
            //prefab.transform.localScale = Vector3.zero; // Reset scale
            //prefab.transform.DOScale(prefabScale, 2f).SetEase(Ease.OutBack);
            //Debug.Log(prefabScale.ToString());
        }
        else
        {
            // Disparition
            prefab.SetActive(false);
        }
    }
}
