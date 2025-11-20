using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;

public class OrbitRendering : MonoBehaviour
{

    public LineRenderer linerenderer;
    public SatelliteBehaviour SatelliteBehaviour;

    public GameObject satellite;
    public GameObject planet;

    public int dotResolution = 200;
    public double mu;

    [SerializeField] private Button componentToggle;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linerenderer.positionCount = (dotResolution);

        componentToggle.onClick.AddListener(toggleOrbit);

        //mu = SatelliteBehaviour.massMultiplier * SatelliteBehaviour.planetMass * SatelliteBehaviour.G;
        orbitRender();
        print("Start had been completed");
        
    }

    // Update is called once per frame
    void Update()
    {

  //      if (
  //    (Input.GetKeyUp(KeyCode.W) == true) ||
  //    ((Input.GetKeyUp(KeyCode.A) == true) ||
  //    ((Input.GetKeyUp(KeyCode.S) == true) ||
  //    ((Input.GetKeyUp(KeyCode.D) == true) ||
  //    ((Input.GetKeyUp(KeyCode.Q) == true) ||
  //    (Input.GetKeyUp(KeyCode.E) == true)))))
  //)
  //      {
            orbitRender();
        //}

    }

    private double muCalculator()
    {
        
        Rigidbody rb = planet.GetComponent<Rigidbody>();
        return rb.mass * SatelliteBehaviour.G * Mathf.Pow(10, (float)SatelliteBehaviour.massMultiplier);
    }

    private void orbitRender()
    {
        mu = muCalculator();
        Vector3 r = satellite.transform.position - planet.transform.position;
        Vector3 v = SatelliteBehaviour.Velocity;
        Vector3 refAxisOfOrbit = Vector3.Cross(r, v);

        Vector3 eVec = Vector3.Cross(v, refAxisOfOrbit) / (float)mu - r.normalized; //Add an edge case where orbit is not elliptic
        //Debug.Log(eVec.magnitude);
        float semiMajor = 1f / (2f / r.magnitude - v.sqrMagnitude / (float)mu);
        float semiMinor = semiMajor * Mathf.Sqrt(1f - eVec.magnitude * eVec.magnitude);

        Vector3 i = eVec.normalized; //'Horizontal'
        Vector3 j = Vector3.Cross(refAxisOfOrbit.normalized, i); //'Vertical'
        Vector3[] dots = new Vector3[dotResolution];
        if (eVec.magnitude >= 1f) //hyperbolic
        {
            Debug.LogWarning("e > 1");
            float a_hyper = -semiMajor; // change direction
            float r_periapsis = a_hyper * (eVec.magnitude - 1f);
            float r_max = 100f * r_periapsis; // max render distance
            float cosh_Hmax = (r_max / a_hyper + 1f) / eVec.magnitude;
            float H_max = Mathf.Log(Mathf.Min(cosh_Hmax, 1000f) + Mathf.Sqrt(Mathf.Min(cosh_Hmax, 1000f) * Mathf.Min(cosh_Hmax, 1000f) - 1f));

            for (int k = 0; k < dotResolution; k++)
            {
                float H = Mathf.Lerp(-H_max, H_max, k / (float)(dotResolution - 1));
                float x = a_hyper * (eVec.magnitude - math.cosh(H));
                float y = a_hyper * Mathf.Sqrt(eVec.magnitude * eVec.magnitude - 1f) * math.sinh(H);
                dots[k] = planet.transform.position + x * i + y * j;
            }
            
        }
        else //elliptical
        {
            for (int k = 0; k <= dotResolution - 1; k++) 
            {
                float E = 2f * Mathf.PI * k / (dotResolution);
                float x = semiMajor * (Mathf.Cos(E) - eVec.magnitude);
                float y = semiMinor * (Mathf.Sin(E));
                dots[k] = planet.transform.position + x * i + y * j;
            }
        }
        linerenderer.SetPositions(dots);
    }

    private void toggleOrbit()
    {
        if (linerenderer.gameObject.activeSelf) //on, and will turn off
        {
            linerenderer.gameObject.SetActive(false);
           
        }
        else //off, and will turn on
        {
            linerenderer.gameObject.SetActive(true);
            
        }
    }
}
