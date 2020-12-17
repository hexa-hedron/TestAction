using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ■定数・プロパティ

    public GameObject EnemyL;
    public GameObject EnemyH;

    private int enemyCount;     // 敵の数
    private int MAX_ENEMY = 3;  // 画面上の敵の最大数

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateEnemy()
    {
        var rand = Random.value;
        var enemy = rand > 0.5f ? (GameObject)Instantiate(EnemyH) : (GameObject)Instantiate(EnemyL);
        enemy.transform.localPosition = new Vector3(); // TODO
    }
}
