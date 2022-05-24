using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaHandler : MonoBehaviour {
    private List<Transform> cubesLR = new();
    private List<Transform> cubesUD = new();

    private GameObject exitCube;

    bool _processing = false;

    [SerializeField]
    private UnityEvent exitFound = new();

    void Start() {
        exitCube = GameObject.FindGameObjectWithTag("ArenaExit");
        if (exitCube == null) {
            Debug.LogError("Нет выходного куба на карте!");
            Destroy(this);
        }

        foreach (var child in GetComponentsInChildren<Transform>()) {
            if (child.CompareTag("ArenaBlockLR")) {
                cubesLR.Add(child);
            } else if (child.CompareTag("ArenaBlockUD")) {
                cubesUD.Add(child);
            } else if (child.CompareTag("ArenaPlayerCube")) {
                cubesLR.Add(child);
                cubesUD.Add(child);
            }
        }
    }

    void Update() {
        if (_processing) return;

        object[] parms = new object[2] { null, null };

        var dir = Vector3.zero;
        if (SwipeDetection.Direction == Swipes.Left || Input.GetKeyDown(KeyCode.LeftArrow)) {
            dir = Vector3.left;
            parms[1] = cubesLR;
        } else if (SwipeDetection.Direction == Swipes.Right || Input.GetKeyDown(KeyCode.RightArrow)) {
            dir = Vector3.right;
            parms[1] = cubesLR;
        } else if (SwipeDetection.Direction == Swipes.Up || Input.GetKeyDown(KeyCode.UpArrow)) {
            dir = Vector3.forward;
            parms[1] = cubesUD;
        } else if (SwipeDetection.Direction == Swipes.Down || Input.GetKeyDown(KeyCode.DownArrow)) {
            dir = Vector3.back;
            parms[1] = cubesUD;
        }
        if (dir != Vector3.zero) {
            _processing = true;
            parms[0] = dir;
            StartCoroutine(nameof(ProcessUserInput), parms);
        }
    }

    bool CheckNearExit() {
        var castPos = exitCube.transform.position;
        RaycastHit hitInfo;
        if (Physics.Raycast(castPos, Vector3.left, out hitInfo, 1f) && hitInfo.transform.CompareTag("ArenaPlayerCube")) {
            return true;
        }
        if (Physics.Raycast(castPos, Vector3.right, out hitInfo, 1f) && hitInfo.transform.CompareTag("ArenaPlayerCube")) {
            return true;
        }
        if (Physics.Raycast(castPos, Vector3.forward, out hitInfo, 1f) && hitInfo.transform.CompareTag("ArenaPlayerCube")) {
            return true;
        }
        if (Physics.Raycast(castPos, Vector3.back, out hitInfo, 1f) && hitInfo.transform.CompareTag("ArenaPlayerCube")) {
            return true;
        }
        return false;
    }

    Vector3 GetCastPosition(BoxCollider col, Vector3 dir) {
        var centerPos = col.bounds.center;
        var size = col.bounds.size;
        float delta;
        if (dir.x == 0) {
            delta = size.z / 2 - size.y / 2;
        } else {
            delta = size.x / 2 - size.y / 2;
        }

        return centerPos + dir * delta;
    }

    private IEnumerator ProcessUserInput(object[] parms) {
        var dir = (Vector3)parms[0];
        var objects = ((List<Transform>)parms[1]).ConvertAll((obj) => {
            return Tuple.Create(obj, obj.GetComponent<BoxCollider>());
        });

        bool movedAny;
        do {
            movedAny = false;
            foreach (var tup in objects) {
                var castPos = GetCastPosition(tup.Item2, dir);
                if (!Physics.Raycast(castPos, dir, 1f)) {
                    tup.Item1.Translate(dir);
                    movedAny = true;
                }
                yield return new WaitForFixedUpdate();
            }
        } while (movedAny);

        if (CheckNearExit()) {
            exitFound.Invoke();
        }

        yield return null;
        _processing = false;
    }
}
