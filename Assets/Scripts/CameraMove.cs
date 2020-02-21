using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{

    public float Speed;


    private void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        float x = 0, z = 0;
        Speed = 1;
        if (Input.GetMouseButton(0))
        {
            x = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
            z = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x - x, transform.position.y, transform.position.z - z);
        }
#endif

#if UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * Speed, 0, -touchDeltaPosition.y * Speed);
            Debug.Log(transform.position.ToString());
        }

#endif


    }

}

