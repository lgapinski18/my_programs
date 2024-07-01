using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public float smooth;
    [SerializeField] public float target_speed;
    [SerializeField] public float x_offset_float;
    [SerializeField] public float y_offset_float;

    //private float velocity;
    private Vector3 x_offset_vector;
    private Vector3 y_offset_vector;
    private Vector3 z_vector;
    private Vector3 target_position;
    private Vector3 current_position;

    private void Awake()
    {
        //velocity = 0.0f;
        x_offset_vector = new Vector3(x_offset_float, 0.0f, 0.0f);
        y_offset_vector = new Vector3(0.0f, y_offset_float, 0.0f);
        z_vector = new Vector3(0.0f, 0.0f, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 tv = player.GetComponent<MovementComponent>().NormalizedMovementDirection;
            current_position = new Vector3(transform.position.x, transform.position.y, 0.0f);
            if ((tv.x == 0) && (tv.y == 0))
            {
                target_position = player.transform.position;
            }

            else if ((tv.x < 0) && (tv.y > 0))
            {
                target_position = player.transform.position - x_offset_vector + y_offset_vector;
            }
            else if ((tv.x == 0) && (tv.y > 0))
            {
                target_position = player.transform.position + y_offset_vector;
            }
            else if ((tv.x > 0) && (tv.y > 0))
            {
                target_position = player.transform.position + x_offset_vector + y_offset_vector;
            }

            else if ((tv.x < 0) && (tv.y == 0))
            {
                target_position = player.transform.position - x_offset_vector;
            }
            else if ((tv.x > 0) && (tv.y == 0))
            {
                target_position = player.transform.position + x_offset_vector;
            }

            else if ((tv.x < 0) && (tv.y < 0))
            {
                target_position = player.transform.position - x_offset_vector - y_offset_vector;
            }
            else if ((tv.x == 0) && (tv.y < 0))
            {
                target_position = player.transform.position - y_offset_vector;
            }
            else if ((tv.x > 0) && (tv.y < 0))
            {
                target_position = player.transform.position + x_offset_vector - y_offset_vector;
            }

            transform.position = Vector3.Lerp(current_position, target_position, smooth) + z_vector;

        }
    }

}
