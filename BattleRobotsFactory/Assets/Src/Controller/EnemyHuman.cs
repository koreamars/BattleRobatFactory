using UnityEngine;
using System.Collections;
using Spine;

public class EnemyHuman : Enemy {

    public GameObject BulletGuideBar;

    private string enemyMoveType = EnemyMoveType.LEFT_MOVE;

    private float nextMotionTime = 0;
    private float _speed = 0.05f;
    private float forBackValue = 0;

    private SkeletonAnimation enemyAnimation;
    
    public override void Start()
    {

        base.Start();

        nextMotionTime = GameManager.getInstance.GetRandomValue(1.8f, 3.5f);
        //print("nextMotionTime : " + nextMotionTime);
        enemyAnimation = this.GetComponent<SkeletonAnimation>();
        enemyAnimation.state.SetAnimation(0, "run", true);

        enemyAnimation.state.Event += EnemyAniEvent;

        forBackValue = -1.8f + GameManager.getInstance.GetRandomValue(0f, 7f);

        //_battleSceneManager = GameManager.getInstance.battleSceneManager;

        //enemyAnimation.skeleton.SetColor(Color.black);
        foreach (Spine.Slot slot in enemyAnimation.skeleton.slots)
        {
            if (slot.data.name != "Type01/GunFire")
            {
                slot.SetColor(Color.black);
            }
        }
    }

	public override void Update () {

        if(currentHP <= 0) {
            EnemyMove(_battleSceneManager.currentMoveSpeed);
            return;
        }
        if (enemyMoveType == EnemyMoveType.LEFT_MOVE) EnemyMove(_speed + _battleSceneManager.currentMoveSpeed);
        if (enemyMoveType == EnemyMoveType.RIGHT_MOVE) EnemyMove((_speed - _battleSceneManager.currentMoveSpeed) * -1f);
        if (enemyMoveType == EnemyMoveType.STAND) EnemyMove(_battleSceneManager.currentMoveSpeed);
        if (enemyMoveType == EnemyMoveType.GUN_FIRE) EnemyMove(_battleSceneManager.currentMoveSpeed);

        nextMotionTime -= Time.deltaTime;
        if(nextMotionTime <= 0) {
            nextMotionTime = GameManager.getInstance.GetRandomValue(1.0f, 1.5f);

            if (enemyMoveType == EnemyMoveType.LEFT_MOVE)
            {
                ChangeEnemyMotion(EnemyMoveType.STAND);
            }
            if (enemyMoveType == EnemyMoveType.RIGHT_MOVE)
            {
                ChangeEnemyMotion(EnemyMoveType.STAND);
            }
            if (enemyMoveType == EnemyMoveType.GUN_FIRE)
            {
                nextMotionTime = GameManager.getInstance.GetRandomValue(0.5f, 1f);
                ChangeEnemyMotion(EnemyMoveType.GUN_FIRE);
            }
            if (enemyMoveType == EnemyMoveType.STAND)
            {
               /* if (nextMotionValue > 0.7f)
                {
                    nextMotionTime = 0.5f;
                    ChangeEnemyMotion(EnemyMoveType.GUN_FIRE);
                }*/
                nextMotionTime = 0.5f;
                ChangeEnemyMotion(EnemyMoveType.GUN_FIRE);
            }
        }

        // 거리 리미트 제한.
        if (this.transform.position.x < forBackValue && enemyMoveType != EnemyMoveType.RIGHT_MOVE)
        {
            nextMotionTime = 1.5f;
            ChangeEnemyMotion(EnemyMoveType.RIGHT_MOVE);
        }

        // HP bar scale
        Vector3 hpScaleVector = new Vector3((currentHP / totalHP) * 5f, 5f, 5f);
        HPBar.transform.localScale = hpScaleVector;
	}
    
    override public void EnemyHit(uint Damage)
    {
        if (currentHP <= 0)
        {
            return;
        }

        currentHP -= Damage;
        
        if(currentHP <= 0) {
            Vector3 hpScaleVector = new Vector3(0f, 5f, 5f);
            HPBar.transform.localScale = hpScaleVector;

            DieAni();
            currentHP = 0;
        }
        else
        {
            //GameManager.getInstance.battleSceneManager.HumanHit.Play(0);
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.ENEMY_HUMAN_HIT);
        }

        StartCoroutine("hitAni", 0.05f); 
    }

    override public void CrashHit(uint Damage)
    {
        EnemyHit(Damage);
        Vector3 hitpos = new Vector3(this.transform.position.x + 3, this.transform.position.y, this.transform.position.z); 
        this.transform.position = hitpos; 
    }

    override public void DieAni()
    {

        Vector3 localScale = this.transform.localScale;
        if (localScale.x < 0) localScale.x = localScale.x * -1;
        this.transform.localScale = localScale;

        enemyAnimation.state.ClearTrack(0);
        enemyAnimation.state.Update(0);

        float aniRandomValue = GameManager.getInstance.GetRandomValue(0f, 1f);
        string aniType;
        if(aniRandomValue > 0.5f) {
            aniType = "die-0";
        } else {
            aniType = "die-1";
        }
        SoundManager.getInstance.EffectSoundPlay(EffectSoundType.ENEMY_HUMAN_DIE); 
        enemyAnimation.state.SetAnimation(0, aniType, false);  
    }

    private void EnemyAniEvent(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if(e.Data.Name == "dieEnd")
        {
            GameManager.getInstance.battleSceneManager.EnemyDestroy(Idx);
            Destroy(this.gameObject);
            
        }
        if (e.Data.Name == "fire")
        {
            //GameObject userRobot = GameObject.Find("UserRobotObject");

            float angle = 175 + GameManager.getInstance.GetRandomValue(-5, 5);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            BulletGuideBar.transform.rotation = rotation;
            
            GameObject bulletPrefab = (GameObject)Instantiate(Resources.Load("common/EnemyBulletPrefab"), BulletGuideBar.transform.position, BulletGuideBar.transform.rotation);
            Rigidbody2D bulletRigidbody = bulletPrefab.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = BulletGuideBar.transform.TransformDirection(new Vector3(GameData.bulletSpeed, 0, 0));

            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.ENEMY_SHOT_AK47);
        }
    }

    IEnumerator hitAni(float time)
    {
        foreach (Spine.Slot slot in enemyAnimation.skeleton.slots)
        {
            if (slot.data.name != "Type01/GunFire")
            {
                slot.SetColor(Color.red);
            }
        }

        yield return new WaitForSeconds(time);

        foreach (Spine.Slot slot in enemyAnimation.skeleton.slots)
        {
            if (slot.data.name != "Type01/GunFire")
            {
                slot.SetColor(Color.black);
            }
        }
    }

    private void ChangeEnemyMotion(string type)
    {
        Vector3 localScale = this.transform.localScale;
        enemyMoveType = type;
        if (type == EnemyMoveType.RIGHT_MOVE)
        {
           if (localScale.x > 0) localScale.x = localScale.x * -1;
            this.transform.localScale = localScale;
            enemyAnimation.state.SetAnimation(0, "run", true);
        }
        if (type == EnemyMoveType.LEFT_MOVE)
        {
            if (localScale.x < 0) localScale.x = localScale.x * -1;
            this.transform.localScale = localScale;
            enemyAnimation.state.SetAnimation(0, "run", true);
        }
        if (type == EnemyMoveType.STAND)
        {
            if (localScale.x < 0) localScale.x = localScale.x * -1;
            this.transform.localScale = localScale;
            enemyAnimation.state.SetAnimation(0, "stand", true); 
        }
        if (type == EnemyMoveType.GUN_FIRE)
        {
            if (localScale.x < 0) localScale.x = localScale.x * -1;
            this.transform.localScale = localScale;
            enemyAnimation.state.ClearTrack(0);
            enemyAnimation.state.Update(0);
            enemyAnimation.state.SetAnimation(0, "fire", false);
        }
        
    }

}
