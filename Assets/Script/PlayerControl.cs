using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }
    public float TurnDirection { get; private set; }
    public InputAction MoveAction { get; private set; }
    public CharacterState characterState;

    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Interactions.html
    //context.action.phase
    //InputActionPhase.Started: ���� ���� �� ȣ��
    //InputActionPhase.Performed: ���� Ȯ��(������ ����) �� ȣ��
    //InputActionPhase.Canceled: ���� ���� �� ȣ��
    //InputActionPhase.Disabled: �׼��� Ȱ��ȭ���� ����
    //InputActionPhase.Waiting: �׼��� Ȱ��ȭ�Ǿ��ְ� �Է��� ��ٸ��� ����

    public void Awake()
    {
        InputActionMap playerInputActionMap = this.gameObject.GetComponent<PlayerInput>().actions.FindActionMap("Player");
        MoveAction = playerInputActionMap.FindAction("Move");
    }

    private Action<Vector3> SetMoveDirectionAction;
    private Action<float> SetTurnDirectionAction;
    private Action<CharacterState.EnumICharacterState> SetStateAction;
    public void Start()
    {
        SetMoveDirectionAction = characterState.SetMoveDirection;
        SetTurnDirectionAction = characterState.SetTurnDirection;
        SetStateAction = characterState.SetState;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed || 
            context.action.phase == InputActionPhase.Canceled)
        {
            Vector2 input = context.ReadValue<Vector2>();
            this.MoveDirection = new Vector3(input.x, 0f, input.y);

            SetMoveDirectionAction(this.MoveDirection);
            //SetStateAction(CharacterState.EnumICharacterState._moveState);
        }
    }

    public void OnTurn(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed ||
            context.action.phase == InputActionPhase.Canceled)
        {
            this.TurnDirection = context.ReadValue<float>();
            SetTurnDirectionAction(this.TurnDirection);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            SetStateAction(CharacterState.EnumICharacterState._jumpState);
        }
    }

    //�κ��丮 ����
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            Inventory();
        }
    }
    private void Inventory()
    {

    }
}
