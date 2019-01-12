using UnityEngine;

public static class Math
{
    public static float TriangleArea(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        return 0.5f * Vector2.Distance(pointA, pointB) * Vector2.Distance(pointA, pointC) * Mathf.Sin(Mathf.Deg2Rad * Vector2.Angle(pointB - pointA, pointC - pointA));
    }

    // Using the 2:1 ratio
    public static Vector2 TriangeCenterOfMass(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        Vector2 ab = pointB - pointA;
        Vector2 mid = pointA + ab / 2;
        Vector2 midc = pointC - mid;
        return mid + midc * (1f / 3f);
    }

    public static bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, ref Vector2 intersection)
    {
        float Ax, Bx, Cx, Ay, By, Cy, d, e, f, num/*,offset*/;
        float x1lo, x1hi, y1lo, y1hi;

        Ax = p2.x - p1.x;
        Bx = p3.x - p4.x;

        // X bound box test/
        if (Ax < 0)
        {
            x1lo = p2.x; x1hi = p1.x;
        }
        else
        {
            x1hi = p2.x; x1lo = p1.x;
        }

        if (Bx > 0)
        {
            if (x1hi < p4.x || p3.x < x1lo) return false;
        }
        else
        {
            if (x1hi < p3.x || p4.x < x1lo) return false;
        }

        Ay = p2.y - p1.y;
        By = p3.y - p4.y;

        // Y bound box test//
        if (Ay < 0)
        {
            y1lo = p2.y; y1hi = p1.y;
        }
        else
        {
            y1hi = p2.y; y1lo = p1.y;
        }

        if (By > 0)
        {
            if (y1hi < p4.y || p3.y < y1lo) return false;
        }
        else
        {
            if (y1hi < p3.y || p4.y < y1lo) return false;
        }

        Cx = p1.x - p3.x;
        Cy = p1.y - p3.y;
        d = By * Cx - Bx * Cy;  // alpha numerator//
        f = Ay * Bx - Ax * By;  // both denominator//

        // alpha tests//
        if (f > 0)
        {
            if (d < 0 || d > f) return false;
        }
        else
        {
            if (d > 0 || d < f) return false;
        }

        e = Ax * Cy - Ay * Cx;  // beta numerator//

        // beta tests //
        if (f > 0)
        {
            if (e < 0 || e > f) return false;
        }
        else
        {
            if (e > 0 || e < f) return false;
        }

        // check if they are parallel
        if (f == 0) return false;

        // compute intersection coordinates //
        num = d * Ax; // numerator //

        //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;   // round direction //
        //    intersection.x = p1.x + (num+offset) / f;
        intersection.x = p1.x + num / f;
        num = d * Ay;

        //    offset = same_sign(num,f) ? f*0.5f : -f*0.5f;
        //    intersection.y = p1.y + (num+offset) / f;
        intersection.y = p1.y + num / f;
        return true;
    }


    public static Vector2 QuadrilateralCenterOfMass(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
    {
        Vector2 a = TriangeCenterOfMass(pointA, pointB, pointD);
        Vector2 b = TriangeCenterOfMass(pointB, pointC, pointA);
        Vector2 c = TriangeCenterOfMass(pointC, pointD, pointB);
        Vector2 d = TriangeCenterOfMass(pointD, pointA, pointC);

        Vector2 intersection = Vector2.zero;
        LineIntersection(a, c, b, d, ref intersection);
        return intersection;
    }

    public static Vector2 PentagonCenterOfMass(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, Vector2 pointE)
    {
        Vector2 s0 = QuadrilateralCenterOfMass(pointA, pointB, pointC, pointE);
        Vector2 t0 = TriangeCenterOfMass(pointC, pointD, pointE);

        Vector2 s1 = QuadrilateralCenterOfMass(pointA, pointB, pointC, pointD);
        Vector2 t1 = TriangeCenterOfMass(pointA, pointD, pointE);

        Vector2 intersection = Vector2.zero;
        LineIntersection(s0, t0, s1, t1, ref intersection);
        return intersection;
    }
}
