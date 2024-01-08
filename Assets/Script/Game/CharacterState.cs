using System.Collections.Generic;
using UnityEngine;

// State Pattern
// 상태 클래스가 한 종류로 묶일 수 있도록 하기 위해 선언하는 부모 인터페이스
public interface ICharacterState
{
    void SetStateValue(CharacterState characterState);
}

// 상태 패턴의 현황을 알기 위한 컨텍스트
public class CharacterStateContext
{
    // 현재 선택된 상태를 담는다.
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
    [Header("움직임 정보")]
    public Animator thisGameObjectModelAnimation;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _turnDirection;
    public float rotateSpeed = 0.18f;
    public float jumpSpeed = 500.0f;
    public float moveSpeed = 5f;
    public bool isAllowMove = true;

    [Header("구독할 액션이 있는 클래스들")]
    public PlayerControl playerControl;
    [SerializeField] private Rigidbody _playerRigidbody;

    // 세팅을 진행한다. 시작하면 딕셔너리에 enum과 state를 매칭시켜서 저장
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
    
    // 이 페이지 밖에서 스테이트를 바꾸려 하는 경우 호출. 상태 변경시 선행 상태에 제약을 두기 위함
    public void SetState(EnumICharacterState state)
    {
        // 점프 중에는 스테이트 변경 안됨
        if (_characterStateContext.CurrentChatacterState == ICharacterStateDictionary[EnumICharacterState._jumpState])
        {
            return;
        }

        _characterStateContext.CharacterStateTransition(ICharacterStateDictionary[state]);
    }
    // 이 캐릭터의 이동 (모션이 아닌 move 메소드에 의한 좌표 이동)를 허용할지 말지 세팅하는 함수
    public void SetisAllowMoveBoolean(bool isAllowMove)
    {
        this.isAllowMove = isAllowMove;
    }

    // 이동과 회전 세팅용 함수
    public void SetMoveDirection(Vector3 moveDirection)
    {
        // 방향이 이동과 상태보다 먼저 불러지도록 주의. 안그러면 방향 전환이 한박자 늦다.
        this._moveDirection = moveDirection;
    }
    public void SetTurnDirection(float turnDirection)
    {
        this._turnDirection = turnDirection;
    }

    // 이동과 회전은 모든 모션에서 상시로 하니까 따로 빼두고, 착지같은 예외에서만 잠그도록 _isAllowMove boolean 추가함
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

    // 상태들
    public class CharacterIdleState : MonoBehaviour, ICharacterState
    {
        private CharacterState _characterState;

        // 그냥 대기 상태
        public void SetStateValue(CharacterState characterState)
        {
            if (!_characterState) _characterState = characterState;

            if (_characterState.thisGameObjectModelAnimation != null)
            {
                _characterState.thisGameObjectModelAnimation.SetInteger("Move", 0);
            }

            // 점프 착지시 idle인데, 이동 지속 입력이면 꼬이기 때문
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

            // 이동 방향에 따른 애니
            if (_characterState._moveDirection.z > 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 1);    // forward
            else if (_characterState._moveDirection.z < 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 2);    // backward
            else if (_characterState._moveDirection.x > 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 3);   // right
            else if (_characterState._moveDirection.x < 0) _characterState.thisGameObjectModelAnimation.SetInteger("Move", 4);   // left

            // 이동 안하면 idle로
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

            // 점프 속도 세팅
            Vector3 jumpVector3 = /*_characterState._moveDirection + */_characterState.jumpSpeed * Vector3.up;
            _characterState._playerRigidbody.AddForce(jumpVector3);

            // 애니
            _characterState.thisGameObjectModelAnimation.SetTrigger("Jumping");

            //// 코루틴으로 하강 대기 : Rigidbody.velocity가 자주 먹통이 되서 사용 안함.
            //StartCoroutine(CheckYPosVelocity());
        }

        // 착지하면 착지 애니 틀고 아이들로 넘어감.
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

                    // idle로의 전이는 CharacterMoveLock에서 수행하려 했으나, 보호 수준때문에..
                    // state는 idle이지만 애니메이터에서 JumpingEnd 애니를 틀고 있고, 
                    // 이 애니 스테이트에 붙은 CharacterMoveLock 스크립트에서 때문에 _isAllowMove boolean를 조작하므로 이동도 일시 중단된다.
                    _characterState._characterStateContext.CharacterStateTransition(_characterState.ICharacterStateDictionary[EnumICharacterState._idleState]);
                }
            }
        }
    }
}


