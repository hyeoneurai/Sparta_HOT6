using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip;
    public AudioSource audioSource;
    public string characterName; // 캐릭터 이름을 저장하기 위한 새로운 변수

    // Start is called before the first frame update
    void Start()
    {
        
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

        

        if (gameManager.i.firstCard == null)
        {
            gameManager.i.firstCard = gameObject;
        }
        else if(gameManager.i.firstCard != null)
        {
            
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
