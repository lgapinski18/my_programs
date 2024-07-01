using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;
//using UnityEngine.UI;

public class ProceduralGenerationAlgorithms
{
    private float tile_w;
    private float tile_h;

    private enum DIRECTIONS
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    };

    private float[] probabilites =
    {
        0.25f,
        0.25f,
        0.25f,
        0.25f
    };

    public Vector2[] directions = new Vector2[4];
    /*{
        new Vector2(0.0f, 1.0f),  // up
        new Vector2(0.0f, -1.0f), // down
        new Vector2(1.0f, 0.0f),  // right
        new Vector2(-1.0f, 0.0f), // left
    };*/

    public ProceduralGenerationAlgorithms() { }

    public ProceduralGenerationAlgorithms(float w, float h) 
    {
        this.tile_h = h;
        this.tile_w = w;

        directions[0] = new Vector2(0.0f, h);
        directions[1] = new Vector2(0.0f, -h);
        directions[2] = new Vector2(w, 0.0f);
        directions[3] = new Vector2(-w, 0.0f);
    }

    public Vector2 GetRandomDirection()
    {
        Vector2 return_value = new Vector2();   

        float probability = UnityEngine.Random.Range(0.0f, 1.0f);
        
        if(probability < probabilites[0])
        {
            return_value = directions[(int)DIRECTIONS.UP];

            float tmp = probabilites[1];
            probabilites[1] = 0.0f;
            probabilites[2] += tmp * 0.5f;
            probabilites[3] += tmp * 0.5f;

            tmp = probabilites[0];
            probabilites[0] = tmp * 0.2f;
            probabilites[2] += tmp * 0.4f;
            probabilites[3] += tmp * 0.4f;

        }
        else if((probability >= probabilites[0]) && ( probability < (probabilites[0] + probabilites[1]) ))
        {
            return_value = directions[(int)DIRECTIONS.DOWN];

            float tmp = probabilites[0];
            probabilites[0] = 0.0f;
            probabilites[2] += tmp * 0.5f;
            probabilites[3] += tmp * 0.5f;

            tmp = probabilites[1];
            probabilites[1] = tmp * 0.2f;
            probabilites[2] += tmp * 0.4f;
            probabilites[3] += tmp * 0.4f;
        }
        else if( (probability >= (probabilites[0] + probabilites[1])) && (probability < (probabilites[0] + probabilites[1] + probabilites[2])) )
        {
            return_value = directions[(int)DIRECTIONS.RIGHT];

            float tmp = probabilites[3];
            probabilites[3] = 0.0f;
            probabilites[0] += tmp * 0.5f;
            probabilites[1] += tmp * 0.5f;

            tmp = probabilites[2];
            probabilites[2] = tmp * 0.2f;
            probabilites[0] += tmp * 0.4f;
            probabilites[1] += tmp * 0.4f;

        }
        else if( (probability >= (probabilites[0] + probabilites[1] + probabilites[2])) && (probability < 1.0f) )
        {
            return_value = directions[(int)DIRECTIONS.LEFT];

            float tmp = probabilites[2];
            probabilites[2] = 0.0f;
            probabilites[0] += tmp * 0.5f;
            probabilites[1] += tmp * 0.5f;

            tmp = probabilites[3];
            probabilites[3] = tmp * 0.2f;
            probabilites[0] += tmp * 0.40f;
            probabilites[1] += tmp * 0.40f;
        }

        //Debug.Log($"UP: {probabilites[0]}, DOWN: {probabilites[1]}, RIGHT: {probabilites[2]}, LEFT: {probabilites[3]}");

        return return_value;
    }

    public HashSet<Vector2> RandomWalk(Vector2 start_position, int lenght, HashSet<Vector2> walls)
    {
        probabilites[0] = 0.25f;
        probabilites[1] = 0.25f;
        probabilites[2] = 0.25f;
        probabilites[3] = 0.25f;

        HashSet<Vector2> return_value = new HashSet<Vector2>();

        return_value.Add(start_position);
        Vector2 prevois_position = start_position;

        for (int i = 0; i < lenght; i++)
        {
            Vector2 new_position = prevois_position + GetRandomDirection();
            return_value.Add(new_position);

            AddWalls(new_position, walls);

            prevois_position = new_position;
        }

        return return_value;
    }

    public HashSet<Vector2> ConnectStartPositions(Vector2[] start_positions, HashSet<Vector2> rooms, HashSet<Vector2> walls)
    {
        HashSet < Vector2 > return_value = new HashSet<Vector2>();

        int i = 0;
        int j = 1;

        while(i < start_positions.Length) 
        {
            if((j ==  start_positions.Length) &&  (i == start_positions.Length - 1))
            {
                j = 0;
            }

            Vector2 connect_point = new Vector2(start_positions[i].x, start_positions[j].y);
            if(!rooms.Contains(connect_point) && (GetAmountOfNeighbours(connect_point, rooms) < 5))
            {
                return_value.Add(connect_point);
                AddWalls(connect_point, walls);
            }
            

            int missing_x_tiles = (int)Math.Abs(connect_point.x - start_positions[j].x);
            int missing_y_tiles = (int)Math.Abs(connect_point.y - start_positions[i].y);

            for( int x = 1 ; x <= missing_x_tiles; x++ ) 
            {
                int dir;
                Vector2 new_position; 

                if (connect_point.x > start_positions[j].x)
                {
                    dir = (int)DIRECTIONS.LEFT;
                    new_position = connect_point + x * directions[dir];
                }
                else
                {
                    dir = (int)DIRECTIONS.RIGHT;
                    new_position = connect_point + x * directions[dir];
                }

                if (!rooms.Contains(new_position) && (GetAmountOfNeighbours(new_position, rooms) < 5))
                {
                    return_value.Add(new_position);
                    AddWalls(new_position, walls);
                }
            }

            for (int y = 1; y <= missing_y_tiles; y++)
            {
                int dir;
                Vector2 new_position;

                if(connect_point.y > start_positions[i].y)
                {
                    dir = (int)DIRECTIONS.DOWN; 
                    new_position = connect_point + y * directions[dir];
                }
                else
                {
                    dir = (int)DIRECTIONS.UP;
                    new_position = connect_point + y * directions[dir];
                }

                if(!rooms.Contains(new_position) && (GetAmountOfNeighbours(new_position, rooms) < 5))
                {
                    return_value.Add(new_position);
                    AddWalls(new_position, walls);
                }
                
            }

            i++;
            j++;
        }

        return return_value;
    }

    public HashSet<Vector2> RandomWalks(Vector2[] start_positions, int[] lenghts, HashSet<Vector2> walls)
    {
        HashSet<Vector2> return_value = new HashSet<Vector2>();
        
        if (lenghts.Length == start_positions.Length) 
        {
            int amount = lenghts.Length;

            for (int i = 0; i < amount; i++)
            {
                HashSet<Vector2> internal_return_value = RandomWalk(start_positions[i], lenghts[i], walls);
                return_value.UnionWith(internal_return_value);
            }
        }
        return return_value;
    }

    /*
     * room_num - number of rooms
     * size - radius of the circle on which all points will be places if size_vairance = 0
     * size_variance - maximum percentage of radius that will be subtracted form the radius upon creation of the new point
     */
    public Vector2[] GenerateStartPositions(int room_num, int size, float size_variance, Transform transform)
    {
        Vector2[] return_value = new Vector2[room_num];
        List<Vector2> start_positions = new List<Vector2>();
        
        start_positions.Add(transform.position); 

        float angle = 360.0f / ((float) room_num);

        for(int i = 0; i < room_num - 1; i++)
        {
            float internal_angle = angle * i;
            float internal_size = size - (size * UnityEngine.Random.Range(0.0f, size_variance));

            int x = (int)(internal_size * Math.Cos(internal_angle));
            int y = (int)(internal_size * Math.Sin(internal_angle));

            Vector2 new_point = new Vector2(x, y);

            start_positions.Add(new_point);
        }

        return_value = start_positions.ToArray();
        return return_value;
    }

    public int[] GenerateRoomSizes(int room_num, int min, int max)
    {
        int[] return_value = new int[room_num];
        List<int> room_sizes = new List<int>();

        for(int i = 0; i < room_num; i++)
        {
            room_sizes.Add( UnityEngine.Random.Range(min, max) );
        }

        return_value = room_sizes.ToArray();
        return return_value;
    }

    public int GetAmountOfNeighbours(Vector2 position, HashSet<Vector2> set)
    {
        int return_value = 0;

        //Top
        if (set.Contains(new Vector2(position.x - tile_w, position.y + tile_h)))
        {
            return_value += 1;
        }

        if (set.Contains(new Vector2(position.x, position.y + tile_h)))
        {
            return_value += 1;
        }

        if (set.Contains(new Vector2(position.x + tile_w, position.y + tile_h)))
        {
            return_value  += 1;
        }

        //Center
        if (set.Contains(new Vector2(position.x - tile_w, position.y)))
        {
            return_value += 1;
        }

        if (set.Contains(new Vector2(position.x + tile_w, position.y)))
        {
            return_value += 1;
        }

        //Bottom
        if (set.Contains(new Vector2(position.x - tile_w, position.y - tile_h)))
        {
            return_value += 1;
        }

        if (set.Contains(new Vector2(position.x, position.y - tile_h)))
        {
            return_value += 1;
        }

        if (set.Contains(new Vector2(position.x + tile_w, position.y - tile_h)))
        {
            return_value += 1;
        }

        return return_value;
    }

    public void AddWalls(Vector2 position, HashSet<Vector2> set)
    {
        set.Remove(position);

        set.Add(new Vector2 (position.x - tile_w, position.y + tile_h));
        set.Add(new Vector2(position.x, position.y + 1));
        set.Add(new Vector2(position.x + tile_w, position.y + tile_h));

        set.Add(new Vector2(position.x - tile_w, position.y));
        set.Add(new Vector2(position.x + tile_w, position.y));

        set.Add(new Vector2(position.x - tile_w, position.y - tile_h));
        set.Add(new Vector2(position.x, position.y - 1));
        set.Add(new Vector2(position.x + tile_w, position.y - tile_h));
    }

    /*public HashSet<Vector2> GenerateStains(GameObject floors, GameObject corridors, float stain_frequency)
    {
        HashSet<Vector2> return_value = new HashSet<Vector2>();

        for (int i = 0; i < floors.transform.childCount; i++)
        {
            float probability = UnityEngine.Random.Range(0.0f, 1.0f);
            if(probability <= stain_frequency)
            {
                Transform tile = floors.transform.GetChild(i);
                int idx = UnityEngine.Random.Range(0, 16);
                Vector2 position = tile.GetChild(idx).position;
                return_value.Add(position);
            }
        }

        for (int i = 0; i < corridors.transform.childCount; i++)
        {
            float probability = UnityEngine.Random.Range(0.0f, 1.0f);
            if (probability <= stain_frequency)
            {
                Transform tile = corridors.transform.GetChild(i);
                int idx = UnityEngine.Random.Range(0, 16);
                Vector2 position = tile.GetChild(idx).position;
                return_value.Add(position);
            }
        }

        return return_value;
    }*/

    public HashSet<Vector2>[] GenerateStains(GameObject floors, int[] stains_counts)
    {
        HashSet<Vector2>[] return_value = new HashSet<Vector2>[stains_counts.Length];

        for(int i = 0; i < stains_counts.Length; i++)
        {
            return_value[i] = new HashSet<Vector2>();
        }

        if (floors.transform.childCount > stains_counts.Sum())
        {
            Random random = new Random();
            List<int> numbers = Enumerable.Range(0, floors.transform.childCount).OrderBy(n => random.Next()).Take(stains_counts.Sum()).ToList();

            int list_counter = 0;

            for(int i = 0; i < stains_counts.Length; i++) 
            {
                for(int j = 0; j < stains_counts[i]; j++)
                {
                    Transform tile = floors.transform.GetChild(numbers[list_counter]);
                    Vector2 position = tile.GetChild(random.Next(tile.childCount)).position;
                    return_value[i].Add(position);
                    list_counter++;
                }
            }

        }
        return return_value;
    }

    public HashSet<Vector2>[] GenerateMonsters(GameObject floors, int[] monsters_counts) 
    {
        HashSet<Vector2>[] return_value = new HashSet<Vector2>[monsters_counts.Length];

        for (int i = 0; i < monsters_counts.Length; i++)
        {
            return_value[i] = new HashSet<Vector2>();
        }

        if (floors.transform.childCount > monsters_counts.Sum())
        {
            Random random = new Random();
            List<int> numbers = Enumerable.Range(0, floors.transform.childCount).OrderBy(n => random.Next()).Take(monsters_counts.Sum()).ToList();

            int list_counter = 0;

            for (int i = 0; i < monsters_counts.Length; i++)
            {
                for (int j = 0; j < monsters_counts[i]; j++)
                {
                    Transform tile = floors.transform.GetChild(numbers[list_counter]);
                    Vector2 position = tile.GetChild(random.Next(tile.childCount)).position;
                    return_value[i].Add(position);
                    list_counter++;
                }
            }

        }

        return return_value;
    }

    public Vector2 PlaceDoorToRoom(HashSet<Vector2> walls, Transform transform)
    {
        Vector2 starting_position = transform.position;
        float[] distances = new float[walls.Count];
        foreach (var wall in walls)
        {
            distances.Append(Vector2.Distance(starting_position, wall));
        }

        int minIndex = Array.IndexOf(distances, distances.Min());
        return walls.ElementAt(minIndex);
    }

    public HashSet<Vector2>[] GenerateBodies(GameObject floors, HashSet<Vector2>[] stains_sets, int[] bodies_counts)
    {
        HashSet<Vector2>[] return_value = new HashSet<Vector2>[bodies_counts.Length];

        for (int i = 0; i < bodies_counts.Length; i++)
        {
            return_value[i] = new HashSet<Vector2>();
        }

        if (floors.transform.childCount > bodies_counts.Sum())
        {
            Random random = new Random();
            List<int> numbers = Enumerable.Range(0, floors.transform.childCount).OrderBy(n => random.Next()).Take(bodies_counts.Sum()).ToList();

            int list_counter = 0;

            for (int i = 0; i < bodies_counts.Length; i++)
            {
                for (int j = 0; j < bodies_counts[i]; j++)
                {
                    Transform tile = floors.transform.GetChild(numbers[list_counter]);
                    Vector2 position = tile.GetChild(random.Next(tile.childCount)).position;

                    bool can_add = true;
                    foreach (var set in stains_sets)
                    {
                        if(set.Contains(position))
                        {
                            can_add = false; 
                            break;
                        }
                    }

                    if(can_add)
                    {
                        return_value[i].Add(position);
                        list_counter++;
                    }
                    else
                    {
                        j--;
                    }
                   
                }
            }

        }
        
        return return_value;
    }

    public HashSet<Vector2>[] GenerateTraps(GameObject floors, HashSet<Vector2>[] stains_sets, int[] traps_counts)
    {
        HashSet<Vector2>[] return_value = new HashSet<Vector2>[traps_counts.Length];

        for (int i = 0; i < traps_counts.Length; i++)
        {
            return_value[i] = new HashSet<Vector2>();
        }

        if (floors.transform.childCount > traps_counts.Sum())
        {
            Random random = new Random();
            List<int> numbers = Enumerable.Range(0, floors.transform.childCount).OrderBy(n => random.Next()).Take(traps_counts.Sum()).ToList();

            int list_counter = 0;

            for (int i = 0; i < traps_counts.Length; i++)
            {
                for (int j = 0; j < traps_counts[i]; j++)
                {
                    Transform tile = floors.transform.GetChild(numbers[list_counter]);
                    Vector2 position = tile.GetChild(random.Next(tile.childCount)).position;

                    bool can_add = true;
                    foreach (var set in stains_sets)
                    {
                        if (set.Contains(position))
                        {
                            can_add = false;
                            break;
                        }
                    }

                    if (can_add)
                    {
                        return_value[i].Add(position);
                        list_counter++;
                    }
                    else
                    {
                        j--;
                    }

                }
            }

        }

        return return_value;
    }
}
