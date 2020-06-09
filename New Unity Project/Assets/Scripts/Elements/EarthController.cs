using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{

    public Transform earthProp;

    [FMODUnity.EventRef]
    public string DestroyWallEvent = "";
    FMOD.Studio.EventInstance destroyWall;

    void Start()
    {
        destroyWall = FMODUnity.RuntimeManager.CreateInstance(DestroyWallEvent);
    }

    void Update()
    {
        destroyWall.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));

    }
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
            destroyWall.start();

        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EarthCube"))
        {
            hit.transform.gameObject.GetComponent<SpawningMovement>().changeCubeSize(2);
            destroyWall.start();
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
