using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameObject ball;/*球*/
    Rigidbody2D ballRb;/*球的刚体*/
    Vector2 lastDirection, nowDirection;/*改变前角度，改变后角度*/
    private AudioSource source;

    public GameObject ballTrailBody;

    float[,] forceWeight = { { 0.6f , 0.3f , 0.1f , 0.01f }  ,/*墙壁等环境的加力权重*/
                                          { 1 , 0.6f , 0.3f , 0.1f } ,/*人物击打的加力权重*/
                                          { 0.8f , 0.6f , 0.4f , 0.2f } };/*障碍物的加力权重*/

    bool canAc = false;
    float[] changeAc = { 0.6f,0,0,0,0.4f };
    int changeAcIndex = 0;

    Vector3[] shapeWeight = { new Vector3( 2f , 2f, 1f ),
                                                new Vector3( 2f , 2f, 1f ),
                                                new Vector3( 1.9f , 2.05f, 1f ),
                                                new Vector3( 1.8f , 2.1f, 1f ),
                                                new Vector3( 1.6f , 2.15f, 1f ),
                                                new Vector3( 1.4f , 2.2f, 1f ),
                                                new Vector3( 1.2f , 2.3f, 1f ),};

    public float force;/*初始力*/
    public Vector2 startDirection;/*初始发射角*/

    void Start()
    {
        ball = this.gameObject;
        ballRb = ball.GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        StartCoroutine(StartGame());
        nowDirection = startDirection.normalized;
        lastDirection = nowDirection;
    }

    void Update()
    {
        if (GameController.GameStart)
        {
            ballTrailBody.transform.position = ball.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (GameController.GameStart)
        {
            if (changeAcIndex >= changeAc.Length)
            {
                canAc = false;
            }
            else if (canAc && changeAcIndex < changeAc.Length)
            {
                Acceleration(nowDirection.normalized, force);
            }
        }
        else
        {
            ballRb.velocity = new Vector2(0.0f, 0.0f);
        }
            
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3.5f);
        ballRb.AddForce(startDirection.normalized * force);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
        ChangeDirection(25f,0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        source.Play();
    }
    private void ChangeDirection(float addForce,int weight)
    {
        if (ballRb.velocity.magnitude != 0)/*修复了极短时间内多次触发碰撞丢失速度的bug，但偶尔会有速度叠加*/
        {
            nowDirection = ballRb.velocity.normalized;/*改变后角度即为球碰撞后改变的速度方向*/
            //nowDirection += new Vector2(Random.Range(0, 0.2f), Random.Range(0, 0.2f));
            //nowDirection = nowDirection.normalized;
            float angle = Vector3.SignedAngle(lastDirection, nowDirection, Vector3.forward);/*计算改变前后的角度*/
            //Debug.Log("lastDir:"+lastDirection+" nowDir:"+nowDirection+" angle:"+angle);
            ball.transform.Rotate(new Vector3(0,0,angle));/*按z轴旋转角度*/
            lastDirection = nowDirection;/*将改变前角度改为改变后角度*/
            ballRb.velocity = Vector2.zero;/*将球速归零*/
            Change(addForce,weight);/*计算出加力乘以权重后的实际施力大小*/
            Debug.Log(force);
            //ballRb.AddForce(nowDirection.normalized * force);/*重新在改变后的角度上施力*/
            
            changeAcIndex = 0;
            canAc = true;
        }             
    }

    private void Change(float addForce,int weight)
    {
        if (force < 500)
        {
            force = 500;
            ball.transform.localScale = shapeWeight[0];
        }
        else if (force < 750)
        {
            force += addForce * forceWeight[weight, 0];
            ball.transform.localScale = shapeWeight[1];
        }
        else if (force < 1000)
        {
            force += addForce * forceWeight[weight, 0];
            ball.transform.localScale = shapeWeight[2];
        }
        else if (force <1250)
        {
            force += addForce * forceWeight[weight, 0];
            ball.transform.localScale = shapeWeight[3];
        }
        else if (force < 1500)
        {
            force += addForce * forceWeight[weight,0];
            ball.transform.localScale = shapeWeight[4];
        }
        else if (force < 1750)
        {
            force += addForce * forceWeight[weight, 1];
            ball.transform.localScale = shapeWeight[5];
        }
        else if (force < 2000)
        {
            force += addForce * forceWeight[weight, 1];
            ball.transform.localScale = shapeWeight[6];
        }
        else if (force < 2400)
        {
            force += addForce * forceWeight[weight, 2];
        }
        else if (force < 2500)
        {
            force += addForce * forceWeight[weight, 3];
        }
        else
        {
            //force = 2500f;
        }
    }

    private void Acceleration(Vector2 direction,float force)
    {
        ballRb.AddForce(direction * force * changeAc[changeAcIndex]);
        changeAcIndex++;
    }
}
