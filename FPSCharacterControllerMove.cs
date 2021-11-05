using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCharacterControllerMove : MonoBehaviour
{
    private CharacterController characterController;
    private Transform characterTransform;
    private Vector3 moveDirection;
    private bool isCrouched;
    public float runSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float Gravity=9.8f;
    public float JumpHeight;
    public float CrouchHeight=1f;//下蹲高度
    private float originHeight;//原始高度
    private float currentSpeed;
    private float horizontal=0;
    private float vertical=0;
    private bool CrouchBlock;//下蹲协程阻塞
    private void Start() 
    {
        characterController=GetComponent<CharacterController>();
        characterTransform=transform;
        originHeight=characterController.height;
    }
    private void Update() 
    {
        //isGrounded只记录最后一次调用Move后的位置
        if(characterController.isGrounded)
        {
            horizontal=Input.GetAxis("Horizontal");
            vertical=Input.GetAxis("Vertical");
            if(vertical!=0)
            {
                horizontal*=0.4f;
            }
            else
            {
                horizontal*=0.8f;
            }
            moveDirection=new Vector3(horizontal,0,vertical);
            //转全局坐标
            moveDirection=characterTransform.TransformDirection(moveDirection);

            if(Input.GetButtonDown("Jump"))
            {
                if(isCrouched)
                {
                    if(!CrouchBlock) StartCoroutine(DoCrouch(originHeight));
                    isCrouched=!isCrouched;
                }
                else
                {
                    moveDirection.y=JumpHeight;
                }
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                var targetHeight=isCrouched?originHeight:CrouchHeight;
                if(!CrouchBlock) StartCoroutine(DoCrouch(targetHeight));
                isCrouched=!isCrouched; 
            }
            if(Input.GetKeyDown(KeyCode.LeftShift)&&isCrouched)
            {
                if(!CrouchBlock) StartCoroutine(DoCrouch(originHeight));
                isCrouched=!isCrouched;
            }
            
        }
        if(isCrouched)
        {
            currentSpeed=crouchSpeed;
        }
        else
        {
            currentSpeed=(Input.GetKey(KeyCode.LeftShift)&&vertical>0)?runSpeed:walkSpeed;
        }  
        moveDirection.y-=Gravity*Time.deltaTime;
        characterController.Move(moveDirection*currentSpeed*Time.deltaTime);
    }
    private IEnumerator DoCrouch(float target)
    {
        CrouchBlock=true;
        float tmp_Velocity=0;
        while(Mathf.Abs(characterController.height-target)>0.01f)
        {
            yield return null;
            characterController.height=Mathf.SmoothDamp(characterController.height,target,ref tmp_Velocity,Time.deltaTime*20);
        }
        CrouchBlock=false;
    }
}
