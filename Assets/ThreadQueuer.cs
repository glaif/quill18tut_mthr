using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class ThreadQueuer : MonoBehaviour {

    List<Action> functionsToRunInMainThread;

    private void Start() {
        Debug.Log("Start() -- started.");
        functionsToRunInMainThread = new List<Action>();

        StartThreadedFunction(() => { SlowFunctionThatDoesAUnityThing(Vector3.zero, new float[4], new Color[100]); });
        Debug.Log("Start() -- done.");
    }

    private void Update() {
        while (functionsToRunInMainThread.Count > 0) {
            Action someFunc = functionsToRunInMainThread[0];
            functionsToRunInMainThread.RemoveAt(0);
            someFunc();
        }
    }

    public void StartThreadedFunction(Action someFunction) {
        Thread t = new Thread(new ThreadStart(someFunction));
        t.Start();
    }

    public void QueueMainThreadFunction(Action someFunction) {
        functionsToRunInMainThread.Add(someFunction);
    }

    void SlowFunctionThatDoesAUnityThing(Vector3 foo, float[] bar, Color[] baz ) {
        Thread.Sleep(2000);

        Action aFunction = () => {
            Debug.Log("The results of the child thread are being applied to a Unity GameObject safely");
            this.transform.position = new Vector3(1, 1, 1);
        };

        QueueMainThreadFunction(aFunction);
    }


}
