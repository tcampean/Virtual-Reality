using ray_tracer;
using System;
using System.Runtime.InteropServices;

namespace rt
{
    class RayTracer
    {
        private Sampler sampler;
        private Geometry[] geometries;
        private Light[] lights;

        public RayTracer(Geometry[] geometries, Light[] lights)
        {
            this.geometries = geometries;
            this.lights = lights;
        }

        public RayTracer(Sampler sampler)
        {
            this.sampler = sampler;
        }

        private double ImageToViewPlane(int n, int imgSize, double viewPlaneSize)
        {
            var u = n * viewPlaneSize / imgSize;
            u -= viewPlaneSize / 2;
            return u;
        }

        private Intersection FindFirstIntersection(Line ray, double minDist, double maxDist)
        {
            var intersection = new Intersection();

            foreach (var geometry in geometries)
            {
                var intr = geometry.GetIntersection(ray, minDist, maxDist);

                if (!intr.Valid || !intr.Visible) continue;

                if (!intersection.Valid || !intersection.Visible)
                {
                    intersection = intr;
                }
                else if (intr.T < intersection.T)
                {
                    intersection = intr;
                }
            }

            return intersection;
        }

        private bool IsLit(Vector point, Light light)
        {
            // ADD CODE HERE: Detect whether the given point has a clear line of sight to the given light
            Line ray = new Line(light.Position, point);
            var intersection = FindFirstIntersection(ray, 0.0, (light.Position - point).Length());

            return !intersection.Valid;
        }

        public void Render(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var image = new Image(width, height);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    // ADD CODE HERE: Implement pixel color calculation
                    var viewParallel = camera.Up ^ camera.Direction;
                    var vectorX1 = camera.Position +
                       camera.Direction * camera.ViewPlaneDistance +
                       camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight) +
                       viewParallel * ImageToViewPlane(i, width, camera.ViewPlaneWidth);
                    var intersection = FindFirstIntersection(new Line(camera.Position, vectorX1), camera.FrontPlaneDistance, camera.BackPlaneDistance);
                    if (intersection.Geometry == null) {
                        image.SetPixel(i, j, new Color());
                    }
                    else {
                        Color objectColor = new Color();
                        Material objectMaterial = intersection.Geometry.Material;
                        foreach (var light in lights) {
                            var lightColor = objectMaterial.Ambient * light.Ambient;

                            if (IsLit(intersection.Position, light)) {
                                var vectorN = (intersection.Position - ((Sphere)intersection.Geometry).Center).Normalize();
                                var vectorT = (light.Position - intersection.Position).Normalize();

                                if (vectorN * vectorT > 0) {
                                    lightColor += objectMaterial.Diffuse * light.Diffuse * (vectorN * vectorT);
                                }
                                var vectorE = (camera.Position - intersection.Position).Normalize();
                                var vectorR = vectorN * (vectorN * vectorT) * 2 - vectorT;

                                if (vectorE * vectorR > 0) {
                                    lightColor += objectMaterial.Specular * light.Specular * Math.Pow((vectorE * vectorR), objectMaterial.Shininess);
                                }

                                lightColor *= light.Intensity;
                            }
                            objectColor += lightColor;
                        }
                        image.SetPixel(i, j, objectColor);
                    }
                }
            }

            image.Store(filename);
        }

        public void Render2(Camera camera, int width, int height, string filename)
        {
            var background = new Color();
            var image = new Image(width, height);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    Vector x0 = camera.Position;
                    Vector x1 = camera.Position +
                                camera.Direction * camera.ViewPlaneDistance +
                                camera.Up * ImageToViewPlane(j, height, camera.ViewPlaneHeight) +
                               (camera.Up ^ camera.Direction) * ImageToViewPlane(i, width, camera.ViewPlaneWidth);
                    Intersection intersection = sampler.getSection().Intersect(new Line(x0, x1));
                    if (!intersection.Valid || !intersection.Visible)
                    {
                        image.SetPixel(i, j, background);
                    }
                    else
                    {
                        Color finalColor = sampler.Sample(new Line(x0, x1), intersection);

                        image.SetPixel(i, j, finalColor);
                    }

                }
            }

            image.Store(filename);
        }
    }
}