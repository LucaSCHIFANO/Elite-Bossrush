using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockUI : MonoBehaviour
{
    GameObject currentTarget;
    [SerializeField] private GameObject targetCircle;

    private void Awake()
    {
        targetCircle.gameObject.SetActive(false);
    }

    void Update()
    {
        if(currentTarget == null)  return;

        targetCircle.transform.position = Camera.main.WorldToScreenPoint(currentTarget.transform.position);
    }

    public void SetTarget(GameObject target)
    {
        currentTarget = target;

        if(target == null) targetCircle.gameObject.SetActive(false);
        else targetCircle.gameObject.SetActive(true);
    }
}
