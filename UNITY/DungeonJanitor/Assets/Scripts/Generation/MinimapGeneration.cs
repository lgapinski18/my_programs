using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGeneration : MonoBehaviour
{
    [SerializeField] public int height;
    [SerializeField] public GameObject traversable_minimap_prefab;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject janitor_portrait;
    [SerializeField] public GameObject wall_minimap_prefab;
    [SerializeField] public GameObject main_camera;
    [SerializeField] public GameObject minimap_camera;
    private GameObject dungeon_minimap;
    [HideInInspector] public HashSet<Vector2> rooms_and_corridors_set;
    [HideInInspector] public HashSet<Vector2> walls_set;



    public void GenerateMinimap()
    {
        if(dungeon_minimap is not null)
        {
            DestroyImmediate(dungeon_minimap);
        }
        dungeon_minimap = new GameObject();
        dungeon_minimap.transform.name = "Dunegon_minimap";
        dungeon_minimap.transform.position = new Vector3(0.0f, 0.0f, height);

        GameObject rooms_and_corridors_minimap = new GameObject();
        rooms_and_corridors_minimap.transform.name = "Rooms_and_corridors_minimap";
        rooms_and_corridors_minimap.transform.position = dungeon_minimap.transform.position;
        rooms_and_corridors_minimap.transform.parent = dungeon_minimap.transform;

        GameObject walls_minimap = new GameObject();
        walls_minimap.transform.name = "Walls_minimap";
        walls_minimap.transform.position = dungeon_minimap.transform.position;
        walls_minimap.transform.parent = dungeon_minimap.transform;

        foreach (Vector2 traversable in rooms_and_corridors_set)
        {
            GameObject new_tile = Instantiate(traversable_minimap_prefab);
            new_tile.transform.parent = rooms_and_corridors_minimap.transform;
            new_tile.transform.position = new Vector3(traversable.x, traversable.y, height);
        }

        foreach (Vector2 wall in walls_set)
        {
            GameObject new_tile = Instantiate(wall_minimap_prefab);
            new_tile.transform.parent = walls_minimap.transform;
            new_tile.transform.position = new Vector3(wall.x, wall.y, height);
        }
    }

    private void FixedUpdate()
    {
        minimap_camera.transform.position = new Vector3(main_camera.transform.position.x, main_camera.transform.position.y, height * 2);
        janitor_portrait.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, height);
    }
}
