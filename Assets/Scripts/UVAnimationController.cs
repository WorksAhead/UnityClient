using UnityEngine;
using System.Collections;

/**
 *  first对应第一个材质贴图 second对应第二个材质贴图
 *  X 对应水平方向 Y对应垂直方向
 *  举例
 *  firstMaxX    水平方向的最大偏移
 *  firstMinX    水平方向的最小偏移
 *  firstMaxX和firstMinX相同时候只向一个方向移动
 *  firstSpeedX  水平方向移动的速度
 *  
 */
public class UVAnimationController : UnityEngine.MonoBehaviour
{
    public float firstMaxX = 0.3F;
    public float firstMinX = -0.3F;
    public float firstSpeedX = 0.5F;
    private float firstRandomX = 0.01f;
    private float firstCurX = 0;

    public float firstMaxY = 0.3F;
    public float firstMinY = -0.3F;
    public float firstSpeedY = 0.5F;
    private float firstRandomY = 0.01f;
    private float firstCurY = 0;





    public float secondMaxX = 0.3F;
    public float secondMinX = -0.3F;
    public float secondSpeedX = 0.5F;
    private float secondRandomX = 0.01f;
    private float secondCurX = 0;

    public float secondMaxY = 0.3F;
    public float secondMinY = -0.3F;
    public float secondSpeedY = 0.5F;
    private float secondRandomY = 0.01f;
    private float secondCurY = 0;

    //是否播放动画    
    public bool isPlay = true;

    void Start()
    {

    }

    void Update()
    {
        if (!isPlay)
        {
            return;
        }

        //设置第一个材质球的偏移        
        CalculateOffset(ref firstRandomX, firstMinX, firstMaxX, firstCurX, ref firstSpeedX);
        firstCurX += Time.deltaTime * firstSpeedX;

        CalculateOffset(ref firstRandomY, firstMinY, firstMaxY, firstCurY, ref firstSpeedY);
        firstCurY += Time.deltaTime * firstSpeedY;
        GetComponent<Renderer>().materials[0].SetTextureOffset("_MainTex", new UnityEngine.Vector2(firstCurX, firstCurY));


        //设置第二个材质球的偏移
        if (GetComponent<Renderer>().materials.Length <= 1)
        {
            return;
        }
        CalculateOffset(ref secondRandomX, secondMinX, secondMaxX, secondCurX, ref secondSpeedX);
        secondCurX += Time.deltaTime * secondSpeedX;


        CalculateOffset(ref secondRandomY, secondMinY, secondMaxY, secondCurY, ref secondSpeedY);
        secondCurY += Time.deltaTime * secondSpeedY;
        GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex", new UnityEngine.Vector2(secondCurX, secondCurY));


    }
    private void CalculateOffset(ref float random, float min, float max, float curOffset, ref float speed)
    {
        if (min == max)
        {
            //如果最大和最小的值相同，则在一个方向运行            
            return;
        }
        if (speed > 0 && curOffset > random)
        {
            random = Random.Range(min, (random + min) / 8);
            speed = -speed;
        }
        if (speed < 0 && curOffset < random)
        {
            random = Random.Range((random + max) * 7 / 8, max);
            speed = -speed;
        }
    }


    void OnBecameInvisible()
    {
        isPlay = false;
    }


    void OnBecameVisible()
    {
        isPlay = true;
    }


}
