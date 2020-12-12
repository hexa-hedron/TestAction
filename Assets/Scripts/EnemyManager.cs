using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region 定数・プロパティ
    public GameObject gameManager;  // ゲームマネージャー
    public GameObject player;       // プレイヤー

    private Rigidbody2D rbody;      // RigidBody2D

    private const float MOVE_SPEED = 2; // 移動速度固定値
    private float moveSpeed;            // 移動速度

    private bool isInvisible = false;       // 無敵状態

    public enum MOVE_DIR
    {
        STOP,
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.LEFT; // 移動方向

    private int hitPoint = 10; // HP

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーに追従するよう方向を決定
        if(player.transform.position.x < this.gameObject.transform.position.x)
        {
            moveDirection = MOVE_DIR.LEFT;
        }
        else if(player.transform.position.x > this.gameObject.transform.position.x)
        {
            moveDirection = MOVE_DIR.RIGHT;
        }
        else
        {
            moveDirection = MOVE_DIR.STOP;
        }
    }

    private void FixedUpdate()
    {
        // 移動方向で処理を分岐
        switch (moveDirection)
        {
            case MOVE_DIR.STOP:
                moveSpeed = 0;
                break;
            case MOVE_DIR.LEFT:
                moveSpeed = MOVE_SPEED * -1;
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x) * -1, transform.localScale.y);
                break;
            case MOVE_DIR.RIGHT:
                moveSpeed = MOVE_SPEED * 1;
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x), transform.localScale.y);
                break;
        }

        rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
    }

    // 接触時処理
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Attack" && !isInvisible)
        {
            // 仮のダメージ
            hitPoint -= 2;
            //hpBar.value = hitPoint;

            if (hitPoint <= 0)
            {
                Destroy(this.gameObject);
            }

            // ノックバック処理
            if (player.transform.position.x < this.gameObject.transform.position.x)
            {
                rbody.AddForce(Vector2.right * 800 + Vector2.up * 280);
            }
            else
            {
                rbody.AddForce(Vector2.right * -800 + Vector2.up * 280);
            }
        }

        // 無敵状態にする
        isInvisible = true;

        StartCoroutine("OffInvisible");
    }

    /// <summary>
    /// 無敵状態を解除する
    /// </summary>
    /// <returns></returns>
    private IEnumerator OffInvisible()
    {
        yield return new WaitForSeconds(0.5f);

        isInvisible = false;
    }
}
