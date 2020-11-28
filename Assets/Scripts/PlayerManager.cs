using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region
    public GameObject gameManager; // �Q�[���}�l�[�W���[

    public LayerMask blockLayer; // �u���b�N���C���[

    private Rigidbody2D rbody; // �v���C���[����pRigidBody2D
    private Animator animator; // �A�j���[�^�[

    private const float MOVE_SPEED = 6; // �ړ����x�Œ�l
    private float moveSpeed; // �ړ����x
    private float jumpPower = 800; // �W�����v�̗�
    private bool goJump = false; // �W�����v�������ۂ�
    private bool canJump = false; // �u���b�N�ɐڒn���Ă��邩�ۂ�
    private bool usingButtons = false; // �{�^���������Ă��邩�ۂ�

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
        rbody = GetComponent<Rigidbody2D>();
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
}