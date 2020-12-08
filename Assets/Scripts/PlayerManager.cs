using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region
    public GameObject gameManager; // �Q�[���}�l�[�W���[

    public LayerMask blockLayer; // �u���b�N���C���[

    public Slider hpBar; // HP�o�[

    private GameObject weapon; // ����

    private Rigidbody2D rbody; // �v���C���[����pRigidBody2D
    private Animator animator; // �A�j���[�^�[

    private int MAX_HIT_POINT = 50;
    private const float MOVE_SPEED = 6;     // �ړ����x�Œ�l
    private float moveSpeed;                // �ړ����x
    private float jumpPower = 800;          // �W�����v�̗�
    private bool goJump = false;            // �W�����v�������ۂ�
    private bool canJump = false;           // �u���b�N�ɐڒn���Ă��邩�ۂ�
    private bool usingButtons = false;      // �{�^���������Ă��邩�ۂ�

    private bool isInvisible = false;       // ���G���

    public enum MOVE_DIR
    {
        STOP,
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.STOP; // �ړ�����

    private int hitPoint = 50; // HP

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        weapon = transform.Find("Weapon").gameObject;
        animator = weapon.GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        hpBar.maxValue = MAX_HIT_POINT;
        hpBar.value = hitPoint;
    }

    // Update is called once per frame
    void Update()
    {
        canJump = Physics2D.Linecast(transform.position - (transform.right * 0.3f) - (transform.up * 0.9f), transform.position - (transform.up * 1.2f), blockLayer) ||
                  Physics2D.Linecast(transform.position + (transform.right * 0.3f) - (transform.up * 0.9f), transform.position - (transform.up * 1.2f), blockLayer);
        if (!isInvisible)
        {
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

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PushJumpButton();
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    weapon.SetActive(true);
                    animator.SetTrigger("Once");
                }
            }
        }
        else
        {
            moveDirection = MOVE_DIR.STOP;
        }
    }

    private void FixedUpdate()
    {
        // �ړ������ŏ����𕪊�
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

        hpBar.transform.localScale = new Vector2(transform.localScale.x * -1, 1);

        // �W�����v����
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

    // �ڐG������
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !isInvisible)
        {
            // ���̃_���[�W
            hitPoint -= 3;
            hpBar.value = hitPoint;

            // �m�b�N�o�b�N����
            if (transform.position.x > col.gameObject.transform.position.x)
            {
                rbody.AddForce(Vector2.right * 800 + Vector2.up * 280);
            }
            else
            {
                rbody.AddForce(Vector2.right * -800 + Vector2.up * 280);
            }

            // ���G��Ԃɂ���
            isInvisible = true;

            StartCoroutine("OffInvisible");
        }
    }

    private IEnumerator OffInvisible()
    {
        yield return new WaitForSeconds(0.5f);

        isInvisible = false;
    }
}
