using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public int bagSlotCount = 12;
    public int equipSlotCount = 3;

    public List<InventoryItem> bagItems = new List<InventoryItem>();
    public List<InventoryItem> equipItems = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;

        bagItems.Clear();
        equipItems.Clear();

        FillEmptySlots(bagItems, bagSlotCount);
        FillEmptySlots(equipItems, equipSlotCount);
    }

    private void FillEmptySlots(List<InventoryItem> list, int slotCount)
    {
        while (list.Count < slotCount)
        {
            list.Add(null);
        }
    }

    // UI 새로고침을 편하게 호출하기 위한 헬퍼 함수
    public void RefreshInventoryUI()
    {
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
        {
            ui.Refresh();
        }
    }

    // ★ [해결책 1] 기존 스택에 합치지 않고 빈 슬롯마다 1개씩 나누어 담는 로직
    public bool AddItem(ItemData itemData, int count = 1)
    {
        if (itemData == null || count <= 0) return false;

        // 빈 칸을 순서대로 찾아 아이템을 1개씩 나누어 집어넣습니다.
        for (int i = 0; i < bagItems.Count; i++)
        {
            if (bagItems[i] == null || bagItems[i].data == null)
            {
                bagItems[i] = new InventoryItem(itemData, 1);
                count--;

                Debug.Log($"{itemData.itemName} 새 슬롯({i}번)에 1개 추가 성공");

                if (count <= 0)
                {
                    RefreshInventoryUI();
                    return true;
                }
            }
        }

        if (count > 0)
        {
            Debug.LogWarning($"가방 공간이 부족하여 {itemData.itemName} {count}개를 더 넣지 못했습니다.");
            RefreshInventoryUI();
        }

        return false;
    }

    public void MoveItem(List<InventoryItem> fromList, int fromIndex, List<InventoryItem> toList, int toIndex)
    {
        if (!IsValidIndex(fromList, fromIndex) || !IsValidIndex(toList, toIndex)) return;

        InventoryItem fromItem = fromList[fromIndex];
        if (IsEmpty(fromItem)) return;

        bool isBagToEquip = fromList == bagItems && toList == equipItems;
        bool isEquipToBag = fromList == equipItems && toList == bagItems;

        if (isBagToEquip)
        {
            MoveOneItemToEquip(fromIndex, toIndex);
            return;
        }

        if (isEquipToBag)
        {
            MoveEquipItemToBag(fromIndex, toIndex);
            return;
        }

        // 가방 안에서 순서 변경 등 일반 이동
        InventoryItem temp = toList[toIndex];
        toList[toIndex] = fromList[fromIndex];
        fromList[fromIndex] = temp;

        RefreshInventoryUI(); // 이동 후 UI 갱신
    }

    // ★ [수정 완료] 가방 ➡️ 장착 칸 이동 시 슬롯 역할 제한 로직 추가
    private void MoveOneItemToEquip(int bagIndex, int equipIndex)
    {
        InventoryItem bagItem = bagItems[bagIndex];
        if (IsEmpty(bagItem)) return;

        if (!IsEmpty(equipItems[equipIndex]))
        {
            Debug.Log("장착 슬롯이 이미 사용 중입니다.");
            return;
        }

        // -----------------------------------------------------------------
        // [슬롯별 아이템 타입 검사 조건문]
        // -----------------------------------------------------------------
        // 0번 슬롯: 무기(Weapon)만 허용
        if (equipIndex == 0 && bagItem.data.itemType != ItemType.Weapon)
        {
            Debug.LogWarning("0번 슬롯에는 무기만 장착할 수 있습니다!");
            return;
        }
        // 1번 슬롯: 방어구(Armor)만 허용
        if (equipIndex == 1 && bagItem.data.itemType != ItemType.Armor)
        {
            Debug.LogWarning("1번 슬롯에는 방어구만 장착할 수 있습니다!");
            return;
        }
        // 2번 슬롯: 포션/소비템(Consumable)만 허용
        if (equipIndex == 2 && bagItem.data.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("2번 슬롯에는 소비 아이템(포션)만 장착할 수 있습니다!");
            return;
        }
        // -----------------------------------------------------------------

        ItemData itemData = bagItem.data;

        equipItems[equipIndex] = new InventoryItem(itemData, 1);
        bagItem.count--;
        if (bagItem.count <= 0)
        {
            bagItems[bagIndex] = null;
        }
        Debug.Log($"{itemData.itemName}을 장착 슬롯 {equipIndex}번에 장착 완료");
        RefreshInventoryUI();
    }

    private bool IsEmpty(InventoryItem item)
    {
        return item == null || item.data == null || item.count <= 0;
    }

    // ★ [수정 완료] 장착 칸 ➡️ 가방 이동(혹은 다른 아이템과 교환) 시 제한 로직 추가
    private void MoveEquipItemToBag(int equipIndex, int bagIndex)
    {
        InventoryItem equipItem = equipItems[equipIndex];
        if (IsEmpty(equipItem)) return;
        InventoryItem bagItem = bagItems[bagIndex];

        // 가방의 목적지 칸이 완전히 비어있다면 제약 없이 즉시 해제
        if (IsEmpty(bagItem))
        {
            bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
            equipItems[equipIndex] = null;
            RefreshInventoryUI();
            return;
        }

        // 가방에 있는 같은 종류의 스택형 아이템과 합쳐질 때
        if (bagItem.data == equipItem.data && bagItem.data.canStack)
        {
            int space = bagItem.data.maxStack - bagItem.count;
            int addCount = Mathf.Min(space, equipItem.count);
            bagItem.count += addCount;
            equipItem.count -= addCount;

            if (equipItem.count <= 0)
            {
                equipItems[equipIndex] = null;
            }

            Debug.Log($"{bagItem.data.itemName} 가방에 {addCount}개 합침");
            RefreshInventoryUI();
            return;
        }

        // 다른 아이템과 다이렉트로 맞바꾸려 할 때 (Swap 제약 추가)
        // 가방에서 장착 칸으로 들어오려는 맞바꿈 템(bagItem)의 자격을 검사합니다.
        if (equipIndex == 0 && bagItem.data.itemType != ItemType.Weapon)
        {
            Debug.LogWarning("무기 슬롯에는 무기 타입의 아이템만 교환해 넣을 수 있습니다.");
            return;
        }
        if (equipIndex == 1 && bagItem.data.itemType != ItemType.Armor)
        {
            Debug.LogWarning("방어구 슬롯에는 방어구 타입의 아이템만 교환해 넣을 수 있습니다.");
            return;
        }
        if (equipIndex == 2 && bagItem.data.itemType != ItemType.Consumable)
        {
            Debug.LogWarning("포션 슬롯에는 소비 아이템 타입만 교환해 넣을 수 있습니다.");
            return;
        }

        // 조건 검사를 다 통과했다면 안전하게 Swap 진행
        InventoryItem temp = bagItems[bagIndex];
        bagItems[bagIndex] = new InventoryItem(equipItem.data, equipItem.count);
        equipItems[equipIndex] = temp;

        RefreshInventoryUI();
    }

    private bool IsValidIndex(List<InventoryItem> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }

    public void RemoveOneBagItem(int bagIndex)
    {
        if (!IsValidIndex(bagItems, bagIndex)) return;
        InventoryItem item = bagItems[bagIndex];

        if (item == null || item.data == null) return;
        item.count--;
        Debug.Log($"{item.data.itemName} 1개 사용");

        if (item.count <= 0)
        {
            bagItems[bagIndex] = null;
        }
        RefreshInventoryUI();
    }
}