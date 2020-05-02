using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierManager : MonoBehaviour
{
    public int detail = 30;
    public GameObject dominoPrefab;
    public List<Bezier> beziers=  new List<Bezier>();

    public bool showCurve = false;
    public int cubeSize = 5;

    private List<GameObject> tmpDominos = new List<GameObject>();

    public void ClearTmp()
    {
        for (int i = 0; i < tmpDominos.Count; i++)
            DestroyImmediate(tmpDominos[i]);
        tmpDominos.Clear();
    }

    public void GenerateDominoByLength()
    {
        if (!dominoPrefab) { Debug.Log("No domino prefab"); return; }
        List<float> length = getArclength();
        float height = dominoPrefab.transform.localScale.y * 2 / 3, t, dist = 0;
        int indexNow = 0;
        List<Vector3> cps = getCpoints(indexNow);
        tmpDominos.Clear();
        while (indexNow < length.Count) 
        {
            t = dist / length[indexNow];
            Vector3 sp = Sample(cps, t);
            Vector3 tan = Tangent(cps, t);
            GameObject tmpDo = GameObject.Instantiate(dominoPrefab, sp, Quaternion.identity);
            tmpDo.transform.right = tan;
            tmpDominos.Add(tmpDo);
            dist += height;
            if (dist > length[indexNow])
            {
                dist -= length[indexNow];
                indexNow++;
                if (indexNow >= length.Count) break;
                cps = getCpoints(indexNow);
            }
        }
    }

    public Vector3 Sample(List<Vector3> cps, float t)
    {
        if (cps.Count != 4) { Debug.Log("cps.Count != 4"); return Vector3.zero; }
        float u = 1 - t;
        return cps[0] * u * u * u + 3 * cps[1] * u * u * t + 3 * cps[2] * u * t * t + cps[3] * t * t * t;
    }

    public Vector3 Tangent(List<Vector3> cps, float t)
    {
        if (cps.Count != 4) { Debug.Log("cps.Count != 4"); return Vector3.zero; }
        float u = 1 - t;
        Vector3 dp0 = 3 * (cps[1] - cps[0]);
        Vector3 dp1 = 3 * (cps[2] - cps[1]);
        Vector3 dp2 = 3 * (cps[3] - cps[2]);
        Vector3 n = dp0 * u * u + 2 * dp1 * u * t + dp2 * t * t;
        return n.normalized;
    }

    public List<Vector3> getCpoints(int index)
    {
        List<Vector3> tmpCps = new List<Vector3>();
        if (index >= beziers.Count - 1) { Debug.Log("getCpoints index out of range"); return tmpCps; }
        tmpCps.Add(beziers[index].point.position);
        tmpCps.Add(beziers[index].handle1.position);
        tmpCps.Add(beziers[index + 1].handle0.position);
        tmpCps.Add(beziers[index + 1].point.position);
        return tmpCps;
    }

    public List<float> getArclength()
    {
        List<float> length = new List<float>();
        for (int i = 0; i < beziers.Count - 1; i++) 
        {
            List<Vector3> cps = getCpoints(i);
            length.Add(getSegLength(cps));
        }
        return length;
    }

    private void OnDrawGizmos()
    {
        if (!showCurve) return;
        if (beziers.Count < 2) return;
        Vector3 size = Vector3.one * cubeSize;
        for (int i = 0; i < beziers.Count; i++)
        {
            if (i != 0) Gizmos.DrawCube(beziers[i].handle0.position, size);
            Gizmos.DrawCube(beziers[i].point.position, size);
            if (i != beziers.Count - 1) Gizmos.DrawCube(beziers[i].handle1.position, size);
        }
        for (int i = 0; i < beziers.Count - 1; i++)
        {
            List<Vector3> cps = getCpoints(i);
            float t = 0;
            Vector3 pre = Sample(cps, t);
            for (int j = 1; j < detail; j++) 
            {
                t = (float)j / detail;
                Vector3 now = Sample(cps, t);
                Gizmos.DrawLine(pre, now);
                pre = now;
            }
            Gizmos.DrawLine(pre, cps[3]);
        }
    }

    float getSegLength(List<Vector3> cps)
    {
        float length = 0;
        if (cps.Count != 4) { Debug.Log("cps.Count != 4"); return 0; }
        float t = 0;
        Vector3 pre = Sample(cps, t);
        for (int i = 1; i < detail; i++) 
        {
            t = (float)i / detail;
            Vector3 now = Sample(cps, t);
            length += (now - pre).magnitude;
            pre = now;
        }
        return length;
    }
}
