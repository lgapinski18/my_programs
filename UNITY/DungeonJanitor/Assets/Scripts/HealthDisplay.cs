using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private int currentHealth;
    public int maxHealth;
    public List<Transform> healthContainers;
    private Sprite fullContainer;
    private Sprite emptyContainer;

    // Start is called before the first frame update
    void Start()
    {
        fullContainer = Resources.Load<Sprite>("sprites/heart");
        emptyContainer = Resources.Load<Sprite>("sprites/background");
        currentHealth = maxHealth;
        Transform tmp = GetChildren(transform)[0];
        tmp = GetChildren(tmp)[0];
        healthContainers = GetChildren(tmp);
        GameObject healthConainer = healthContainers[0].gameObject;
        for(int i = 1; i < maxHealth; i++) 
        { 
            GameObject clone = Instantiate(healthConainer);
            clone.transform.SetParent(tmp.transform);
            clone.transform.localScale = Vector3.one;
        }
        healthContainers = GetChildren(tmp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateDisplay(int health, int maximumHealth)
    {
        currentHealth = health;
        maxHealth = maximumHealth;
        int tmp = currentHealth;
        bool lastHit = false;
        for (int i = 0; i < maxHealth; i++) 
        {
            if (currentHealth - i > 0)
            {
                healthContainers[i].GetComponent<UnityEngine.UI.Image>().sprite = fullContainer;
            }
            else
            {
                if (!lastHit)
                {
                    //healthContainers[i].GetComponent<Animator>().Play("HealthAnimation");
                }
                lastHit = true;
                healthContainers[i].GetComponent<UnityEngine.UI.Image>().sprite = emptyContainer;
            }
        }
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
}
