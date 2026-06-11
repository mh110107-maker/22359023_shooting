using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    public TMP_Text itemNameText;
    public TMP_Text itemDescText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameObject.SetActive(false); // 처음에는 숨김
    }

    

    // 정보창 띄우기
    public void ShowTooltip(ItemData itemData)
    {
        if (itemData == null) return;

        itemNameText.text = itemData.itemName;
        itemDescText.text = itemData.itemDescription; // ItemData에 설명 변수가 있어야 합니다.

        gameObject.SetActive(true);
    }

    // 정보창 숨기기
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}