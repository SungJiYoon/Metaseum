using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExhibitController : MonoBehaviour
{
    public float walkSpeed = 20f; // �����ӵ�
    public float horizontalSpeed = 100f; // ���� ȸ�� �ӵ�

    public CinemachineVirtualCamera virtualCamera;

    private bool isGround = true;           // ĳ���Ͱ� ���� �ִ��� Ȯ���� ����
    private Rigidbody myRigid;
    Vector3 moveVec;
    float _moveDirX;
    float _moveDirZ;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>(); // Rigidbody �Ӽ� ������ �ִ� ������Ʈ �ε�
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal"); // �¿� Ű
        _moveDirZ = Input.GetAxisRaw("Vertical"); // ���� Ű
        VerticalMove(); // ĳ���� ������
        Rotate(); // ĳ���� �¿�ȸ��
    }

    void OnCollisionEnter(Collision collision)
    {
        // �ε��� ��ü�� �±װ� "Ground"���
        if (collision.gameObject.CompareTag("Ground"))
        {
            // isGround�� true�� ����
            isGround = true;
        }
    }
    void VerticalMove()
    {
        Vector3 _moveVertical = transform.forward * _moveDirZ; // ���� ��ǥ
        moveVec = _moveVertical.normalized;

        Vector3 _velocity = moveVec * walkSpeed; // �ӵ�

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // �ӵ��� ���� ��ġ�̵�
    }

    void Rotate() // �¿� ĳ���� ȸ�� - Ű����
    {
        float horizontal = _moveDirX * horizontalSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, Vector3.up, horizontal);
    }
}
