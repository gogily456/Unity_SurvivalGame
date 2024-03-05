using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance { get; set; }

    public GameObject craftingScreanUI;
    public GameObject toolsScreanUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN;

    //Craft Button
    Button craftAxeBTN;

    //Requirement Text
    TextMeshProUGUI AxeReq1, AxeReq2;

    public bool isOpen;

    //All Blueprints
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);
    

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        isOpen = false;

        toolsBTN = craftingScreanUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        //Axe
        AxeReq1 = toolsScreanUI.transform.Find("Axe").transform.Find("req1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = toolsScreanUI.transform.Find("Axe").transform.Find("req2").GetComponent<TextMeshProUGUI>();

        craftAxeBTN = toolsScreanUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }


    void Update()
    {
        //refreshNeededItem();

        if (Input.GetKeyDown(KeyCode.G) && !isOpen)
        {

            Debug.Log("i is pressed");
            craftingScreanUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.G) && isOpen)
        {
            craftingScreanUI.SetActive(false);
            toolsScreanUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            isOpen = false;
        }
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        Debug.Log("Crafting item: " + blueprintToCraft.itemName);

        InventorySystem.Instance.addToInventory(blueprintToCraft.itemName);

        if(blueprintToCraft.numOfRequirements == 1) 
        {
            InventorySystem.Instance.removeItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.removeItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
            InventorySystem.Instance.removeItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
        }

        StartCoroutine(calculate());

        //refreshNeededItem();
    }

    public IEnumerator calculate()
    {
        yield return 0;

        InventorySystem.Instance.reCalculateList();

        refreshNeededItem();
    }

    void OpenToolsCategory()
    {
        craftingScreanUI.SetActive(false);
        toolsScreanUI.SetActive(true);
    }

    public void refreshNeededItem()
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count++;
                    break;
                case "Stick":
                    stick_count++;
                    break;

            }
        }

        //-----AXE-----//
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if(stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }

}
