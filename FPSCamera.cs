using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField]
    private Transform characterBodyTransform;
    private Vector3 cameraRotation;
    private Vector3 characterBodyRotation;
    public float xSpeed=5f;
    public float ySpeed=5f;
    public float yAngleMinLimit=-50f;
    public float yAngleMaxLimit=50f;
    // Start is called before the first frame update
    void Start()
    {
        if(!characterBodyTransform)
        {
            UnityEngine.Debug.Log("未绑定人物模型");
        }
    }
    void LateUpdate()
    {
        var MouseX=Input.GetAxis("Mouse X");
        var MouseY=Input.GetAxis("Mouse Y");
        cameraRotation.x-=MouseY*ySpeed;
        cameraRotation.x=ClampAngle(cameraRotation.x,yAngleMinLimit,yAngleMaxLimit);
        characterBodyRotation.y+=MouseX*xSpeed;

        characterBodyTransform.rotation=Quaternion.Euler(0,characterBodyRotation.y,0);
        transform.rotation=Quaternion.Euler(cameraRotation.x,characterBodyRotation.y,0); 
    }
    static float ClampAngle(float angle,float min,float max)
    {
        if(angle<-360)
            angle+=360;
        if(angle>360)
            angle-=360;
        return Mathf.Clamp(angle,min,max);
    }
}
