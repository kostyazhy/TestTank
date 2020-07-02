using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    Rigidbody _rigidbody;

    void Start () {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Shoot(Transform spawnTransform, float speed)
    {
        _rigidbody.velocity = spawnTransform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.name == "Terrain") {
            StartCoroutine(SphereIndicator());
        }
    }

    private IEnumerator SphereIndicator()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position;
        sphere.transform.localScale = new Vector3(5, 5, 5);
        yield return new WaitForSeconds(2.5f);
        Destroy(sphere); 
        Destroy(gameObject);
    }
}
