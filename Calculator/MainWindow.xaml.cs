using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.FormattableString;

namespace WpfHelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _result = 0.0;
        private double _lastUserInput = 0.0;
        private bool _wasOperationRequested;
        private bool _wasContentUpdatedByUser;
        private Func<double, double, double> _operator;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberBtnClick(object sender, RoutedEventArgs e)
        {
            var btnContent = (sender as Button).Content;
            resultLabel.Content = resultLabel.Content.ToString() == "0" || _wasOperationRequested ? $"{btnContent}" : $"{resultLabel.Content}{btnContent}";
            try
            {
                _lastUserInput = ParseLabelContentToDouble();
                _wasContentUpdatedByUser = true;
            }
            catch (FormatException)
            {
            }

            _wasOperationRequested = false;
        }

        private void ClearBtnClick(object sender, RoutedEventArgs e)
        {
            resultLabel.Content = "0";
            _operator = null;
            _result = 0.0;
        }


        private void AddBtnClick(object sender, RoutedEventArgs arg) => HandleOperationSelect((a, b) => a + b);

        private void SubtractBtnClick(object sender, RoutedEventArgs args) => HandleOperationSelect((a, b) => a - b);

        private void MultiplyBtnClick(object sender, RoutedEventArgs args) => HandleOperationSelect((a, b) => a * b);

        private void DivideBtnClick(object sender, RoutedEventArgs args) => HandleOperationSelect((a, b) =>
        {
            if (b == 0)
            {
                MessageBox.Show("Division by 0 not supported", "Wrong operation", MessageBoxButton.OK, MessageBoxImage.Error);

                return 0;
            }

            return a / b;
        });

        private void EqualsBtnClick(object sender, RoutedEventArgs args)
        {
            CalculateResult();
        }

        private void NegativeBtnClick(object sender, RoutedEventArgs args)
        {
            if (!_wasContentUpdatedByUser)
            {
                return; 
            }

            _lastUserInput *= -1;
            resultLabel.Content = _lastUserInput;
        }

        private void DotBtnClick(object sender, RoutedEventArgs args)
        {
            if (resultLabel.Content.ToString().Contains("."))
            {
                return;
            }

            resultLabel.Content = $"{resultLabel.Content}.";
        }

        private void PercentBtnClick(object sender, RoutedEventArgs args)
        {
            if (!_wasContentUpdatedByUser)
            {
                return;
            }

            _lastUserInput /= 100;
            resultLabel.Content = _lastUserInput;
        }

        private void CalculateResult()
        {
            _wasContentUpdatedByUser = false;
            if (_operator is null)
            {
                _result = _lastUserInput;

                return;
            }

            _result = _operator(_result, _lastUserInput);
            resultLabel.Content = Invariant($"{_result:G5}");
        }

        private double ParseLabelContentToDouble() => double.Parse(resultLabel.Content.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);

        private void HandleOperationSelect(Func<double, double, double> calcOperator)
        {
            if (_wasContentUpdatedByUser)
            {
                CalculateResult();
            }

            _wasOperationRequested = true;
            _operator = calcOperator;
        }

    }
}
