using UnityEngine;
using UnityEngine.UI;

public class SetActivateUI : MonoBehaviour
{
    [SerializeField] private GameObject _targetUI;
    private void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => _targetUI.SetActive(true));
    }
}
