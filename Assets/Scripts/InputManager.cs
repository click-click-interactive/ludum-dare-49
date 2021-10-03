using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;


[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public Skill currentSkill = Skill.Attack;
    private PlayerInput _playerInput;
    private Dictionary<InputType, InputActionMap> _actionMaps;

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

    public void ToggleActionMap(InputType inputType)
    {
        if (!_actionMaps.ContainsKey(inputType)) return;

        _playerInput.SwitchCurrentActionMap(_actionMaps[inputType].name);
    }

    public void OnSkillOne()
    {
        currentSkill = Skill.Overload;
    }

    public void OnClick()
    {
        if (!Camera.main) return;
        if (!Mouse.current.leftButton.wasReleasedThisFrame) return;

        Ray ray;
        RaycastHit hit;

        switch (currentSkill)
        {
            case Skill.Attack:
                ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        Debug.Log("CLICKED " + hit.collider.gameObject.name);
                        hit.collider.gameObject.SendMessage("OnClick");
                    }
                }

                break;

            case Skill.Overload:
                ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("RepairStation"))
                    {
                        Debug.Log("Overload " + hit.collider.gameObject.name);
                        hit.collider.gameObject.SendMessage("OnOverload");
                    }
                }

                break;
        }

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 0))
        {
            if (hit.collider != null)
            {
                Debug.Log("CLICKED " + hit.collider.gameObject.name);
                hit.collider.gameObject.SendMessage("OnClick");
            }
        }
    }
}
