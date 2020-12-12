using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region �萔�E�v���p�e�B
    public GameObject gameManager;  // �Q�[���}�l�[�W���[
    public GameObject player;       // �v���C���[

    private Rigidbody2D rbody;      // RigidBody2D

    private const float MOVE_SPEED = 2; // �ړ����x�Œ�l
    private float moveSpeed;            // �ړ����x

    private bool isInvisible = false;       // ���G���

    public enum MOVE_DIR
    {
        STOP,
        LEFT,
        RIGHT,
    };

    private MOVE_DIR moveDirection = MOVE_DIR.LEFT; // �ړ�����

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
        // �v���C���[�ɒǏ]����悤����������
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
        // �ړ������ŏ����𕪊�
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

    // �ڐG������
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Attack" && !isInvisible)
        {
            // ���̃_���[�W
            hitPoint -= 2;
            //hpBar.value = hitPoint;

            if (hitPoint <= 0)
            {
                Destroy(this.gameObject);
            }

            // �m�b�N�o�b�N����
            if (player.transform.position.x < this.gameObject.transform.position.x)
            {
                rbody.AddForce(Vector2.right * 800 + Vector2.up * 280);
            }
            else
            {
                rbody.AddForce(Vector2.right * -800 + Vector2.up * 280);
            }
        }

        // ���G��Ԃɂ���
        isInvisible = true;

        StartCoroutine("OffInvisible");
    }

    /// <summary>
    /// ���G��Ԃ���������
    /// </summary>
    /// <returns></returns>
    private IEnumerator OffInvisible()
    {
        yield return new WaitForSeconds(0.5f);

        isInvisible = false;
    }
}
