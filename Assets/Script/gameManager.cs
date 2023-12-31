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
    public GameObject mtime; // 켰다 껐다 할 오브젝트 설정
    public Animator mtimeAni; // 애니메이션 트리거를 위한 설정
    public Text selectedCharacterTxt; // 추가: 일치하는 캐릭터 이름을 표시할 UI 텍스트

    public AudioClip start; //시작시 소리
    public AudioClip miss; //불일치시 소리
    public AudioClip win; //게임완료시 소리
    bool timeSound = false;

    public float time = 30.0f; 
    float timeScore = 0f;
    float totalScore = 0f;
    float recordTime = 0f;
    int count = 0;

    void Awake()
    {
        i = this;
        audioSource.PlayOneShot(start); //시작할때마다 소리지원

    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        
        int[] rtans = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7,};
        // 각 카드의 팀원이름 추가
        string[] characterNames = { "이진우", "이진우", "김준서", "김준서", "채호선", "채호선", "김현래", "박재현" };

        // 랜덤정렬
        rtans = rtans.OrderBy(item => Random.Range(-1f, 1f)).ToArray();

        for (int i = 0; i< 16; i++) // for문으로 카드배치 
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;
            
            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            
            string rtanName = rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);

            // 캐릭터 이름을 카드와 연결하기 위한 추가 코드
            newCard.GetComponent<card>().characterName = characterNames[rtans[i]];
        }
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime; ;
        time = Mathf.Max(0.0f, time); // 시간이 음수일 경우 0으로 설정
        timeTxt.text = time.ToString("N2");

        if (firstCard != null && secondCard == null)
        {
            recordTime += Time.deltaTime;
        }
        if (recordTime >= 4.5)
        {
            firstCard.GetComponent<card>().closeCard();
            firstCard.transform.Find("back").GetComponent<SpriteRenderer>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f, 255 / 255f);
            firstCard = null;
            recordTime = 0;
        }

        if (time <= 0.0f)
        {
            gameOver();
        }
        if (time <= 10.0f)
        {
            timeTxt.text = "<color=#FF2D00>" + time.ToString("N2") + "</color>"; //타이머색 변경
            if (timeSound == false) //중첩재생 방지
            {
                audioSource.clip = timeLess;
                audioSource.Play(); //재생
                audioSource.loop = true; //소리 반복재생
                timeSound = true; //중첩방지
            }
        }
    }

    public void isMatched()
    {
        recordTime = 0;
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        mtime.SetActive(false); // mtime 꺼짐 (1번만 실행)

        if (firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            int cardsLeft = GameObject.Find("cards").transform.childCount;
            if (cardsLeft == 2)
            {
                gameOver();
                audioSource.PlayOneShot(win); //게임완료시 소리지원

            }

            // 일치하는 캐릭터 이름을 UI에 표시
            string characterName = firstCard.GetComponent<card>().characterName;

            // Debug.Log
            Debug.Log("First Card Image: " + firstCardImage);
            Debug.Log("Character Name: " + characterName);
            Debug.Log("Selected Characters Text: " + selectedCharacterTxt.text);

            selectedCharacterTxt.text = characterName;

            Invoke("ClearFailureMessage", 1.0f);

            count -= 1;
        }
        else
        {


            mtime.SetActive(true); // mtime(-1) 켜짐
            mtimeAni.SetTrigger("endMtime"); // 조건당 1번만 실행을 위한 트리거
            // 불일치하는 경우 실패 메시지 표시
            Debug.Log("Not Matched! Try Again.");
            selectedCharacterTxt.text = "실패!";
            audioSource.PlayOneShot(miss); //카드불일치시 소리지원


            Invoke("ClearFailureMessage", 1.0f);// 2초 후에 ClearFailureMessage 메서드 호출
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
        audioSource.Stop(); // 게임 종료시 음악 정지

        timeScore = time * 2;
        timeScoreTxt.text = timeScore.ToString("N2");
        totalScore = timeScore - count;
        totalScore = Mathf.Max(0.0f, totalScore);
        totalScoreTxt.text = totalScore.ToString("N2");
    }

    // 실패 메시지 지우는 메서드
    void ClearFailureMessage()
    {
        selectedCharacterTxt.text = ""; // 빈 문자열로 설정하여 텍스트를 지움
    }
}
