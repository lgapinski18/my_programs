using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningUI : MonoBehaviour
{
    [SerializeField] public CleaningProductsInventory inventory;
    [SerializeField] public GameObject border;
    [SerializeField] public GameObject slider;
    [SerializeField] public GameObject target;
    [Range(0.1f, 2.0f)]
    [SerializeField] public float slider_speed;

    private float width;
    private float acc_bonus;
    private float y;
    private float idx;

    private void Awake()
    {
        acc_bonus = target.GetComponent<RectTransform>().rect.width / 2 + slider.GetComponent<RectTransform>().rect.width;
        width = border.GetComponent<RectTransform>().rect.width;
        y = border.GetComponent<RectTransform>().localPosition.y;
        Debug.Log(width);
        idx = 0;
    }

    private void MoveSlider()
    {
        idx += Time.deltaTime * slider_speed * Mathf.PI;
        slider.GetComponent<RectTransform>().localPosition = new Vector2(Mathf.Sin(idx) * width / 2, y);
    }

    private void NewTarget()
    {
        float new_x = Random.Range(-width/2, width/2);
        target.GetComponent<RectTransform>().localPosition = new Vector2(new_x, y);
    }

   
    // Update is called once per frame
    void Update()
    {
        MoveSlider();

        if (Input.GetMouseButtonDown(0))
        {
            if(Mathf.Abs(slider.GetComponent<RectTransform>().localPosition.x - target.GetComponent<RectTransform>().localPosition.x) <= acc_bonus)
            {
                //Debug.Log(inventory.cleaning_power);
                transform.parent.GetComponent<InteractableStain>().DealDmg();
                NewTarget();
            }
        }

        if (idx > 2 * Mathf.PI)
        {
            idx = 0.01f * Mathf.PI;
        }
    }
}
