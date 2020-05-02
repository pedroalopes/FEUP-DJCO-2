using UnityEngine;

public class CheckCameraPoint : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    public PlayerController player;
    public Transform newObject;
    private Transform _selection;
    private LineRenderer lr;
    public Transform firePrefab;
    private Transform fireObject;

    //WIND
    public GameObject firePoint;
    public GameObject pulse;

	//WATER
	private float dropCooldown = 0.00f;
    private Transform waterObject;
	public Transform dropPrefab;
	
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            lr.SetPosition(1, hit.point);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MoveableObject"))
            {
                if (hit.transform.gameObject.GetComponent<PickUp>().Interact()) { return; }
            }
        } else if(Input.GetKey(KeyCode.E) || Input.GetKeyUp(KeyCode.E))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MoveableObject"))
            {
                if (hit.transform.gameObject.GetComponent<PickUp>().canInteract()) { return; }
            }
        }

        switch (player.element)
        {
            case Element.Earth:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HandleEarth(hit, ray);
                }
                break;
            case Element.Wind:
                if (Input.GetKey(KeyCode.E))
                {
                    HandleWind(hit, ray);
                }
                break;
            case Element.Fire:
                if (Input.GetKey(KeyCode.E))
                {
                    HandleFire(hit);

                } else if(Input.GetKeyUp(KeyCode.E))
                {
                    Vector3 initialPosition = transform.position;
                    Vector3 finalPosition = hit.point;

                    FireShooter(initialPosition, finalPosition, hit);
                }
                break;
            case Element.Water:
                if (Input.GetKey(KeyCode.E))
                {
                    HandleWater(hit, ray);
					
                } 
				else if(Input.GetKeyUp(KeyCode.E))
                {
                    CreateDrop();
					
                }
                break;
        }
   
           
       
    }
    
    private void HandleEarth(RaycastHit hit, Ray ray)
    {
        if (!Physics.Raycast(ray, out hit))
            return;

        var position = hit.point;

        //TODO: Divide by element casted
        /*   EARTH BENDING   */
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            position += Vector3.Scale(new Vector3(-4, -4, -4), hit.normal);
            Debug.Log(hit.normal);
            newObject.GetComponent<SpawningMovement>().normalDir = hit.normal;
            Instantiate(newObject, position, Quaternion.FromToRotation(Vector3.up, hit.normal));

        }
        else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EarthCube"))
        {
            hit.transform.gameObject.GetComponent<SpawningMovement>().changeCubeSize(2);

        }
    }

    private void HandleFire(RaycastHit hit)
    {
        if (fireObject == null)
        {
            fireObject = Instantiate(firePrefab, transform.position + (transform.forward * 2), transform.rotation);

        }
        else
        {
            fireObject.position = transform.position + (transform.forward * 2);
            if (fireObject.localScale.x < 1)
            {

                fireObject.localScale = fireObject.localScale + new Vector3(0.01f, 0.01f, 0.01f);

                fireObject.GetComponent<FireObjectScript>().addDamage();
                fireObject.GetComponent<FireObjectScript>().removeForce();
            }
        }

    }

    
    private void FireShooter(Vector3 initialPosition, Vector3 finalPosition, RaycastHit hit)
    {
        Rigidbody rb = fireObject.gameObject.GetComponent<Rigidbody>();
        Vector3 shoot = (finalPosition - fireObject.position).normalized;
        rb.AddForce(shoot * (int)fireObject.GetComponent<FireObjectScript>().getForce());

        fireObject = null;
    }

    private void HandleWind(RaycastHit hit, Ray ray)
    {
        if (!Physics.Raycast(ray, out hit))
            return;

        if (!hit.transform.CompareTag("Levitate"))
        {
            Debug.Log("Can't levitate that");
            return;
        }

      
        LevitationProperty prop = hit.collider.gameObject.GetComponent<LevitationProperty>();
        prop.EnableLevitate();
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
        if (waterObject == null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") && Time.time > dropCooldown)
        {
            waterObject = Instantiate(dropPrefab, hit.point + new Vector3(0,1,0),  Quaternion.identity);

        }
		
        else if (waterObject != null)
        {
            waterObject.position = hit.point + new Vector3(0,1,0);
			
		
            waterObject.localScale = waterObject.localScale + new Vector3(0.01f, 0.01f, 0.01f);
			
			Rigidbody rb = waterObject.GetComponent<Rigidbody>();
			rb.mass += 0.05f;
		}
		if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Ground") && waterObject != null) {	
			CreateDrop();
		}
		
    }

    private void CreateDrop()
    {
		if(waterObject != null) {
			waterObject = null;
			dropCooldown = Time.time + 2f;
		}
    }
}