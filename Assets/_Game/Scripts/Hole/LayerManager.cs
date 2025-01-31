using UnityEngine;

public class LayerManager : MonoBehaviour
{
    [SerializeField] private string[] _layers = { "Default", "noColl" };

    private void OnTriggerEnter(Collider other)
    {
        // Nesnenin boyutunu kontrol etmeden layer de?i?tirme
        if (IsObjectSmallEnough(other))
        {
            ChangeLayer(other, 1); // noColl layer'?
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Nesne delikten �?karsa layer'? eski haline d�nd�r
        ChangeLayer(other, 0); // Default layer
    }

    private bool IsObjectSmallEnough(Collider other)
    {
        // Nesnenin boyutunu al ve deli?in boyutuyla kar??la?t?r
        float objectSize = GetObjectSize(other.gameObject);
        float holeSize = transform.localScale.x; // Deli?in boyutunu al

        return objectSize <= holeSize; // Sadece k���k nesneler layer de?i?ikli?i yapabilir
    }

    private void ChangeLayer(Collider other, int index)
    {
        other.gameObject.layer = LayerMask.NameToLayer(_layers[index]);
    }

    private float GetObjectSize(GameObject obj)
    {
        // Nesnenin en b�y�k boyutunu almak i�in Collider'?n? kullan?yoruz
        Bounds bounds = obj.GetComponent<Collider>().bounds;
        return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
    }
}
