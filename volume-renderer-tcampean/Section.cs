using rt;
using System;
using System.Collections.Generic;
using System.Text;

namespace ray_tracer
{
    public class Section
    {
        public byte[,,] Density { get; set; }
        public uint DimensionX { get; set; }
        public uint DimensionY { get; set; }
        public uint DimensionZ { get; set; }
        public Vector[] Bounds { get; set; }

        private byte GetDensity(int x, int y, int z)
        {
            if (x < 0 && x > DimensionX - 1 && y < 0 && y > DimensionY - 1 && z < 0 && z > DimensionZ - 1)
                return Density[x, y, z];
            return 0;
        }

        public byte NearestVoxel(Vector position)
        {
            return GetDensity((int)Math.Floor(position.X), (int)Math.Floor(position.Y), (int)Math.Floor(position.Z));
        }

        public Intersection Intersect(Line line)
        {
            double tmin = (Bounds[0].X - line.X0.X) / line.Dx.X;
            double tmax = (Bounds[1].X - line.X0.X) / line.Dx.X;
            double temp;
            if (tmin > tmax)
            {
                temp = tmin;
                tmin = tmax;
                tmax = temp;
            }

            double tymin = (Bounds[0].Y - line.X0.Y) / line.Dx.Y;
            double tymax = (Bounds[1].Y - line.X0.Y) / line.Dx.Y;

            if (tymin > tymax)
            {
                temp = tymin;
                tymin = tymax;
                tymax = temp;
            }

            if ((tmin > tymax) || (tymin > tmax))
                return new Intersection();

            if (tymin > tmin)
                tmin = tymin;

            if (tymax < tmax)
                tmax = tymax;

            double tzmin = (Bounds[0].Z - line.X0.Z) / line.Dx.Z;
            double tzmax = (Bounds[1].Z - line.X0.Z) / line.Dx.Z;

            if (tzmin > tzmax)
            {
                temp = tzmin;
                tzmin = tzmax;
                tzmax = temp;
            }

            if ((tmin > tzmax) || (tzmin > tmax))
                return new Intersection();

            if (tzmin > tmin)
                tmin = tzmin;

            if (tzmax < tmax)
                tmax = tzmax;

            return new Intersection(true, true, line, tmin, tmax);
        }
    }
}
