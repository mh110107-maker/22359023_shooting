using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// IDropHandler를 상속받아 마우스 드롭을 감지합니다.
public class TrashCanUI : MonoBehaviour, IDropHandler
{
    private Image trashCanImage;

    private void Awake()
    {
        // 휴지통 이미지의 Raycast Target을 코드로 확실하게 켜줍니다.
        trashCanImage = GetComponent<Image>();
        if (trashCanImage != null)
        {
            trashCanImage.raycastTarget = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 1. 드래그 중인 물체가 없으면 무시
        if (eventData.pointerDrag == null) return;

        // 2. 방금 휴지통에 떨어진 물체가 인벤토리 슬롯인지 확인
        InventorySlotUI fromSlot = eventData.pointerDrag.GetComponentInParent<InventorySlotUI>();

        // 3. 슬롯이 맞다면, 그 슬롯에게 "너 스스로 아이템 지워!"라고 명령 전달
        if (fromSlot != null)
        {
            fromSlot.TrashThisItem();
        }
    }
}