using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement_ : MonoBehaviour
{
    public Transform target;
    public float velocidadDeRotacion;

    [SerializeField] float speed = 25;
    Vector3[] path;
    int targetIndex;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PathRequestsManager.RequestPath(transform.position, target.position, OnPathFound);
        }
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            TurnToTarget(currentWaypoint);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void TurnToTarget(Vector3 target)
    {
        Vector3 targetOrientation = target - transform.position;

        Quaternion targetOrientationQuaternion = Quaternion.LookRotation(targetOrientation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetOrientationQuaternion, velocidadDeRotacion * Time.deltaTime);
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.3f, 0.3f, 0.3f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
