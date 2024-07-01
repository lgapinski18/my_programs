//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Generator : MonoBehaviour, IGenerator
{
    [Header("Map generation")]
    [SerializeField] public GameObject[] tile_prefabs;
    [SerializeField] private int room_num;
    [SerializeField] private int size;
    [SerializeField] private float size_variance;
    [SerializeField] private int min_room_size;
    [SerializeField] private int max_room_size;
    [SerializeField] private float tile_width;
    [SerializeField] private float tile_height;

    [Header("Dungeon elements")]
    [SerializeField] public GameObject door_to_room;
    
    [Header("Stains")]
    [SerializeField] public GameObject[] stains_prefabs;
    [SerializeField] private int[] stains_counts;


    [Header("Bodies")]
    [SerializeField] public GameObject[] bodies_prefabs;
    [SerializeField] private int[] bodies_counts;

    [Header("Monsters")]
    [SerializeField] public GameObject[] monsters_prefabs;
    [SerializeField] private int[] monsters_counts;

    [Header("Traps")]
    [SerializeField] public GameObject[] traps_prefabs;
    [SerializeField] private int[] traps_counts;

    [HideInInspector] public HashSet<Vector2> walls_set; 
    [HideInInspector] public HashSet<Vector2> rooms_set;
    [HideInInspector] public HashSet<Vector2> corridors_set;

    [HideInInspector] public HashSet<Vector2>[] stains_sets;
    [HideInInspector] public HashSet<Vector2>[] monsters_sets;
    [HideInInspector] public HashSet<Vector2>[] bodies_sets;
    [HideInInspector] public HashSet<Vector2>[] traps_sets;

    [HideInInspector] public GameObject dungeon;
    [HideInInspector] public GameObject rooms;
    [HideInInspector] public GameObject corridors;
    [HideInInspector] public GameObject walls;
    [HideInInspector] public GameObject stains;
    [HideInInspector] public GameObject monsters;
    [HideInInspector] public GameObject bodies;
    [HideInInspector] public GameObject traps;

    [SerializeField]
    private MultiformSpriteExtractor wallExtractor = null;
    [SerializeField]
    private MultivariantSpritesheetExtractor dungeonWallExtractor = null;

    private ProceduralGenerationAlgorithms algorithms;
    private Vector2[] start_positions;
    private int[] room_sizes;

    public int Room_num { get => room_num; set => room_num = value; }
    public int Size { get => size; set => size = value; }
    public float Size_variance { get => size_variance; set => size_variance = value; }
    public int Min_room_size { get => min_room_size; set => min_room_size = value; }
    public int Max_room_size { get => max_room_size; set => max_room_size = value; }
    public float Tile_width { get => tile_width; set => tile_width = value; }
    public float Tile_height { get => tile_height; set => tile_height = value; }
    public int[] Stains_counts { get => stains_counts; set => stains_counts = value; }
    public int[] Bodies_counts { get => bodies_counts; set => bodies_counts = value; }
    public int[] Monsters_counts { get => monsters_counts; set => monsters_counts = value; }
    public int[] Traps_counts { get => traps_counts; set => traps_counts = value; }

    private void Awake()
    {
        algorithms = new ProceduralGenerationAlgorithms(Tile_width, Tile_height);
        InitGenerator();
    }

    public void InitGenerator()
    {
        start_positions = algorithms.GenerateStartPositions(Room_num, Size, Size_variance, transform);
        room_sizes = algorithms.GenerateRoomSizes(Room_num, Min_room_size, Max_room_size);

        stains_sets = new HashSet<Vector2>[Stains_counts.Length];

        if(dungeon is not null)
        {
            DestroyImmediate(dungeon);
        }

        dungeon = new GameObject();
        rooms = new GameObject();
        walls = new GameObject();
        corridors = new GameObject();
        stains = new GameObject();
        monsters = new GameObject();
        bodies = new GameObject();
        traps = new GameObject();

        dungeon.transform.name = "Dunegon";
        dungeon.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        rooms.transform.name = "Rooms";
        rooms.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        rooms.transform.parent = dungeon.transform;
        rooms.transform.gameObject.layer = 8;

        corridors.transform.name = "Corridors";
        corridors.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        corridors.transform.parent = dungeon.transform;
        corridors.transform.gameObject.layer = 9;

        walls.transform.name = "Walls";
        walls.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        walls.transform.parent = dungeon.transform;
        walls.transform.gameObject.layer = 6;

        stains.transform.name = "Stains";
        stains.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        stains.transform.parent = dungeon.transform;
        stains.transform.gameObject.layer = 7;

        
        monsters.transform.name = "Monsters";
        monsters.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        monsters.transform.parent = dungeon.transform;
        monsters.transform.gameObject.layer = 7;

        bodies.transform.name = "Bodies";
        bodies.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        bodies.transform.parent = dungeon.transform;
        bodies.transform.gameObject.layer = 7;

        traps.transform.name = "Traps";
        traps.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        traps.transform.parent = dungeon.transform;
        traps.transform.gameObject.layer = 7;

        walls_set = new HashSet<Vector2>();
        rooms_set = new HashSet<Vector2>();
        corridors_set = new HashSet<Vector2>();
        stains_sets = new HashSet<Vector2>[Stains_counts.Length];
        monsters_sets = new HashSet<Vector2>[Monsters_counts.Length];
        bodies_sets = new HashSet<Vector2>[Bodies_counts.Length];
        traps_sets = new HashSet<Vector2>[Traps_counts.Length];
    }


    public void PopulateRoomsSet()
    {
        rooms_set = algorithms.RandomWalks(start_positions, room_sizes, walls_set);
    }

    public void CreateRoomsGameObjects()
    {
        foreach (Vector2 room in rooms_set)
        {
            GameObject new_tile = Instantiate(tile_prefabs[0], room, Quaternion.identity);
            new_tile.transform.parent = rooms.transform;
        }
    }

    public void GenerateRooms()
    {
        PopulateRoomsSet();
        CreateRoomsGameObjects();
    }


    public void PopulateCorridorsSet()
    {
        corridors_set = algorithms.ConnectStartPositions(start_positions, rooms_set, walls_set);
    }

    public void CreateCorridorsGameObjects()
    {
        foreach (Vector2 corridor in corridors_set)
        {
            GameObject new_tile = Instantiate(tile_prefabs[1], corridor, Quaternion.identity);
            new_tile.transform.parent = corridors.transform;
        }
    }

    public void GenerateCorridors()
    {
        PopulateCorridorsSet();
        CreateCorridorsGameObjects();
    }


    public void PrepareWallsSet()
    {
        foreach (Vector2 room in rooms_set)
        {
            walls_set.Remove(room);
        }

        foreach (Vector2 corridor in corridors_set)
        {
            walls_set.Remove(corridor);
        }
    }

    public void CreateWallGameObjects()
    {
        dungeonWallExtractor.CreateVariantsBasingOnSpritesheet();

        foreach (Vector2 wall in walls_set)
        {
            GameObject new_tile = Instantiate(tile_prefabs[2], wall, Quaternion.identity);
            new_tile.GetComponent<SpriteRenderer>().sprite = ExtractSpriteFromWallSpriteExtractor(wall, rooms_set, corridors_set, walls_set);
            new_tile.transform.parent = walls.transform;
        }
    }

    public void GenerateWalls()
    {
        PrepareWallsSet();
        CreateWallGameObjects();
    }


    public void PrepareStainsSet()
    {
        stains_sets = algorithms.GenerateStains(rooms, Stains_counts);
    }

    public void CreateStainsGameObjects()
    {
        int stain_prefab_idx = 0;
        for (int i = 0; i < stains_sets.Length; i++)
        {
            foreach (Vector2 stain in stains_sets[i])
            {
                //GameObject new_stain = Instantiate(stains_prefabs[stain_prefab_idx]);
                GameObject new_stain = Instantiate(stains_prefabs[stain_prefab_idx], stain, Quaternion.identity, stains.transform);
                new_stain.GetComponent<MainStain>().CreateSubtains();
                //new_stain.transform.parent = stains.transform;
                //new_stain.transform.position = stain;
            }
            stain_prefab_idx++;
        }
    }

    public void GenerateStains()
    {
        PrepareStainsSet();
        CreateStainsGameObjects();
    }


    public void PopulateMonstersSet()
    {
        monsters_sets = algorithms.GenerateMonsters(rooms, Monsters_counts);
    }

    public void CreateMonstersGameObjects()
    {
        int monster_prefab_idx = 0;
        for (int i = 0; i < monsters_sets.Length; i++)
        {
            foreach (Vector2 monster in monsters_sets[i])
            {
                GameObject new_monster = Instantiate(monsters_prefabs[monster_prefab_idx], monster, Quaternion.identity);
                new_monster.transform.parent = monsters.transform;
            }
            monster_prefab_idx++;
        }
    }

    public void GenerateMonsters()
    {
        PopulateMonstersSet();
        CreateMonstersGameObjects();
    }


    public void PopulateBodiesSet()
    {
        bodies_sets = algorithms.GenerateBodies(rooms, stains_sets, Bodies_counts);
    }

    public void CreateBodiesGameObjects()
    {
        int dead_body_prefab_idx = 0;
        for (int i = 0; i < bodies_sets.Length; i++)
        {
            foreach (Vector2 body in bodies_sets[i])
            {
                GameObject new_body = Instantiate(bodies_prefabs[dead_body_prefab_idx], body, Quaternion.identity);
                new_body.transform.parent = bodies.transform;
            }
            dead_body_prefab_idx++;
        }
    }

    public void GenerateBodies()
    {
        PopulateBodiesSet();
        CreateBodiesGameObjects();
    }

    public void PopulateTrapsSet()
    {
        traps_sets = algorithms.GenerateTraps(rooms, stains_sets, Traps_counts);
    }

    public void CreateTrapsGameObjects()
    {
        int trap_prefab_idx = 0;
        for (int i = 0; i < traps_sets.Length; i++)
        {
            foreach (Vector2 trap in traps_sets[i])
            {
                GameObject new_trap = Instantiate(traps_prefabs[trap_prefab_idx], trap, Quaternion.identity);
                new_trap.transform.parent = traps.transform;
            }
            trap_prefab_idx++;
        }
    }

    public void GenerateTraps()
    {
        PopulateTrapsSet();
        CreateTrapsGameObjects();
    }

    public void GenerateMinimap()
    {
        rooms_set.UnionWith(corridors_set);
        GetComponent<MinimapGeneration>().rooms_and_corridors_set = rooms_set;
        GetComponent<MinimapGeneration>().walls_set = walls_set;
        GetComponent<MinimapGeneration>().GenerateMinimap();
    }


    public void GenerateMagicDoor()
    {
        Vector2 door_to_room_position = algorithms.PlaceDoorToRoom(walls_set, transform);
        door_to_room.transform.position = (Vector3)door_to_room_position;
    }


    public void InstancePathFinderNodes()
    {
        PathfindingController.Instance.MapNodes();
    }

    public void AutoGenerate()
    {
        InitGenerator();
        GenerateRooms();
        GenerateCorridors();
        GenerateWalls();
        InstancePathFinderNodes();
        GenerateMagicDoor();
        GenerateStains();
        GenerateMonsters();
        GenerateBodies();
        GenerateMinimap();
    }

    bool idx = true;

    private void Start()
    {

        /*dungeon.transform.name = "Dunegon";
        dungeon.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        
        #region Rooms, Corridors, Walls
        
        
        rooms.transform.name = "Rooms";
        rooms.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        rooms.transform.parent = dungeon.transform;
        rooms.transform.gameObject.layer = 8;

        
        corridors.transform.name = "Corridors";
        corridors.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        corridors.transform.parent = dungeon.transform;
        corridors.transform.gameObject.layer = 9;

        
        walls.transform.name = "Walls";
        walls.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        walls.transform.parent = dungeon.transform;
        walls.transform.gameObject.layer = 6;

        walls_set = new HashSet<Vector2>();
        //rooms_set = algorithms.RandomWalks(start_positions, room_sizes, walls_set);

        //corridors_set = algorithms.ConnectStartPositions(start_positions, rooms_set, walls_set);
        */
        //GenerateRooms();
        //GenerateCorridors();
        //GenerateWalls();


        /*foreach (Vector2 room in rooms_set)
        {
            walls_set.Remove(room);
        }

        foreach (Vector2 corridor in corridors_set)
        {
            walls_set.Remove(corridor);
        }*/

        /*foreach (Vector2 room in rooms_set)
        {
            GameObject new_tile = Instantiate(tile_prefabs[0], room, Quaternion.identity);
            new_tile.transform.parent = rooms.transform;
        }*/

        /* foreach (Vector2 corridor in corridors_set)
         {
             GameObject new_tile = Instantiate(tile_prefabs[1], corridor, Quaternion.identity);
             new_tile.transform.parent = corridors.transform;
         }*/

        /*dungeonWallExtractor.CreateVariantsBasingOnSpritesheet();

        foreach (Vector2 wall in walls_set)
        {
            GameObject new_tile = Instantiate(tile_prefabs[2], wall, Quaternion.identity);
            new_tile.GetComponent<SpriteRenderer>().sprite = ExtractSpriteFromWallSpriteExtractor(wall, rooms_set, corridors_set, walls_set);
            new_tile.transform.parent = walls.transform;
        }*/

        //Mapping rooms and corridors to pathfidernodes
        //InstancePathFinderNodes();

        #region Dungeon Elements


        /*stains.transform.name = "Stains";
        stains.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        stains.transform.parent = dungeon.transform;
        stains.transform.gameObject.layer = 7;

        
        monsters.transform.name = "Monsters";
        monsters.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        monsters.transform.parent = dungeon.transform;
        monsters.transform.gameObject.layer = 7;

        
        bodies.transform.name = "Bodies";
        bodies.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        bodies.transform.parent = dungeon.transform;
        bodies.transform.gameObject.layer = 7;*/

        /*stains_sets = algorithms.GenerateStains(rooms, corridors, stains_counts);
        monsters_sets = algorithms.GenerateMonsters(rooms, corridors, rooms_set, monsters_counts);
        bodies_sets = algorithms.GenerateBodies(rooms, corridors, stains_sets, bodies_counts);*/
        /*Vector2 door_to_room_position = algorithms.PlaceDoorToRoom(walls_set, transform);

        door_to_room.transform.position = (Vector3)door_to_room_position;*/

        //GenerateMagicDoor();

        /*int stain_prefab_idx = 0;
        for (int i = 0; i < stains_sets.Length; i++)
        {
            foreach (Vector2 stain in stains_sets[i])
            {
                GameObject new_stain = Instantiate(stains_prefabs[stain_prefab_idx]);
                new_stain.transform.parent = stains.transform;
                new_stain.transform.position = stain;
            }
            stain_prefab_idx++;
        }

        int monster_prefab_idx = 0;
        for(int i = 0;i < monsters_sets.Length;i++)
        {
            foreach (Vector2 monster in monsters_sets[i])
            {
                GameObject new_monster = Instantiate(monsters_prefabs[monster_prefab_idx], monster, Quaternion.identity);
                new_monster.transform.parent = monsters.transform;
            }
            monster_prefab_idx++;
        }

        int dead_body_prefab_idx = 0;
        for (int i = 0; i < bodies_sets.Length; i++)
        {
            foreach (Vector2 body in bodies_sets[i])
            {
                GameObject new_body = Instantiate(bodies_prefabs[dead_body_prefab_idx], body, Quaternion.identity);
                new_body.transform.parent = bodies.transform;
            }
            dead_body_prefab_idx++;
        }*/

        //GenerateStains();
        //GenerateMonsters();
        //GenerateBodies();

        //GenerateMinimap();

        /*rooms_set.UnionWith(corridors_set);
        GetComponent<MinimapGeneration>().rooms_and_corridors_set = rooms_set;
        GetComponent<MinimapGeneration>().walls_set = walls_set;
        GetComponent<MinimapGeneration>().GenerateMinimap();*/

        #endregion

        //AutoGenerate();

        //StainsCleaningTaskManager.Instance().SetNumberOfStains(stains_set.Count());

        /*Debug.Log($"Wall count: {walls_set.Count()}");
        Debug.Log($"Room count: {rooms_set.Count()}");
        Debug.Log($"Corridor count: {corridors_set.Count()}");
        Debug.Log($"Stain count: {stains_sets.Count()}");
        Debug.Log($"Monster count: {monsters_sets.Count()}");*/


    }

    private Sprite ExtractSpriteFromWallSpriteExtractor(Vector2 wall, HashSet<Vector2> rooms_set, HashSet<Vector2> corridors_set, HashSet<Vector2> walls_set)
    {
        const int featureCount = 8;
        short[] featureVector = new short[featureCount];

        Vector2 dstVector = wall + (new Vector2(-1, 1));
        for (int i = 0; i < featureCount; i++)
        {
            //Debug.Log(i + "WallP: " + wall + "DstVector: " + dstVector);
            Vector2? foundVector = null;
            short type = 0;

            foreach (Vector2 wallPos in walls_set)
            {
                if (Vector2.Distance(dstVector, wallPos) < 0.1f)
                {
                    foundVector = wallPos;
                    type = 1;
                    break;
                }
            }
            if (foundVector == null)
            {
                foreach (Vector2 corridorPos in corridors_set)
                {
                    if (Vector2.Distance(dstVector, corridorPos) < 0.1f)
                    {
                        foundVector = corridorPos;
                        type = 2;
                        break;
                    }
                }
                if (foundVector == null)
                {
                    foreach (Vector2 floorPos in rooms_set)
                    {
                        if (Vector2.Distance(dstVector, floorPos) < 0.1f)
                        {
                            foundVector = floorPos;
                            type = 2;
                            break;
                        }
                    }
                }
            }
            if (i % 2 == 0)
            {
                if (type == 0 || type == 1)
                {
                    featureVector[i] += 0;
                }
                else if (type == 2)
                {
                    featureVector[i] = 1;
                }
            }
            else
            {
                if (type == 0)
                {
                    featureVector[i - 1] += 0;
                    featureVector[i] += 0;
                    featureVector[(i + 1) % 8] += 0;
                }
                else if (type == 1)
                {
                    featureVector[i - 1] += 2;
                    featureVector[i] = 0;
                    featureVector[(i + 1) % 8] += 2;
                }
                else if (type == 2)
                {
                    featureVector[i - 1] = 1;
                    featureVector[i] = 1;
                    featureVector[(i + 1) % 8] = 1;
                }
            }

            //Debug.Log(i + "WallP: " + wall + "DstVector: " + dstVector + " Type: " + type + " FV: " + featureVector[i] + " FV: " + foundVector);
            if (dstVector.x < wall.x && dstVector.y < wall.y)
            {
                dstVector.y += 1;
            }
            else if (dstVector.y < wall.y)
            {
                dstVector.x -= 1;
            }
            else if (dstVector.x > wall.x)
            {
                dstVector.y -= 1;
            }
            else if (dstVector.y > wall.y)
            {
                dstVector.x += 1;
            }
        }
        for (int j = 0; j < featureVector.Length; j++)
        {
            featureVector[j] %= 2;
        }


        string arrayString = "";
        foreach (short v in featureVector)
        {
            arrayString += v;
        }
        //Debug.Log("WallP: " + wall + "Extraction FV: " + featureVector.ToString());
        //Debug.Log("WallP: " + wall + "Extraction FV: " + arrayString);
        return dungeonWallExtractor.ExtractVariant(featureVector);
    }
}
