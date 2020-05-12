using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateObject : MonoBehaviour
{
    [SerializeField] private float aimingAtY;

    // DEBUG
    [SerializeField] private float heightDiff;
    [SerializeField] private float playerY;
    [SerializeField] private float cameraY;

    public Transform player;

    private void Start()
    {
    }

    void Update()
    {

        playerY = player.position.y;
        cameraY = Camera.main.transform.position.y;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("LevitateObject")))
            {

                Vector3 _colliderCenter = hit.collider.gameObject.transform.position;
                Vector3 _cameraPos = Camera.main.transform.position;
                Vector2 _cameraRotation = Camera.main.GetComponent<CameraMovement>().getMouseAbsolute();

                aimingAtY = calculateAimingY(_cameraPos, _cameraRotation, _colliderCenter);

                LevitationProperty propLevitation = hit.collider.gameObject.GetComponentInParent<LevitationProperty>();

                propLevitation.EnableLevitateTest(aimingAtY);
            }
        }
    }

    float calculateAimingY(Vector3 cameraPosition, Vector2 cameraRotation, Vector3 colliderCenter)
    {
        float targetY = 0;

        Vector3 horizonAtCollider = new Vector3(colliderCenter.x, cameraPosition.y, colliderCenter.z);
        float horizonDist = Vector3.Distance(cameraPosition, horizonAtCollider);

        heightDiff = horizonDist * Mathf.Tan(Mathf.Abs(cameraRotation.y * Mathf.Deg2Rad));

        if (cameraRotation.y < 0)
            targetY = cameraPosition.y - heightDiff;
        else
            targetY = cameraPosition.y + heightDiff;

        return targetY;
    }

}
