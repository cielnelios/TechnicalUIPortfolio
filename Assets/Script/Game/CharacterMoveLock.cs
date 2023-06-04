using UnityEngine;

//StateMachineBehaviour의 취득 방법
//private Animator _animator;
//private StateMachineExample _stateMachineExample;
//void Start()
//{
//    _animator = GetComponent<Animator>();
//    _stateMachineExample = _animator.GetBehaviour<StateMachineExample>();
//}


public class CharacterMoveLock : StateMachineBehaviour
{
    private CharacterState _characterState;

    public void GetCharacterStateInstance(CharacterState characterState)
    {
        _characterState = characterState;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // 새로운 상태로 변할 때 실행
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterState.SetisAllowMoveBoolean(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // 처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // 상태가 다음 상태로 바뀌기 직전에 실행
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterState.SetisAllowMoveBoolean(true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // Implement code that processes and affects root motion
    // MonoBehaviour.OnAnimatorMove 직후에 실행
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    // Implement code that sets up animation IK (inverse kinematics)
    // MonoBehaviour.OnAnimatorIK 직후에 실행
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // 스크립트가 부착된 상태 기계로 전환이 왔을때 실행
    //public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //}

    // 스크립트가 부착된 상태 기계에서 빠져나올때 실행
    //public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //}
}
