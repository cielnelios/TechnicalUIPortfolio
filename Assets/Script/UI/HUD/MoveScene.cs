using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private LoadSceneMode mode;
    // LoadSceneMode.Single: 현재 씬을 해제하고 새로운 씬을 로드합니다.
    // LoadSceneMode.Additive: 현재 씬을 유지한 채 새로운 씬을 로드합니다.

    private void Awake()
    {
        // 버튼에 씬 여는 기능을 등록
        Button thisButton = this.gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener( () => this.MoveSceneMethod(nextSceneName) );
    }

    private void MoveSceneMethod(string nextScene)
    {
        // 씬이 없으면 씬을 로딩하고, ESC로 닫을 수 있게 LodedSceneListClass의 스택에 등록
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadScene(nextScene, mode);
            GameManager.Instance.AddUIScene(SceneManager.GetSceneByName(nextScene));
        }
    }
}
