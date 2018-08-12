using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
    //Our physics arrays
    float[] xpositions;
    float[] ypositions;
    float[] velocities;
    float[] accelerations;

    //Our meshes and colliders
    GameObject[] meshobjects;
    BoxCollider2D[] colliders;
    Mesh[] meshes;


    //The material we're using for the top of the water
    public Material mat;

    //The GameObject we're using for a mesh
    public GameObject watermesh;

    public GameObject splashPrefab;

    //All our constants
    public float springconstant = 0.02f;
    public float damping = 0.04f;
    public float spread = 0.05f;
    public float z = -1f;
    public float amplitude = 0.5f;
    public float delay = 0.5f;
    //The properties of our water
    float baseheight;
    float left;
    float bottom;



    void Start()
    {
        //Spawning our water
        SpawnWater();
    }

    bool check;
    float timer;

    private void Update()
    {
        if (Time.time > timer)
        {
            timer = Time.time + delay;
            int index = Random.Range(1, xpositions.Length - 1);
            Splash(xpositions[index], Random.Range(0, amplitude));
            GameObject splash = Instantiate(splashPrefab, new Vector3(xpositions[index], 3.14f * colliders[index].bounds.size.y + transform.position.y, transform.position.z), Quaternion.identity);
            splash.transform.SetParent(this.transform);
            GameObject.Destroy(splash, 2.5f);
        }

    }

    public void Splash(float xpos, float velocity)
    {
        //If the position is within the bounds of the water:
        if (xpos >= xpositions[0] && xpos <= xpositions[xpositions.Length - 1])
        {
            //Offset the x position to be the distance from the left side
            xpos -= xpositions[0];

            //Find which spring we're touching
            int index = Mathf.RoundToInt((xpositions.Length - 1) * (xpos / (xpositions[xpositions.Length - 1] - xpositions[0])));

            //Add the velocity of the falling object to the spring
            velocities[index] += velocity;

            //Set the lifetime of the particle system.
            float lifetime = 0.93f + Mathf.Abs(velocity) * 0.07f;

            //Set the correct position of the particle system.
            Vector3 position = new Vector3(xpositions[index], ypositions[index] - 0.35f, 5);

            //This line aims the splash towards the middle. Only use for small bodies of water:
            Quaternion rotation = Quaternion.LookRotation(new Vector3(xpositions[Mathf.FloorToInt(xpositions.Length / 2)], baseheight + 8, 5) - position);

        }
    }

    public void SpawnWater()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float Left = collider.bounds.min.x;
        float Width = collider.bounds.size.x;
        float Bottom = collider.bounds.min.y;
        float Top = collider.bounds.max.y;

        //Calculating the number of edges and nodes we have
        int edgecount = Mathf.RoundToInt(Width) * 5;
        int nodecount = edgecount + 1;

        //Declare our physics arrays
        xpositions = new float[nodecount];
        ypositions = new float[nodecount];
        velocities = new float[nodecount];
        accelerations = new float[nodecount];

        //Declare our mesh arrays
        meshobjects = new GameObject[edgecount];
        meshes = new Mesh[edgecount];
        colliders = new BoxCollider2D[edgecount];

        //Set our variables
        baseheight = Top;
        bottom = Bottom;
        left = Left;

        //For each node, set the line renderer and our physics arrays
        for (int i = 0; i < nodecount; i++)
        {
            ypositions[i] = Top;
            xpositions[i] = Left + Width * i / edgecount;
            accelerations[i] = 0;
            velocities[i] = 0;
        }

        //Setting the meshes now:
        for (int i = 0; i < edgecount; i++)
        {
            //Make the mesh
            meshes[i] = new Mesh();

            //Create the corners of the mesh
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
            Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
            Vertices[2] = new Vector3(xpositions[i], bottom, z);
            Vertices[3] = new Vector3(xpositions[i + 1], bottom, z);

            //Set the UVs of the texture
            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);

            //Set where the triangles should be.
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            //Add all this data to the mesh.
            meshes[i].vertices = Vertices;
            meshes[i].uv = UVs;
            meshes[i].triangles = tris;

            //Create a holder for the mesh, set it to be the manager's child
            meshobjects[i] = Instantiate(watermesh, new Vector3(0, 0, transform.position.z), Quaternion.identity) as GameObject;
            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.parent = transform;

            //Create our colliders, set them be our child
            GameObject tmp = new GameObject();
            colliders[i] = tmp.AddComponent<BoxCollider2D>();
            colliders[i].name = "Trigger";
            colliders[i].transform.parent = transform;

            //Set the position and scale to the correct dimensions
            colliders[i].transform.position = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, transform.position.z);
            colliders[i].transform.localScale = new Vector3(Width / edgecount, 0.2f, 1);

            //Add a WaterDetector and make sure they're triggers
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].gameObject.AddComponent<WaterDetector>();

        }




    }

    //Same as the code from in the meshes before, set the new mesh positions
    void UpdateMeshes()
    {
        for (int i = 0; i < meshes.Length; i++)
        {

            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
            Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
            Vertices[2] = new Vector3(xpositions[i], bottom, z);
            Vertices[3] = new Vector3(xpositions[i + 1], bottom, z);

            meshes[i].vertices = Vertices;
        }
    }

    //Called regularly by Unity
    void FixedUpdate()
    {
        //Here we use the Euler method to handle all the physics of our springs:
        for (int i = 0; i < xpositions.Length; i++)
        {
            float force = springconstant * (ypositions[i] - baseheight) + velocities[i] * damping;
            accelerations[i] = -force;
            ypositions[i] += velocities[i];
            velocities[i] += accelerations[i];
        }

        //Now we store the difference in heights:
        float[] leftDeltas = new float[xpositions.Length];
        float[] rightDeltas = new float[xpositions.Length];

        //We make 8 small passes for fluidity:
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xpositions.Length; i++)
            {
                //We check the heights of the nearby nodes, adjust velocities accordingly, record the height differences
                if (i > 0)
                {
                    leftDeltas[i] = spread * (ypositions[i] - ypositions[i - 1]);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < xpositions.Length - 1)
                {
                    rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }

            //Now we apply a difference in position
            for (int i = 0; i < xpositions.Length; i++)
            {
                if (i > 0)
                    ypositions[i - 1] += leftDeltas[i];
                if (i < xpositions.Length - 1)
                    ypositions[i + 1] += rightDeltas[i];
            }
        }
        //Finally we update the meshes to reflect this
        UpdateMeshes();
    }



}
