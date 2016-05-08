using UnityEngine;
using System.Collections;

public class BackgroundMovie1 : BackgroundMovie {

    private GameObject BgBuild1;
    private GameObject BgBuild2;
    private GameObject Floor1;
    private GameObject Floor2;
    private GameObject Object1;
    private GameObject Object2;
    private GameObject Object3;
    private GameObject Object4;
    private GameObject Object5;
    private GameObject Object6;
    private GameObject Smoke;

    void Start()
    {
        BgBuild1 = this.transform.Find("BgBuild1").gameObject;
        BgBuild2 = this.transform.Find("BgBuild2").gameObject;
        Floor1 = this.transform.Find("Floor1").gameObject;
        Floor2 = this.transform.Find("Floor2").gameObject;
        Object1 = this.transform.Find("Object1").gameObject;
        Object2 = this.transform.Find("Object2").gameObject;
        Object3 = this.transform.Find("Object3").gameObject;
        Object4 = this.transform.Find("Object4").gameObject;
        Object5 = this.transform.Find("Object5").gameObject;
        Object6 = this.transform.Find("Object6").gameObject;
        //Smoke = this.transform.Find("Smoke").gameObject;
    }

	void Update () {
        if (Floor1 != null)
        {
            MoveXObject(Floor1, moveSpeed * -1f);
            if (Floor1.transform.position.x < -18.4f) SetXObject(Floor1, 18.4f);
            MoveXObject(Floor2, moveSpeed * -1f);
            if (Floor2.transform.position.x < -18.4f) SetXObject(Floor2, 18.4f);

            /*MoveXObject(Smoke, moveSpeed * -1f);
            if (Smoke.transform.position.x < -18.4f) SetXObject(Smoke, 18.4f);
            */
            MoveXObject(Object1, moveSpeed * -0.6f);
            if (Object1.transform.position.x < -18.4f) SetXObject(Object1, 18.4f);
            MoveXObject(Object2, moveSpeed * -0.6f);
            if (Object2.transform.position.x < -18.4f) SetXObject(Object2, 18.4f);
            MoveXObject(Object3, moveSpeed * -0.6f);
            if (Object3.transform.position.x < -18.4f) SetXObject(Object3, 18.4f);
            MoveXObject(Object4, moveSpeed * -0.8f);
            if (Object4.transform.position.x < -18.4f) SetXObject(Object4, 18.4f);
            MoveXObject(Object5, moveSpeed * -0.8f);
            if (Object5.transform.position.x < -18.4f) SetXObject(Object5, 18.4f);
            MoveXObject(Object6, moveSpeed * -0.5f);
            if (Object6.transform.position.x < -18.4f) SetXObject(Object6, 18.4f);

            MoveXObject(BgBuild1, moveSpeed * -0.2f);
            if (BgBuild1.transform.position.x < -18.4f) SetXObject(BgBuild1, 18.4f);
            MoveXObject(BgBuild2, moveSpeed * -0.2f);
            if (BgBuild2.transform.position.x < -18.4f) SetXObject(BgBuild2, 18.4f); 
        }
	}

    public override void SetAllBack(float x)
    {
        MoveXObject(Floor1, x);
        MoveXObject(Floor2, x);
        MoveXObject(Object1, x * 0.6f);
        MoveXObject(Object2, x * 0.6f);
        MoveXObject(Object3, x * 0.6f);
        MoveXObject(Object4, x * 0.8f);
        MoveXObject(Object5, x * 0.8f);
        MoveXObject(Object6, x * 0.5f);
        MoveXObject(BgBuild1, x * 0.2f);
        MoveXObject(BgBuild2, x * 0.2f);
    }
}
