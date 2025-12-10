using System;
using UnityEngine;
using UnityEngine.UI;


public class VerletSatelliteBehaviour4body : MonoBehaviour
{
    public Vector3 Velocity;
    public GameObject referencePlanet1;
    public GameObject referencePlanet2;
    public GameObject referencePlanet3;
   // public GameObject lineRenderer;
    public const double G = 6.674e-11;
    public double massMultiplier1;
    public double massMultiplier2;
    public double massMultiplier3;


    private double planetMass1;
    private double mu1;
    private double planetMass2;
    private double mu2;
    private double planetMass3;
    private double mu3;


    public double radius;
    //public because may be referenced by orbit rendering in the future

    [SerializeField] private const float dt = 0.01f; 


    [SerializeField] private float r1 = 0;
    [SerializeField] private float r2 = 0;
    [SerializeField] private float r3 = 0;



    Rigidbody rb;
    Rigidbody planetRb1;
    Rigidbody planetRb2;
    Rigidbody planetRb3;
    LineRenderer lr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();
         planetRb1 = referencePlanet1.GetComponent<Rigidbody>();
        planetRb2 = referencePlanet2.GetComponent<Rigidbody>();
        planetRb3 = referencePlanet3.GetComponent<Rigidbody>();

        double satelliteMass = rb.mass;
        planetMass1 = planetRb1.mass * Math.Pow(10, massMultiplier1);
        mu1 = planetMass1 * G;

        planetMass2 = planetRb2.mass * Math.Pow(10, massMultiplier2);
        mu2 = planetMass2 * G;

        planetMass3 = planetRb3.mass * Math.Pow(10, massMultiplier3);
        mu3 = planetMass2 * G;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //VELOCITY VERLET HAPPENS IN FOLLOWING STEPS:
        //1. FIND v(t + 1/2dt) = v(t) + a(t)(1/2dt) (. THIS IS AVERAGE VELOCITY BETWEEN TWO POINTS (IN dt)
        //2. FIND x(t + dt) = x(t) + v(t + 1/2dt)dt. MULTIPLYING AVERAGE VELOCITY WITH TIME YIELDS TOTAL DISTANCE TRAVELLED IN dt
        //3. CALCULATE a(t + dt) FROM THE POSITION ARRIVED (x(t + dt)). 
        //4. FIND v(t + dt) = v(t + 1/2dt) + 1/2a(t + dt)dt. REPEAT. 
        

        Vector3 positionv = rb.position;
        Vector3 velocityv = Velocity;
       
            Vector3 direction = referencePlanet1.transform.position - rb.position;
            float r = direction.magnitude;
        radius = r;
            float acceleration = (float)(G * planetMass1) / (r * r);
            Vector3 accelerationv = acceleration * direction.normalized;
            movementManager(ref accelerationv); //ACCELERATION CALCULATION NO.1

        direction = referencePlanet2.transform.position - rb.position;
        r = direction.magnitude;
        radius = r;
        acceleration = (float)(G * planetMass2) / (r * r);
        accelerationv += acceleration * direction.normalized; //NOTICE += SYMBOL HERE
        movementManager(ref accelerationv); //ACCELERATION CALCULATION NO.2

        direction = referencePlanet3.transform.position - rb.position;
        r = direction.magnitude;
        radius = r;
        acceleration = (float)(G * planetMass3) / (r * r);
        accelerationv += acceleration * direction.normalized; //NOTICE += SYMBOL HERE
        movementManager(ref accelerationv); //ACCELERATION CALCULATION NO.3



        velocityv = velocityv + accelerationv * dt / 2; //STEP 1
        positionv = positionv + velocityv * dt; //STEP 2
        rb.MovePosition(positionv);



         direction = referencePlanet1.transform.position - rb.position;
         r = direction.magnitude;
        radius = r;
         acceleration = (float)(G * planetMass1) / (r * r);
         accelerationv = acceleration * direction.normalized;
        movementManager(ref accelerationv); //STEP 3; ACCELERATION CALCULATION NO.1

        direction = referencePlanet2.transform.position - rb.position;
        r = direction.magnitude;
        radius = r;
        acceleration = (float)(G * planetMass2) / (r * r);
        accelerationv += acceleration * direction.normalized; //NOTICE += SYMBOL HERE
        movementManager(ref accelerationv); //STEP 3; ACCELERATION CALCULATION NO.2

        direction = referencePlanet3.transform.position - rb.position;
        r = direction.magnitude;
        radius = r;
        acceleration = (float)(G * planetMass3) / (r * r);
        accelerationv += acceleration * direction.normalized; //NOTICE += SYMBOL HERE
        movementManager(ref accelerationv); //STEP 3; ACCELERATION CALCULATION NO.2

        Velocity = velocityv + accelerationv * dt / 2; //STEP 4

        apoperifinder(r);

        

    }

    void movementManager(ref Vector3 aVec)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("upAndDown"); //y is vertical 
        aVec = aVec + new Vector3(x, y, z);
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

}
