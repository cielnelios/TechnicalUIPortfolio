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
        // 컴포넌트 슬롯 프리팹 배치하고
        GameObject newObject = Instantiate(_itemPrefab, _contentContainer.transform) as GameObject;
        // 슬롯에 이 캐릭터 프리팹을 기록함
        newObject.GetComponent<CharacterComponentSlotPrefab>().characterComponentInfoPrefab = this;
        // 사전에 추가해서 각 슬롯에 배치될 전략이 뭔지 파악할 수 있도록 한다.
        StrategyInSlotDictionary.Add(newObject,null);
    }

}
