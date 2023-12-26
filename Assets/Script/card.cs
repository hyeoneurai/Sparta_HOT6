using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip;
    public AudioSource audioSource;
    public string characterName; // 캐릭터 이름을 저장하기 위한 새로운 변수
    float recordTime; //시간기록변수
    bool record = false; //시간기록 작동

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (record == true) //
        {
            recordTime += Time.deltaTime; //시간기록 시작
            if (recordTime > 5) //5초가 경과하면
            {
                record = false; //카운트 완료후 다시 초기화
                recordTime = 0; //카운트 완료후 다시 초기화2
                Invoke("closeCardInvoke", 0f);
                gameManager.i.firstCard = null; //첫번째 카드 초기화

            }
        }
    }
    public void openCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("isOpen", true);
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (gameManager.i.firstCard == null)
        {
            gameManager.i.firstCard = gameObject;
            record = true; // 기록시작
        }
        else
        {
            record = false; //두번째 카드 열어서 시간 기록취소
            gameManager.i.secondCard = gameObject;
            gameManager.i.isMatched();

        }
    }

    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.5f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 0.5f);
    }

    void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
}
