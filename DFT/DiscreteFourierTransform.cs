using System;
using System.Collections.Generic;
using System.Numerics;

namespace DFT
{
    public class DiscreteFourierTransform
    {
        public DiscreteFourierTransform()
        {
        }

        public List<Complex> Transform(List<double> input)
        {
            var n = input.Count;
            var arg = -2.0 * Math.PI / n;
            var output = new List<Complex>();

            for (var k = 0; k < n; k++)
            {
                Complex sum = 0;
                for (var t = 0; t < n; t++)
                {
                    sum += input[t] * Complex.FromPolarCoordinates(1, arg * k * t);
                }

                output.Add(new Complex(sum.Real, sum.Imaginary));
            }

            return output;
        }

        public List<Complex> InverseTransform(List<Complex> input)
        {
            var n = input.Count;
            var arg = 2.0 * Math.PI / n;
            var output = new List<Complex>();

            for (int k = 0; k < n; k++)
            {
                Complex sum = 0;
                for (int t = 0; t < n; t++)
                {
                    sum += input[t] * Complex.FromPolarCoordinates(1, arg * k * t);
                }

                output.Add(new Complex(sum.Real, sum.Imaginary) / n);
            }

            return output;
        }
    }
}
