using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * The class controls the behavior of the scope
 */
public class Crosshair : MonoBehaviour
{
    public MovePlatform platform;
    public float focusedSpread;
    public float speedSpread;

    public PartCrosshair[] parts;

    float _time;
    float _curSpread;

    private void OnEnable()
    {
        BarrelControl.LocatedTargetEvent += ChangeTarget;
    }

    private void OnDisable()
    {
        BarrelControl.LocatedTargetEvent -= ChangeTarget;
    }

    private void Start()
    {
        foreach (PartCrosshair part in parts) {
            part.SetColorPart(Color.white);
        }
    }

    void Update () {
        CrosshairUpdate();
    }

    // Focuses the scope
    public void CrosshairUpdate()
    {
        _time = Time.deltaTime * speedSpread;
        _curSpread = Mathf.Lerp(_curSpread, focusedSpread, _time);
        foreach(PartCrosshair part in parts) {
            part.trf.anchoredPosition = part.pos * _curSpread;
        }
    }
    // changing the color of the sight
    void ChangeTarget(bool goodTarget)
    {
        Color crosshairColor;
        if (goodTarget) {
            crosshairColor = Color.white;
        } else {
            crosshairColor = Color.red;
        }

        foreach (PartCrosshair part in parts) {
            part.SetColorPart(crosshairColor);
        }
    }

    // parts of the scope are stored
    [System.Serializable]
    public class PartCrosshair
    {
        public RectTransform trf;
        public Vector2 pos;
        private Image img;

        public void SetColorPart(Color _color)
        {
            img = trf.gameObject.GetComponent<Image>();
            img.color = _color;
        }
    }
}
