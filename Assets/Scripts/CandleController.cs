using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour
{

    #region MainMethods

    void Awake()
    {
        StartCoroutine("DestroyObject");
    }

    #endregion

    #region ObjectHandler
    
    // Coroutine to destroy the object after 10 seconds it is instantiated
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    #endregion
}
