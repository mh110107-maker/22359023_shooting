using UnityEngine;

public class MonsterDropper : MonoBehaviour
{
    [System.Serializable]
    public class DropTable
    {
        public ItemData itemData;
        [Range(0f, 1f)] public float dropRate = 0.5f;
        public int minCount = 1;
        public int maxCount = 1;
    }

    public GameObject dropPrefab;
    public DropTable[] dropTables;

    public void Drop()
    {  
        if (dropPrefab == null || dropTables == null) return; 

        foreach (DropTable table in dropTables)
        { 
            if (table.itemData == null) continue;
            if (Random.value > table.dropRate) continue;


            /*
             * dropPrefab을 생성하고 DropItem에 데이터 넣기
             */

            // 몬스터 위치에 DropItem 프리팹 생성
            GameObject dropObject = Instantiate(
                dropPrefab,
                transform.position,
                Quaternion.identity
            );

            // 생성된 DropItem 프리팹에서 DropItem 컴포넌트 가져오기
            DropItem dropItem = dropObject.GetComponent<DropItem>();

            // DropItem 컴포넌트가 존재하면 아이템 정보 설정
            if (dropItem != null)
            {
                // 드랍될 아이템 데이터 설정
                dropItem.itemData = table.itemData;

                // 최소 개수와 최대 개수 사이에서 랜덤 개수 설정
                dropItem.count = Random.Range(table.minCount, table.maxCount + 1);
            }

        }
    }







}
