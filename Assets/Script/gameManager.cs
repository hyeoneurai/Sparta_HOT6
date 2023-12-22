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
    public Text timeTxt;
    public GameObject endPanel;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public Text countTxt;
    public Text timeScoreTxt;
    public Text totalScoreTxt;

    float time = 30.0f;
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

        rtans = rtans.OrderBy(item => Random.Range(-1f, 1f)).ToArray();

        for (int i = 0; i< 16; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            
            string rtanName = rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
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
    }

    public void isMatched()
    {
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

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
        }
        else
        {
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

        timeScore = time * 10;
        timeScoreTxt.text = timeScore.ToString("N2");
        totalScore = timeScore - (count * 2);
        totalScoreTxt.text = totalScore.ToString("N2");
    }
}
