using UnityEngine;

//StateMachineBehaviour�� ��� ���
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
    // ���ο� ���·� ���� �� ����
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterState.SetisAllowMoveBoolean(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // ó���� ������ �������� ������ �� ������ ������ ����
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // ���°� ���� ���·� �ٲ�� ������ ����
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterState.SetisAllowMoveBoolean(true);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    // Implement code that processes and affects root motion
    // MonoBehaviour.OnAnimatorMove ���Ŀ� ����
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    // Implement code that sets up animation IK (inverse kinematics)
    // MonoBehaviour.OnAnimatorIK ���Ŀ� ����
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // ��ũ��Ʈ�� ������ ���� ���� ��ȯ�� ������ ����
    //public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //}

    // ��ũ��Ʈ�� ������ ���� ��迡�� �������ö� ����
    //public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //}
}
