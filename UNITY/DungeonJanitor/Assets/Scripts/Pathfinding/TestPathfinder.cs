using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPathfinder : MonoBehaviour
{
    public GameObject target = null;
    public GameObject player = null;
    public GameObject monster = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TestMapping(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //PathfindingController.Instance.MapNodes();
            target = Instantiate(monster, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        }
    }

    public void Test(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log(target.transform.position + "Clicked position: " + player.transform.position );
            target.GetComponent<MonsterMovementComponent>().SetDestination(player.transform.position);
        }
    }
}
