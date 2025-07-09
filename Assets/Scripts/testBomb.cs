using UnityEngine;

public class testBomb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(explore), 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void explore()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 10f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(1f, explosionPos, 10f, 1f, ForceMode.Impulse);
                Debug.Log("ke");
            }
            Debug.DrawLine(explosionPos, hit.transform.position, Color.red, 1f);

        }
    }
}
