using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private float speed;
    private Spoon spoon;
    public float rotateSpeed = 1f;
    [SerializeField] public GameObject projectile;

    public double currentpuan;
    public double puan;
    public CinemachineVirtualCamera atýsCam;
    public bool isWinnable = false;

    void Start()
    {
        spoon = GetComponentInChildren<Spoon>();
        atýsCam = GameObject.FindGameObjectWithTag("atýsCam").GetComponent<CinemachineVirtualCamera>();
    }


    void Update()
    {
        if (spoon == null)
        {
            spoon = GetComponentInChildren<Spoon>();
        }
        PlayerForwardMovement();
        if (transform.position.z > 2.5f)
        {
            if (GameManager.Instance.currentState == GameManager.GameState.Finish ||
                GameManager.Instance.currentState == GameManager.GameState.Normal)
            {
                ThrowFeed();
            }
        }
    }

    public void PlayerForwardMovement()
    {
        Vector3 targetDirection = new Vector3(0, 0, GameManager.Instance.playerSpeed);
        targetDirection += transform.localPosition;
        if (GameManager.Instance.currentState != GameManager.GameState.Normal)
        {
            targetDirection = transform.position;
        }

        transform.position = Vector3.Lerp(transform.position, targetDirection,
            GameManager.Instance.playerSmooth * Time.deltaTime);

    }


    private void ThrowFeed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.currentState != GameManager.GameState.Finish)
            {
                atýsCam.Priority = 10;
            }

            spoon.transform.DORotate(new Vector3(-60f, 180f, 0f), 0.75f, RotateMode.Fast)
                .OnComplete(() => spoon.SetLineActive(true));
            speed = GameManager.Instance.playerSpeed;
            GameManager.Instance.playerSpeed = 0;
            
        }
        else if (Input.GetMouseButton(0))
        {
            spoon.IsThrowing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.Instance.currentState != GameManager.GameState.Finish)
            {
                atýsCam.Priority = 0;
            }

            Vector3 target = new Vector3(-10f,0f,0f);
            spoon.transform.DORotate(target, 0.5f, RotateMode.Fast);
            spoon.transform.DORotate(new Vector3(0f, 0f, 0f), 0.75f)
                .OnComplete(() => GameManager.Instance.playerSpeed = speed);
            spoon.IsThrowing = false;
            spoon.SetLineActive(false);
            GameObject projectileInstance = Instantiate(projectile, spoon.fillSphere.position, Quaternion.identity);
            projectileInstance.GetComponent<Rigidbody>().velocity = spoon.fillSphere.up * 25f;
            if (GameManager.Instance.currentState == GameManager.GameState.Finish)
            {
                spoon.projectileInstance.GetComponent<BoxCollider>().center = new Vector3(0.08205364f, 0.281014f,0.25f);
                spoon.projectileInstance.GetComponent<BoxCollider>().size = new Vector3(2.606035f, 2.423266f,1);
                isWinnable = true;
                float value = GameObject.FindGameObjectWithTag("TimingBar").GetComponent<Slider>().value;
                currentpuan = GetPuan(value);
                projectileInstance.transform.localScale += spoon.takenMamas * 0.05f * Vector3.one * value/5;
                spoon.mama.SetActive(false);
            }
        }
    }

    private int GetPuan(float value)
    {
        int deger = Mathf.FloorToInt(value);
        int point = 10;
        switch (deger)
        {
            case 1:
                point = 10;
                break;
            case 2:
                point = 15;
                break;
            case 3:
                point = 40;
                break;
            case 4:
                point = 70;
                break;
            case 5:
                point = 100;
                break;
            case 6:
                point = 250;
                break;
            case 7:
                point = 500;
                break;
            default:
                point = 10;
                break;
        }

        return point;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            //GameManager.Instance.onHitObstacle?.Invoke();
            //can azalt
            GameManager.Instance.onHitObstacle?.Invoke();
            Destroy(other.gameObject);

        }
        else if (other.gameObject.tag.Equals("Feed"))
        {
            puan += 10;
            Destroy(other.gameObject);
            GameManager.Instance.onFeedTake?.Invoke();
            spoon.TakeFeed();
            spoon.ShowPraiseText("++"+ 10);
            //mama toplama

        }
        else if (other.gameObject.tag.Equals("FinishLine"))
        {
            if (GameManager.Instance.currentState != GameManager.GameState.Finish)
            {
                GameManager.Instance.currentState = GameManager.GameState.Finish;
            }
        }
    }

    public void PuanEkle()
    {
        puan += currentpuan;
        spoon.uManager.UpdateMamaText();
        spoon.ShowPraiseText("++" + currentpuan);
    }
}
