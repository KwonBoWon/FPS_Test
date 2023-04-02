using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 앉았을 때 얼마나 앉을 지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지 여부
    private CapsuleCollider capsuleCollider;

    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRoatationLimit;
    private float currentCameraRoatationX = 0f;

    // 필요한 컴포넌트
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
        originPosY = theCamera.transform.localPosition.y;// 플레이어가 앉는것이 아니라 카메라가 앉는것(플레이어에 자식이므로 localPos)
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
    //앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    // 앉기 동작
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
    // 부드러운 앉기
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
    // 땅 확인(raycast)
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y * 1.1f);// bound.extents는 크기의 반이다

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
        // 앉은상태에서 점프하면 앉은상태 해제
        if (isCrouch)
        {
            Crouch();
        }
        myRigid.velocity = transform.up * jumpForce;// (0, 1, 0)
    }
    
    // 달리기 시도
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
    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrossHair.RunningAnimation(isRun);
        applySpeed = walkSpeed;

    }
    // 달리기
    private void Running()
    {
        if (isCrouch)// 앉은상태에서 달리면 앉기 취소
        {
            Crouch();
        }
        theGunController.CancelFineSight();
        
        isRun = true;
        theCrossHair.RunningAnimation(isRun);
        applySpeed = runSpeed;
        
    }
    // wasd움직임
    private void MovePlayer()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; //normalized(벡터 정규화)-> 방향만 쓸것이기 때문에

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); //Time.deltaTime ~= 0.016
        
    }
    /// <summary>
    /// 좌우 캐릭터 회전
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
    /// 상하 카메라 회전
    /// </summary>
    private void CameraRoatate()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRoatationX -= _cameraRotationX;
        currentCameraRoatationX = Mathf.Clamp(currentCameraRoatationX, -cameraRoatationLimit, cameraRoatationLimit); //Mathf.Clamp: 최소 최대값 넘지않게

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRoatationX, 0f, 0f);
    }

}
