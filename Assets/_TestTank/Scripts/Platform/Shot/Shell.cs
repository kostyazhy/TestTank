using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * The class controls the behavior of the barrel
 */

public class Shell : MonoBehaviour
{
    Rigidbody _rigidbody;

    void Start () {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // set the speed, place of appearance and launch the barrel
    public void Shoot(Transform spawnTransform, float speed)
    {
        _rigidbody.velocity = spawnTransform.forward * speed;
    }

    // setting the behavior when colliding with the ground
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Terrain") {
            StartCoroutine(SphereIndicator());
        }
    }

    // Creating a sphere on the spot and deleting objects
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
