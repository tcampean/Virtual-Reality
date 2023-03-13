using ray_tracer;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace rt
{
    class Sampler
    {
        private Section section;
        private SortedDictionary<int, Color> colorMap;

        public Sampler(Section section, SortedDictionary<int, Color> colorMap)
        {
            this.section = section;
            this.colorMap = colorMap;
        }

        public Section getSection()
        {
            return section;
        }

        public Color Sample(Line ray, Intersection intersection)
        {
            double totalDistance = 0;
            const double stepSize = 0.05;
            int numberOfSteps = (int)Math.Ceiling((intersection.Tmax - intersection.Tmin) / stepSize);

            Vector currentPosition = intersection.Position;
            byte currentDensity = section.NearestVoxel(currentPosition);
            Color finalColor = new Color();
            for (int i = 0; i < numberOfSteps; i++)
            {
                currentPosition = ray.CoordinateToPosition(intersection.Tmin + totalDistance);
                currentDensity = section.NearestVoxel(currentPosition);

                if (currentDensity >= 255)
                {
                    return colorMap[256];
                }

                foreach (KeyValuePair<int, Color> colorMapping in colorMap)
                {
                    if (currentDensity < colorMapping.Key)
                    {
                        double R1 = finalColor.Red * finalColor.Alpha + colorMapping.Value.Red * (1 - finalColor.Alpha);
                        double G1 = finalColor.Green * finalColor.Alpha + colorMapping.Value.Green * (1 - finalColor.Alpha);
                        double B1 = finalColor.Blue * finalColor.Alpha + colorMapping.Value.Blue * (1 - finalColor.Alpha);
                        double Alpha1 = finalColor.Alpha + (1 - finalColor.Alpha) * colorMapping.Value.Alpha;
                        finalColor = new Color(R1, G1, B1, Alpha1);
                        if (1 - finalColor.Alpha < 0.001)
                        {
                            return finalColor;
                        }
                        break;

                    }
                }
                totalDistance += stepSize;
            }
            return finalColor;
        }
    }
}
