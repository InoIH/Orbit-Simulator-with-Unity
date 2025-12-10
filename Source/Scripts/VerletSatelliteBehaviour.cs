using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class VerletSatelliteBehaviour : MonoBehaviour
{
    public Vector3 Velocity;
    public GameObject referencePlanet;
   // public GameObject lineRenderer;
    public const double G = 6.674e-11;
    public double massMultiplier;
    public double planetMass;
    public double mu;
    public double radius;
    //public because may be referenced by orbit rendering in the future

    [SerializeField] private float dt = 0.01f;
    //private float qdt = 0.01f;
    //[SerializeField] private float targetdt;

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider planetMassSlider;
    [SerializeField] private GameObject sessionOver; //gameover? its not game tho

    [SerializeField] private float r1 = 0;
    [SerializeField] private float r2 = 0;
    [SerializeField] private float r3 = 0;


  


    Rigidbody rb;
    Rigidbody planetRb;
    LineRenderer lr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ///targetdt = dt;

       rb = GetComponent<Rigidbody>();
         planetRb = referencePlanet.GetComponent<Rigidbody>();
// lr = lineRenderer.GetComponent<LineRenderer>();

        //lr.startColor = Color.grey;
        //lr.SetPosition(0, rb.position);

        double satelliteMass = rb.mass;
        planetMass = planetRb.mass * Math.Pow(10, massMultiplier);
        mu = planetMass * G;


        speedSlider.onValueChanged.AddListener(updateTimescale);
        planetMassSlider.onValueChanged.AddListener(updatePlanetMass);

        StartCoroutine(VariableTimeStep());

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

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

    private void apoperifinder(float rad)
    {
        r1 = r2;
        r2 = r3;
        r3 = rad;

        if (r1 != 0)
        {
            if (r2 > r1 && r2 > r3) //r2 is max
            {
                Debug.Log(r2 + 1000); //apoapsis
            }
            if (r2 < r1 && r2 < r3)
            {
                Debug.Log(r2 + 2000);
            } //r2 is min

        }
    }

    //private void adaptivedt()
    //{
    //    if (radius < 20)
    //    {
    //        targetdt = 0.001f;

    //    }
    //    else if (radius < 50)
    //    {
    //        targetdt = 0.01f;
    //    }
    //    else
    //    {
    //        targetdt = 0.05f;
    //    }

    //}

    private IEnumerator VariableTimeStep()
    {
        float singleFrameTime = 0f;
        while (true) 
        {
            
            singleFrameTime += Time.deltaTime;
            //adaptivedt();

            //qdt = Mathf.Lerp(qdt, targetdt, 0.1f);
            int counter = 0;

            int executions = 0;
                while (singleFrameTime >= dt)
                {
                
                    VerletCalculation();
                    singleFrameTime -= dt; //THIS ALLOWS VERLET TO BE PERFORMED singleFrameTime / dt TIMES PER FRAME.
                executions++;
                    if (executions > 1000)
                {
                    singleFrameTime = 0f;
                    break;
                }
                counter++;
                }
            Debug.Log($"{counter} Counted, radius {radius}");
            yield return null; 
        }

    }

    private void VerletCalculation()
    {
        //VELOCITY VERLET HAPPENS IN FOLLOWING STEPS:
        //1. FIND v(t + 1/2dt) = v(t) + a(t)(1/2dt) (. THIS IS AVERAGE VELOCITY BETWEEN TWO POINTS (IN dt)
        //2. FIND x(t + dt) = x(t) + v(t + 1/2dt)dt. MULTIPLYING AVERAGE VELOCITY WITH TIME YIELDS TOTAL DISTANCE TRAVELLED IN dt
        //3. CALCULATE a(t + dt) FROM THE POSITION ARRIVED (x(t + dt)). 
        //4. FIND v(t + dt) = v(t + 1/2dt) + 1/2a(t + dt)dt. REPEAT. 


      
        Vector3 positionv = rb.position;
        Vector3 velocityv = Velocity;

        Vector3 direction = referencePlanet.transform.position - rb.position;
        float r = direction.magnitude;
        radius = r;
        float acceleration = (float)(G * planetMass) / (r * r);
        Vector3 accelerationv = acceleration * direction.normalized;
        movementManager(ref accelerationv); //ACCELERATION CALCULATION

        velocityv = velocityv + accelerationv * dt / 2; //STEP 1
        positionv = positionv + velocityv * dt; //STEP 2
        rb.MovePosition(positionv);

        direction = referencePlanet.transform.position - rb.position;
        r = direction.magnitude;
        radius = r;
        acceleration = (float)(G * planetMass) / (r * r);
        accelerationv = acceleration * direction.normalized;
        movementManager(ref accelerationv); //STEP 3; ACCELERATION CALCULATION 2

        Velocity = velocityv + accelerationv * dt / 2; //STEP 4

        apoperifinder(r);

    }

}
