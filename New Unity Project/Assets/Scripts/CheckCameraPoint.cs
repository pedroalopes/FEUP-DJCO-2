using UnityEngine;

public class CheckCameraPoint : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;


    public Transform newObject;
    private Transform _selection;
    private LineRenderer lr;
    public Transform firePrefab;
    private Transform fireObject;

    //WIND
    public GameObject firePoint;
    public GameObject pulse;

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
            Debug.Log("pressed E");

            if (Physics.Raycast(ray, out hit))
            {
                //TODO: Insert Element that was casted
                HandleUse(hit);
            }
        }
        else if (Input.GetKey(KeyCode.F))
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
        else if (Input.GetKeyUp(KeyCode.F))
        {
            Vector3 initialPosition = transform.position;
            Vector3 finalPosition = hit.point;

            handleFire(initialPosition, finalPosition, hit);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnWindPulse(ray);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            if(!Physics.Raycast(ray, out hit))
                return; 

            if (!hit.transform.CompareTag("Levitate"))
            {
                Debug.Log("Can't levitate that");
                return;
            }

            LevitationProperty prop = hit.collider.gameObject.GetComponent<LevitationProperty>();
            prop.EnableLevitate();

        }

        /*if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }

                _selection = selection;
            }
        }*/
    }

    private void HandleUse(RaycastHit hit)
    {
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
        /********************************/
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MoveableObject"))
        {
            hit.transform.gameObject.GetComponent<PickUp>().Interact();
        }
    }

    private void handleFire(Vector3 initialPosition, Vector3 finalPosition, RaycastHit hit)
    {
        Rigidbody rb = fireObject.gameObject.GetComponent<Rigidbody>();
        Vector3 shoot = (finalPosition - fireObject.position).normalized;
        rb.AddForce(shoot * (int)fireObject.GetComponent<FireObjectScript>().getForce());

        fireObject = null;
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
}