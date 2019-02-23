using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    public int getDamage() { return damage; }
    
    public void hit()
    {

        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
       
        Destroy(damageDealer);
        
        
    }

}
