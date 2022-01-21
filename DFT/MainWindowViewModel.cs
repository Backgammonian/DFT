using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OxyPlot;

namespace DFT
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DiscreteFourierTransform _dft;
        private readonly MathHelper _math;
        private readonly Converter _converter;
        private readonly GraphHelper _graphHelper;
        private readonly double _dt;

        private readonly PlotModel _signal;
        private readonly PlotModel _phaseSpectre;
        private readonly PlotModel _magnitudeSpectre;
        private readonly PlotModel _cardinalSineGraph;
        private readonly PlotModel _restoredSineGraph;

        private double _frequency;
        private double _phase;
        private double _magnitude;
        private double _samplingRate;
        private int _discreteCounts;

        private RelayCommand _calculateCommand;

        public MainWindowViewModel()
        {
            _dft = new DiscreteFourierTransform();
            _math = new MathHelper();
            _converter = new Converter();
            _graphHelper = new GraphHelper();
            _dt = 0.001;

            _signal = new PlotModel();
            _phaseSpectre = new PlotModel();
            _magnitudeSpectre = new PlotModel();
            _cardinalSineGraph = new PlotModel();
            _restoredSineGraph = new PlotModel();

            Frequency = "200";
            Phase = "0";
            Magnitude = "1";
            SamplingRate = "1000";
            DiscreteCounts = "25";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public string Frequency
        {
            get { return _frequency.ToString(); }
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _frequency = result;
                    OnPropertyChanged(nameof(Frequency));
                }
            }
        }

        public string Phase
        {
            get { return _phase.ToString(); }
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _phase = result;
                    OnPropertyChanged(nameof(Phase));
                }
            }
        }

        public string Magnitude
        {
            get { return _magnitude.ToString(); }
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _magnitude = result;
                    OnPropertyChanged(nameof(Magnitude));
                }
            }
        }

        public string SamplingRate
        {
            get { return _samplingRate.ToString(); }
            set
            {
                if (double.TryParse(value, out double result) &&
                    result > 0)
                {
                    _samplingRate = result;
                    OnPropertyChanged(nameof(SamplingRate));
                }
            }
        }

        public string DiscreteCounts
        {
            get { return _discreteCounts.ToString(); }
            set
            {
                if (int.TryParse(value, out int result) &&
                    result > 0)
                {
                    _discreteCounts = result;
                    OnPropertyChanged(nameof(DiscreteCounts));
                }
            }
        }

        public PlotModel Signal => _signal;
        public PlotModel MagnitudeSpectre => _magnitudeSpectre;
        public PlotModel PhaseSpectre => _phaseSpectre;
        public PlotModel CardinalSineGraph => _cardinalSineGraph;
        public PlotModel RestoredSineGraph => _restoredSineGraph;

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(obj =>
                {
                    Main();
                });
            }
        }

        private void Main()
        {
            var discreteCounts = _math.GenerateDiscreteCounts(_discreteCounts, Sine);
            var functionValues = _converter.GetYs(discreteCounts);
            var waveTransformed = _dft.Transform(functionValues);
            var magnitudes = _math.CalculateMagnitudes(waveTransformed, _samplingRate);
            var phases = _math.CalculatePhases(waveTransformed, _samplingRate);
            var frequencyResponses = _math.GenerateFrequencyResponses(magnitudes, CardinalSine);
            var restoredWave = _dft.InverseTransform(waveTransformed);
            var restoredWaveYs = _converter.GetReals(restoredWave);
            var restoredWaveXs = _math.LinSpace(0, restoredWave.Count, 1);
            var restoredWavePoints = _converter.GetPoints(restoredWaveXs, restoredWaveYs);

            DrawSignal(discreteCounts);
            DrawPhases(phases);
            DrawMagnitudeSpectre(magnitudes);
            DrawCardinalSine(frequencyResponses);
            DrawRestoredWave(restoredWavePoints);
        }

        private double Sine(double x)
        {
            return _magnitude * Math.Sin((2.0 * Math.PI * _frequency * x / _samplingRate) + _phase);
        }

        private double CardinalSine(double x)
        {
            var t = _samplingRate / Convert.ToDouble(_discreteCounts);
            var value = x / t - _frequency / t;
            return value == 0.0 ? (_discreteCounts / 2.0) : Math.Abs(Math.Sin(Math.PI * value) / (Math.PI * value) * (_discreteCounts / 2.0));
        }

        private void DrawSignal(List<DataPoint> discretePoints)
        {
            var wave = _graphHelper.GetSineWave(_discreteCounts, _dt, Sine, OxyColors.Blue);
            var pins = _graphHelper.GetPins(discretePoints, OxyColors.Black);
            var axis = _graphHelper.GetHorizontalAxis(0, _discreteCounts, OxyColors.Black);

            _signal.Series.Clear();
            _signal.Series.Add(axis);
            _signal.Series.Add(wave);
            foreach (var value in pins)
            {
                _signal.Series.Add(value);
            }

            _signal.InvalidatePlot(true);
        }

        private void DrawPhases(List<DataPoint> phases)
        {
            var pins = _graphHelper.GetPins(phases, OxyColors.Black);
            var axis = _graphHelper.GetHorizontalAxis(phases[0].X, phases[^1].X, OxyColors.Black);

            _phaseSpectre.Series.Clear();
            _phaseSpectre.Series.Add(axis);
            foreach (var value in pins)
            {
                _phaseSpectre.Series.Add(value);
            }

            _phaseSpectre.InvalidatePlot(true);
        }

        private void DrawMagnitudeSpectre(List<DataPoint> magnitudes)
        {
            var pins = _graphHelper.GetPins(magnitudes, OxyColors.Black);
            var axis = _graphHelper.GetHorizontalAxis(magnitudes[0].X, magnitudes[^1].X, OxyColors.Black);

            _magnitudeSpectre.Series.Clear();
            _magnitudeSpectre.Series.Add(axis);
            foreach (var value in pins)
            {
                _magnitudeSpectre.Series.Add(value);
            }

            _magnitudeSpectre.InvalidatePlot(true);
        }

        private void DrawCardinalSine(List<DataPoint> frequencyResponses)
        {
            var wave = _graphHelper.GetCardinalSineWave(0, frequencyResponses[^1].X, _dt, CardinalSine, OxyColors.Blue);
            var axis = _graphHelper.GetHorizontalAxis(0, frequencyResponses[^1].X, OxyColors.Black);
            var pins = _graphHelper.GetPins(frequencyResponses, OxyColors.Black);

            _cardinalSineGraph.Series.Clear();
            _cardinalSineGraph.Series.Add(axis);
            _cardinalSineGraph.Series.Add(wave);
            foreach (var value in pins)
            {
                _cardinalSineGraph.Series.Add(value);
            }

            _cardinalSineGraph.InvalidatePlot(true);
        }

        private void DrawRestoredWave(List<DataPoint> points)
        {
            var signal = _graphHelper.GetRestoredSignal(points, OxyColors.Blue);
            var axis = _graphHelper.GetHorizontalAxis(points[0].X, points[^1].X, OxyColors.Black);

            _restoredSineGraph.Series.Clear();
            _restoredSineGraph.Series.Add(axis);
            _restoredSineGraph.Series.Add(signal);

            _restoredSineGraph.InvalidatePlot(true);
        }
    }
}
