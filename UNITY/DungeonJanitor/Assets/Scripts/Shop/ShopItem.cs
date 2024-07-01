using UnityEngine;

abstract public class ShopItem : ScriptableObject
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public Sprite icon;
    [SerializeField]
    public int itemPrice;
    [SerializeField]
    public int maxNumber;
    [SerializeField]
    public int currentNumber;
    public bool reloaded = false;

    protected ShopItem() { }

    abstract public void BuyItem();

    abstract public void ApplyItem(GameObject player);

}