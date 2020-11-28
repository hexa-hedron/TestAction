using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region 定数・プロパティ
    public GameObject gameManager;  // ゲームマネージャー
    public GameObject player;       // プレイヤー

    private Rigidbody2D rbody;      // RigidBody2D

    private const float MOVE_SPEED = 3; // 移動速度固定値
    private float moveSpeed;        // 移動速度

    public enum MOVE_DIR
    {
        STOP,
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.LEFT; // 移動方向
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
                transform.localScale = new Vector2(-1, 1);
                break;
            case MOVE_DIR.RIGHT:
                moveSpeed = MOVE_SPEED * 1;
                transform.localScale = new Vector2(1, 1);
                break;
        }

        rbody.velocity = new Vector2(moveSpeed, rbody.velocity.y);
    }
}
