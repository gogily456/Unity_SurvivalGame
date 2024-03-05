using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    
    public static PlayerState Instance { get; set; }

    //---- PlayerHealth -----//
    public float currentHealth, maxHealth;




    //---- PlayerCalories -----//
    public float currentCalories, maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPostion;

    public GameObject playerBody;



    //---- PlayerHydration -----//
    public float currentHydration, maxHydration;

    public bool isHydrationActive;



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


    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydration = maxHydration;


        StartCoroutine(decreaceHydration());
    }

    IEnumerator decreaceHydration()
    {
        while (true)
        {
            currentHydration -= 1;
            yield return new WaitForSeconds(10);
        }
    }

    // Update is called once per frame
    void Update()
    {



        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPostion);
        lastPostion = playerBody.transform.position;

        if(distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }


        //testing the health bar
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }
}
