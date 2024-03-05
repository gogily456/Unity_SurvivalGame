using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager instance { get; set; }

    public bool onTarget;

    public GameObject selectedObject;

    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text;

    public Image centerCrosshairImage;
    public Image handIcon;


    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("pickable"))
                {
                    centerCrosshairImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    centerCrosshairImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                }


            }
            else//if there is a hit but without the inreractable script
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);

                centerCrosshairImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

        }
        else// if there is no hit at all 
        {

            onTarget = false;
            interaction_Info_UI.SetActive(false);

            centerCrosshairImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }
}

