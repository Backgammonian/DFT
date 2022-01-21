using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;

namespace DFT
{
    public class GraphHelper
    {
        public GraphHelper()
        {
        }

        public LineSeries GetHorizontalAxis(double start, double end, OxyColor color)
        {
            var horizontalAxis = new LineSeries();
            horizontalAxis.Color = color;
            horizontalAxis.Points.Add(new DataPoint(start, 0));
            horizontalAxis.Points.Add(new DataPoint(end, 0));

            return horizontalAxis;
        }

        public List<LineSeries> GetPins(List<DataPoint> points, OxyColor color)
        {
            var output = new List<LineSeries>();

            foreach (var value in points)
            {
                var line = new LineSeries();
                line.Color = color;
                line.Points.Add(new DataPoint(value.X, 0));
                line.Points.Add(new DataPoint(value.X, value.Y));

                output.Add(line);

                var point = new LineSeries();
                point.Color = color;
                point.MarkerType = MarkerType.Circle;
                point.Points.Add(new DataPoint(value.X, value.Y));

                output.Add(point);
            }

            return output;
        }

        public FunctionSeries GetSineWave(double length, double dt, Func<double, double> sineFunction, OxyColor color)
        {
            var output = new FunctionSeries(sineFunction, 0, length, dt);
            output.Color = color;

            return output;
        }

        public FunctionSeries GetCardinalSineWave(double start, double end, double dt, Func<double, double> cardinalSineFunction, OxyColor color)
        {
            var output = new FunctionSeries(cardinalSineFunction, start, end, dt);
            output.Color = color;

            return output;
        }

        public LineSeries GetRestoredSignal(List<DataPoint> points, OxyColor color)
        {
            var signal = new LineSeries();
            signal.Color = color;
            foreach (var point in points)
            {
                signal.Points.Add(new DataPoint(point.X, point.Y));
            }

            return signal;
        }
    }
}
