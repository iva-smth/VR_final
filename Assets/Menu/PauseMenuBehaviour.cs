using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuBehaviour : MonoBehaviour
{
    [SerializeField] InputActionReference secondaryButton;
    [SerializeField] GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float buttonValue = secondaryButton.action.ReadValue<float>();
        //Debug.Log(buttonValue);
        if (buttonValue == 1f)
        {
            menu.SetActive(true);
        }
    }
}
