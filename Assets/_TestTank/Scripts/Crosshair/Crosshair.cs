using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public float focusedSpread;
    public float speedSpread;

    public PartCrosshair[] parts;
    public MovePlatform platform;

    float _time;
    float _curSpread;

    private void OnEnable()
    {
        Shot.LocatedTargetEvent += ChangeTarget;
    }

    private void OnDisable()
    {
        Shot.LocatedTargetEvent -= ChangeTarget;
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

    public void CrosshairUpdate()
    {
        _time = Time.deltaTime * speedSpread;
        _curSpread = Mathf.Lerp(_curSpread, focusedSpread, _time);
        foreach(PartCrosshair part in parts) {
            part.trf.anchoredPosition = part.pos * _curSpread;
        }
    }

    void ChangeTarget(bool target)
    {
        Color crosshairColor;
        if (target) {
            crosshairColor = Color.white;
        } else {
            crosshairColor = Color.red;
        }

        foreach (PartCrosshair part in parts) {
            part.SetColorPart(crosshairColor);
        }
    }

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
