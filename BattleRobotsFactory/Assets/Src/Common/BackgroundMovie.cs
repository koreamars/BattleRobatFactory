using UnityEngine;
using System.Collections;

public class BackgroundMovie : MonoBehaviour {

    public float moveSpeed = 0.01f;

    internal void MoveXObject(GameObject targetObj, float x)
    {
        Vector3 oriVector = targetObj.transform.position;
        oriVector.x += x;
        targetObj.transform.position = oriVector;
    }

    internal void SetXObject(GameObject targetObj, float x)
    {
        Vector3 oriVector = targetObj.transform.position;
        oriVector.x = x;
        targetObj.transform.position = oriVector;
    }

    public virtual void SetAllBack(float x)
    {
        Transform childObj = null;
        int childCount = this.transform.GetChildCount();
        for (int i = 0; i < childCount; i++)
        {
            childObj = this.transform.GetChild(i);
            Vector3 childPos = childObj.position;
            childPos.x += x;
            childObj.transform.position = childPos;
        }
    }
}
