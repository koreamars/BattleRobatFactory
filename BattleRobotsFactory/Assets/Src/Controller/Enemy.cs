using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject HPBar;
    public float totalHP = 0;
    public float currentHP = 0;
    public int Idx;

    protected BattleSceneManager _battleSceneManager;

    public virtual void Start()
    {
        _battleSceneManager = GameManager.getInstance.battleSceneManager;
    }

    public virtual void Update()
    {
        EnemyMove(_battleSceneManager.currentMoveSpeed);

        // HP bar scale
        Vector3 hpScaleVector = new Vector3((currentHP / totalHP) * 5f, 5f, 5f);
        HPBar.transform.localScale = hpScaleVector;
    }

    public virtual void EnemyHit(uint Damage)
    {
        if (currentHP <= 0)
        {
            return;
        }

        currentHP -= Damage;

        if (currentHP <= 0)
        {
            Vector3 hpScaleVector = new Vector3(0f, 5f, 5f);
            HPBar.transform.localScale = hpScaleVector;

            currentHP = 0;
            DieAni();
        }
        else
        {
            //GameManager.getInstance.battleSceneManager.HumanHit.Play(0);
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.OTHER_FLOOR_HIT);
        }

    }

    public virtual void CrashHit(uint Damage)
    {
        EnemyHit(Damage);
        
    }

    public virtual void DieAni()
    {
        GameManager.getInstance.battleSceneManager.EnemyDestroy(Idx);
        Destroy(this.gameObject);
    }

    protected virtual void EnemyMove(float currentSpeed)
    {
        Vector3 oriVector = this.transform.position;
        oriVector.x -= currentSpeed;
        this.transform.position = oriVector;
    }
}
