using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public void OnBagItemClicked(InventoryItem item, int index)
    {
        if (item == null || item.data == null) return;

        Debug.Log($"InventoryUI에서 클릭 처리: {item.data.itemName}");

        /* * 여기서 아이템 설명창 열기, 선택 표시, 사용 버튼 활성화 등을 처리하면 됨
         */
        // 1. PlayerInventory 인스턴스가 없으면 아이템 삭제 처리 중단
        if (PlayerInventory.Instance == null) return;
        // 2. 현재 슬롯 인덱스에 해당하는 가방 아이템 1개 삭제
        PlayerInventory.Instance.RemoveOneBagItem(index);
        // 3. 아이템 삭제 후 인벤토리 UI 새로고침
        Refresh();
    }

    public GameObject inventoryPanel;
    public InventorySlotUI[] bagSlots;
    public InventorySlotUI[] equipSlots;

    private void Start()
    {
        // 시작할 때는 완전히 닫힌 상태(시간 정상)로 세팅
        CloseInventory();
        Refresh();
    }

    // ★ Toggle 함수가 실행될 때 패널만 켜고 끄는 게 아니라, 시간 정지 함수를 호출하도록 변경했습니다.
    public void Toggle()
    {
        if (inventoryPanel == null)
        {
            Debug.LogWarning("Inventory Panel이 연결되지 않았습니다.");
            return;
        }

        // 현재 패널이 켜져있다면? -> 닫으면서 시간 재생
        if (inventoryPanel.activeSelf)
        {
            CloseInventory();
        }
        // 현재 패널이 꺼져있다면? -> 열으면서 시간 정지
        else
        {
            OpenInventory();
        }
    }

    public void Refresh()
    {
        // bagSlots와 equipSlots에 PlayerInventory의 리스트 연결
        PlayerInventory inventory = PlayerInventory.Instance;           // 1. PlayerInventory 싱글톤 인스턴스 가져오기

        if (inventory == null)                                          // 2. PlayerInventory가 존재하는지 확인    
        {
            Debug.LogWarning("PlayerInventory.Instance가 없습니다.");   // 3. 인벤토리 인스턴스가 없으면 경고 로그 출력 
            return;                                                     // 4. 더 이상 실행하지 않고 함수 종료
        }

        for (int i = 0; i < bagSlots.Length; i++)                   // 5. 가방 슬롯 UI에 PlayerInventory의 bagItems 리스트 연결 
        {
            bagSlots[i].SetSlot(this, inventory.bagItems, i);       // 6. 현재 가방 슬롯에 UI 매니저, 아이템 리스트, 슬롯 인덱스 설정
        }

        for (int i = 0; i < equipSlots.Length; i++)                 // 7. 장비 슬롯 UI에 PlayerInventory의 equipItems 리스트 연결
        {
            equipSlots[i].SetSlot(this, inventory.equipItems, i);   // 8. 현재 장비 슬롯에 UI 매니저, 아이템 리스트, 슬롯 인덱스 설정
        }
    }

    // ★ 실제 인벤토리를 열 때 패널을 켜고 시간을 멈춥니다.
    public void OpenInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
        }

        // 게임 속 시간을 완전히 멈춥니다. (0배속)
        Time.timeScale = 0f;

        Refresh(); // 열릴 때 최신 데이터로 새로고침
        Debug.Log("인벤토리 열림: 게임 일시 정지 (TimeScale = 0)");
    }

    // ★ 실제 인벤토리를 닫을 때 패널을 끄고 시간을 정상으로 돌립니다.
    public void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        // 게임 속 시간을 다시 정상으로 돌립니다. (1배속)
        Time.timeScale = 1f;

        Debug.Log("인벤토리 닫힘: 게임 재개 (TimeScale = 1)");
    }
}
