using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IAttackable, IStainAffected
{
    public List<AInteractable> InteractableObjects = new List<AInteractable>();
    public int selectedInterObj = -1;

    [Header("UI Data")]
    public GameObject ShiftNumberPanel;
    public ShiftTimeManager TimePanel;
    public GameObject TaskPanel;
    public GameObject TaskList;
    private bool taskPanelIsActive = false;
    //public GameObject SummaryTaskPanel;
    //public GameObject SummaryTaskContent;
    //public RankPanel rankPanel = null;

    private ControllHint InteractionControllHint = null;
    private ControllHint SelectPrevControllHint = null;
    private ControllHint SelectNextControllHint = null;


    [Header("Inventory Data")]
    public GameObject inventoryBar;
    [SerializeField]
    public int inventorySize = 3;
    //public InputActionReference nextItem;
    //public InputActionReference prevItem;
    //public InputActionReference dropItem;

    [Header("Health Data")]
    public int currentHealth;
    [SerializeField]
    public int maxHealth = 3;
    [SerializeField]
    public GameObject healthBar;
    public bool isDamageable = true;
    private float damageCooldown = 1.0f;
    public MovementComponent movementComponent;
    // Start is called before the first frame update
    void Start()
    {
        // Reloading tool objects and items
        ReloadToolItems();

        movementComponent = GetComponent<MovementComponent>();

        // Applying bought items
        ApplyBoughtItems();

        //movementComponent.speed = speedModifier;

        // Player health setup
        currentHealth = maxHealth;
        healthBar.AddComponent<HealthDisplay>().maxHealth = maxHealth;

        // Find inventory bar object
        Inventory inventory1 = new Inventory(inventorySize);

        // Starting items
        //GameObject blueDetergent = Instantiate(Resources.Load<GameObject>("Prefabs/BlueDetergent"));
        //GameObject redDetergent = Instantiate(Resources.Load<GameObject>("Prefabs/RedDetergent"));
        //GameObject greenDetergent = Instantiate(Resources.Load<GameObject>("Prefabs/GreenDetergent"));
        GameObject mop = Instantiate(Resources.Load<GameObject>("Prefabs/Mop"));


        //// Trap testing
        //TrapManager tm = ScriptableObject.CreateInstance<TrapManager>();
        //tm.GenerateTrap(this.GetComponent<Transform>().position.x + 1, this.GetComponent<Transform>().position.y, Trap.TrapType.Spike, 5);
        //tm.GenerateTrap(this.GetComponent<Transform>().position.x - 1, this.GetComponent<Transform>().position.y, Trap.TrapType.Fire, 7);
        //tm.GenerateTrap(this.GetComponent<Transform>().position.x, this.GetComponent<Transform>().position.y, Trap.TrapType.Crush, 6);

        //// Corpses testing
        //CorpseManager cm = ScriptableObject.CreateInstance<CorpseManager>();
        //cm.GenerateCorpse(this.GetComponent<Transform>().position.x + 3, this.GetComponent<Transform>().position.y, Corpse.CorpseSize.Normal, Corpse.CorpseType.Flesh);
        //cm.GenerateCorpse(this.GetComponent<Transform>().position.x + 3, this.GetComponent<Transform>().position.y, Corpse.CorpseSize.Small, Corpse.CorpseType.Rock, Corpse.SmallCorpseSubType.Arm);
        //cm.GenerateCorpse(this.GetComponent<Transform>().position.x + 3, this.GetComponent<Transform>().position.y, Corpse.CorpseSize.Normal, Corpse.CorpseType.Bone);


        // Pick up starting items
        inventoryBar.AddComponent<InventoryDisplay>().inventory = inventory1;
        inventoryBar.GetComponent<InventoryDisplay>().PickUpItem(mop);
        //inventoryBar.GetComponent<InventoryDisplay>().PickUpItem(redDetergent);
        //inventoryBar.GetComponent<InventoryDisplay>().PickUpItem(greenDetergent);
        //inventoryBar.GetComponent<InventoryDisplay>().PickUpItem(blueDetergent);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metody zwi¹zane z taskami
    public void OnChangeVisibilityTaskPanel(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        taskPanelIsActive = !taskPanelIsActive;
        TaskPanel.SetActive(taskPanelIsActive);
    }

    //Metody zwi¹zane z interakcjami

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        if ((selectedInterObj < 0))
        {
            return;
        }


        if (InteractableObjects[selectedInterObj].interact())
        {
            AInteractable interactable = InteractableObjects[selectedInterObj];

            interactable.unselectInteractable();

            if (selectedInterObj == (InteractableObjects.Count - 1))
            {
                selectedInterObj -= 1;
            }

            InteractableObjects.Remove(interactable);

            if (selectedInterObj > -1)
            {
                Debug.Log(InteractableObjects[selectedInterObj].getObjectName());
                InteractableObjects[selectedInterObj].selectInteractable();
            }

            Destroy(interactable.gameObject);
        }


        /*if (InteractableObjects.Count == 0 && InteractionControllHint != null)
        {
            ControllHintsManager.Instance().RemoveControllHint(InteractionControllHint);
        }

        if (InteractableObjects.Count == 1 && SelectNextControllHint != null)
        {
            ControllHintsManager.Instance().RemoveControllHint(SelectNextControllHint);
            ControllHintsManager.Instance().RemoveControllHint(SelectPrevControllHint);
        }/**/
    }

    public void OnSelectPrevInteractable(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        InteractableObjects[selectedInterObj].unselectInteractable();
        selectedInterObj = selectedInterObj + InteractableObjects.Count - 1;
        selectedInterObj %= InteractableObjects.Count;
        InteractableObjects[selectedInterObj].selectInteractable();
    }

    public void OnSelectNextInteractable(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        //Debug.Log("OnSelectNextInteractable!");
        InteractableObjects[selectedInterObj].unselectInteractable();
        selectedInterObj += 1;
        selectedInterObj %= InteractableObjects.Count;
        InteractableObjects[selectedInterObj].selectInteractable();
    }


    //Metody zwi¹zane z kolizjami

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            Debug.Log(other.gameObject.GetComponent<AInteractable>().getObjectName());
            InteractableObjects.Add(other.gameObject.GetComponent<AInteractable>());

            if (selectedInterObj == -1)
            {
                selectedInterObj = 0;
                InteractableObjects[selectedInterObj].selectInteractable();
            }

            /*if (InteractableObjects.Count > 0 && InteractionControllHint == null)
            {
                InteractionControllHint = ControllHintsManager.Instance().AddControllHint(ControllHintsManager.Keys.E, "WejdŸ w interakcje");
            }

            if (InteractableObjects.Count > 1 && SelectNextControllHint == null)
            {
                SelectPrevControllHint = ControllHintsManager.Instance().AddControllHint(ControllHintsManager.Keys.LEFT, "Zmieñ obiekt interakcji");
                SelectNextControllHint = ControllHintsManager.Instance().AddControllHint(ControllHintsManager.Keys.RIGHT, "Zmieñ obiekt interakcji");
            }/**/
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }

        if (other.tag == "Interactable")
        {
            if (InteractableObjects.Contains(other.gameObject.GetComponent<AInteractable>()))
            {
                InteractableObjects[selectedInterObj].unselectInteractable();

                if (selectedInterObj == (InteractableObjects.Count - 1))
                {
                    selectedInterObj -= 1;
                }

                InteractableObjects.Remove(other.gameObject.GetComponent<AInteractable>());

                if (selectedInterObj > -1)
                {
                    InteractableObjects[selectedInterObj].selectInteractable();
                }

                /*if (InteractableObjects.Count == 0 && InteractionControllHint != null)
                {
                    ControllHintsManager.Instance().RemoveControllHint(InteractionControllHint);
                }

                if (InteractableObjects.Count == 1 && SelectNextControllHint != null)
                {
                    ControllHintsManager.Instance().RemoveControllHint(SelectNextControllHint);
                    ControllHintsManager.Instance().RemoveControllHint(SelectPrevControllHint);
                }/**/
            }
        }
    }

    //Metody zwi¹zane z zarz¹dzaniem ekwipunkiem
    public void OnNextInventoryItem(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }
        UnityEngine.Debug.Log("Next item");
        inventoryBar.GetComponent<InventoryDisplay>().inventory.ShiftLeft();
    }

    public void OnPrevInventoryItem(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }
        UnityEngine.Debug.Log("Previous item");
        inventoryBar.GetComponent<InventoryDisplay>().inventory.ShiftRight();
    }

    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }
        UnityEngine.Debug.Log("Drop item");
        inventoryBar.GetComponent<InventoryDisplay>().DropItem();
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }
        UnityEngine.Debug.Log("Use item");
        inventoryBar.GetComponent<InventoryDisplay>().UseItem();
    }

    public void Attack(AttackArgs args)
    {
        Debug.Log("Player Attack");
        if (isDamageable)
        {
            foreach (AttackEffect attackEffect in args.attackEffects)
            {
                switch (attackEffect.Type)
                {
                    case AttackEffect.AttackType.KNOCKBACK:
                        {
                            movementComponent.ApplyKnockback(transform.position - args.source.transform.position, attackEffect.Magnitude, attackEffect.Time);
                        }
                        break;
                    case AttackEffect.AttackType.DAMAGE:
                        {
                            Debug.Log("Player takes damage");
                            currentHealth--;
                            healthBar.GetComponent<HealthDisplay>().UpdateDisplay(currentHealth, maxHealth);
                            if (currentHealth <= 0)
                            {
                                Debug.Log("Player died");
                                PlayerDeath();
                            }

                        }
                        break;
                }
            }
            StartCoroutine(StartDamageCooldown());
        }
    }

    public IEnumerator StartDamageCooldown()
    {
        isDamageable = false;

        yield return new WaitForSeconds(damageCooldown);

        isDamageable = true;
    }

    private void PlayerDeath()
    {
        GameManager.instance.FinishShift(true);
    }

    private void ReloadToolItems()
    {
        GameObject toolItem = GameObject.Find("RedDetergent");
        if (toolItem != null)
        {
            toolItem.GetComponent<InstanceItemContainer>().item.itemType.ReloadItem();
            toolItem.GetComponent<InstanceItemContainer>().item.itemType.toolObject.SetActive(false);
        }
        else
        {
            GameObject.Find("RedDetergent_v2").SetActive(false);
        }

        toolItem = GameObject.Find("GreenDetergent");
        if (toolItem != null)
        {
            toolItem.GetComponent<InstanceItemContainer>().item.itemType.ReloadItem();
            toolItem.GetComponent<InstanceItemContainer>().item.itemType.toolObject.SetActive(false);
        }
        else
        {
            GameObject.Find("GreenDetergent_v2").SetActive(false);
        }

        toolItem = GameObject.Find("BlueDetergent");
        if (toolItem != null)
        {
            toolItem.GetComponent<InstanceItemContainer>().item.itemType.ReloadItem();
            toolItem.GetComponent<InstanceItemContainer>().item.itemType.toolObject.SetActive(false);
        }
        else
        {
            GameObject.Find("BlueDetergent_v2").SetActive(false);
        }
    }

    private void ApplyBoughtItems()
    {
        List<ShopItem> items = GameManager.instance.boughtItems;
        for (int i = 0; i < items.Count; i++) 
        {
            items[i].ApplyItem(this.gameObject);
        }
    }
}