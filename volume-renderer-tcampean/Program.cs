using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Common;
using rt;
using ray_tracer;

namespace vr
{
    public class Program
    {

        public static void InitHeadConfig(ref Section section, ref String fileName, ref Vector middle, ref string location)
        {
            section.DimensionX = 181;
            section.DimensionY = 217;
            section.DimensionZ = 181;

            section.Density = new byte[section.DimensionX, section.DimensionY, section.DimensionZ];
            section.Bounds = new Vector[2];
            section.Bounds[0] = new Vector(0, 0, 0);
            section.Bounds[1] = new Vector(160, 210, 175);
            fileName = "../../../head-181x217x181.dat";
            middle = new Vector(80, 120, 100);
            location = "/head/";
        }

        public static void InitVertebraeConfig(ref Section section, ref String fileName, ref Vector middle, ref string location)
        {
            section.DimensionX = 47;
            section.DimensionY = 512;
            section.DimensionZ = 512;
            section.Density = new byte[section.DimensionX, section.DimensionY, section.DimensionZ];
            section.Bounds = new Vector[2];
            section.Bounds[0] = new Vector(0, 120, 150);
            section.Bounds[1] = new Vector(47, 250, 360);
            fileName = "../../../vertebra-47x512x512.dat";
            middle = new Vector(26, 180, 256);
            location = "/vertebrae/";
        }
        public static void Main(string[] args)
        {

            // Cleanup
            const string frames = "../../../frame";
            if (Directory.Exists(frames))
            {
                var d = new DirectoryInfo(frames);
                foreach (var file in d.EnumerateFiles("*.png"))
                {
                    file.Delete();
                }
            }
            Directory.CreateDirectory(frames);

            Section section = new Section();
            string fileName = "";
            string location = "";

            var middle = new Vector(0, 0, 0);

            //InitVertebraeConfig(ref section, ref fileName, ref middle, ref location);
            InitHeadConfig(ref section, ref fileName, ref middle, ref location);



            using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    for (int i = 0; i < section.DimensionX; i++)
                    {
                        for (int j = 0; j < section.DimensionY; j++)
                        {
                            for (int k = 0; k < section.DimensionZ; k++)
                            {
                                section.Density[i, j, k] = br.ReadByte();
                            }
                        }
                    }
                }
            }

            SortedDictionary<int, Color> colorMap = new SortedDictionary<int, Color>();


            colorMap.Add(20, new Color(0, 0, 0, 0));
            colorMap.Add(50, new Color(1, 0, 0, 0.01));
            colorMap.Add(75, new Color(1, 0, 0.8, 0.01));
            colorMap.Add(100, new Color(1, 0.8, 0.8, 0.01));
            colorMap.Add(150, new Color(1, 1, 0.2, 0.01));
            colorMap.Add(200, new Color(0, 1, 0.1, 0.01));
            colorMap.Add(250, new Color(1, 1, 1, 0.01));

            var sampler = new Sampler(section, colorMap);
            var rayTracer = new RayTracer(sampler);

            const int width = 800;
            const int height = 600;

            var up = new Vector(-Math.Sqrt(0.125), -Math.Sqrt(0.75), Math.Sqrt(0.125)).Normalize();
            var first = (middle ^ up).Normalize();
            const double dist = 200.0;
            const int n = 90;
            const double step = 360.0 / n;

            var tasks = new Task[n];
            for (var i = 0; i < n; i++)
            {
                var ind = new[] { i };
                tasks[i] = Task.Run(() =>
                {
                    var k = ind[0];
                    var a = (step * k) * Math.PI / 180.0;
                    var ca = Math.Cos(a);
                    var sa = Math.Sin(a);

                    var dir = first * ca + (up ^ first) * sa + up * (up * first) * (1.0 - ca);

                    var camera = new Camera(
                        middle + dir * dist,
                        dir * -0.5,
                        up,
                        65.0,
                        160.0,
                        120.0,
                        0.0,
                        1000.0
                    );

                    var filename = frames + location + $"{k + 1:000}" + ".png";
                    rayTracer.Render2(camera, width, height, filename);
                    Console.WriteLine($"Frame {k + 1}/{n} completed");
                });
            }

            Task.WaitAll(tasks);

        }
    }
}