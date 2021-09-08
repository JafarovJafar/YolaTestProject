using System;

namespace YolaTestProject
{
    class MapPointsGapper
    {
        public MapPoint[] GapMapPoints(MapPoint[] points,int interval)
        {
            MapPoint[] finalMapPoints;

            if (interval >= points.Length)
            {
                finalMapPoints = new MapPoint[1];
            }
            else if (interval > points.Length / (double)2)
            {
                finalMapPoints = new MapPoint[2];
            }
            else
            {
                int pointCounts = (int)Math.Ceiling(points.Length / (double)interval);

                finalMapPoints = new MapPoint[pointCounts];
            }

            for (int i = 0; i < finalMapPoints.Length; i++)
            {
                finalMapPoints[i] = points[i * interval];
            }

            return finalMapPoints;
        }
    }
}