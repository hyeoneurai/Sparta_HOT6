using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip;
    public AudioSource audioSource;
    public string characterName; // ĳ���� �̸��� �����ϱ� ���� ���ο� ����
    float recordTime; //�ð���Ϻ���
    bool record = false; //�ð���� �۵�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (record == true) //
        {
            recordTime += Time.deltaTime; //�ð���� ����
            if (recordTime > 5) //5�ʰ� ����ϸ�
            {
                record = false; //ī��Ʈ �Ϸ��� �ٽ� �ʱ�ȭ
                recordTime = 0; //ī��Ʈ �Ϸ��� �ٽ� �ʱ�ȭ2
                Invoke("closeCardInvoke", 0f);
                gameManager.i.firstCard = null; //ù��° ī�� �ʱ�ȭ

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
            record = true; // ��Ͻ���
        }
        else
        {
            record = false; //�ι�° ī�� ��� �ð� ������
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
