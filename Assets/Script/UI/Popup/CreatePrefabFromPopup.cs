using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreatePrefabFromPopup : MonoBehaviour
{
    [SerializeField] private Button _confirmPopupButton;

    [SerializeField] private GameObject _fieldObject;
    [SerializeField] private TMP_InputField _Xpos;
    [SerializeField] private TMP_InputField _Ypos;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _contentsContainer;


    // Ȯ�� ��ư�� ��� ���
    private void Start()
    {
        this._confirmPopupButton.onClick.AddListener(() => {
            //CreatePrefab();
            CreateObjectOnfield();
            });
    }

    private void CreateObjectOnfield()
    {
        // �������� ����
        GameObject newPrefab = Instantiate(_prefab, _contentsContainer.transform) as GameObject;

        // �� ������Ʈ ����
        // ��ǥ
        if (!int.TryParse(_Xpos.text, out int xPos)) xPos = 0;
        if (!int.TryParse(_Ypos.text, out int yPos)) yPos = 0;
       
        Vector3 position = new Vector3(xPos, 1.0f, yPos);

        // �� ã�Ƽ� ����
        GameObject[] gameObjects = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.CompareTag("Ground"))
            {
                GameObject newObject = Instantiate(_fieldObject, position, new Quaternion(), gameObject.transform) as GameObject;
                newPrefab.GetComponent<CharacterComponentInfoPrefab>().targetCharacter = newObject;
                break;
            }
        }
    }
}

