using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeldObject : MonoBehaviour
{
    private Image heldObjectImage;
    // Start is called before the first frame update

    void Awake()
    {
        heldObjectImage = transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
        heldObjectImage.gameObject.SetActive(false);
    }

    private void SetHeldObjectImage(GameObject objectToPickUp)
    {
        heldObjectImage.gameObject.SetActive(true);
        SpriteRenderer sr = objectToPickUp.GetComponent<SpriteRenderer>();
        heldObjectImage.sprite = sr.sprite;
        heldObjectImage.color = sr.color;
    }

    private void ClearHeldObjectImage()
    {
        heldObjectImage.sprite = null;
        heldObjectImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerInteractor.onCarryObject += SetHeldObjectImage;
        PlayerInteractor.onPlaceObject += ClearHeldObjectImage;
    }

    private void OnDisable()
    {
        PlayerInteractor.onCarryObject -= SetHeldObjectImage;
        PlayerInteractor.onPlaceObject -= ClearHeldObjectImage;
    }
}
