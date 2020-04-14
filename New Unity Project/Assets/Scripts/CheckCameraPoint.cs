using UnityEngine;

public class CheckCameraPoint: MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;


    public Transform newObject;
    private Transform _selection;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("pressed E");
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                var position = hit.point;

                Instantiate(newObject, position, Quaternion.identity);
            }
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
}