using UnityEngine;
using System.Collections;

public class footSmokeSC : MonoBehaviour {

    private float deleteDelay = 2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        deleteDelay -= Time.deltaTime;
        if(deleteDelay <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
