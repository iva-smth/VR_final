using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public InputActionProperty TapButton;
    public Transform playerCamera; // ������ �� ������ ������ 
    public Transform menu;
    public float distanceFromPlayer = 2.0f; // ���������� ����� ������� 
    public bool isMenuCalled = false;

    public void MoveInFront()
    {/*
        if (playerCamera != null)
        {
            // ������������ ������� ����� ������� �� �������� ���������� 
            Vector3 newPosition = playerCamera.position + playerCamera.forward * distanceFromPlayer; 

            // ������������� ������� ������� 
            menu.position = newPosition;

            // ������������ ������, ����� �� ��� ��������� �� ������ 
            menu.LookAt(new Vector3(playerCamera.position.x, transform.position.y, playerCamera.position.z)); 
        }
        */

        
    }

    private void OnEnable()
    {
        // �������� �� ������� 
        TapButton.action.performed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // ������� �� ������� 
        TapButton.action.performed -= OnButtonPressed;
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("hi");
        if (!isMenuCalled)
        {
            menu.gameObject.SetActive(true);
            MoveInFront();
        }
        else
        {
            menu.gameObject.SetActive(false);
            menu.transform.position = new Vector3(0f, 100f, 0f);
        }
        isMenuCalled = !isMenuCalled;
    }
}
