using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // ���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // ������ üũ ����
    private Vector3 lastPos;

    // �ɾ��� �� �󸶳� ���� �� �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // �� ���� ����
    private CapsuleCollider capsuleCollider;

    // �ΰ���
    [SerializeField]
    private float lookSensitivity;

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRoatationLimit;
    private float currentCameraRoatationX = 0f;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private CrossHair theCrossHair;



    void Start()
    {
        //theCamera = FindObjectOfType<Camera>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrossHair = FindObjectOfType<CrossHair>();


        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;// �÷��̾ �ɴ°��� �ƴ϶� ī�޶� �ɴ°�(�÷��̾ �ڽ��̹Ƿ� localPos)
        applyCrouchPosY = originPosY;
        MoveCheck();
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.0001f)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            theCrossHair.WalkingAnimation(isWalk);
            lastPos = transform.position;
            Debug.Log(Vector3.Distance(lastPos, transform.position));
        }

    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        MovePlayer();
        CameraRoatate();
        CharacterRotate();
        MoveCheck();
    }
    //�ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    // �ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrossHair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CouchCoroutine());
        
    }
    // �ε巯�� �ɱ�
    IEnumerator CouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
            {
                break;
            }
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }
    // �� Ȯ��(raycast)
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y * 1.1f);// bound.extents�� ũ���� ���̴�

    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }

        
    }
    private void Jump()
    {
        // �������¿��� �����ϸ� �������� ����
        if (isCrouch)
        {
            Crouch();
        }
        myRigid.velocity = transform.up * jumpForce;// (0, 1, 0)
    }
    
    // �޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    // �޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        theCrossHair.RunningAnimation(isRun);
        applySpeed = walkSpeed;

    }
    // �޸���
    private void Running()
    {
        if (isCrouch)// �������¿��� �޸��� �ɱ� ���
        {
            Crouch();
        }
        theGunController.CancelFineSight();
        
        isRun = true;
        theCrossHair.RunningAnimation(isRun);
        applySpeed = runSpeed;
        
    }
    // wasd������
    private void MovePlayer()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //normalized(���� ����ȭ)-> ���⸸ �����̱� ������

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); //Time.deltaTime ~= 0.016
        
    }
    /// <summary>
    /// �¿� ĳ���� ȸ��
    /// </summary>
    private void CharacterRotate() 
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //Debug.Log(myRigid.rotation);
        //Debug.Log(myRigid.rotation.eulerAngles);

    }
    /// <summary>
    /// ���� ī�޶� ȸ��
    /// </summary>
    private void CameraRoatate()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRoatationX -= _cameraRotationX;
        currentCameraRoatationX = Mathf.Clamp(currentCameraRoatationX, -cameraRoatationLimit, cameraRoatationLimit); //Mathf.Clamp: �ּ� �ִ밪 �����ʰ�

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRoatationX, 0f, 0f);
    }

}
