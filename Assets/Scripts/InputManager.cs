using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;


[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Dictionary<InputType, InputActionMap> _actionMaps;
    public Float unstability;
    private GameObject currentlyMousedOverObject = null;
    
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        _playerInput = gameObject.GetComponent<PlayerInput>();
        _actionMaps = new Dictionary<InputType, InputActionMap>();

        if (_playerInput)
        {
            _actionMaps.Add(InputType.Player, _playerInput.actions.FindActionMap("Player"));
            _actionMaps.Add(InputType.UI, _playerInput.actions.FindActionMap("UI"));
        }
    }
    
    void FixedUpdate()
    {
        if (!Camera.main) return;
        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out hit))
        {   
            if (hit.collider != null)
            {
                GameManager gameManager = GetComponent<GameManager>();
                if (gameManager.skill != null)
                {
                    if(hit.collider.gameObject.CompareTag(gameManager.skill.TargetTag))
                    {
                        currentlyMousedOverObject = hit.collider.gameObject;
                        hit.collider.gameObject.SendMessage("ShowTooltip");
                    }
                    else if(currentlyMousedOverObject != null)
                    {
                        currentlyMousedOverObject.SendMessage("HideTooltip");
                        currentlyMousedOverObject = null;
                    }
                }
            }
        }
    }

    public void ToggleActionMap(InputType inputType)
    {
        if (!_actionMaps.ContainsKey(inputType)) return;

        _playerInput.SwitchCurrentActionMap(_actionMaps[inputType].name);
    }

    public void OnClick()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (!Camera.main) return;
        if (!Mouse.current.leftButton.wasReleasedThisFrame) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        GameManager gameManager = GetComponent<GameManager>();

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("CLICKED " + hit.collider.gameObject.name);
                
                if (gameManager.skill != null)
                {
                    if(hit.collider.gameObject.CompareTag(gameManager.skill.TargetTag))
                    {
                        Debug.Log("Confirming select skill " + gameManager.skill.Name );
                        
                        hit.collider.gameObject.SendMessage("OnAction", gameManager.skill);    
                        
                        
                        if (currentlyMousedOverObject != null)
                        {
                            hit.collider.gameObject.SendMessage("HideTooltip");
                        }
                        gameManager.skill = null;
                    }
                    else
                    {
                        if (gameManager.skill != null)
                        {
                            unstability.value += gameManager.skill.UnstabilityCost;
                            gameManager.skill = null;
                        }
                        
                    }
                }
                else
                {
                    Debug.Log("Default click action");
                    hit.collider.gameObject.SendMessage("OnClick");    
                }
            }
        }
    }
}
