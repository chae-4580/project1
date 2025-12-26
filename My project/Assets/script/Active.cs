using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    public int Id;
    public int weight;

    public bool isPlayerEnter;
    public bool isSolved;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        isPlayerEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerEnter && Input.GetButtonDown("Trigger"))
        {
            if(gameObject.CompareTag("Items"))
            {
                switch (Id)
                {
                    case -2:
                        gameObject.SetActive(false);
                        GameManager.Instance.KeyAmount++;
                        break;
                    default:

                        if(GameManager.Instance.SlotAmount < 8)
                        {
                            if (GameManager.Instance.Weight + weight > GameManager.Instance.MaxWeight)
                            {
                                Debug.Log("가방이 무겁습니다.");
                                StartCoroutine(Player.Instance.Speak(1));
                            }
                            if(GameManager.Instance.SlotAmount >= GameManager.Instance.SlotLimt)
                            {
                                Debug.Log("가방이 포화상태입니다.");
                                StartCoroutine(Player.Instance.Speak(0));
                            }
                            GameManager.Instance.SlotId[GameManager.Instance.SlotAmount] = Id;
                            GameManager.Instance.SlotAmount++;
                            GameManager.Instance.Weight += weight;
                            gameObject.SetActive(false);
                            GameManager.Instance.SlotSetting();
                        }
                        
                        else {
                            Debug.Log("가방이 가득 찼습니다.");
                            StartCoroutine(Player.Instance.Speak(2));
                        }
                        break;
                }

                gameObject.SetActive(false);
            }
            else if(gameObject.name.Equals("door"))
            {
                if(GameManager.Instance.KeyAmount > 0)
                {
                    GameManager.Instance.KeyAmount--;
                    gameObject.SetActive(false);
                }
                
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            isPlayerEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            isPlayerEnter = false;
        }
    }
}
