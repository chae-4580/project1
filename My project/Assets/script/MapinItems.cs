using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapinItems : MonoBehaviour
{
    public GameObject items;
    public GameObject monsters;
    public GameObject[] Jem;

    public bool[] isGotten;
    public bool[] get;

    public bool isClear = false;
    bool reseting = false;

    int findertarget;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isMapStart) Startroutine();

        if (!reseting && (GameManager.Instance.Hp <= 0 || GameManager.Instance.curTime <= 0)) Loss();

        if (GameManager.Instance.Hp <= 0 || GameManager.Instance.curTime <= 0) return;

        if (findertarget != -1 && (GameManager.Instance.FoundTresure == null || !Jem[findertarget].activeSelf))
        {
            GameManager.Instance.waveCenter.gameObject.SetActive(false);
            FoundTresure();
        }

        if(isClear && GameManager.Instance.isClear)
        {
            Debug.Log("isClear");
            GameManager.Instance.isClear = true;
            StartCoroutine(Player.Instance.Speak(14));
            return;
        }

        isClear = true;
        for(int i = 0; i <Jem.Length; i++)
        {
            if (!Jem[i].activeSelf && !isGotten[i])
            {
                get[i] = true;
                isGotten[i] = true;
            }

            if (!isGotten[i]) isClear = false;
        }
        
    }

    void Startroutine()
    {
        GameManager.Instance.isMapStart = false;
        reseting = false;
        FoundTresure();

        for(int i = 0; i < Jem.Length; i++)
        {
            get[i] = false;
        }

        foreach (Transform child in items.transform)
        {
            if (child.gameObject.name.Equals("Key") || child.gameObject.CompareTag("Coin")) continue;

            child.gameObject.SetActive(true);
        }
        for (int i = 0; i < Jem.Length; i++)
        {
            if (!isGotten[i])
            {
                continue;
            }

            Jem[i].SetActive(false);
        }

        if (monsters == null) return;

        foreach(Transform child in monsters.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    void Loss()
    {
        Debug.Log("loss");
        reseting = true;
        isClear = false;
        GameManager.Instance.isClear = false;
        for(int i = 0; i < Jem.Length; i++)
        {
            if (get[i])
            {
                get[i] = false;
                isGotten[i] = false;
            }
        }
    }

    void FoundTresure()
    {
        GameManager.Instance.FoundTresure = null;

        for (int i = 0; i< Jem.Length; i++)
        {
            if (!isGotten[i])
            {
                Debug.Log("target : " + Jem[i].gameObject.name);
                findertarget = i;
                GameManager.Instance.FoundTresure = Jem[i].gameObject;
                return;
            }
        }

        if(GameManager.Instance.FoundTresure == null)
        {
            findertarget = -1;
            Debug.Log("None");
        }
    }
}
