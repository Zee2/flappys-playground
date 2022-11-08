using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UX;
using Microsoft.MixedReality.Toolkit.Input;
using TMPro;

public class MenuHelper : MonoBehaviour
{
    public StatefulInteractable HandMeshButton;

    public StatefulInteractable MenuButton;

    public TMP_Text speedLabel;

    public Slider Slider;

    public Birb Birb;

    private List<RiggedHandMeshVisualizer> handsCache = new List<RiggedHandMeshVisualizer>();

    private List<RiggedHandMeshVisualizer> handMeshes
    {
        get
        {
            if (handsCache.Count == 0)
            {
                handsCache.AddRange(FindObjectsOfType<RiggedHandMeshVisualizer>());
            }
            return handsCache;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Slider.SliderValue = Birb.Speed;

        HandMeshButton.OnClicked.AddListener(() =>
        {
            foreach (var handMesh in handMeshes)
            {
                if (HandMeshButton.IsToggled)
                {
                    // Make sure we'll show the hands even if the platform wouldn't otherwise.
                    handMesh.ShowHandsOnTransparentDisplays = true;
                }
                handMesh.enabled = HandMeshButton.IsToggled;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        Birb.Speed = Slider.SliderValue;
        Slider.SliderValue = Birb.Speed;
        speedLabel.text = Birb.Speed.ToString("F2");
    }
}
