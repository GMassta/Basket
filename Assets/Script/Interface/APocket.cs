using UnityEngine;

public abstract class APocket: MonoBehaviour
{
    //Throw selecte item from pocket
    abstract public void ThrowItem(Vector3 direction);

    //Draw throw trace
    abstract public void DrawTrace(Vector3 direction);
}
