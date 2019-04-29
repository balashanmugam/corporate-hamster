using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hammerplay.Utils;

public class Entity : MonoBehaviour , IPoolable{



public EntityType type;

SpriteRenderer sprite;
    public void PoolInstantiate(Vector3 position, Quaternion rotation)
    { 
       
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.gameObject.SetActive(true);
    }

    public void PoolDestroy()
    {
        this.gameObject.SetActive(false);
    }

    public bool IsAlive()
    {   
        return gameObject.activeSelf;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
 /// <summary>
/// Awake is called when the script instance is being loaded.
/// </summary>
void Awake()
{
    sprite = GetComponent<SpriteRenderer>();
}

/// <summary>
/// This function is called when the object becomes enabled and active.
/// </summary>

 /// <summary>
/// Update is called every frame, if the MonoBehaviour is enabled.
/// </summary>
void Update()
{
    /* if(!sprite.isVisible && canDestroy){
      gameObject.SetActive(false);
    }
    if(sprite.isVisible && !canDestroy){
       canDestroy =true;
    }*/
}
void OnTriggerEnter2D(Collider2D other)
{
    if(other.gameObject.GetComponent<Detector>())
    {
            PoolDestroy();
    }
}
}

/// <summary>
/// Sent when another object enters a trigger collider attached to this
/// object (2D physics only).
/// </summary>
/// <param name="other">The other Collider2D involved in this collision.</param>


public enum EntityType{Salary, Vacation, Platform};