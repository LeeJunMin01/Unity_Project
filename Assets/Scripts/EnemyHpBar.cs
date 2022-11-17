using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Image hpBar;
    public Enemy enemy;
    private float EnemyHp = 1; // HP 값 변수

    // Start is called before the first frame update
    public void DecreseHp()
    {
        EnemyHp = ((float)enemy.curHealth / (float)enemy.maxHealth);
        if(EnemyHp < 0){
            EnemyHp = 0;
        }
        hpBar.rectTransform.localScale = new Vector3(EnemyHp, 1f, 1f);
    }

    void LateUpdate() {
        DecreseHp();
    }

    void Start()
    {
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
