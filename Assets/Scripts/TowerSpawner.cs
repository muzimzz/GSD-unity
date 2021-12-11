using System.Collections;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate; //타워 정보(공격력, 공격속도 등)
    //[SerializeField]
    //private GameObject towerPrefab;
    //[SerializeField]
    //private int towerBuildGold = 50;//타원 건설에 사용되는 골드
    [SerializeField]
    private EnemySpawner enemySpawner;// 현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    [SerializeField]
    private PlayerGold playerGold;//타워 건설 시 골드 감소를 위해.
    [SerializeField]
    private SystemTextViewer systemTextViewer; // 돈 부족, 건설 불가와 같은 시스템 메세지 출력
    private bool isOnTowerButton = false; // 타워 건설 버튼을 눌렀는지 체크
    private GameObject followTowerClone = null; // 임시 타워 사용 완료 시 삭제를 위해 저장하는 변수
    private int towerType; // 타워 속성
    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        //버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if(isOnTowerButton ==true)
        {
            return;
        }
        //타워 건설 가능 여부 확인
        //타워 건설할 만큼 돈이 없으면 타워 건설 x
        
        if(towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            //골드가 부족해서 타워 건설이 불가느아다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        //타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;
        //마우스를 다라다니는 임시타워 생성
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        //타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }
    public void SpawnTower(Transform tileTransform)
    {
        //타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if(isOnTowerButton == false)
        {
            return;
        }
   
    // 타워 건설 가능 여부 확인
  
    Tile tile = tileTransform.GetComponent<Tile>();

     //2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 x
    if(tile.IsBuildTower == true)
    {
        //현재 위치에 타워 건설이 불가능하다고 출력
        systemTextViewer.PrintText(SystemType.Build);
         return;
    }
    //다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
    isOnTowerButton = false;
     // 타워가 건설되어 있음으로 설정
    tile.IsBuildTower = true;
    //타워 건설에 필요한 골드만큼 감소
    playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
    // 선택한 타일의 위치에 타워 건설 (타일보다 z축 -1의 위치에 배치)
    Vector3 position = tileTransform.position + Vector3.back;
    //GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
    GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
    // 타워 무기에 enemySpawner, playerGold, tile 정보 전달
    clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold,tile);      
    //Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
    
    //타워를 배치했기 떄문에 마우스를 따라다니는 임시 타워 삭제
    Destroy(followTowerClone);
    //타워 건설을 취소할 수 있는 코루틴 함수 중지
    StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while(true)
        {
            //ESC키 또는 마우스 오른쪽 버튼을 눌렀을 때 타워 건설 취소
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                //마우스를 따라다니는 임시 타워 삭제
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }
 }




//File: TowerSpawner.cs
//Desc: 타워 생성 제어