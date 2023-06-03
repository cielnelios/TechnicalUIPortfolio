using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private LoadSceneMode mode;
    [SerializeField] private UIMasterIndex LodedSceneListClass;

    // LoadSceneMode.Single: 현재 씬을 해제하고 새로운 씬을 로드합니다.
    // LoadSceneMode.Additive: 현재 씬을 유지한 채 새로운 씬을 로드합니다.

    private void Start()
    {
        Button thisButton = this.gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener( () => MoveSceneMethod(nextSceneName) );
    }

    private void MoveSceneMethod(string nextScene)
    {
        // 없으면 씬을 로딩
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadScene(nextScene, mode);
            LodedSceneListClass.AddUIScene(SceneManager.GetSceneByName(nextScene));
        }
        // 있으면 UI 활성화
        //else
        //{
        //    GameObject[] nextSceneRootObject = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
        //    foreach (GameObject RootGameObject in nextSceneRootObject)
        //    {
        //        break;
        //    }
        //}
    }
}
