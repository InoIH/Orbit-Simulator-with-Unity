using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text speedDisplay;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private GameObject normalUIBumo; //assign 'nominal' UI empty parent
    [SerializeField] private Button returnToMainMenu;
    [SerializeField] private GameObject escMenuBumo;
    

    
    private Boolean isUIshown = true;
    //private double dumbCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        returnToMainMenu.onClick.AddListener(mainMenu);

        
    }

    // Update is called once per frame
    void Update()
    {
        speedDisplay.SetText($"{speedSlider.value}x multiplied");
        if (Input.GetKeyDown(KeyCode.T))
        {
            isUIshown =  !isUIshown;
            normalUIBumo.SetActive(isUIshown);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("getkeydownentered");
            if (escMenuBumo.activeSelf) //on, and will turn off
            {
                escMenuBumo.SetActive(false);
                normalUIBumo.SetActive(isUIshown);
                Time.timeScale = 1;
            }
            else //off, and will turn on
            {
                escMenuBumo.SetActive(true);
                normalUIBumo.SetActive(false);
                Time.timeScale = 0;
            }


        }

        //dumbCounter =+ 1;
        
        //if (dumbCounter == 50)
        //{
         //   Debug.Log("dumbcounterINitiated");
          //  ButtonMovementJangsik(escMenuBumo);
            //dumbCounter = 0;

        //}
            
        

    }
    void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void ButtonMovementJangsik(GameObject gm)
    {
        for(int i = 0; i < 50; i++)
        {
            gm.transform.position = new Vector3(0, 1, 0);
            Debug.Log("i5020");
        }
        for (int i = 50; i > 0; i--)
        {
            gm.transform.position = new Vector3(0, -1, 0);
            Debug.Log("i0250");
        }
    }
}
