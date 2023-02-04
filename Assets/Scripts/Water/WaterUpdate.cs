using UnityEngine;

public class WaterUpdate : MonoBehaviour
{
    [SerializeField] private Transform lineStart;
    [SerializeField] private Transform lineEnd;
    
    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        _lineRenderer.SetPosition(0, lineStart.position);
        _lineRenderer.SetPosition(1, lineEnd.position);
    }
}
