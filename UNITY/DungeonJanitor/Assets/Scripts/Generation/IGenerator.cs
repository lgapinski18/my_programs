using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IGenerator
{

    public int Room_num { get ; set ; }
    public int Size { get; set; }
    public float Size_variance { get; set; }
    public int Min_room_size { get; set; }
    public int Max_room_size { get; set; }
    public float Tile_width { get; set; }
    public float Tile_height { get; set ; }
    // NIE ZMIENIAC DLUGOSCI TYCH TABLIC, tylko wartosci elementow
    public int[] Stains_counts { get; set; }
    public int[] Bodies_counts { get; set; }
    public int[] Monsters_counts { get; set; }
    public int[] Traps_counts { get; set; }

    public void InitGenerator();


    public void PopulateRoomsSet();

    public void CreateRoomsGameObjects();

    public void GenerateRooms();


    public void PopulateCorridorsSet();

    public void CreateCorridorsGameObjects();

    public void GenerateCorridors();


    public void PrepareWallsSet();

    public void CreateWallGameObjects();

    public void GenerateWalls();


    public void PrepareStainsSet();

    public void CreateStainsGameObjects();

    public void GenerateStains();


    public void PopulateMonstersSet();

    public void CreateMonstersGameObjects();

    public void GenerateMonsters();


    public void PopulateBodiesSet();
    public void CreateBodiesGameObjects();
    public void GenerateBodies();

    public void PopulateTrapsSet();
    public void CreateTrapsGameObjects();
    public void GenerateTraps();

    public void GenerateMinimap();


    public void GenerateMagicDoor();


    public void InstancePathFinderNodes();

    public void AutoGenerate();
}
