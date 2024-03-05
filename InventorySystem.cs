using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotTOEquip;

    public bool isOpen;
    //public bool isFull;

    //pickup popup
    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public Image pickupImage;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;

        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);

            if (!CraftingSystem.instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            isOpen = false;
        }
    }

    public void addToInventory(string itemName)
    {

        whatSlotTOEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotTOEquip.transform.position, whatSlotTOEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotTOEquip.transform);

        itemList.Add(itemName);

        triggerPickupPop(itemName, itemToAdd.GetComponent<Image>().sprite);

        reCalculateList();
        CraftingSystem.instance.refreshNeededItem();
    }

    void triggerPickupPop(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }

        }
        return new GameObject();
    }

    public bool checkIfFull()
    {
        int count = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                count++;
            }

        }

        if (count == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void removeItem(string nameToRemove, int amountToRemove)
    {
        int count = amountToRemove;

        for(var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && count != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    count --;
                }
            }
        }

        reCalculateList();
        CraftingSystem.instance.refreshNeededItem();
    }

    public void reCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name; // stone (Clone)

                string str2 = "(Clone)";

                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }

}
