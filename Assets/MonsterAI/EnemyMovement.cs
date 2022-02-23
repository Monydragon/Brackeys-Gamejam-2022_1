using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float MoveSpeed = 1f;

    public void Move(Vector2 moveDirection)
    {
        Vector2 Position2D = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Position2D += moveDirection.normalized * MoveSpeed * Time.deltaTime;
        gameObject.transform.position = Position2D;
    }
}
