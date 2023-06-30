using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterComponentInfoPrefab : MonoBehaviour
{
    public GameObject targetCharacter;
    
    [SerializeField] private GameObject _contentContainer;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private TextMeshProUGUI _thisText;
    public (float xPos, float yPos) thisPosition;

    public Dictionary<GameObject, IComponentStrategy> StrategyInSlotDictionary
        = new Dictionary<GameObject, IComponentStrategy>();

    private void Start()
    {
        _thisText.text = $"Object on ({thisPosition.xPos},{thisPosition.yPos})";
        this.CreateSlot();
    }

    public void CreateSlot()
    {
        // ������Ʈ ���� ������ ��ġ�ϰ�
        GameObject newObject = Instantiate(_itemPrefab, _contentContainer.transform) as GameObject;
        // ���Կ� �� ĳ���� �������� �����
        newObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab = this;
        // ������ �߰��ؼ� �� ���Կ� ��ġ�� ������ ���� �ľ��� �� �ֵ��� �Ѵ�.
        StrategyInSlotDictionary.Add(newObject,null);
    }

}
