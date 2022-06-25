using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerController player;
    private UIManager uýManager;
    

    void Start()
    {
        LevelManager.onNewLevelLoaded += DestroyThisObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uýManager = GameObject.FindObjectOfType<UIManager>();
        if(GameManager.Instance.currentState != GameManager.GameState.Finish)
        Destroy(gameObject,5f);
    }

    private void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }

    void OnDisable()
    {
        LevelManager.onNewLevelLoaded -= DestroyThisObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            uýManager.ShowPraiseText("");
            player.puan += 25;
            uýManager.UpdateMamaText();
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Baby")
        {
            
            if (GameManager.Instance.currentState == GameManager.GameState.Finish && player.isWinnable)
            {
                player.PuanEkle();
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                GameManager.onWinEvent?.Invoke();
                
            }
        }
        else if (other.gameObject.tag == "Plane")
        {
            Destroy(this.gameObject);
        }

    }
}
