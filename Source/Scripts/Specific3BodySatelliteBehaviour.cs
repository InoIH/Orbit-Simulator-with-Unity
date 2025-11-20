using System;
using UnityEngine;
using UnityEngine.UI;


public class Specific3BodySatelliteBehaviour : MonoBehaviour
{
    public Vector3 Velocity;
    public GameObject referencePlanet;
    [SerializeField] private GameObject referencePlanet2;
    // public GameObject lineRenderer;
    public const double G = 6.674e-11;
    public double massMultiplier;
    public double planetMass;
    public double mu;
    //public because may be referenced by orbit rendering in the future
    //[SerializeField] private double numberOfReferenceBodies; //exclude the satellite itself (1 for 2 body, 2 for 3 body, ...)

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider planetMassSlider;
    [SerializeField] private GameObject sessionOver; //gameover? its not game tho

    Rigidbody rb;
    Rigidbody planetRb;
    LineRenderer lr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planetRb = referencePlanet.GetComponent<Rigidbody>();
        // lr = lineRenderer.GetComponent<LineRenderer>();

        //lr.startColor = Color.grey;
        //lr.SetPosition(0, rb.position);

        double satelliteMass = rb.mass;
        planetMass = planetRb.mass * Math.Pow(10, massMultiplier);
        mu = planetMass * G;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        speedSlider.onValueChanged.AddListener(updateTimescale);
        planetMassSlider.onValueChanged.AddListener(updatePlanetMass);

        //for(int i = 0; i <= numberOfReferenceBodies; i++)
        //{
            //how to reference the varying number of reference bodies? 
            //check out gameobject id system for 
            //1) get the gameobject id if applicable and 2) put them in variable sized array
        //}

        

        Vector3 tempPosition = rb.position;
        Vector3 tempVelocity = Velocity;
        Vector3 tempThrust = new Vector3(0f, 0f, 0f);

        Vector3 direction = referencePlanet.transform.position - rb.position;
        float r = direction.magnitude;
        float acceleration = (float)(G * planetMass) / (r * r);
        Vector3 accelerationVector = acceleration * direction.normalized;
        direction = referencePlanet2.transform.position - rb.position;
        r = direction.magnitude;
        acceleration = (float)(G * planetMass) / (r * r);
        accelerationVector += acceleration * direction.normalized;
        movementManager(ref accelerationVector);
        tempVelocity += accelerationVector * Time.fixedDeltaTime;
        tempPosition += tempVelocity * Time.fixedDeltaTime;


        rb.MovePosition(tempPosition);
        Velocity = tempVelocity;



    }

    void movementManager(ref Vector3 aVec)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("upAndDown"); //y is vertical 
        aVec = aVec + new Vector3(x, y, z);
    }

    private void updateTimescale(float value)
    {
        Time.timeScale = value;
    }

    private void updatePlanetMass(float value)
    {
        planetMass = planetMass = planetRb.mass * Math.Pow(10, value);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Time.timeScale = 0;
        sessionOver.SetActive(true);
    }

}
