using System;

namespace rt
{
    public class Sphere : Geometry
    {
        public Vector Center { get; set; }
        public double Radius { get; set; }

        public Sphere(Vector center, double radius, Material material, Color color) : base(material, color)
        {
            Center = center;
            Radius = radius;
        }
        
        public override Intersection GetIntersection(Line line, double minDist, double maxDist)
        {
            // ADD CODE HERE: Calculate the intersection between the given line and this sphere
            var posClosestCenter = (Center - line.X0) * line.Dx;
            var vectorToClosest = line.CoordinateToPosition(posClosestCenter);
            var distanceCenterToVector = (Center - vectorToClosest).Length();
            var distanceBetweenPoints = Math.Sqrt(Radius * Radius - distanceCenterToVector * distanceCenterToVector);
            if (posClosestCenter > minDist && posClosestCenter < maxDist && distanceCenterToVector <= Radius) {
                return new Intersection(true, true, this, line, posClosestCenter - distanceBetweenPoints);
            }
            return new Intersection();
        }
        public override Vector Normal(Vector v)
        {
            var n = v - Center;
            n.Normalize();
            return n;
        }
    }
}