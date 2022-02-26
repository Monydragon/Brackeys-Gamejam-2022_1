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
            var anim = objectToMove.GetComponent<Animator>();
            if (anim != null)
            {
                var dif = targetPosition.position - objectToMove.transform.position;
                anim.SetFloat("MoveX", dif.x);
                anim.SetFloat("MoveY", dif.y);
                anim.SetBool("isMoving", true);
            }
            yield return null;
        }
    }
}
