using System.Collections.Generic;
using UnityEngine;

// State Pattern
// ���� Ŭ������ �� ������ ���� �� �ֵ��� �ϱ� ���� �����ϴ� �θ� �������̽�
public interface ICharacterState
{
    void SetStateValue(CharacterState characterState);
}

// ���� ������ ��Ȳ�� �˱� ���� ���ؽ�Ʈ
public class CharacterStateContext
{
    // ���� ���õ� ���¸� ��´�.
    public ICharacterState CurrentChatacterState { get; set; }
    private readonly CharacterState _characterState;

    public CharacterStateContext(CharacterState characterState)
    {
        _characterState = characterState;
    }

    public void CharacterStateTransition()
    {
        CurrentChatacterState.SetStateValue(_characterState);
    }

    public void CharacterStateTransition(ICharacterState state)
    {
        CurrentChatacterState = state;
        CurrentChatacterState.SetStateValue(_characterState);
    }
}

public class CharacterState : MonoBehaviour
{
    [Header("������ ����")]
    public Animator thisGameObjectModelAnimation;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _turnDirection;
    public float rotateSpeed = 0.18f;
    public float jumpSpeed = 500.0f;
    public float moveSpeed = 5f;
    public bool isAllowMove = true;

    [Header("������ �׼��� �ִ� Ŭ������")]
    public PlayerControl playerControl;
    [SerializeField] private Rigidbody _playerRigidbody;

    // ������ �����Ѵ�. �����ϸ� ��ųʸ��� enum�� state�� ��Ī���Ѽ� ����
    private CharacterStateContext _characterStateContext;

    public Dictionary<EnumICharacterState, ICharacterState> ICharacterStateDictionary 
        = new Dictionary<EnumICharacterState, ICharacterState>();

    public enum EnumICharacterState
    {
        _idleState, _moveState, _jumpState, _attackState,
        // damaged, died, special
    }
    private void Start()
    {
        _characterStateContext = new CharacterStateContext(this);

        ICharacterStateDictionary.Add(EnumICharacterState._idleState, this.gameObject.AddComponent<CharacterIdleState>());
        ICharacterStateDictionary.Add(EnumICharacterState._moveState, this.gameObject.AddComponent<CharacterMoveState>());
        ICharacterStateDictionary.Add(EnumICharacterState._jumpState, this.gameObject.AddComponent<CharacterJumpState>());
        
        this.isAllowMove = true;

        _characterStateContext.CharacterStateTransition(ICharacterStateDictionary[EnumICharacterState._idleState]);
    }
    
    // �� ������ �ۿ��� ������Ʈ�� �ٲٷ� �ϴ� ��� ȣ��. ���� ����� ���� ���¿� ������ �α� ����
    public void SetState(EnumICharacterState state)
    {
        // ���� �߿��� ������Ʈ ���� �ȵ�
        if (_characterStateContext.CurrentChatacterState == ICharacterStateDictionary[EnumICharacterState._jumpState])
        {
            return;
        }

        _characterStateContext.CharacterStateTransition(ICharacterStateDictionary[state]);
    }
    // �� ĳ������ �̵� (����� �ƴ� move �޼ҵ忡 ���� ��ǥ �̵�)�� ������� ���� �����ϴ� �Լ�
    public void SetisAllowMoveBoolean(bool isAllowMove)
    {
        this.isAllowMove = isAllowMove;
    }

    // �̵��� ȸ�� ���ÿ� �Լ�
    public void SetMoveDirection(Vector3 moveDirection)
    {
        // ������ �̵��� ���º��� ���� �ҷ������� ����. �ȱ׷��� ���� ��ȯ�� �ѹ��� �ʴ�.
        this._moveDirection = moveDirection;
    }
    public void SetTurnDirection(float turnDirection)
    {
        this._turnDirection = turnDirection;
    }

    // �̵��� ȸ���� ��� ��ǿ��� ��÷� �ϴϱ� ���� ���ΰ�, �������� ���ܿ����� ��׵��� _isAllowMove boolean �߰���
    void FixedUpdate()
    {
        if (isAllowMove)
        {
            Move();
            Rotate();
        }
    }
    private void Move()
    {
        Vector3 normalizedMove = ( this._moveDirection.z * transform.forward
            + this._moveDirection.x * transform.right).normalized;

        this._playerRigidbody.MovePosition(this._playerRigidbody.position +
            normalizedMove * this.moveSpeed * Time.deltaTime );
    }
    private void Rotate()
    {
        //this.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up) * this.gameObject.transform.rotation;
        Vector3 turn = Vector3.up * this._turnDirection * this.rotateSpeed;
        _playerRigidbody.rotation *= Quaternion.Euler(turn);
    }

    // ���µ�
    public class CharacterIdleState : MonoBehaviour, ICharacterState
    {
        private CharacterState _characterState;

        // �׳� ��� ����
        public void SetStateValue(CharacterState characterState)
        {
            if (!_characterState) _characterState = characterState;

            if (_characterState.thisGameObjectModelAnimation != null)
            {
                _characterState.thisGameObjectModelAnimation.SetInteger("Move", 0);
            }

            // ���� ������ idle�ε�, �̵� ���� �Է��̸� ���̱� ����
            if ( (_characterState._moveDirection.z != 0) || (_characterState._moveDirection.x != 0) )
            {
                _characterState._characterStateContext.CharacterStateTransition(_characterState.ICharacterStateDictionary[EnumICharacterState._moveState]);
            }
        }
    }

    public class CharacterMoveState : MonoBehaviour, ICharacterState
    {
        private CharacterState _characterState;

        public void SetStateValue(CharacterState characterState)
        {
            if (!_characterState) _characterState = characterState;

            // �̵� ���⿡ ���� �ִ�
            if (_characterState._moveDirection.z > 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 1);    // forward
            else if (_characterState._moveDirection.z < 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 2);    // backward
            else if (_characterState._moveDirection.x > 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 3);   // right
            else if (_characterState._moveDirection.x < 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 4);   // left

            // �̵� ���ϸ� idle��
            else
            {
                _characterState.thisGameObjectModelAnimation.SetInteger("Move", 0);
                _characterState._characterStateContext.CharacterStateTransition(_characterState.ICharacterStateDictionary[EnumICharacterState._idleState]);
            }
        }
    }

    public class CharacterJumpState : MonoBehaviour, ICharacterState
    {
        private CharacterState _characterState;

        public void SetStateValue(CharacterState characterState)
        {
            if (!_characterState)
            {
                _characterState = characterState;
                _characterState.thisGameObjectModelAnimation.GetBehaviour<CharacterMoveLock>().GetCharacterStateInstance(_characterState);
            }

            // ���� �ӵ� ����
            Vector3 jumpVector3 = /*_characterState._moveDirection + */_characterState.jumpSpeed * Vector3.up;
            _characterState._playerRigidbody.AddForce(jumpVector3);

            // �ִ�
            _characterState.thisGameObjectModelAnimation.SetTrigger("Jumping");

            //// �ڷ�ƾ���� �ϰ� ��� : Rigidbody.velocity�� ���� ������ �Ǽ� ��� ����.
            //StartCoroutine(CheckYPosVelocity());
        }

        // �����ϸ� ���� �ִ� Ʋ�� ���̵�� �Ѿ.
        private void OnCollisionEnter(Collision collision)
        {
            if (_characterState)
            {
                if (_characterState._characterStateContext.CurrentChatacterState
                    == _characterState.ICharacterStateDictionary[EnumICharacterState._jumpState] &&
                    collision.gameObject.CompareTag("Ground"))
                {
                    // Debug.Log(FunctionManager.MsTime() + "Landing");
                    _characterState.thisGameObjectModelAnimation.SetTrigger("JumpingEnd");

                    // idle���� ���̴� CharacterMoveLock���� �����Ϸ� ������, ��ȣ ���ض�����..
                    // state�� idle������ �ִϸ����Ϳ��� JumpingEnd �ִϸ� Ʋ�� �ְ�, 
                    // �� �ִ� ������Ʈ�� ���� CharacterMoveLock ��ũ��Ʈ���� ������ _isAllowMove boolean�� �����ϹǷ� �̵��� �Ͻ� �ߴܵȴ�.
                    _characterState._characterStateContext.CharacterStateTransition(_characterState.ICharacterStateDictionary[EnumICharacterState._idleState]);
                }
            }
        }
    }
}


