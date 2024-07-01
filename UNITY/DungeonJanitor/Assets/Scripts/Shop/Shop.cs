using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int money = 0;
    [SerializeField]
    public List<GameObject> shopItems;
    [SerializeField]
    public List<ShopItem> items;
    [SerializeField]
    public GameObject moneyDisplay;
    private GameManager gm;
    [SerializeField]
    private List<GameObject> itemPopUps;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        UpdateShopState();

        List<Transform> tmp = GetChildren(transform);
        tmp = GetChildren(tmp[0]);
        tmp = GetChildren(tmp[0]);
        for (int i = 0; i < tmp.Count; i++)
        {
            shopItems.Add(tmp[i].gameObject);
            //shopItems[i].AddComponent<ShopItem>();
        }
        
        money = gm.money;
        

        if (gm.shopCounter == 0)
        {
            GameObject.Find("BeginingTMT").GetComponent<TutorialMessageTrigger>().TutorialManualTrigger();
        }

        gm.shopCounter += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.shopCounter == 1)
        {
            Cursor.visible = true;
        }

        for (int i = 0; i < shopItems.Count; i++)
        {
            List<Transform> tmp = GetChildren(shopItems[i].transform);
            if (i >= items.Count || items[i].currentNumber == 0)
            {
                tmp[0].gameObject.SetActive(false);
                shopItems[i].GetComponent<Image>().color = new UnityEngine.Color(1f, 1f, 1f, 0f);
            }
            else
            {
                shopItems[i].SetActive(true);
                shopItems[i].GetComponent<UnityEngine.UI.Image>().sprite = items[i].icon;
                
                tmp = GetChildren(tmp[0]);
                tmp = GetChildren(tmp[0]);
                tmp[0].gameObject.GetComponent<Text>().text = items[i].itemPrice.ToString();
                tmp[1].gameObject.GetComponent<Text>().text = items[i].currentNumber.ToString() + "/" + items[i].maxNumber.ToString();
            }
        }
        moneyDisplay.GetComponent<Text>().text = money.ToString();

    }

    public void BuyShopItem(int index)
    {
        itemPopUps[index].SetActive(false);
        if (money - items[index].itemPrice >= 0)
        {
            items[index].BuyItem();
            money -= items[index].itemPrice;
            gm.boughtItems.Add(items[index]);
        }

    }

    public void OpenItemPopUp(int index)
    {
        itemPopUps[index].SetActive(true);
    }

    public void CloseItemPopUp(int index)
    {
        itemPopUps[index].SetActive(false);
    }

    public void CloseShop()
    {
        gm.money = money;

        Cursor.visible = false;
        gm.NextShift();
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }

    private void UpdateShopState()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].currentNumber = items[i].maxNumber;
        }
        for (int i = 0; i < gm.boughtItems.Count; i++) 
        { 
            for (int j = 0; j < items.Count; j++) 
            {
                if (items[j].itemName == gm.boughtItems[i].itemName)
                {
                    items[j].currentNumber -= 1;
                }
            }
        }
    }
}
