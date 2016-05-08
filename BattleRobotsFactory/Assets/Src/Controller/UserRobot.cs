using UnityEngine;
using System.Collections;
using Spine;

public class UserRobot : MonoBehaviour {

    const float LowerRotationBound = -60.0f;
    const float UpperRotationBound = 60.0f;
    
    public GameObject robotBody;
    public GameObject leftDridge;
    public GameObject rightDridge;
    public GameObject weaponBody;
    public GameObject gunObject;

    public string currentMoveType;

    private GameObject bodyDamageEffect = null;
    private GameObject thighDamageEffect = null;
    
    private SkeletonAnimation bodyAnimation;
    private SkeletonAnimation leftLegAnimation;
    private SkeletonAnimation rightLegAnimation;
    private SkeletonAnimation weaponAnimation;

    private Bone bodyRoot;
    private Vector3 weaponVector;
    private Vector3 currentWeaponVector;

    private bool isGunFire = false;

    private Vector3 currentTargetPos;
    private Vector3 object_pos;
    private float angle;

    private GameObject footSmokePrefab;

    private Quaternion gunDefaultRotation;
    
    // Use this for initialization
    void Start () {

        currentMoveType = RobotMoveType.MOVE;

        bodyAnimation = robotBody.GetComponent<SkeletonAnimation>();
        leftLegAnimation = leftDridge.GetComponent<SkeletonAnimation>();
        rightLegAnimation = rightDridge.GetComponent<SkeletonAnimation>();
        weaponAnimation = weaponBody.GetComponent<SkeletonAnimation>();

        leftLegAnimation.state.Event += FootAniEvent;
        rightLegAnimation.state.Event += FootAniEvent;
        weaponAnimation.state.Event += BulletFire;

        //weaponAnimation.state.SetAnimation(0, "fire", true);

        weaponAnimation.state.SetAnimation(0, "idle", false);

        bodyRoot = bodyAnimation.skeleton.FindBone("root");

        weaponVector = weaponAnimation.transform.position;
        //currentWeaponVector = weaponVector;

        gunDefaultRotation = gunObject.transform.rotation;

        SoundManager.getInstance.EffectSoundPlay(EffectSoundType.USER_ROBOT_MOVE);
        
        currentTargetPos = new Vector3(10, 0, 0);

        SetAniType(currentMoveType);
    }
    
    void FootAniEvent(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if(e.Data.Name == "footCenter")
        {
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.USER_ROBOT_MOVE);
            footSmokePrefab = Instantiate(Resources.Load("common/footSmoke00")) as GameObject;
            footSmokePrefab.transform.position = this.transform.position;
        } else {
            state.SetAnimation(0, currentMoveType, false);
            rightLegAnimation.state.SetAnimation(0, "right" + currentMoveType, false);
            bodyAnimation.state.SetAnimation(0, currentMoveType, false); 
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.USER_ROBOT_MOVE);
            footSmokePrefab = Instantiate(Resources.Load("common/footSmoke00")) as GameObject;
            footSmokePrefab.transform.position = this.transform.position;
        }
    }

    void BulletFire(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        RobotData robotData = GameManager.getInstance.robotData;

        if (robotData.currentBullet <= 0)
        {
            weaponAnimation.state.SetAnimation(0, "idle", false);
            return;
        }
        //Quaternion
        Vector3 bulletStartPos = new Vector3(weaponAnimation.transform.position.x + 1, weaponAnimation.transform.position.y, weaponAnimation.transform.position.z);
        GameObject bulletPrefab = (GameObject)Instantiate(Resources.Load("common/bulletPrefab"), bulletStartPos, weaponAnimation.transform.rotation);
        Rigidbody2D bulletRigidbody = bulletPrefab.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = weaponAnimation.transform.TransformDirection(new Vector3(GameData.bulletSpeed, 0, 0));

        SoundManager.getInstance.EffectSoundPlay(EffectSoundType.USER_WEAPON_M249);

        robotData.currentBullet -= 1;

    }

    public void SetAniType(string type)
    {
        currentMoveType = type;
        float yPos = 0;
        
        if (RobotMoveType.MOVE == type)
        {
            yPos = -3.12f;
            bodyAnimation.state.SetAnimation(0, type, false);
            leftLegAnimation.state.SetAnimation(0, type, false);
            rightLegAnimation.state.SetAnimation(0, "right" + type, false);
        }
        else if (RobotMoveType.IDLE == type)
        {
            yPos = -3.12f;
            bodyAnimation.state.SetAnimation(0, type, false);
            leftLegAnimation.state.SetAnimation(0, type, false);
            rightLegAnimation.state.SetAnimation(0, type, false);
        }
        else if (RobotMoveType.DEFANSE == type)
        {
            yPos = -3.55f;
            bodyAnimation.state.SetAnimation(0, type, false);
            leftLegAnimation.state.SetAnimation(0, type, false);
            rightLegAnimation.state.SetAnimation(0, type, false);
        } else {
            yPos = -3.55f;
            bodyAnimation.state.SetAnimation(0, type, true);
            leftLegAnimation.state.SetAnimation(0, type, true);
            rightLegAnimation.state.SetAnimation(0, type, true); 
        }

        iTween.MoveTo(this.transform.root.gameObject, iTween.Hash("y", yPos));
        

    }

    public void GunFire(bool isValue)
    {
        RobotData robotData = GameManager.getInstance.robotData;
        if (robotData.currentBullet <= 0)
        {
            isGunFire = false; 
            return;
        }

        isGunFire = isValue;

        if (isGunFire == true)
        {
            weaponAnimation.state.AddAnimation(0, "fire", true, 0);
        }
        else
        {
            weaponAnimation.state.ClearTrack(0);
            weaponAnimation.state.Update(0);
            weaponAnimation.state.AddAnimation(0, "idle", false, 0);
        }
    }

    public void NewUpdateGunAngle(Vector3 targetPos) {
        currentTargetPos = targetPos;
    }

    public void SetDamageShow(string type)
    {
        if (bodyDamageEffect == null && type == "body")
        {
            Transform bodyRootTF = robotBody.transform.FindChild("SkeletonUtility-Root") as Transform;
            Vector3 bodyEffectPos = new Vector3(bodyRootTF.transform.position.x, bodyRootTF.transform.position.y, bodyRootTF.transform.position.z);
            //bodyDamageEffect = (GameObject)Instantiate(Resources.Load("common/NormalDamage"), effectPos, this.transform.rotation);
            bodyDamageEffect = (GameObject)Instantiate(Resources.Load("common/NormalDamage"), bodyEffectPos, this.transform.rotation);
            bodyDamageEffect.transform.parent = bodyRootTF;
            
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.OTHER_EXPLOSION_01);
        }

        if (thighDamageEffect == null && type == "thigh")
        {
            Transform thighRootTF = rightDridge.transform.FindChild("SkeletonUtility-Root") as Transform;
            Vector3 thighEffectPos = new Vector3(thighRootTF.transform.position.x, thighRootTF.transform.position.y - 1, thighRootTF.transform.position.z);
            //thighDamageEffect = (GameObject)Instantiate(Resources.Load("common/NormalDamage"), effectPos, this.transform.rotation);
            thighDamageEffect = (GameObject)Instantiate(Resources.Load("common/NormalDamage"), thighEffectPos, this.transform.rotation);
            thighDamageEffect.transform.parent = thighRootTF;
            
            SoundManager.getInstance.EffectSoundPlay(EffectSoundType.OTHER_EXPLOSION_01);
        }

    }

    // Update is called once per frame
    void Update () {
        
        //currentWeaponVector.y = weaponVector.y + (bodyRoot.y * 0.35f);

        //weaponAnimation.transform.position = currentWeaponVector;

        GunAngleUpdate(currentTargetPos);
    }

    private void GunAngleUpdate(Vector3 targetPos) {
        
        targetPos.z = 5.23f; //The distance between the camera and object
        object_pos = Camera.main.WorldToScreenPoint(gunObject.transform.position);
        Vector2 traget_Pos = Camera.main.WorldToScreenPoint(targetPos); 
        Vector2 offset = new Vector2(traget_Pos.x - object_pos.x, traget_Pos.y - object_pos.y);
        
        if (isGunFire == true)
        {
            float angleRan = GameManager.getInstance.GetRandomValue(-3.5f, 3.5f);
            angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg) + angleRan;
        }
        else
        {
            angle = (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
        }
        gunObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        SetBoneRotation(weaponAnimation.transform, gunDefaultRotation, gunObject.transform.rotation);

    }

    private void SetBoneRotation(Transform bone, Quaternion defaultRotation, Quaternion rotation)
    {
        /*if (animated) bone.Rotate(rotation.eulerAngles, Space.World);
        else bone.localRotation = defaultRotation * rotation;*/
        bone.localRotation = defaultRotation * rotation;
    }
}
