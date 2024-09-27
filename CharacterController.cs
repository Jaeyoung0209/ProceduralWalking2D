using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private const float legLength = 1.21f;
    private Vector2 rayLocation;
    public LayerMask groundLayer;
    private float rayOffset;

    public float walkDistance;

    private Rigidbody2D rb;
    public float walkVelocity;

    public GameObject LeftLegTarget;
    public GameObject RightLegTarget;

    public Transform LeftArmTarget;
    public Transform RightArmTarget;
    
    private bool rightLegPlanted = true;
    [SerializeField]
    private bool moveFoot = true;

    private Vector2 targetFootLocation;

    public GameObject TorsoPosition;
    private Vector2 footNormal;

    public Transform leftFoot;
    public Transform rightFoot;
    
    public float torsoAmplitude;
    public float footAmplitude;

    private bool forward = true;
   



    private void Start()
    {
        rayOffset = transform.position.x + 0.5f;
        rb = GetComponent<Rigidbody2D>();
        LeftLegTarget.transform.position = new Vector2(transform.position.x, -0.57f);
        RightLegTarget.transform.position = new Vector2(transform.position.x, -0.57f);
        rayLocation = new Vector2(rayOffset, TorsoPosition.transform.position.y - 0.5f);
        
        RaycastHit2D hit;
        hit = Physics2D.Raycast(rayLocation, Vector2.down, 10, groundLayer);
        targetFootLocation = hit.point;
        footNormal = hit.normal;
        moveFoot = true;
        //walkDistance = walkVelocity / 3;
    }
    void Update()
    {
       
        if ((Mathf.Abs(rayOffset - transform.position.x) - Mathf.Abs(walkDistance)) >= 0)
        {
            if (forward)
                rb.velocity = new Vector2(walkVelocity, rb.velocity.y);
            else
                rb.velocity = new Vector2(-walkVelocity, rb.velocity.y);
        } else
        {
            rb.velocity = Vector2.zero;
            
            if (moveFoot)
            {
                HandleInput();
                
            }
        }

    }

    private IEnumerator BezierCoroutine(GameObject legTarget, Vector2 targetLocation, bool isleft, float leverage)
    {
        moveFoot = false;
        Vector2 startVector = new Vector2(legTarget.transform.position.x, legTarget.transform.position.y);
        Vector2 bezierVector = PositionTorso(startVector, targetLocation) + new Vector2(0, (leverage > 0)? leverage * 3 - 0.5f : -0.5f);
        for(float i = 0; i < 21; i++)
        {
            float t = i / 20;
            yield return new WaitForSeconds((1/walkVelocity)/20);

            legTarget.transform.position = Mathf.Pow(1 - t, 2) * startVector + 2 * (1 - t) * t * bezierVector + Mathf.Pow(t, 2) * targetLocation;
        }
        moveFoot = true;
        if (isleft)
        {
            leftFoot.rotation = Quaternion.Euler(leftFoot.eulerAngles.x, leftFoot.eulerAngles.y, 90 - Mathf.Atan2(footNormal.y, footNormal.x) * 180 / Mathf.PI );
           
        }
        else
        {
            rightFoot.rotation = Quaternion.Euler(rightFoot.eulerAngles.x, rightFoot.eulerAngles.y, 90 - Mathf.Atan2(footNormal.y, footNormal.x) * 180 / Mathf.PI );
        }
    }

    private IEnumerator TorsoCoroutine(Vector2 current, Vector2 final)
    {

        Vector2 startVector = new Vector2(current.x, current.y);
        Vector2 bezierVector = new Vector2((current.x + final.x)/2, Mathf.Max(current.y, final.y) + torsoAmplitude);
        for (float i = 0; i < 21; i++)
        {
            float t = i / 20;
            yield return new WaitForSeconds((1 / walkVelocity) / 20);

            TorsoPosition.transform.position = Mathf.Pow(1 - t, 2) * startVector + 2 * (1 - t) * t * bezierVector + Mathf.Pow(t, 2) * final;
        }
    }

    private IEnumerator ArmCoroutine(bool isleft)
    {
        for (float i = 0; i < 21; i++)
        {
            float t = i / 20;
            yield return new WaitForSeconds((1 / walkVelocity) / 20);

            if (!isleft)
            {
                LeftArmTarget.position = new Vector2(-Mathf.Cos(Mathf.PI * t)/2 + transform.position.x, TorsoPosition.transform.position.y - 0.4f + 0.15f * Mathf.Cos(Mathf.PI * 2 * t));
                RightArmTarget.position = new Vector2(Mathf.Cos(Mathf.PI * t)/2 + transform.position.x, TorsoPosition.transform.position.y - 0.4f + 0.15f * Mathf.Cos(Mathf.PI * 2 * t));

            }
            else
            {
                LeftArmTarget.position = new Vector2(Mathf.Cos(Mathf.PI * t)/2 + transform.position.x, TorsoPosition.transform.position.y - 0.4f + 0.15f * Mathf.Cos(Mathf.PI * 2 * t));
                RightArmTarget.position = new Vector2(-Mathf.Cos(Mathf.PI * t)/2 + transform.position.x, TorsoPosition.transform.position.y - 0.4f + 0.15f * Mathf.Cos(Mathf.PI * 2 * t));
            }

        }
    }


    private void HandleInput()
    {
        float input = Input.GetAxis("Horizontal");
        if (input != 0)
        {
            if (input > 0)
            {
                if (!forward)
                {
                    rayOffset += walkDistance * 2f;
                    rightLegPlanted = !rightLegPlanted;
                }
                TorsoPosition.transform.rotation = Quaternion.Euler(0, 180, 88.43f);
                forward = true;
                rayOffset += walkDistance * 2f;
            }
            else
            {
                if (forward)
                {
                    rayOffset -= walkDistance * 2f;
                    rightLegPlanted = !rightLegPlanted;
                }
                TorsoPosition.transform.rotation = Quaternion.Euler(0, 0, 88.43f);
                forward = false;
                rayOffset -= walkDistance * 2f;
            }
            rayLocation = new Vector2(rayOffset, TorsoPosition.transform.position.y - 0.5f);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(rayLocation, Vector2.down, 10, groundLayer);
            targetFootLocation = hit.point;
            footNormal = -hit.normal;
                                    

            
            if (rightLegPlanted)
            {
                StartCoroutine(BezierCoroutine(LeftLegTarget, targetFootLocation, true, RightLegTarget.transform.position.y - LeftLegTarget.transform.position.y));
                StartCoroutine(TorsoCoroutine(TorsoPosition.transform.position, PositionTorso(RightLegTarget.transform.position, targetFootLocation)));
                StartCoroutine(ArmCoroutine(false));
                

            }
            else
            {
                
                StartCoroutine(BezierCoroutine(RightLegTarget, targetFootLocation, false, LeftLegTarget.transform.position.y - RightLegTarget.transform.position.y));
                StartCoroutine(TorsoCoroutine(TorsoPosition.transform.position, PositionTorso(LeftLegTarget.transform.position, targetFootLocation)));
                StartCoroutine(ArmCoroutine(true));
                

            }
            rightLegPlanted = !rightLegPlanted;
        }
    }

    private Vector2 PositionTorso(Vector2 pivotFoot, Vector2 stepFoot)
    {
        if (stepFoot.y >= pivotFoot.y)
        {
            return new Vector2((forward)? TorsoPosition.transform.position.x + walkDistance * 2 : TorsoPosition.transform.position.x - walkDistance * 2, Mathf.Sqrt(Mathf.Pow(legLength, 2) - Mathf.Pow(walkDistance, 2)) + pivotFoot.y);
        } 
        else
        {
            return new Vector2((forward)? TorsoPosition.transform.position.x + walkDistance * 2 : TorsoPosition.transform.position.x - walkDistance * 2, Mathf.Sqrt(Mathf.Pow(legLength, 2) - Mathf.Pow(walkDistance, 2)) + stepFoot.y);
        }
    }
}
