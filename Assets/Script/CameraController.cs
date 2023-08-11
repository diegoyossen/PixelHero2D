using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform myTransform;
    private PlayerController playerController;
    private Transform playerTransform;
    private BoxCollider2D levelLimit;
    private float cameraSizeHorizontal;
    private float cameraSizeVertical;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerTransform = playerController.GetComponent<Transform>();
        levelLimit = GameObject.Find("LevelLimit").GetComponent<BoxCollider2D>();

        cameraSizeVertical = Camera.main.orthographicSize;
        cameraSizeHorizontal = Camera.main.orthographicSize * Camera.main.aspect;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(playerController != null)
        {
            float x = Mathf.Clamp(playerTransform.position.x, levelLimit.bounds.min.x + cameraSizeHorizontal, levelLimit.bounds.max.x - cameraSizeHorizontal);
            float y = Mathf.Clamp(playerTransform.position.y, levelLimit.bounds.min.y + cameraSizeVertical, levelLimit.bounds.max.y - cameraSizeVertical);
            float z = myTransform.position.z;
            myTransform.position = new Vector3(x,y,z);
        }
    }
}
