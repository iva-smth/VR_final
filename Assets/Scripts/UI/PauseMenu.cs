using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public InputActionProperty TapButton;
    public Transform playerCamera; // Ссылка на камеру игрока 
    public Transform menu;
    public float distanceFromPlayer = 2.0f; // Расстояние перед игроком 
    public bool isMenuCalled = false;

    public void MoveInFront()
    {/*
        if (playerCamera != null)
        {
            // Рассчитываем позицию перед игроком на заданном расстоянии 
            Vector3 newPosition = playerCamera.position + playerCamera.forward * distanceFromPlayer; 

            // Устанавливаем позицию объекта 
            menu.position = newPosition;

            // Поворачиваем объект, чтобы он был направлен на игрока 
            menu.LookAt(new Vector3(playerCamera.position.x, transform.position.y, playerCamera.position.z)); 
        }
        */

        
    }

    private void OnEnable()
    {
        // Подписка на событие 
        TapButton.action.performed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // Отписка от события 
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
