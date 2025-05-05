using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints, followMouse };

    [Header("AI Settings")]
    public AIMode aiMode;
    public float maxSpeed = 16;

    bool isRunningStuckCheck = false;
    bool isFirstTemporaryWaypoint = false;
    int stuckCheckCounter = 0;
    List<Vector2> temporaryWaypoints = new List<Vector2>();
    float angleToTarget = 0;

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    WaypointNode currentWaypoint = null;
    WaypointNode[] allWaypoints;

    CarController carController;
    AStarLite aStarLite;

    void Awake()
    {
        carController = GetComponent<CarController>();
        allWaypoints = FindObjectsOfType<WaypointNode>();

        aStarLite = GetComponent<AStarLite>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.GetGameStates() == GameStates.countDown) return;

        Vector2 inputVector = Vector2.zero;

        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;
            case AIMode.followWaypoints:
                if (temporaryWaypoints.Count == 0) FollowWaypoints();
                else FollowTemporaryWaypoints();
                break;
            case AIMode.followMouse:
                FollowMousePosition();
                break;
        }
        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

        if (carController.GetVelocityMagnitude() < 0.5f && Mathf.Abs(inputVector.x) > 0.01f && !isRunningStuckCheck)
            StartCoroutine(StuckCheckCO());

        if (stuckCheckCounter >= 4 && !isRunningStuckCheck) StartCoroutine(StuckCheckCO());

        carController.SetInputVector(inputVector);
    }

    void FollowPlayer()
    {
        if (targetTransform == null) targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targetTransform != null) targetPosition = targetTransform.position;


    }

    void FollowMousePosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        targetPosition = worldPosition;
    }

    void FollowWaypoints()
    {
        if (currentWaypoint == null) currentWaypoint = FindClosestWaypoint();

        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.transform.position;

            float distanceToWayPoint = (targetPosition - transform.position).magnitude;

            if (distanceToWayPoint <= currentWaypoint.minDistanceToReachWaypoint)
            {
                if (currentWaypoint.maxSpeed > 0) maxSpeed = currentWaypoint.maxSpeed;
                else maxSpeed = 1000;

                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }
    }

    void FollowTemporaryWaypoints()
    {
        targetPosition = temporaryWaypoints[0];

        float distanceToWayPoint = (targetPosition - transform.position).magnitude;

        maxSpeed = 5;

        float minDistanceToReachWaypoint = 1.5f;

        if (!isFirstTemporaryWaypoint) minDistanceToReachWaypoint = 3.0f;

        if (distanceToWayPoint <= minDistanceToReachWaypoint)
        {
            temporaryWaypoints.RemoveAt(0);
            isFirstTemporaryWaypoint = false;
        }
    }

    WaypointNode FindClosestWaypoint()
    {
        return allWaypoints.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    }

    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        angleToTarget = Vector2.SignedAngle(transform.right, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    float ApplyThrottleOrBrake(float inputX)
    {
        if (carController.GetVelocityMagnitude() > maxSpeed) return 0;

        float throttle = 1.0f - Mathf.Abs(inputX) / 1.0f; ;

        if (temporaryWaypoints.Count != 0)
        {
            if (angleToTarget > 70) throttle = throttle * -1;
            else if (angleToTarget < -70) throttle = throttle * -1;
            else if (stuckCheckCounter > 3) throttle = throttle * -1;
        }

        return throttle;
    }

    /*bool IsCarsInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)
    {
        RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));
    }
    */
    IEnumerator StuckCheckCO()
    {
        Vector3 initialStuckPosition = transform.position;

        isRunningStuckCheck = true;

        yield return new WaitForSeconds(0.7f);

        if ((transform.position - initialStuckPosition).sqrMagnitude < 3)
        {
            temporaryWaypoints = aStarLite.FindPath(currentWaypoint.transform.position);

            if (temporaryWaypoints == null) temporaryWaypoints = new List<Vector2>();

            stuckCheckCounter++;

            isFirstTemporaryWaypoint = true;
        }
        else stuckCheckCounter = 0;

        isRunningStuckCheck = false;
    }
}
