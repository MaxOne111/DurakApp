using UnityEngine;

public static class BezierCurve
{
    private static Vector3 GetPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 p01 = Vector3.Lerp(p0, p1, t);
        Vector3 p12 = Vector3.Lerp(p1, p2, t);
        Vector3 p23 = Vector3.Lerp(p2, p3, t);
        
        Vector3 p012 = Vector3.Lerp(p01, p12, t);
        Vector3 p123 = Vector3.Lerp(p12, p23, t);
        
        Vector3 p0123 = Vector3.Lerp(p012, p123, t);
        
        return p0123;
    }

    public static Vector3[] GetCurve(Transform p0, Transform p1, Transform p2, Transform p3, int count)
    {
        Vector3[] points = new Vector3[count * 2];
        
        for (int i = 0; i < count; i++)
        {
            float param = (float)i / count;

            Vector3 point = GetPoints(p0.position, p1.position, p2.position, p3.position, param);

            points[i] = point;
        }
        
        return points;
    }
    
    public static Vector3[] GetSleeveCurve(Transform p0, Transform p1, Transform p2, Transform p3, int count)
    {
        Vector3[] points = new Vector3[count * 2];

        float offset = 0;

        if (count % 2 == 0)
            offset = -1;
        else
            offset = -2;


        float factor = 0.5f;
        
        for (int i = 0; i < count * 2; i++)
        {
            float param = i / (count * 2 + offset);

            Vector3 point = GetPoints(p0.position, p1.position, p2.position, p3.position, param);

            points[i] = point;
        }
        
        Vector3[] sleevePoints = new Vector3[count];
        
        if (count == 1)// Test
        {
            sleevePoints = new Vector3[1];
            sleevePoints[0] = new Vector3((p1.position.x + p2.position.x) / 2, p1.position.y);

            return sleevePoints;
        }
        
        Debug.Log(points.Length);

        int j = 3;
        int halfSleeve = (count / 2);
        
        for (int i = 0; i < count; i++)
        {
            int startPoint = (halfSleeve  + i);

            sleevePoints[i] = points[startPoint];
        }
        
        return sleevePoints;
    }
}