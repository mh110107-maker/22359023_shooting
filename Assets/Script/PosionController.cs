using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionController : MonoBehaviour
{
    // 유니티 인스펙터 창에서 포션의 ItemData를 드래그해서 넣어주세요.
    public ItemData potionData;
    public int amount = 1;

    // ★ 포션이 맵에 스폰(생성)되자마자 실행되는 함수
    private void Start()
    {
        // -----------------------------------------------------------------
        // [핵심 기능] 생성된 지 5초가 지나면 이 게임 오브젝트를 자동으로 파괴합니다.
        // -----------------------------------------------------------------
        Destroy(gameObject, 5f);
        // -----------------------------------------------------------------
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            if (PlayerInventory.Instance != null)
            {
                // 인벤토리에 아이템 추가 시도
                bool isSuccess = PlayerInventory.Instance.AddItem(potionData, amount);

                if (isSuccess)
                {
                    Debug.Log("포션 획득 성공 및 UI 갱신 완료!");

                    // 캐릭터가 5초 안에 몸으로 먹었으므로, 
                    // 5초 타이머를 기다리지 않고 즉시 맵에서 삭제합니다.
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("인벤토리가 가득 찼습니다!");
                }
            }
        }
    }
}