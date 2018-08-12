using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator INSTANCE;                      // the singleton instance for the levelgenerator


    private const string BOSS_ROOMS = "BOSS";                   // Identifiers for special rooms (BONUS ?)

    public static float initialTimeScale;                       // the standard time scale value
    public static float initialFixedTimeScale;                  // the standard time scale value (FOR RIGIDBODIES !!!)

    public int SEED = 14441;                                    // the seed for the current level ---> players can share seeds
    public int currentLevel = 1;                                // the current level index 

    private WaterRising waterRising;                            // reference to the rising fluids ---> must reset after next level load !
    private Camera gameCamera;                                  // reference to the game camera (to reset to a new level --> avoid camera and player being out of sync after "cutscene")
    private Transform playerTransform;                          // reference to the player

    private Dictionary<string, List<GameObject>> roomPrefabs;   // a library of prefabs to chose from 
    private List<GameObject> currentRooms;                      // a list of currently spawned rooms
    private Vector3 currentSpawnPosition = Vector3.zero;        // the current position to spawn a room at


    private Vector3 initialCameraOffset;                        // the initial camera offset
    private Vector3 initialPlayerOffset;                        // the initial player offset
    private Vector3 initialLiquidOffset;                        // the initial liquid offset




    //we need to load a list of prefabs based on their "difficulty" (from 1-5)
    private void Awake()
    {
        if (INSTANCE == null)
        {
            currentRooms = new List<GameObject>();
            gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            waterRising = GameObject.FindObjectOfType<WaterRising>();

            initialCameraOffset = gameCamera.transform.position;
            initialPlayerOffset = playerTransform.transform.position;
            initialLiquidOffset = waterRising.transform.position;

            INSTANCE = this;
            LevelGenerator.initialFixedTimeScale = Time.fixedDeltaTime;
            LevelGenerator.initialTimeScale = Time.timeScale;
            Random.InitState(SEED);

            /*-------

            I removed this because I restart the game by reloading the scene.

            -------*/

            //DontDestroyOnLoad(this.gameObject); //--> Why would this reset the current level?


        }
        else
        {
            GameObject.Destroy(this);
        }

        roomPrefabs = new Dictionary<string, List<GameObject>>();

        //LOAD BOSS ROOMS
        LoadRooms(BOSS_ROOMS);
        //LOAD NORMAL ROOMS

        for (int i = 1; i < 5; i++)
        {
            LoadRooms(i.ToString());
        }

    }

    /// <summary>
    /// Loads the rooms under the identifier (FOLDER NAME IN PREFABS !!!!)
    /// </summary>
    /// <param name="identifier"></param>
    void LoadRooms(string identifier)
    {
        List<GameObject> rooms;
        if (!roomPrefabs.ContainsKey(identifier))
        {
            rooms = new List<GameObject>();
        }
        else
        {
            rooms = roomPrefabs[identifier];
        }

        Object[] lv1Rooms = Resources.LoadAll("Prefab/Rooms/" + identifier, typeof(GameObject));
        foreach (Object aPrefab in lv1Rooms)
        {
            rooms.Add((GameObject)aPrefab);
        }
        roomPrefabs.Add(identifier, rooms);
    }

    /// <summary>
    /// Spawns rooms for the current level, adds a boss room at the end
    /// </summary>
    void SpawnRooms()
    {
        for (int i = 0; i < 4; i++)
        {
            AddRoom(currentLevel.ToString());
        }
        AddRoom(BOSS_ROOMS);
        //we need to seal the bottom of the first room
        currentRooms[0].GetComponentInChildren<PistonDoorAnimation>().SealRoom();

    }

    /// <summary>
    /// Add a random room for the identifier provided
    /// </summary>
    /// <param name="identifier"></param>
    void AddRoom(string identifier)
    {
        GameObject roomPrefab = roomPrefabs[identifier][Random.Range(0, roomPrefabs[identifier].Count)];
        GameObject roomInstance = Instantiate(roomPrefab, transform);
        roomInstance.transform.position = currentSpawnPosition;
        currentSpawnPosition = new Vector3(currentSpawnPosition.x, currentSpawnPosition.y + roomInstance.GetComponent<BoxCollider2D>().bounds.size.y, currentSpawnPosition.z);
        currentRooms.Add(roomInstance);
    }

    private void OnDisable()
    {
        foreach (GameObject currentRoom in currentRooms)
        {
            GameObject.Destroy(currentRoom);
        }
        currentRooms.Clear();
        currentSpawnPosition = Vector3.zero;
        gameCamera.transform.position = initialCameraOffset;
        if (playerTransform != null)
        {
            playerTransform.position = initialPlayerOffset;
        }
        //reset the liquid as well !
        waterRising.transform.position = initialLiquidOffset;
        waterRising.speedIncrease += currentLevel / 20;
        waterRising.risingSpeed = 0;
    }

    void OnEnable()
    {
        SpawnRooms();
    }

}
