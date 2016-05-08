using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleSceneManager : MonoBehaviour {

    public GameObject UserRobotObj;
    public GameObject TargetMarkObject;

    public Text AmmoGunTxt;
    public GameObject BtnAutoFire;
    public GameObject BtnBooster;
    public GameObject BtnDefense;
    public GameObject BtnSubWeapon;
    public float currentMoveSpeed;

    private UserRobot _userRobot;
    private RobotData robotData;

    private bool isUserGunFire = false;
    private bool currentGunFire = false;
    private bool touchDown = false;
    private bool isAutoFire = false;
    private bool isBooster = false;
    private bool isDefense = false;
    private bool isSubWeaponFire = false;

    private GameManager _GameManager;
    private GameObject[] _CurrentEnemyObjs;

    private GameObject _UserCurrentHeadHpBar;
    private GameObject _UserCurrentBodyHpBar;
    private GameObject _UserCurrentLowerHpBar;
    private GameObject _UserMoveLengthTxt;

    private GameObject _mainBg;
    private BackgroundMovie _backgroundMovie;

    private float currentMoveLength = 0;

    private float _currentUpdateTimer = 0;
    private float _updateTime = 1;
    private float _nextEnemyCreateLength = 0;

	void Start () {

        _nextEnemyCreateLength = GameData.enemyCreateLength;

        currentMoveSpeed = GameData.defaultMoveSpeed;

        _mainBg = (GameObject)Instantiate(Resources.Load("Scene/Battle/BackMove/BgMovieType01"));
        _backgroundMovie = _mainBg.GetComponent<BackgroundMovie>() as BackgroundMovie;

        SetMoveSpeed();
        
        _userRobot = UserRobotObj.GetComponent<UserRobot>();
        robotData = GameManager.getInstance.robotData;

        _UserCurrentLowerHpBar = GameObject.Find("UserRobot_HPBar_Thigh");
        _UserCurrentBodyHpBar = GameObject.Find("UserRobot_HPBar_Body");
        _UserCurrentHeadHpBar = GameObject.Find("UserRobot_HPBar_Head");
        _UserMoveLengthTxt = GameObject.Find("Score_Txt");

        _GameManager = GameManager.getInstance;
        _GameManager.battleSceneManager = this.GetComponent<BattleSceneManager>();

        _CurrentEnemyObjs = new GameObject[7]{null, null, null, null, null, null, null};

        DragingScript targetDrag = TargetMarkObject.GetComponent<DragingScript>();
        targetDrag.setTouchDown = SetGunFire;

        GameManager.getInstance.ShowLog("Start");

	}

	// Update is called once per frame
	void Update () {
        _currentUpdateTimer += Time.deltaTime;
        
        _userRobot.NewUpdateGunAngle(TargetMarkObject.transform.position);
        if (isUserGunFire != currentGunFire)
        {
            _userRobot.GunFire(isUserGunFire);
            currentGunFire = isUserGunFire;
        }
        
        // UI updata
        AmmoGunTxt.text = robotData.currentBullet.ToString();
        if (robotData.currentBullet < 100)
        {
            AmmoGunTxt.color = Color.red;
        }

        if(isBooster == true) {
            _updateTime = GameData.defaultUpdateTime / 2;
        } else {
            _updateTime = GameData.defaultUpdateTime;
        }
        if (_currentUpdateTimer >= _updateTime)
        {
            _currentUpdateTimer = 0;
            if(isDefense == false) {
                currentMoveLength += GameData.defaultMoveLength;
                _nextEnemyCreateLength -= GameData.defaultMoveLength;
            }
        }
        Text lengthTxt = _UserMoveLengthTxt.GetComponent<Text>();
        lengthTxt.text = currentMoveLength.ToString();

        if (_nextEnemyCreateLength <= 0)
        {
            _nextEnemyCreateLength = GameData.enemyCreateLength;
            
            foreach (GameObject enemy in _CurrentEnemyObjs)
            {
                if(enemy == null) {
                    EnemyCreate();
                    break;
                }
            }
        }
	}

    public void SetGunFire(bool isFire)
    {
        if (isAutoFire == false) isUserGunFire = isFire;
    }

    public void SetAutoFire()
    {
        isAutoFire = (isAutoFire == true) ? false : true;

        //Sprite onImg = Resources.Load("Scene/Battle/btn_autofire_on") as Sprite;
        if(isAutoFire) {
            isUserGunFire = true; 
            BtnAutoFire.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_autofire_on");
        }
        else
        {
            BtnAutoFire.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_autofire_off");
        }
    }

    public void SetPooster(bool booster)
    {
        isBooster = booster;
        
        //Sprite onImg = Resources.Load("Scene/Battle/btn_autofire_on") as Sprite;
        if (isBooster)
        {
            BtnBooster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_booster_on");
            currentMoveSpeed = GameData.defaultFastMoveSpeed;
            _userRobot.SetAniType(RobotMoveType.SPEEDMOVE);
        }
        else
        {
            BtnBooster.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_booster_off");
            currentMoveSpeed = GameData.defaultMoveSpeed;
            _userRobot.SetAniType(RobotMoveType.MOVE);
        }

        SetMoveSpeed();

        if (isDefense == true)
        {
            isDefense = false; 
            BtnDefense.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_defense_off");
        }
    }

    public void SetDefense()
    {
        isDefense = (isDefense == true) ? false : true;
        
        //Sprite onImg = Resources.Load("Scene/Battle/btn_autofire_on") as Sprite;
        if (isDefense)
        {
            BtnDefense.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_defense_on");
            currentMoveSpeed = 0;
            _userRobot.SetAniType(RobotMoveType.DEFANSE);
        }
        else
        {
            BtnDefense.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_defense_off");
            currentMoveSpeed = GameData.defaultMoveSpeed;
            _userRobot.SetAniType(RobotMoveType.MOVE);
        }

        SetMoveSpeed();
    }

    public void SetSubWeapon(bool isShot)
    {
        isSubWeaponFire = isShot;

        //Sprite onImg = Resources.Load("Scene/Battle/btn_autofire_on") as Sprite;
        if (isSubWeaponFire)
        {
            BtnSubWeapon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_subweapon_on");
        }
        else
        {
            BtnSubWeapon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Scene/Battle/btn_subweapon_off");
        }
    }

    public void EnemyCreate()
    {
        Vector3 EnemyUnitPos = new Vector3(14f, -1.8f, 0);
        GameObject EnemyUnitObj = null;

        if(GameManager.getInstance.GetRandomValue(0, 1f) > 0.8f) {
            EnemyUnitObj = (GameObject)Instantiate(Resources.Load("Obstacle/Obstacle_Type00"), EnemyUnitPos, Quaternion.identity);
        } else {
            EnemyUnitObj = (GameObject)Instantiate(Resources.Load("Enemy/Enemy_Human_Type01"), EnemyUnitPos, Quaternion.identity);
        }

        Enemy enemyUnit = EnemyUnitObj.GetComponent<Enemy>();
        //enemyUnit.Idx = _CurrentEnemyObjs.Count;
        enemyUnit.totalHP = (int)Random.Range(5f, 50f);
        enemyUnit.currentHP = enemyUnit.totalHP; 

        int index = 0;
        foreach (GameObject enemyObj in _CurrentEnemyObjs)
        {
            if(enemyObj == null) {
                _CurrentEnemyObjs[index] = EnemyUnitObj;
                enemyUnit.Idx = index;
                break;
            }
            index += 1;
        }
    }

    public void EnemyDestroy(int idx)
    {
        _CurrentEnemyObjs[idx] = null;
        //StartCoroutine("DelayEnemyCreate", GameManager.getInstance.GetRandomValue(1f, 8f)); 
    }

    public void EnemyCrash(uint userDamage)
    {

        if(userDamage <= 5) {
            StartCoroutine("EnemyCrashDelay", 0.5f);
        } else {
            StartCoroutine("EnemyCrashDelay", 1f);
            StageBack(1f);
        }
    }

    public void StageBack(float x)
    {
        _backgroundMovie.SetAllBack(x);
        foreach (GameObject enemy in _CurrentEnemyObjs) 
        {
            if (enemy == null) continue;
            Vector3 currentPos = enemy.transform.position;
            currentPos.x += x;
            enemy.transform.position = currentPos;
        }

        currentMoveLength -= x;
    }

    IEnumerator EnemyCrashDelay(float delayTime)
    {
        
        currentMoveSpeed = 0;
        _userRobot.SetAniType(RobotMoveType.IDLE);
        SetMoveSpeed();
        yield return new WaitForSeconds(delayTime);

        if(isBooster == true) {
            currentMoveSpeed = GameData.defaultFastMoveSpeed;
            _userRobot.SetAniType(RobotMoveType.SPEEDMOVE);
        } else if (isDefense == true) {
            currentMoveSpeed = 0;
            _userRobot.SetAniType(RobotMoveType.DEFANSE);
        } else {
            currentMoveSpeed = GameData.defaultMoveSpeed;
            _userRobot.SetAniType(RobotMoveType.MOVE);
        }
        SetMoveSpeed();
    }

    public void UserRobotUI()
    {
        RobotData robotData = _GameManager.robotData;

        UserRobotHpUpdate(_UserCurrentHeadHpBar.transform as RectTransform, robotData.currentHeadHP, robotData.maxHeadHP);
        UserRobotHpUpdate(_UserCurrentBodyHpBar.transform as RectTransform, robotData.currentBodyHP, robotData.maxBodyHP);
        UserRobotHpUpdate(_UserCurrentLowerHpBar.transform as RectTransform, robotData.currentLowerHP, robotData.maxLowerHP);

        //print(robotData.currentBodyHP + "/" + (robotData.maxBodyHP * 0.25f));
        if (robotData.currentBodyHP < (robotData.maxBodyHP * 0.25f))
        {
            UserRobot userRobot = UserRobotObj.GetComponent<UserRobot>();
            userRobot.SetDamageShow("body");
        }
        if (robotData.currentLowerHP < (robotData.maxLowerHP * 0.25f))
        {
            UserRobot userRobot = UserRobotObj.GetComponent<UserRobot>(); 
            userRobot.SetDamageShow("thigh");
        }
    }

    private void UserRobotHpUpdate(RectTransform rectTracsform, float currentHP, float maxHP)
    {
        Vector2 HpSize = rectTracsform.sizeDelta;
        if (currentHP <= 0)
        {
            currentHP = 0;
            HpSize.x = 0;
            rectTracsform.sizeDelta = HpSize;
        }
        else
        {
            HpSize.x = 274 * (currentHP / maxHP);
            rectTracsform.sizeDelta = HpSize;
        }
        
    }

    private void SetMoveSpeed()
    {
        _backgroundMovie.moveSpeed = currentMoveSpeed;
    }
}
