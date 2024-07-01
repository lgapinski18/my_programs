using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentCleanersUiScript : MonoBehaviour
{
    [SerializeField] public CleaningProductsInventory inventory;
    [SerializeField] public GameObject cleaning_products_picker_ui;
    [SerializeField] public GameObject[] slots;
    [SerializeField] public Sprite[] cleaning_products_sprites;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            cleaning_products_picker_ui.SetActive(true);
        }
        else
        {
            cleaning_products_picker_ui.SetActive(false);
        }

        slots[0].transform.GetComponent<Image>().sprite = cleaning_products_sprites[(int)inventory.Zero()];
        slots[1].transform.GetComponent<Image>().sprite = cleaning_products_sprites[(int)inventory.One()];
    }
}
