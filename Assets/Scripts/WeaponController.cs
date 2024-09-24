using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    #region Variables

    // Scriptable Object
    public Weapon weapon;

    // Components
    private SpriteRenderer spriteRenderer;

    #endregion

    #region MainMethods

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.sprite;
        StartCoroutine("DestroyObject");
    }

    #endregion

    #region ObjectHandler

    // Coroutine to destroy the object after 20 seconds it is instantiated
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

    #endregion

}
