using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator INSTANCE;                      // the singleton instance for the levelgenerator
    public int ZENROOMS_CLIMBED = 0;

    public GameObject EndRoomTemplate;                          // the last room
    public GameObject MazeRoom;                                 // a maze room

    public int initialTowerHeight = 5;

    private const string BOSS_ROOMS = "BOSS";                   // Identifiers for special rooms (BONUS ?)

    public static float initialTimeScale;                       // the standard time scale value
    public static float initialFixedTimeScale;                  // the standard time scale value (FOR RIGIDBODIES !!!)

    public static int SEED = 14441;                             // the seed for the current level ---> players can share seeds
    public static int currentLevel = 1;                         // the current level index 
    private WaterRising waterRising;                            // reference to the rising fluids ---> must reset after next level load !
    private Camera gameCamera;                                  // reference to the game camera (to reset to a new level --> avoid camera and player being out of sync after "cutscene")
    private Vector3 initialCameraPosition;                      // reference to the initial camera position
    private Transform playerTransform;                          // reference to the player

    private Dictionary<string, List<GameObject>> roomPrefabs;   // a library of prefabs to chose from 
    private List<GameObject> currentRooms;                      // a list of currently spawned rooms
    private Vector3 currentSpawnPosition = Vector3.zero;        // the current position to spawn a room at


    private Vector3 initialCameraOffset;                        // the initial camera offset
    private Vector3 initialPlayerOffset;                        // the initial player offset
    private Vector3 initialLiquidOffset;                        // the initial liquid offset

    private Vector3 currentDoorPosition;                        // the door position 

    public static void StoreCurrentLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("Current_Level", currentLevel);
    }

    public static void LoadCurrentLevel()
    {
        currentLevel = PlayerPrefs.GetInt("Current_Level");
        if (currentLevel < 1)
        {
            currentLevel = 1;
        }
    }

    public static void ResetLevel()
    {
        currentLevel = 1;
        PlayerPrefs.SetInt("Current_Level", currentLevel);
    }

    public void ReloadLevel()
    {
        //basically we use the seed to reconstruct everything
        OnDisable();
        //Respawn the rooms
        SpawnRooms();
        //Move the player to the initial position
    }

    //we need to load a list of prefabs based on their "difficulty" (from 1-5)
    private void Awake()
    {
        if (INSTANCE == null)
        {
            ResetLevel();
            LevelGenerator.LoadCurrentLevel();
            currentRooms = new List<GameObject>();
            gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
            initialCameraPosition = gameCamera.transform.parent.position;
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

            DontDestroyOnLoad(this.gameObject); //--> Why would this reset the current level?


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

    void SpawnChallengeRooms()
    {
        gameCamera.transform.parent.position = initialCameraPosition;
        currentSpawnPosition = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            AddMazeRoom();
        }
        if (Random.value > 0.3)
        {
            AddRoom("1");
        }
        else
        {
            AddMazeRoom();
        }
        for (int i = 0; i < 3; i++)
        {
            AddMazeRoom();
        }
        AddTowerSeal();
        //we need to seal the bottom of the first room
        currentRooms[0].GetComponentInChildren<PistonDoorAnimation>().SealRoom();
        LevelTrigger.currentRoom = currentRooms[0].GetComponent<LevelTrigger>();
        //move the player to the spawn of the first room
        GameObject Player = GameObject.FindObjectOfType<Health>().gameObject;
        if (Player)
        {
            Player.transform.position = LevelTrigger.currentRoom.spawn.position;
            Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Spawns rooms for the current level, adds a boss room at the end
    /// </summary>
    void SpawnNormalRooms()
    {
        gameCamera.transform.parent.position = initialCameraPosition;
        currentSpawnPosition = Vector3.zero;
        //TODO fix later ---> JUST keeping it at 1 for now ... put everything in here and hope nobody notices !

        int tmpToFix = 1;
        AddMazeRoom();
        for (int i = 0; i < initialTowerHeight + currentLevel; i++)
        {
            AddRoom(tmpToFix.ToString()).name = "Level_" + currentLevel + "_" + (i + 1);
            AddMazeRoom();
        }
        if (currentLevel >= 5)
        {
            AddRoom(BOSS_ROOMS).name = "Level_" + currentLevel + "_Boss";
        }
        else
        {
            AddTowerSeal();
        }
        //we need to seal the bottom of the first room
        currentRooms[0].GetComponentInChildren<PistonDoorAnimation>().SealRoom();
        LevelTrigger.currentRoom = currentRooms[0].GetComponent<LevelTrigger>();
        //move the player to the spawn of the first room
        GameObject Player = GameObject.FindObjectOfType<Health>().gameObject;
        if (Player)
        {
            Player.transform.position = LevelTrigger.currentRoom.spawn.position;
            Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

    }

    /// <summary>
    /// Add a random room for the identifier provided
    /// </summary>
    /// <param name="identifier"></param>
    GameObject AddRoom(string identifier)
    {
        GameObject roomPrefab = roomPrefabs[identifier][Random.Range(0, roomPrefabs[identifier].Count)];
        GameObject roomInstance = Instantiate(roomPrefab, transform);
        roomInstance.transform.position = currentSpawnPosition;
        currentSpawnPosition = new Vector3(currentSpawnPosition.x, currentSpawnPosition.y + roomInstance.GetComponent<BoxCollider2D>().bounds.size.y, currentSpawnPosition.z);
        currentRooms.Add(roomInstance);
        /*    var roomDoorObject = roomInstance.transform.Find("DOOR_TRIGGER").gameObject;
            if (roomDoorObject.gameObject)
            {
                Vector3 tmp = roomDoorObject.gameObject.transform.position;
                roomDoorObject.gameObject.transform.position = currentDoorPosition;
                currentDoorPosition = tmp;
            }*/
        return roomInstance;
    }

    /// <summary>
    /// Add a sealing room for the identifier provided
    /// </summary>
    GameObject AddTowerSeal()
    {
        GameObject roomInstance = Instantiate(EndRoomTemplate, transform);
        roomInstance.transform.position = currentSpawnPosition;
        currentSpawnPosition = new Vector3(currentSpawnPosition.x, currentSpawnPosition.y + roomInstance.GetComponent<BoxCollider2D>().bounds.size.y, currentSpawnPosition.z);
        currentRooms.Add(roomInstance);
        return roomInstance;
    }

    /// <summary>
    /// Add a sealing room for the identifier provided
    /// </summary>
    GameObject AddMazeRoom()
    {
        GameObject roomInstance = Instantiate(MazeRoom, transform);
        roomInstance.transform.position = currentSpawnPosition;
        currentSpawnPosition = new Vector3(currentSpawnPosition.x, currentSpawnPosition.y + roomInstance.GetComponent<BoxCollider2D>().bounds.size.y, currentSpawnPosition.z);
        currentRooms.Add(roomInstance);
        return roomInstance;
    }

    private void OnDisable()
    {
        //reset the seed
        if (PreferencesManager.INSTANCE && !PreferencesManager.INSTANCE.ZEN_MODE)
        {
            Random.InitState(42);
            Random.InitState(SEED);
        }

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
        if (PreferencesManager.INSTANCE && !PreferencesManager.INSTANCE.ZEN_MODE)
        {
            waterRising.risingSpeed = 0;
        }
    }

    void SpawnRooms()
    {
        if (PreferencesManager.INSTANCE && !PreferencesManager.INSTANCE.ZEN_MODE)
        {
            SpawnNormalRooms();
        }
        else
        {
            SpawnChallengeRooms();
        }
    }

    void OnEnable()
    {
        SpawnRooms();
    }

}
