using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractable : MonoBehaviour
{
    public GameObject InteractUI;
    public GameObject PriceUI;
    public GameObject LackOfCoinUI;
    
    public float InteractRange;
    
    private PlayerInput playerInput;
    private Player player;
    [SerializeField] private Camera playerCamera;

    private bool interact = false;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
        playerCamera = GetComponentInChildren<WeaponsControls>().camPlayer;
        
        InputAction _interact = playerInput.actions["Interact"];
        _interact.performed += InteractPerformed;
        
        LackOfCoinUI.SetActive(false);
    }

    private void Update()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * InteractRange, Color.red);
        Ray _r = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(_r, out RaycastHit _hit, InteractRange))
            {
                if (_hit.collider.gameObject.TryGetComponent(out IInteractable _interactable) && _hit.collider.gameObject.TryGetComponent(out ItemsSettings _itemsSettings))
                {
                    InteractUI.SetActive(true);
                    Text _priceText = PriceUI.GetComponentInChildren<Text>();
                    _priceText.text = _itemsSettings.Price.ToString() + " $";
                    if (_itemsSettings.Price > 0)
                    {
                        PriceUI.SetActive(true);
                    }
                }
                else
                {
                    InteractUI.SetActive(false);
                    PriceUI.SetActive(false);
                }
            }
            else
            {
                InteractUI.SetActive(false);
                PriceUI.SetActive(false);
                interact = false;
            }

            if (interact == true)
            {
                if (_hit.collider.gameObject.TryGetComponent(out IInteractable _interactable) && _hit.collider.gameObject.TryGetComponent(out ItemsSettings _itemsSettings))
                {
                    if (player.Coin >= _itemsSettings.Price)
                    {
                        _interactable.Interact(player.gameObject);
                        player.Coin -= _itemsSettings.Price;
                        interact = false;
                    }
                    else
                    {
                        StartCoroutine("UIForTime");
                        interact = false;
                    }
                }
                else
                {
                    interact = false;
                }
            }
    }

    public void InteractPerformed(InputAction.CallbackContext _ctx)
    {
        interact = _ctx.performed;
    }

    IEnumerator UIForTime()
    {
        LackOfCoinUI.SetActive(true);
        yield return new WaitForSeconds(3);
        LackOfCoinUI.SetActive(false);
    }
}