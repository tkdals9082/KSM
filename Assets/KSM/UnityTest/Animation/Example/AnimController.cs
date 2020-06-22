using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Animator anim;

    bool zIsPressed => Input.GetKey(KeyCode.Z);
    bool xIsPressed => Input.GetKey(KeyCode.X);
    bool yIsPressed => Input.GetKey(KeyCode.Y);

    private void Update()
    {
        anim.SetBool("isXPressed", xIsPressed);
        anim.SetBool("isYPressed", yIsPressed);
        anim.SetBool("isZPressed", zIsPressed);
    }
}
