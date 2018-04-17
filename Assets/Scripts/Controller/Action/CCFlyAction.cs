/** 
 * 这个文件是实现飞碟的飞行动作 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction
{
    private float acceleration; //重力加速度=9.8f

    private float horizontalSpeed; //飞碟水平方向的速度
    private float verticalSpeed;   //飞碟竖直方向的速度

    private Vector3 direction; //飞碟的初始飞行方向

    public override void Start()
    {
        enable = true;
        acceleration = 9.8f;
        verticalSpeed = 0;
        horizontalSpeed = gameobject.GetComponent<DiskData>().speed;
        direction = gameobject.GetComponent<DiskData>().direction;
    }

    // Update is called once per frame  
    public override void Update()
    {
        if (gameobject.activeSelf)
        {
            verticalSpeed += Time.deltaTime * acceleration;

            transform.Translate(direction * horizontalSpeed * Time.deltaTime); //水平运动

            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime); //竖直运动

            if (transform.position.y < -4)
            {
                destroy = true;
                enable = false;
                callback.SSActionEvent(this);
            }
        }

    }

    public static CCFlyAction GetSSAction()
    {
        CCFlyAction action = CreateInstance<CCFlyAction>();
        return action;
    }
}