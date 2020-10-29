using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : MonoBehaviour
{
    public bool createTween = true;
    public bool createSequence = true;
    public bool completeImmediately = true;
    public bool completeWCallbacks = false;
    public bool onUpdateCallFirst = true;
    public Transform target;

    bool testCalled;
    Tween t, s;

    void Start()
    {
        if (createTween) {
            Debug.Log("<color=#71cfe9>Creating TWEEN</color>");
            t = target.DOMoveX(2, 1).SetId("TWEEN")
                .OnComplete(() => Debug.Log("<color=#00ff00>Tween complete</color>"));
        }

        if (createSequence) {
            Debug.Log("<color=#71cfe9>Creating SEQUENCE</color>");
            s = DOTween.Sequence().SetId("SEQUENCE").SetAutoKill(false)
                .AppendCallback(() => Debug.Log("<color=#71cfe9>Sequence internal callback 0</color>"))
                .Append(target.DOMoveY(2, 1).OnComplete(() => Debug.Log("<color=#71cfe9>Nested tween complete</color>")))
                .AppendCallback(() => Debug.Log("<color=#71cfe9>Sequence internal callback 1</color>"))
                .OnRewind(() => Debug.Log("<color=#71cfe9>Sequence rewound</color>"))
                .OnPlay(() => Debug.Log("<color=#71cfe9>Sequence onPlay</color>"))
                .OnComplete(() => Debug.Log("<color=#00ff00>Sequence complete</color>"));
        }

        Debug.Log("<color=#71cfe9>Creating CALLER</color>");
        DOTween.Sequence().SetId("CALLER")
            .InsertCallback(2f, () => Test(false))
            .AppendInterval(3)
            .OnUpdate(() => {
                if (!testCalled && Time.time > (onUpdateCallFirst ? 0.5f : 3)) Test(true);
            })
            .OnComplete(() => Debug.Log("<color=#71cfe9>CALLER complete</color>"));
    }

    void Test(bool fromOnUpdate)
    {
        testCalled = true;

        if (fromOnUpdate) Debug.Log("<color=#71cfe9>TEST called from CALLER OnUpdate</color>");
        else Debug.Log("<color=#71cfe9>TEST called from CALLER callback</color>");
        if (t.IsActive()) t.Complete(completeWCallbacks);
        if (s.IsActive()) s.Complete(completeWCallbacks);
    }
}