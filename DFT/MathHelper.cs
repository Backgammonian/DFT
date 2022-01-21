using System;
using System.Collections.Generic;
using System.Numerics;
using OxyPlot;

namespace DFT
{
    public class MathHelper
    {
        public MathHelper()
        {
        }

        public List<double> LinSpace(double start, double end, double step)
        {
            var result = new List<double>();

            if (start > end)
            {
                var temp = start;
                start = end;
                end = temp;
            }

            for (var i = start; i < end; i += step)
            {
                result.Add(i);
            }

            return result;
        }

        public List<DataPoint> CalculateMagnitudes(List<Complex> waveTransformed, double samplingRate)
        {
            var result = new List<DataPoint>();
            var N = waveTransformed.Count;

            for (var i = 0; i <= N / 2; i++)
            {
                result.Add(new DataPoint(i * samplingRate / N, waveTransformed[i].Magnitude));
            }

            return result;
        }

        public List<DataPoint> CalculatePhases(List<Complex> waveTransformed, double samplingRate)
        {
            var result = new List<DataPoint>();
            var N = waveTransformed.Count;

            for (var i = 0; i <= N / 2; i++)
            {
                result.Add(new DataPoint(i * samplingRate / N, waveTransformed[i].Phase * 180.0 / Math.PI));
            }

            return result;
        }

        public List<DataPoint> GenerateDiscreteCounts(int discreteCounts, Func<double, double> sineFunction)
        {
            var xn = LinSpace(0, discreteCounts, 1);
            var output = new List<DataPoint>();

            for (var i = 0; i < xn.Count; i += 1)
            {
                output.Add(new DataPoint(xn[i], sineFunction(xn[i])));
            }

            return output;
        }

        public List<DataPoint> GenerateFrequencyResponses(List<DataPoint> magnitudes, Func<double, double> cardinaSineFunction)
        {
            var output = new List<DataPoint>();

            for (int i = 0; i < magnitudes.Count; i++)
            {
                output.Add(new DataPoint(magnitudes[i].X, cardinaSineFunction(magnitudes[i].X)));
            }

            return output;
        }
    }
}