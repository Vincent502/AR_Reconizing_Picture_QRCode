using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    [SerializeField] Transform _text;
    private Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        //on recupere la direction dans la quel regarde la camera
        Vector3 direction = _camera.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(direction);
        _text.rotation = rotation;
    }
}
