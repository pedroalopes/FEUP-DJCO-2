using UnityEngine;

public class CheckCameraPoint : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    public PlayerController player;
    public Transform newObject;
    private Transform _selection;
    public Transform firePrefab;
    private Transform fireObject;

    public Transform camera;


    //WIND
    public GameObject firePoint;
    public GameObject pulse;

    //WATER
    private float dropCooldown = 0.00f;
    private Transform waterObject;
    public Transform dropPrefab;

    [FMODUnity.EventRef]
    public string DestroyWallEvent = "";
    FMOD.Studio.EventInstance destroyWall;

    [FMODUnity.EventRef]
    public string WindEvent = "";
    FMOD.Studio.EventInstance wind;

    void Start()
    {
        destroyWall = FMODUnity.RuntimeManager.CreateInstance(DestroyWallEvent);
        wind = FMODUnity.RuntimeManager.CreateInstance(WindEvent);
    }

    private void Update()
    {
        destroyWall.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
        wind.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MoveableObject"))
                {
                    if (hit.transform.gameObject.GetComponent<PickUp>().Interact()) { return; }
                }
            }
            else if (Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.E))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MoveableObject"))
                {
                    if (hit.transform.gameObject.GetComponent<PickUp>().canInteract()) { return; }
                }
            }

            switch (player.element)
            {
                case Element.Earth:
                    if (Input.GetMouseButtonDown(0))
                    {
                        HandleEarth(hit, ray);
                    }
                    break;
                case Element.Wind:
                    if (Input.GetMouseButton(0))
                    {
                        HandleWind(hit, ray);
                    }
                    break;
                case Element.Fire:
                    if (Input.GetMouseButton(0))
                    {
                        HandleFire(hit);

                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        Vector3 initialPosition = transform.position;
                        Vector3 finalPosition = hit.point;

                        FireShooter(initialPosition, finalPosition);
                    }
                    break;
                case Element.Water:
                    if (Input.GetMouseButton(0))
                    {
                        HandleWater(hit, ray);

                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        CreateDrop();

                    }
                    break;
            }

        }

    }

    private void HandleEarth(RaycastHit hit, Ray ray)
    {
        if (!Physics.Raycast(ray, out hit))
            return;

        var position = hit.point;
        if (hit.normal.y == 1)
        {
            position.x += -1;
            position.z += 1;
        }
        else if (hit.normal.x == 1)
        {
            position.y += 1;
            position.z += 1;
        }
        else if (hit.normal.x == -1)
        {
            position.y += -1;
            position.z += 1;
        }
        else if (hit.normal.z == -1)
        {
            position.y += 1;
            position.x += -1;
        }
        else if (hit.normal.z == 1)
        {
            position.y += -1;
            position.x += -1;
        }
        else if (hit.normal.y < -0.9)
        {
            position.z += -1;
            position.x += -1;
        }


        //(0,1,0) --> (-1,0,1)
        //(1,0,0) --> (0,1,1)
        //(-1,0,0) -->(0,-1,1)
        //(0,0,-1) -->(-1,1,0)
        //(0,0,1) -->(-1,-1,0)
        //(0,-1,0) --> (-1,0,-1)


        //TODO: Divide by element casted
        /*   EARTH BENDING   */
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            position += Vector3.Scale(new Vector3(-4, -4, -4), hit.normal);
            Debug.Log(hit.normal);
            newObject.GetComponent<SpawningMovement>().normalDir = hit.normal;
            position.x -= 1f;
            position.z -= 3f;

            destroyWall.start();
            Instantiate(newObject, position, Quaternion.FromToRotation(Vector3.up, hit.normal));

        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EarthCube"))
        {
            destroyWall.start();
            hit.transform.gameObject.GetComponent<SpawningMovement>().changeCubeSize(2);

        }
    }

    private void HandleFire(RaycastHit hit)
    {
        if (fireObject == null)
        {
            Vector3 cameraPosition = transform.position;
            cameraPosition.y += 0.75f;

            fireObject = Instantiate(firePrefab, cameraPosition + (camera.transform.forward * 1), transform.rotation);
            fireObject.SetParent(firePoint.transform);
        }
        else
        {
            if (fireObject.localScale.x < 1)
            {
                fireObject.localScale = fireObject.localScale + new Vector3(0.01f, 0.01f, 0.01f);
                fireObject.GetComponent<FireObjectScript>().addDamage();
                fireObject.GetComponent<FireObjectScript>().removeForce();
            }
        }

    }


    private void FireShooter(Vector3 initialPosition, Vector3 finalPosition)
    {
        Rigidbody rb = fireObject.gameObject.GetComponent<Rigidbody>();
        Vector3 shoot = (finalPosition - fireObject.position).normalized;

        rb.AddForce(shoot * (int)fireObject.GetComponent<FireObjectScript>().getForce());
        fireObject.SetParent(null);
        fireObject = null;
    }

    private void HandleWind(RaycastHit hit, Ray ray)
    {
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("LevitateObject")))
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

    private void SpawnWindPulse(Ray rayMouse)
    {
        GameObject obj;
        RaycastHit hit;
        Vector3 direction;

        if (firePoint != null)
        {
            obj = Instantiate(pulse, firePoint.transform.position, Quaternion.identity);

            if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, 100))
            {
                direction = hit.point - gameObject.transform.position;
                obj.transform.localRotation = Quaternion.LookRotation(direction);
            }
            else
            {
                var pos = rayMouse.GetPoint(100);
                direction = pos - gameObject.transform.position;
                obj.transform.localRotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            Debug.Log("No firepoint");
        }
    }

    private void HandleWater(RaycastHit hit, Ray ray)
    {
        Vector3 vecX = new Vector3(-0.7f, 0, 0);
        Vector3 vecY = new Vector3(0, -0.7f, 0);
        Vector3 vecZ = new Vector3(0, 0, -0.7f);

        Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));

        if (waterObject == null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Water") && Time.time > dropCooldown)
        {
            Vector3 finalVec = new Vector3(0, 0, 0);

            /*if(hit.normal.y == -1)		//check for roof
				finalVec += vecY;
			else if(hit.normal.y == 1)
				finalVec -= vecY;
				
			if(hit.normal.z == -1)
				finalVec += vecZ;
			else if(hit.normal.z == 1)
				finalVec -= vecZ;
			
			if(hit.normal.x == -1)
				finalVec += vecX;
			else if(hit.normal.x == 1)
				finalVec -= vecX;*/

            finalVec += Vector3.Scale(new Vector3(-0.7f, -0.7f, -0.7f), hit.normal);
            //else
            //	waterObject = Instantiate(dropPrefab, hit.point + hit.normal * 2 - vec,  Quaternion.identity);
            waterObject = Instantiate(dropPrefab, hit.point + hit.normal + finalVec, Quaternion.identity);
        }

        else if (waterObject != null)
        {   /*
			if(hit.normal.y == -1)
				waterObject.position = hit.point + hit.normal + vecY;
			else if(hit.normal.y == 1)
				waterObject.position = hit.point + hit.normal - vecY;
				
			if(hit.normal.z == -1)
				waterObject.position = hit.point + hit.normal + vecZ;
			else if(hit.normal.z == 1)
				waterObject.position = hit.point + hit.normal - vecZ;
			
			if(hit.normal.x == -1)
				waterObject.position = hit.point + hit.normal + vecX;
			else if(hit.normal.x == 1)
				waterObject.position = hit.point + hit.normal - vecX;
			
			//else
			//	waterObject.position = hit.point + hit.normal * 2 - vec;
			*/

            waterObject.localScale = waterObject.localScale + new Vector3(0.01f, 0.01f, 0.01f);
            waterObject.gameObject.GetComponent<WaterDrop>().freezeMotion();

            Rigidbody rb = waterObject.GetComponent<Rigidbody>();
            rb.mass += 0.05f;
        }
        if (waterObject != null && ((hit.transform.gameObject.layer != LayerMask.NameToLayer("Water") && hit.transform.gameObject.layer != LayerMask.NameToLayer("WaterBall")) || waterObject.localScale.x > 1.5f))
        {
            CreateDrop();
        }

    }

    private void CreateDrop()
    {
        if (waterObject != null)
        {
            waterObject.gameObject.GetComponent<WaterDrop>().freezeMotion();
            waterObject = null;
            dropCooldown = Time.time + 2f;
        }
    }
}