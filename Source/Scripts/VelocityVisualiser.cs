using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class VelocityVisualiser : MonoBehaviour
{

    [SerializeField] private GameObject XCylinder;
    [SerializeField] private GameObject YCylinder;
    [SerializeField] private GameObject ZCylinder;
    [SerializeField] private GameObject netCylinder;
    [SerializeField] private SatelliteBehaviour SatelliteBehaviour;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject whereToBe;
    [SerializeField] private Button componentToggle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        componentToggle.onClick.AddListener(toggleMechanism);
    }

    // Update is called once per frame
    void Update()
    {
        
        parent.transform.position = whereToBe.transform.position;
        cylinderMorph(ref XCylinder, SatelliteBehaviour.Velocity.x, Vector3.right);
        cylinderMorph(ref YCylinder, SatelliteBehaviour.Velocity.y, Vector3.up);
        cylinderMorph(ref ZCylinder, SatelliteBehaviour.Velocity.z, Vector3.forward);
        
        netCylinder.transform.localScale = new Vector3(0.25f, Mathf.Abs(SatelliteBehaviour.Velocity.magnitude) / 2f, 0.25f);
        netCylinder.transform.localPosition = (SatelliteBehaviour.Velocity / 2f);
        netCylinder.transform.rotation = Quaternion.LookRotation(SatelliteBehaviour.Velocity) * Quaternion.Euler(90f, 0f, 0f); //* means to apply one first here
    }
    private void cylinderMorph(ref GameObject type, float velocity,Vector3 orientation)
    {
        
        type.transform.localScale = new Vector3(0.25f, Mathf.Abs(velocity) /2f, 0.25f);
        type.transform.localPosition = orientation * (velocity / 2f);

    }

    private void toggleMechanism()
    {
        if (XCylinder.activeSelf) //on, and will turn off
        {
            XCylinder.SetActive(false);
            YCylinder.SetActive(false);
            ZCylinder.SetActive(false);
        }
        else //off, and will turn on
        {
            XCylinder.SetActive(true);
            YCylinder.SetActive(true);
            ZCylinder.SetActive(true);
        }
    }

}
