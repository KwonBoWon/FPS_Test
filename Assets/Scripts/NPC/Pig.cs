using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;

    private Vector3 direction;

    private bool isAction;
    private bool isWalking;

    [SerializeField] private float walkTime;
    [SerializeField] private float waitTime;
    private float currentTime;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider boxCol;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking)
        {
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
        }
    }
    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ResetAni();
            }
        }
    }

    private void ResetAni()
    {
        isWalking = false; isAction = true;
        anim.SetBool("Walking", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        isAction = true;
        int _random = Random.Range(0, 4); // ´ë±â, Ç®¶â±â, µÎ¸®¹ø, °È±â
        
        if(_random == 0)
        {
            Wait();
        }
        else if(_random == 1)
        {
            Eat();
        }
        else if (_random == 2)
        {
            Peek();
        }
        else if (_random == 3)
        {
            TryWalk();
        }
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("´ë±â");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Ç®¶â±â");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("µÎ¸®¹ø");
    }
    private void TryWalk()
    {
        isWalking = true; 
        currentTime = waitTime;
        anim.SetBool("Walking", isWalking);
        Debug.Log("°È±â");
    }

}
