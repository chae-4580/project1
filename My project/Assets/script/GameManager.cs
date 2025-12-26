using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D;
using UnityEditor.Animations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Camera;

    [Header("# UI")]
    public GameObject InDungeon;
    public GameObject Store;
    public GameObject Result;
    public GameObject Tutorial;
    public GameObject[] text = new GameObject[5];

    [Header("# System Info")]
    public int Gold = 0;
    public int Score = 0;
    public Text ScoreText;
    public Text GoldText;
    public Text KeyText;
    public GameObject StageText;
    public bool isClear = false;

    [Header("# Object")]
    public GameObject Slot;
    public Image HpGauge;
    public Image O2Gauge;
    public Toggle Pause;
    public Transform player;

    [Header("# Timer Info")]
    public int curTime = 45;
    public float O2tic = 1;
    float time = 0;

    [Header("# map Info")]
    public GameObject[] maps;
    public int mapId;
    public int Stage = 1;
    public int stageLimt = 5;
    public bool isMapStart = false;
    public GameObject FoundTresure = null;
    public Image wace;
    public GameObject waveCenter;

    [Header("# Player Info")]
    public int Hp = 8;
    public float HitTime = 0.1f;
    public float AttackDelay = 0f;
    public float Delay = 0f;
    public int JumpLimt = 0;
    public int Damege = 1;
    public GameObject Dark;

    [Header("# Bag Info")]
    public Image[] itemsimage;
    public Image[] slotitems;
    public Text WeightText;
    public int SlotLimt = 4;
    public int SlotAmount = 0;
    public int Weight = 0;
    public int MaxWeight = 150;
    public int[] SlotId = { -1 };
    public int KeyAmount = 0;
    bool isFull = false;
    public bool isHard = false;

    [Header("# Store Info")]
    public GameObject[] SText;
    public int[] SPrice;
    public int O2level = 1;
    public int BagLavel = 1;
    public int LightLevel = 1;
    public int KnifeLevel = 1;
    public bool isFree = false;
    bool stat = true;

    [Header("# Something")]
    public Color Hurt = new Color(1, 126 / 255f, 126 / 255f);
    int Fancount = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = player.position + new Vector3(0, 0, -1);

        GoldText.text = Gold.ToString() + "G";
        KeyText.text = KeyAmount.ToString();
        ScoreText.text = Score.ToString();

        HpGauge.fillAmount = 0.125f * Hp;
        O2Gauge.fillAmount = 0.0222222f * curTime;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            curTime = 45;
            Hp = 8;
        }

        if (Hp > 8) Hp = 8;
        if (curTime > 45) curTime = 45;

        time += Time.deltaTime;
        if (time >= O2tic && 0 < curTime)
        {
            curTime--;
            time = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Slotactive(1, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Slotactive(2, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Slotactive(3, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Slotactive(4, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Slotactive(5, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Slotactive(6, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Slotactive(7, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Slotactive(8, false);
        }
    }


    public void Slotactive(int i, bool immediately)
    {
        int s_id;

        if (immediately) s_id = i;

        else
        {
            s_id = SlotId[i - 1];
            Weight -= 10;
        }

        if (s_id != -1 || s_id <= 5)
        {
            StartCoroutine(Player.Instance.Speak(s_id + 6));

        }
        switch (s_id)
        {
            case -1:
                Weight += 10;
                StartCoroutine(Player.Instance.Speak(2));
                break;
            case 0:
                Hp += 2;
                break;
            case 1:
                curTime += 9;
                break;
            case 2:
                if(FoundTresure == null) StartCoroutine(Player.Instance.Speak(4));
                else
                {
                    waveCenter.transform.rotation = Quaternion.FromToRotation(Vector3.up, FoundTresure.transform.position - player.position);
                    waveCenter.gameObject.SetActive(true);
                }

                Debug.Log("pathfinder");
                break;
            case 3:
                if(Player.Instance.Speed <= 8)
                {
                    Player.Instance.Speed = 8;
                    Fancount++;
                }
                break;
            case 4:
                Player.Instance.Speed = 10;
                Fancount++;
                break;
            case 5:
                HitTime = 10;
                player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                break;
            case 6:
                Dark.GetComponent<RectTransform>().localScale *= 1.2f;
                break;
            case 7:
                Player.Instance.GetComponent<Rigidbody2D>().gravityScale *= 0.6f;
                break;
            default:
                Weight += 10;
                StartCoroutine(Player.Instance.Speak(3));
                break;
        }

        if (immediately) return;

        if(0 <= s_id && s_id <= 7)
        {
            s_id = -1;

            for(int j = i; j < SlotAmount && SlotId[j] != -1; j++)
            {
                SlotId[j - 1] = SlotId[j];
                SlotId[j] = -1;
            }
            SlotAmount--;

            SlotSetting();
        }
    }
        
    public void SlotSetting()
    {
        for (int i = 0; i < 8; i++)
        {
            Transform Slotitem = Slot.transform.GetChild(i);
            foreach (Transform child in Slotitem)
            {
                if (child.gameObject.name.Equals("num")) continue;
                Destroy(child.gameObject);
            }
            if (i >= SlotAmount || SlotId[i] == -1) continue;

            slotitems[i] = Instantiate(itemsimage[SlotId[i]], Slotitem.position, Quaternion.identity, Slotitem);
        }
        WeightText.GetComponent<Text>().text = $"{Weight}/{MaxWeight}";

        if (!isFull && SlotAmount > SlotLimt)
        {
            isFull = true;
            Player.Instance.GetComponent<Rigidbody2D>().gravityScale /= 0.6f;
        }
        else if (isFull && SlotAmount <= SlotLimt)
        {
            isFull = false;
            Player.Instance.GetComponent<Rigidbody2D>().gravityScale *= 0.6f;
        }

        if (!isHard && Weight > MaxWeight)
        {
            isHard = true;
            WeightText.GetComponent<Text>().color = Color.red;
        }
        else if (isHard && Weight <= MaxWeight)
        {
            isHard = false;
            WeightText.GetComponent<Text>().color = Color.white;
        }
    }

    IEnumerator ItemabilityEnd(int i)
    {
        int nowfan = Fancount;
        yield return new WaitForSeconds(10f);
        switch (i)
        {
            case 3:
            case 4:
                if (nowfan == Fancount) Player.Instance.Speed = 5;
                break;
            case 5:
                player.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 6:
                Dark.GetComponent<RectTransform>().localScale /= 1.2f;
                break;
            case 7:
                Player.Instance.GetComponent<Rigidbody2D>().gravityScale /= 0.6f;
                break;
        }

    }
    public void StageLoad(int i)
    {
        curTime = 45;
        Hp = 8;
        Weight = 0;
        FoundTresure = null;

        if(i == 0)
        {
            mapId = Stage;

            isMapStart = true;
        }
        else if(i != 0)
        {
            mapId = 0;
            if(isClear)
            {
              maps[Stage].SetActive(false);
              Stage++;
              Score += Gold;
            }
        }

        StageText.transform.GetChild(0).GetComponent<Text>().text = Stage.ToString();

        Debug.Log("" + mapId);

        player.position = new Vector3(1, -0.5f, 0);

        switch (mapId)
        {
            case 1:
            case 2:
                O2tic = 1 * O2level;
                break;
            case 3:
            case 4:
                O2tic = 1f / 2 * O2level;
                break;
            case 5:
                O2tic = 1f / 4 * O2level;
                break;
        }

        if(mapId != 0)
        {
            maps[0].SetActive(false);
            maps[Stage - 1].SetActive(false);
            maps[Stage].SetActive(true);
        }
        else
        {
            maps[Stage].SetActive(false);
            maps[0].SetActive(true);
        }

        if(mapId == 0) InDungeon.SetActive(false);
        else InDungeon.SetActive(true);

        isClear = false;
    }
}
