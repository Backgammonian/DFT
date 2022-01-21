using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using OxyPlot;

namespace DFT
{
    public class Converter
    {
        public List<DataPoint> ConvertComplexToDataPoint(List<Complex> input)
        {
            return input.Select(value => new DataPoint(value.Real, value.Imaginary)).ToList();
        }

        public List<Complex> ConvertDataPointToComplex(List<DataPoint> input)
        {
            return input.Select(value => new Complex(value.X, value.Y)).ToList();
        }

        public List<double> GetYs(List<DataPoint> input)
        {
            return input.Select(value => value.Y).ToList();
        }

        public List<double> GetXs(List<DataPoint> input)
        {
            return input.Select(value => value.X).ToList();
        }

        public List<double> GetImaginaries(List<Complex> input)
        {
            return input.Select(value => value.Imaginary).ToList();
        }

        public List<double> GetReals(List<Complex> input)
        {
            return input.Select(value => value.Real).ToList();
        }

        public List<DataPoint> GetPoints(List<double> xs, List<double> ys)
        {
            if (xs.Count != ys.Count)
            {
                throw new Exception(string.Format("Lengths of input lists do not match! len(xs) = {0}, len(ys) = {1}", xs.Count, ys.Count));
            }

            var result = new List<DataPoint>();

            for (int i = 0; i < xs.Count; i++)
            {
                result.Add(new DataPoint(xs[i], ys[i]));
            }

            return result;
        }
    }
}
