using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager i;
    public AudioSource audioSource;
    public AudioClip match;
    public AudioClip timeLess;
    public Text timeTxt;
    public GameObject endPanel;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public Text countTxt;
    public Text timeScoreTxt;
    public Text totalScoreTxt;
    public GameObject mtime;
    public Animator mtimeAni;
    public Text selectedCharacterTxt; // �߰�: ��ġ�ϴ� ĳ���� �̸��� ǥ���� UI �ؽ�Ʈ

    bool timeSound = false;
    
    public float time = 30.0f; 
    float timeScore = 0f;
    float totalScore = 0f;
    int count = 0;

    void Awake()
    {
        i = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        
        int[] rtans = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        // �� ī���� �����̸� �߰�
        string[] characterNames = { "������", "������", "���ؼ�", "���ؼ�", "äȣ��", "äȣ��", "������", "������" };

        // ��������
        rtans = rtans.OrderBy(item => Random.Range(-1f, 1f)).ToArray();

        for (int i = 0; i< 16; i++) // for������ ī���ġ 
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            
            string rtanName = rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);

            // ĳ���� �̸��� ī��� �����ϱ� ���� �߰� �ڵ�
            newCard.GetComponent<card>().characterName = characterNames[rtans[i]];
        }
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime; ;
        timeTxt.text = time.ToString("N2");

        if (time <= 0.0f)
        {
            gameOver();
        }
        if (time <= 10.0f)
        {
            timeTxt.text = "<color=#FF2D00>" + time.ToString("N2") + "</color>"; //Ÿ�̸ӻ� ����
            if (timeSound == false) //��ø��� ����
            {
                audioSource.clip = timeLess;
                audioSource.Play(); //���
                audioSource.loop = true; //�Ҹ� �ݺ����
                timeSound = true; //��ø����
            }
        }
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        mtime.SetActive(false);

        if (firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            if (cardsLeft == 2)
            {
                gameOver();
            }

            // ��ġ�ϴ� ĳ���� �̸��� UI�� ǥ��
            string characterName = firstCard.GetComponent<card>().characterName;

            // Debug.Log
            Debug.Log("First Card Image: " + firstCardImage);
            Debug.Log("Character Name: " + characterName);
            Debug.Log("Selected Characters Text: " + selectedCharacterTxt.text);

            selectedCharacterTxt.text = characterName;

            Invoke("ClearFailureMessage", 1.0f);
        }
        else
        {
            mtime.SetActive(true);
            mtimeAni.SetTrigger("endMtime");
            // ����ġ�ϴ� ��� ���� �޽��� ǥ��
            Debug.Log("Not Matched! Try Again.");
            selectedCharacterTxt.text = "����!";

            Invoke("ClearFailureMessage", 1.0f);// 2�� �Ŀ� ClearFailureMessage �޼��� ȣ��
            time -= 1f;


            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();

            firstCard.transform.Find("back").GetComponent<SpriteRenderer>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f, 255 / 255f);
            secondCard.transform.Find("back").GetComponent<SpriteRenderer>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f, 255 / 255f);
        }

        firstCard = null;
        secondCard = null;

        count += 1;
        countTxt.text = count.ToString();
    }

    public void gameOver()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0f;
        audioSource.Stop(); // ���� ����� ���� ����

        timeScore = time * 10;
        timeScoreTxt.text = timeScore.ToString("N2");
        totalScore = timeScore - count;
        totalScoreTxt.text = totalScore.ToString("N2");
    }

    // ���� �޽��� ����� �޼���
    void ClearFailureMessage()
    {
        selectedCharacterTxt.text = ""; // �� ���ڿ��� �����Ͽ� �ؽ�Ʈ�� ����
    }
}
