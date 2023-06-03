using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private LoadSceneMode mode;
    [SerializeField] private UIMasterIndex LodedSceneListClass;

    // LoadSceneMode.Single: ���� ���� �����ϰ� ���ο� ���� �ε��մϴ�.
    // LoadSceneMode.Additive: ���� ���� ������ ä ���ο� ���� �ε��մϴ�.

    private void Start()
    {
        Button thisButton = this.gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener( () => MoveSceneMethod(nextSceneName) );
    }

    private void MoveSceneMethod(string nextScene)
    {
        // ������ ���� �ε�
        if (!SceneManager.GetSceneByName(nextScene).isLoaded)
        {
            SceneManager.LoadScene(nextScene, mode);
            LodedSceneListClass.AddUIScene(SceneManager.GetSceneByName(nextScene));
        }
        // ������ UI Ȱ��ȭ
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
