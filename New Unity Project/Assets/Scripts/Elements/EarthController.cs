using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{

    public Transform earthProp;
    public void handleStart(RaycastHit hit)
    {

        var position = calculatePosition(hit.point, hit.normal);

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            position += Vector3.Scale(new Vector3(-4, -4, -4), hit.normal);
            earthProp.GetComponent<SpawningMovement>().normalDir = hit.normal;
            position.x -= 1f;
            position.z -= 3f;

            Instantiate(earthProp, position, Quaternion.FromToRotation(Vector3.up, hit.normal));

        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EarthCube"))
        {
            hit.transform.gameObject.GetComponent<SpawningMovement>().changeCubeSize(2);

        }
    }

    Vector3 calculatePosition(Vector3 point, Vector3 normal)
    {
        var position = point;
        if (normal.y == 1)
        {
            position.x += -1;
            position.z += 1;
        }
        else if (normal.x == 1)
        {
            position.y += 1;
            position.z += 1;
        }
        else if (normal.x == -1)
        {
            position.y += -1;
            position.z += 1;
        }
        else if (normal.z == -1)
        {
            position.y += 1;
            position.x += -1;
        }
        else if (normal.z == 1)
        {
            position.y += -1;
            position.x += -1;
        }
        else if (normal.y < -0.9)
        {
            position.z += -1;
            position.x += -1;
        }

        return position;
    }
}
