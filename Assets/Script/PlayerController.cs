


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float walkSpeed = 20f; // �����ӵ�
    public float horizontalSpeed = 100f; // ���� ȸ�� �ӵ�
    private float jumpForce = 5f;  // �����ϴ� ��

    public TMP_Text nickNameUi; // �г��� UI

    private bool isGround = true;           // ĳ���Ͱ� ���� �ִ��� Ȯ���� ����
    private Rigidbody myRigid;
    private CinemachineVirtualCamera virtualCamera;
    private Animator animator;
    private TMP_InputField chatInputField;

    Vector3 moveVec;
    float _moveDirX;
    float _moveDirZ;

    void Awake()
    {
        myRigid = GetComponent<Rigidbody>(); // Rigidbody �Ӽ� ������ �ִ� ������Ʈ �ε�
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
        chatInputField = GameObject.FindObjectOfType<TMP_InputField>();
    }
    void Start() // ����
    {
        // �г��� ����
        nickNameUi.text = photonView.Owner.NickName;

        // �ڽ��� ĳ������ ��� �ó׸ӽ� ī�޶� ����
        if (photonView.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
            // �г��� ��Ȱ��ȭ(�� �г����� ���� �Ⱥ��̰�)
            nickNameUi.gameObject.SetActive(false);
        }
    }

    void Update() // �뷫 1�ʿ� 60������ �ݺ�
    {
        // �ڽ��� ĳ����(��Ʈ��ũ ��ü)�� ��Ʈ��
        if (photonView.IsMine)
        {
            _moveDirX = Input.GetAxisRaw("Horizontal"); // �¿� Ű
            _moveDirZ = Input.GetAxisRaw("Vertical"); // ���� Ű
            if (!chatInputField.isFocused)
            {
                VerticalMove(); // ĳ���� ������
                Rotate(); // ĳ���� �¿�ȸ��
            }
        }
    }

    // �浹 �Լ�
    void OnCollisionEnter(Collision collision)
    {
        // �ε��� ��ü�� �±װ� "Ground"���
        if (collision.gameObject.CompareTag("Ground"))
        {
            // isGround�� true�� ����
            isGround = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), false);
        }
    }

    void VerticalMove()
    {
        Vector3 _moveVertical = transform.forward * _moveDirZ; // ���� ��ǥ
        moveVec = _moveVertical.normalized;

        Vector3 _velocity = moveVec * walkSpeed; // �ӵ�

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // �ӵ��� ���� ��ġ�̵�

        animator.SetBool("isWalking", moveVec != Vector3.zero); // Run �ִϸ��̼� true
    }
    void Rotate() // �¿� ĳ���� ȸ�� - Ű����
    {
        float horizontal = _moveDirX * horizontalSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, Vector3.up, horizontal);
    }
}