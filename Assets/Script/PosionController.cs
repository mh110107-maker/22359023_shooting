using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionController : MonoBehaviour
{
    // 유니티 인스펙터 창에서 포션의 ItemData를 드래그해서 넣어주세요.
    public ItemData potionData;
    public int amount = 1;

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
                    Destroy(gameObject); // 가방에 들어갔으니 필드 포션 삭제
                }
                else
                {
                    Debug.LogWarning("인벤토리가 가득 찼습니다!");
                }
            }
        }
    }
}
