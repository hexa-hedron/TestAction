using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ■定数・プロパティ

    public GameObject Player;
    public GameObject EnemyL;
    public GameObject EnemyH;

    private int enemyCount;     // 敵の数
    private int MAX_ENEMY = 3;  // 画面上の敵の最大数

    private int norma = 15;     // 敵残数

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        for(var i = enemyCount + 1; i <= MAX_ENEMY; i++)
        {
            CreateEnemy();
            enemyCount = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (enemyCount < MAX_ENEMY)
        {
            for (var i = enemyCount + 1; i <= MAX_ENEMY; i++)
            {
                CreateEnemy();
                enemyCount = i;
            }
        }
    }

    // 敵を追加
    void CreateEnemy()
    {
        var rand = Random.value;
        var enemy = rand > 0.5f ? Instantiate(EnemyH) : Instantiate(EnemyL);
        var enemyManager = enemy.GetComponent<EnemyManager>();
        enemyManager.player = Player;
        enemyManager.gameManager = this.gameObject;
        enemy.transform.localPosition = new Vector3(
            Random.Range(-1.0f,1.0f),
            -2.0f
            ); // TODO
    }

    public void DecreceEnemyCount()
    {
        enemyCount--;
        norma--;
    }
}
