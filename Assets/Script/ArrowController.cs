using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    private Rigidbody2D myRigibody;
    [SerializeField] private float arrowSpeed;
    private Vector2 _arrowDirection;
    public Vector2 ArrowDirection { get => _arrowDirection; set => _arrowDirection = value; }

    [SerializeField] private GameObject arrowImpact;
    private Transform myTransform;

    private void Awake()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        myRigibody.velocity = ArrowDirection * arrowSpeed;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if(collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(arrowImpact, myTransform.position, Quaternion.identity);
            Destroy(gameObject);
        }       
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
