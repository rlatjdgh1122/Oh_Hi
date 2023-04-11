using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject crackObj;
    private Vector3 dir;
    void Update()
    {
        dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Horizontal"));
        dir.Normalize();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(crackObj, this.transform.position, Quaternion.identity);
            Rigidbody[] rigid = obj.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigid)
            {
                rb.AddForce(dir * 3 + Vector3.up * 2, ForceMode.Impulse);
            }
            this.gameObject.SetActive(false);
        }
    }
}
