using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string WindEvent = "";
    FMOD.Studio.EventInstance wind;

    void Start()
    {
        wind = FMODUnity.RuntimeManager.CreateInstance(WindEvent);
    }

    void Update()
    {
        wind.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
    }

    public void handleCharge(RaycastHit hit)
    {

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MoveableObject"))
        {
            wind.start();

            Vector3 _colliderCenter = hit.collider.gameObject.transform.position;
            Vector3 _cameraPos = Camera.main.transform.position;
            Vector2 _cameraRotation = Camera.main.GetComponent<CameraMovement>().getMouseAbsolute();

            float aimingAtY = calculateAimingY(_cameraPos, _cameraRotation, _colliderCenter);

            LevitationProperty propLevitation = hit.collider.gameObject.GetComponentInParent<LevitationProperty>();
            propLevitation.EnableLevitate(aimingAtY);
        }

    }

    float calculateAimingY(Vector3 cameraPosition, Vector2 cameraRotation, Vector3 colliderCenter)
    {
        float targetY = 0;

        Vector3 horizonAtCollider = new Vector3(colliderCenter.x, cameraPosition.y, colliderCenter.z);
        float horizonDist = Vector3.Distance(cameraPosition, horizonAtCollider);

        float heightDiff = horizonDist * Mathf.Tan(Mathf.Abs(cameraRotation.y * Mathf.Deg2Rad));

        if (cameraRotation.y < 0)
            targetY = cameraPosition.y - heightDiff;
        else
            targetY = cameraPosition.y + heightDiff;

        return targetY;
    }

}
