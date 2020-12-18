using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ���萔�E�v���p�e�B

    public GameObject Player;
    public GameObject EnemyL;
    public GameObject EnemyH;

    private int enemyCount;     // �G�̐�
    private int MAX_ENEMY = 3;  // ��ʏ�̓G�̍ő吔

    private int norma = 15;     // �G�c��

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        for(var i = enemyCount; i <= MAX_ENEMY; i++)
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
            for (var i = enemyCount; i <= MAX_ENEMY; i++)
            {
                CreateEnemy();
                enemyCount = i;
            }
        }
    }

    // �G��ǉ�
    void CreateEnemy()
    {
        var rand = Random.value;
        var enemy = rand > 0.5f ? Instantiate(EnemyH) : Instantiate(EnemyL);
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
