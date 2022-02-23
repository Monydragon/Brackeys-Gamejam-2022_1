using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private float DistanceEpsilon = 0.01f;

    public void MoveGameobject(GameObject objectToMove, Transform targetPosition, float speed)
    {
        StartCoroutine(MoveGameObjectToPos(objectToMove, targetPosition, speed));
    }

    IEnumerator MoveGameObjectToPos(GameObject objectToMove, Transform targetPosition, float speed)
    {
        while (Vector2.Distance(objectToMove.transform.position, targetPosition.position) > DistanceEpsilon)
        {
            objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, targetPosition.position, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
