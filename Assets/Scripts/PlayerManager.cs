using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region
    public GameObject gameManager; // ゲームマネージャー

    public LayerMask blockLayer; // ブロックレイヤー

    public Slider hpBar; // HPバー

    private Rigidbody2D rbody; // プレイヤー制御用RigidBody2D
    private Animator animator; // アニメーター

    private int MAX_HIT_POINT = 50;
    private const float MOVE_SPEED = 6;     // 移動速度固定値
    private float moveSpeed;                // 移動速度
    private float jumpPower = 800;          // ジャンプの力
    private bool goJump = false;            // ジャンプしたか否か
    private bool canJump = false;           // ブロックに接地しているか否か
    private bool usingButtons = false;      // ボタンを押しているか否か

    private bool isInvisible = false;       // 無敵状態

    public enum MOVE_DIR
    {
        STOP,
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.STOP; // 移動方向

    private int hitPoint = 50; // HP

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        hpBar.maxValue = MAX_HIT_POINT;
        hpBar.value = hitPoint;
    }

    // Update is called once per frame
    void Update()
    {
        canJump = Physics2D.Linecast(transform.position - (transform.right * 0.3f) - (transform.up * 0.9f), transform.position - (transform.up * 1.2f), blockLayer) ||
                  Physics2D.Linecast(transform.position + (transform.right * 0.3f) - (transform.up * 0.9f), transform.position - (transform.up * 1.2f), blockLayer);

        if (!usingButtons)
        {
            float x = Input.GetAxisRaw("Horizontal");

            if (x == 0)
            {
                moveDirection = MOVE_DIR.STOP;
            }
            else
            {
                if (x < 0)
                {
                    moveDirection = MOVE_DIR.LEFT;
                }
                else
                {
                    moveDirection = MOVE_DIR.RIGHT;
                }
            }

            if (Input.GetKeyDown("space"))
            {
                PushJumpButton();
            }
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

        // ジャンプ処理
        if (goJump)
        {
            rbody.AddForce(Vector2.up * jumpPower);
            goJump = false;
        }
    }

    public void PushLeftButton()
    {
        moveDirection = MOVE_DIR.LEFT;
        usingButtons = true;
    }

    public void PushRightButton()
    {
        moveDirection = MOVE_DIR.RIGHT;
        usingButtons = true;
    }

    public void ReleaseMoveButton()
    {
        moveDirection = MOVE_DIR.STOP;
        usingButtons = false;
    }

    public void PushJumpButton()
    {
        if (canJump)
        {
            goJump = true;
        }
    }

    // 接触時処理
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !isInvisible)
        {
            // 仮のダメージ
            hitPoint -= 3;
            hpBar.value = hitPoint;

            // ノックバック処理
            if (transform.position.x > col.gameObject.transform.position.x)
            {
                rbody.AddForce(Vector2.right * 800 + Vector2.up * 280);
            }
            else
            {
                rbody.AddForce(Vector2.right * -800 + Vector2.up * 280);
            }

            // 無敵状態にする
            isInvisible = true;

            StartCoroutine("OffInvisible");
        }
    }

    private IEnumerator OffInvisible()
    {
        yield return new WaitForSeconds(1);

        isInvisible = false;
    }
}
