using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject expl; 
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {

            Instantiate(expl, transform.position, Quaternion.identity);
            GridBuilder.gm.GetComponent<GridBuilder>().Damage();
            Destroy(this.gameObject);

        }
    }
}
