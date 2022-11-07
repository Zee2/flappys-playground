using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;

public class Perch : MonoBehaviour
{
    public bool Valid = true;

    HandsAggregatorSubsystem hands;

    void Start()
    {
        hands = XRSubsystemHelpers.GetFirstSubsystem<HandsAggregatorSubsystem>();
    }

    public void Update()
    {
        bool gotJoint = false;
        HandJointPose intermediateJoint;
        HandJointPose distalJoint;

        if ((hands.TryGetJoint(TrackedHandJoint.IndexIntermediate, XRNode.LeftHand, out intermediateJoint) &&
            hands.TryGetJoint(TrackedHandJoint.IndexDistal, XRNode.LeftHand, out distalJoint)) ||
            (hands.TryGetJoint(TrackedHandJoint.IndexIntermediate, XRNode.RightHand, out intermediateJoint) &&
            hands.TryGetJoint(TrackedHandJoint.IndexDistal, XRNode.RightHand, out distalJoint)))
        {
            Vector3 alongFinger = (intermediateJoint.Position - distalJoint.Position).normalized;

            if (Mathf.Abs(Vector3.Dot(alongFinger, Vector3.up)) < 0.3f)
            {
                Valid = true;
                transform.position = (intermediateJoint.Position + distalJoint.Position) / 2 + Vector3.up * 0.01f;
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.up, -alongFinger), Vector3.up);
                return;
            }
        }

        Valid = false;
    }
}
