using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region Variables

    // Scriptable Object
    public Weapon weapon;

    // Components
    private SpriteRenderer spriteRenderer;

    // Access to the SpawnPoint
    public GameObject spawnPoint;
    public GameController gameController;

    #endregion

    #region MainMethods

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.sprite;/*
        // ARRUMAR PARA QUE O QUICK OUTLINE FUNCIONE ASSIM QUE O OBJETO(ARMA) APARECER
        gameObject.AddComponent<Outline>();
        gameObject.GetComponent<Outline>().enabled = true;
        gameObject.GetComponent<Outline>().OutlineColor = Color.black;
        gameObject.GetComponent<Outline>().OutlineWidth = 6.0f;*/
        //Destroy(gameObject, 20f);
        StartCoroutine("DestroyObject");
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) gameController._weaponsSpots.Add(spawnPoint);
    }

    #region ObjectHandler

    // Coroutine to destroy the object after 20 seconds it is instantiated
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
        gameController._weaponsSpots.Add(spawnPoint);
    }

    #endregion

}
