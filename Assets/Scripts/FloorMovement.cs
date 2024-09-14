using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{
    public void Move(float movX, float movY)
    {
        transform.position += new Vector3(movX, movY, 0f);
    }
}
