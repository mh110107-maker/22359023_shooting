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

        gameObject.SetActive(false);
    }

    // 1단계에서 수정했던 ShowTooltip 함수
    public void ShowTooltip(ItemData itemData)
    {
        if (itemData == null) return;

        itemNameText.text = itemData.itemName;

        if (string.IsNullOrEmpty(itemData.itemDescription))
        {
            itemDescText.text = "설명이 없는 아이템입니다.";
        }
        else
        {
            itemDescText.text = itemData.itemDescription;
        }

        gameObject.SetActive(true);
    }

    // ===================================================================
    // ★ [체크] 이 함수가 없거나 이름이 다르면 빨간 줄이 뜹니다! 똑같이 넣어주세요.
    // ===================================================================
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
    // ===================================================================
}