using UnityEngine;
using System.Collections;

public class bulletSC : MonoBehaviour
{
    private float deleteDelay = 2f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deleteDelay -= Time.deltaTime;
        if (deleteDelay <= 0)
        {
            Destroy(this.gameObject); 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Ray ray = Camera.main.ScreenPointToRay(this.transform.position);
        //RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);  
        //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction); 
        //print(collision.transform.root.transform.tag); 
        //print("kk : " + Physics.Raycast(transform.position, transform.forward, 1f, 1));
        if (collision.transform.root.transform.tag == "EnemyHuman")
        {
            Enemy enemySC = collision.transform.root.GetComponent<Enemy>();
            enemySC.EnemyHit(2);
        }
        else if (collision.transform.root.transform.tag == "UserRobot") 
        {
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.USER_ROBOT_BODY_HIT);
            GameObject hitEffectobj = Instantiate(Resources.Load("common/hitEffect")) as GameObject;
            hitEffectobj.transform.position = this.transform.position;
            
            RobotData robotData = GameManager.getInstance.robotData; 
            if (collision.transform.tag == "UserRobot-Thigh") 
            {
                robotData.currentLowerHP -= 5; 
            }
            else if (collision.transform.tag == "UserRobot-Head")
            {
                robotData.currentHeadHP -= 5;
            } else {
                robotData.currentBodyHP -= 5;
            }
            GameManager.getInstance.battleSceneManager.UserRobotUI();
        }
        else if (collision.transform.root.transform.tag == "EnemyObject")
        {
            Enemy enemySC = collision.transform.root.GetComponent<Enemy>();
            enemySC.EnemyHit(2);

            GameObject hitEffectobj = Instantiate(Resources.Load("common/hitEffect")) as GameObject;
            hitEffectobj.transform.position = this.transform.position;
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.OTHER_FLOOR_HIT);
        }
        else
        {
            GameObject hitEffectobj = Instantiate(Resources.Load("common/hitEffect")) as GameObject;
            hitEffectobj.transform.position = this.transform.position;
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.OTHER_FLOOR_HIT);

        }

        Destroy(this.gameObject);
    }
}
