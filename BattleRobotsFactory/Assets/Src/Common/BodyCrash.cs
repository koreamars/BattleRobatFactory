using UnityEngine;
using System.Collections;

public class BodyCrash : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyHuman-Body")
        {
            print("BodyCrash !!! -> " + collision.transform.tag);
            Enemy enemy = collision.transform.root.transform.GetComponent<Enemy>();
            enemy.CrashHit(100);

            if(GameManager.getInstance.battleSceneManager != null) {
                GameManager.getInstance.battleSceneManager.EnemyCrash(0);
            }
        }

        if (collision.transform.tag == "EnemyObject")
        {
            print("EnemyObject !!! -> " + collision.transform.tag);
            Enemy enemy = collision.transform.root.transform.GetComponent<Enemy>();
            print("enemy hp : " + enemy.currentHP);
            enemy.CrashHit(10);

            if (GameManager.getInstance.battleSceneManager != null)
            {
                GameManager.getInstance.battleSceneManager.EnemyCrash(10);
            }
        }
    }
}
