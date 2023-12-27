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
    bool record = false; //시간기록 작동 여부

    // Start is called before the first frame update
    void Start()
    {
        recordTime = 0; // Start 메서드에서 recordTime 초기화
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void openCard()
    {
        audioSource.PlayOneShot(flip);

        anim.SetBool("isOpen", true);
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if (record == true)
        {
            recordTime += Time.deltaTime; //시간기록 시작
            // 수정: recordTime이 5초 이상인지 확인하는 조건으로 변경
            if (recordTime >= 5) //5초가 경과하면
            {
                record = false; //카운트 완료후 다시 초기화
                recordTime = 0; //카운트 완료후 다시 초기화2
                Invoke("closeCardInvoke", 0f);
                gameManager.i.firstCard = null; //첫번째 카드 초기화
            }
        }

        if (gameManager.i.firstCard == null)
        {
            gameManager.i.firstCard = gameObject;
            record = true; // 첫 번째 카드를 열 때 시간 기록 시작
        }
        else
        {
            record = false; // 두 번째 카드를 열 때 시간 기록 중지
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
