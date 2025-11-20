using Unity.VisualScripting;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject reference;
    [SerializeField] private Camera zkapfk;

    private float lateral = 0f;
    private float vertical = 0f;
    private float zoom = 5f;
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float zoomSensitivity = 1f;

    [SerializeField] private Vector3 originalOffset = new Vector3(0f, 5f, 0f);
    private Vector3 offset = new Vector3(0f, 0f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            lateral += Input.GetAxis("Mouse Y") * sensitivity;
            vertical += Input.GetAxis("Mouse X") * sensitivity; //invert this if opposite

        }
        
            zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * zoom * 0.5f; //proportional to current zoom level, making it exponential
            zoom = Mathf.Clamp(zoom, 1, 100); //max and min zoom scale

        Quaternion rotation = Quaternion.Euler(lateral, vertical, 0);
        offset = originalOffset.normalized * zoom;
        zkapfk.transform.position = reference.transform.position +  rotation * offset;
        zkapfk.transform.LookAt(reference.transform.position);
    }
}
