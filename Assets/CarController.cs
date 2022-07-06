using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    private Rigidbody2D rb;
    bool move = false;
    public float speed = 20f;
    public GameObject lineSegment;
    public GameObject lineSegmentref;
    public GameObject ground;
    public GameObject playButton;
    bool isGameStarted = false;
    int i = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && isGameStarted)
        {
            move = true; 
            
        }
        if (Input.GetButtonUp("Fire1"))
        {
            move = false;
        }
    }
  
    private void FixedUpdate()
    {
        if (move ==true)
        {
            rb.AddForce(transform.right * speed * Time.fixedDeltaTime * 100f, ForceMode2D.Force);
        }
        

    }
    IEnumerator SpwanLines()
    {
        
        while (i!=0)
        {
            float center = lineSegment.GetComponent<EdgeCollider2D>().bounds.size.x;
            float xPos = lineSegment.transform.position.x;
            float offSet = (center - xPos) * 2;
            float newXpos = center + offSet;

            float waitTime = Random.Range(0.7f, 2f);
            yield return new WaitForSeconds(waitTime);
            Vector3 pos = new Vector3((newXpos * i) - (newXpos * i / 2), lineSegment.transform.position.y, lineSegment.transform.position.z);
            Instantiate(lineSegmentref, pos, Quaternion.identity);


            float groundcenter = ground.transform.localScale.x;
            float groundxPos = ground.transform.position.x;
            float groundnewXpos = groundcenter + groundxPos;

            float groundwaitTime = Random.Range(0.7f, 2f);
            yield return new WaitForSeconds(groundwaitTime);
            Vector3 groundpos = new Vector3((groundnewXpos * i) - (groundnewXpos * i / 2), ground.transform.position.y, ground.transform.position.z);
            Instantiate(ground, groundpos, Quaternion.identity);

            i++;
        }
        
    }
    public void OnGroundHit()
    {
        SceneManager.LoadScene("SampleScene");
        move = false;
        isGameStarted = false;
        i = 0;
        StopCoroutine("SpwanLines");
        playButton.SetActive(true);
        rb.gravityScale = 0;
    }
    public void OnRestart()
    {
        playButton.SetActive(false);
        isGameStarted = true;
        i = 1;        
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        StartCoroutine("SpwanLines");

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "base")
        {
            OnGroundHit();
        }
    }

}
