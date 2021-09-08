namespace YolaTestProject
{
    struct MapPoint
    {
        public double X => _x;
        public double Y => _y;

        private double _x;
        private double _y;

        public MapPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }
    }
}