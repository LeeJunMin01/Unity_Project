using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
   public PlayerController player; //플레이어
   public int stage; //스테이지 변수
   public bool isBattle; //스테이지 시작 or 종료 변수
   public int enemyCnt; // 적 남은 수
   public Text stageTxt; // 스테이지 UI 변수
   public Image playerHealth; // HP UI 변수
   private float fillAmount = 1; // HP 값 변수

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    void Awake()
    {
        enemyList = new List<int>();
    }

    public void DecreseHp()
    {
        fillAmount = ((float)player.health / (float)player.maxHealth);
        playerHealth.fillAmount = fillAmount;
    }

    void LateUpdate() {
        DecreseHp();
        stageTxt.text = "STAGE " + stage;
    }

    public void StageStart()
    {
        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        player.transform.position = Vector3.up * 0.8f;

        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;

        Invoke("StageStart", 3f); //3초뒤에 스테이지 시작
    }

    IEnumerator InBattle()
    {
        for(int index=0; index < stage; index++){
            int ran = Random.Range(0, 3);
            enemyList.Add(ran);

            enemyCnt++;
        }

        while(enemyList.Count > 0){
            int ranZone = Random.Range(0, 2);
            GameObject instantEnemy = Instantiate(enemies[0], enemyZones[ranZone].position ,enemyZones[ranZone].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);
        }

        while(enemyCnt > 0){
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        StageEnd();

    } 
    // Update is called once per frame
    void Start() {
        StageStart();
    }
    void Update()
    {
        if(stage > 10){
            GameOver();
        }
    }

    public void GameOver(){
        SceneManager.LoadScene("ClearScene");
    }
}
