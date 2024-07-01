using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AdditionalVisibilityChecker : MonoBehaviour
{
    [SerializeField] public int tmp_layer;
    [SerializeField] public float max_distance;
    private Transform player_transform;
    [Range(0f, 31f)]
    //public int[] visible_layers; 
    private LayerMask layer_mask;

    GameObject stains;
    GameObject monsters;
    GameObject bodies;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesHitTriggers = true;
        layer_mask = 0b0100_0000_0000_0000_0000_0000_0100_0000;
        stains = GetComponent<Generator>().stains;
        monsters = GetComponent<Generator>().monsters;
        bodies = GetComponent<Generator>().bodies;
    }

    // Update is called once per frame
    void Update()
    {
        player_transform = GameObject.Find("Player").transform;

        for (int i = 0; i < stains.transform.childCount; i++)
        {
            int current_layer = stains.transform.GetChild(i).gameObject.layer;
            stains.transform.GetChild(i).gameObject.layer = tmp_layer;
            Transform stain = stains.transform.GetChild(i);

            Vector3 direction = (stain.position - player_transform.position).normalized;

            RaycastHit2D hitInfo = Physics2D.Raycast(player_transform.position, direction, max_distance, layer_mask);

            if (hitInfo)
            {
                if (stain == hitInfo.transform)
                {
                    for (int j = 0; j < stain.childCount; j++)
                    {
                        if(stain.GetChild(j).GetComponent<SpriteRenderer>())
                        {
                            stain.GetChild(j).GetComponent<SpriteRenderer>().enabled = true;
                        }
                              
                    }
                }
                else
                {
                    for (int j = 0; j < stain.childCount; j++)
                    {
                        if(stain.GetChild(j).GetComponent<SpriteRenderer>())
                        {
                            stain.GetChild(j).GetComponent<SpriteRenderer>().enabled = false;
                        }
                        
                    }
                }
            }

            stains.transform.GetChild(i).gameObject.layer = current_layer;
        }

        
        for (int i = 0; i < bodies.transform.childCount; i++)
        {
            int current_layer = bodies.transform.GetChild(i).gameObject.layer;
            bodies.transform.GetChild(i).gameObject.layer = tmp_layer;

            Transform body = bodies.transform.GetChild(i);

            Vector3 direction = (body.position - player_transform.position).normalized;
            
            RaycastHit2D hitInfo = Physics2D.Raycast(player_transform.position, direction, max_distance, layer_mask);
            
            if (hitInfo)
            {
                if (hitInfo.transform == body)
                {
                    body.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    body.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            bodies.transform.GetChild(i).gameObject.layer = current_layer;
        }
        
        
        for (int i = 0; i < monsters.transform.childCount; i++)
        {
            //int current_layer = monsters.transform.GetChild(i).GetChild(0).gameObject.layer;
            int current_layer = monsters.transform.GetChild(i).gameObject.layer;
            //monsters.transform.GetChild(i).GetChild(0).gameObject.layer = tmp_layer;
            monsters.transform.GetChild(i).gameObject.layer = tmp_layer;

            //Transform monster = monsters.transform.GetChild(i).GetChild(0);
            Transform monster = monsters.transform.GetChild(i);

            Vector3 direction = (monster.position - player_transform.position).normalized;
            

            RaycastHit2D hitInfo = Physics2D.Raycast(player_transform.position, direction, max_distance, layer_mask);
            Debug.DrawRay(player_transform.position, direction * hitInfo.distance);

            if (hitInfo)
            {
                //if (hitInfo.collider == monster.GetComponent<PolygonCollider2D>())
                if (hitInfo.collider == monster.GetComponent<Collider2D>())
                {
                    Debug.Log("Visible");
                    monster.GetComponent<SpriteRenderer>().enabled = true;
                    //monster.transform.parent.GetComponent<SpriteRenderer>().enabled = true; 
                }
                else
                {
                    Debug.Log("Occluded");
                    monster.GetComponent<SpriteRenderer>().enabled = false;
                    //monster.transform.parent.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            //monsters.transform.GetChild(i).GetChild(0).gameObject.layer = current_layer;
            monsters.transform.GetChild(i).gameObject.layer = current_layer;
        }
    }
}
