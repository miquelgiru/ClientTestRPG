using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float Speed;
    private float width;
    private float height;
    private Vector3 position;

    private void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        position = Vector3.zero;
    }

    private void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        float x = 0, z = 0;
        if (Input.GetMouseButton(0))
        {
            x = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
            z = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x - x, transform.position.y, transform.position.z - z);
        }
#endif

#if UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(-pos.x, pos.y, 0.0f);

                transform.position = position;
            }
        }
         
#endif

    }

}
