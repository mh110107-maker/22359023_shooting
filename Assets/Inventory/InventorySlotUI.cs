using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{


    public Image iconImage;
    public TMP_Text countText;

    private InventoryUI inventoryUI;
    private List<InventoryItem> itemList;
    private int index;

    public void SetSlot(InventoryUI inventoryUI, List<InventoryItem> itemList, int index)
    {
        this.inventoryUI = inventoryUI;
        this.itemList = itemList;
        this.index = index;
        // 현재 슬롯의 아이콘과 개수 텍스트 갱신
        RefreshView();
    }
    public void RefreshView()
    {
        if (itemList == null || index < 0 || index >= itemList.Count)
        {
            ClearView();
            return;
        }
        InventoryItem item = itemList[index];
        if (item == null || item.data == null)
        {
            ClearView();
            return;
        }

        iconImage.enabled = true;
        iconImage.sprite = item.data.icon;
        iconImage.color = Color.white;
        // 1. 아이콘 이미지가 마우스 클릭, 드래그 같은 UI 이벤트를 받을 수 있게 설정
        iconImage.raycastTarget = true;

        if (item.count > 1)
        {
            countText.text = item.count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    private void ClearView()
    {
        if (iconImage != null)
        {
            iconImage.enabled = false;
            iconImage.sprite = null;
            // 2. 아이콘 이미지가 UI 이벤트를 받을 수 있게 설정
            iconImage.raycastTarget = true;
        }

        if (countText != null)                                        // 17. 개수 텍스트 컴포넌트가 연결되어 있는지 확인  
        {
            countText.text = "";                                      // 18. 개수 텍스트 초기화      
        }
    }

    private RectTransform iconRect;

    // 1. vector2에서 vector3로 변경, 
    // z축 추가 아이템이 위에 보이게.
    //private Vector2 iconStartPosition; 
    private Vector3 iconStartPosition;

    // 2. UI가 들어있는 최상위 Canvas,
    // Drag 중 아이템을 Canvas 맨 위로 올리기 위해 사용
    private Canvas rootCanvas;
    // 3. Drag 전 아이템 원래 부모 위치
    private Transform originalParent;
    // 4. Drag 전 아이템 원래 형제 순서
    private int originalSiblingIndex;

    private void Awake()
    {
        if (iconImage != null)
        {
            iconRect = iconImage.GetComponent<RectTransform>();
            // 5. 아이템 Click / Drop 이벤트를 직접 받지 않게 한다. 
            // 아이템 아이콘이 있는 Slot이 이벤트를 받게 하는 구조.
            iconImage.raycastTarget = false;
        }

        if (countText != null)
        {
            countText.raycastTarget = false;
        }

        // 6. 이 아이템의 부모 Slot이 속한 Canvas를 찾는다.
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemList == null || index < 0 || index >= itemList.Count) return;
        if (itemList[index] == null || itemList[index].data == null) return;
        if (iconRect == null) return;

        if (rootCanvas == null) return; // 7. Canvas가 없으면 드래그 하지 않는다. 

        Debug.Log("드래그 시작: " + itemList[index].data.itemName);

        // 8. 아이콘의 현재 위치 저장, Vector3로 변경 z축 사용하기 위해
        //iconStartPosition = iconRect.anchoredPosition;    // UI 좌표
        iconStartPosition = iconRect.position;              // World 좌표

        originalParent = iconRect.parent;                   // 9. 아이템 부모 위치 저장 
        originalSiblingIndex = iconRect.GetSiblingIndex();  // 10. 아이템이 부모 몇 번째 자식인지 

        // 11. 아이템을 원래 Slot의 자식으로 둔 채 움직이면, 다른 Slot 뒤에 가려질 수 있다. 
        //     그래서 드래그 중에는 아이템을 Canvas 자식으로 옮긴다.
        iconRect.SetParent(rootCanvas.transform, true);

        // 12. Canvas 안에서 가장 마지막 자식으로 보내면, 화면에 가장 위에 그려진다.
        iconRect.SetAsLastSibling();

        if (iconImage != null)
        {
            iconImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemList == null || index < 0 || index >= itemList.Count) return;
        if (itemList[index] == null || itemList[index].data == null) return;
        if (iconRect == null) return;

        //iconRect.anchoredPosition += eventData.delta; 
        // 13. UI좌표->World좌표, 아이템을 마우스 위치로 이동
        iconRect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 종료");

        if (iconRect != null && originalParent != null)
        {
            iconRect.SetParent(originalParent, true);
            iconRect.SetSiblingIndex(originalSiblingIndex);
            iconRect.anchoredPosition = Vector2.zero;
        }

        if (iconRect != null)
        {
            //iconRect.anchoredPosition = iconStartPosition;
            // 14. Drag 후 UI를 갱신해서 아이템을 보이게 해준다.
            inventoryUI.Refresh();
        }

        if (iconImage != null)
        {
            iconImage.raycastTarget = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //slot 중심
        //InventorySlotUI fromSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();    
        //item 중심
        // 15. 아이템이 Canvas로 이동했기 때문에 GetComponentInParent 사용
        InventorySlotUI fromSlot = eventData.pointerDrag.GetComponentInParent<InventorySlotUI>();

        if (fromSlot == null)
        {
            Debug.LogWarning("드래그 시작 슬롯을 찾지 못했습니다.");
            return;
        }

        if (fromSlot == this) return;

        if (PlayerInventory.Instance == null)
        {
            Debug.LogWarning("PlayerInventory.Instance가 없습니다.");
            return;
        }

        Debug.Log("드롭 성공");

        PlayerInventory.Instance.MoveItem(
            fromSlot.itemList,
            fromSlot.index,
            this.itemList,
            this.index
        );

        if (inventoryUI != null) inventoryUI.Refresh();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemList == null || index < 0 || index >= itemList.Count) return;
        InventoryItem item = itemList[index];
        if (item == null || item.data == null) return;
        if (PlayerInventory.Instance == null) return;
        if (itemList != PlayerInventory.Instance.bagItems) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"Bag 아이템 클릭: {item.data.itemName} / 개수: {item.count}");
            if (inventoryUI != null)
            {
                /*
                 * 가방 아이템을 클릭했을 때 클릭한 아이템 정보와 
                 * 슬롯 인덱스 전달하고 실행할 UI 처리 함수 호출 
                 */
                inventoryUI.OnBagItemClicked(item, index);
            }
        }
    }
}
