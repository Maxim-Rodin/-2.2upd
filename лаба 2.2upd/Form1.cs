using System.ComponentModel;

namespace лаба_2._2upd
{
    public partial class Form1 : Form
    {
        private BackgroundWorker backgroundWorker;
        private bool isPause = false;
        private ManualResetEvent pauseEvent = new ManualResetEvent(true);
        private string curretMethod = "";

        public Form1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += backgroundWorker1_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker1_ProgressChanged;

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            comboBox1.SelectedIndex = 0;
            labelSstatus.Text = "Готово к вычислению";
            labelResult.Text = "Результат появится здесь";
            textBoxAccuracy.Text = "0,0001"; 
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            CalculationParams parameters = (CalculationParams)e.Argument;

            string method = parameters.Method;
            double accuracy = parameters.Accuracy;

            ICalculate calculator = null;
            double result = 0;
            int iterations = 0;

           
            switch (method)
            {
                case "Ряд Лейбница":
                    calculator = new GotfridaLaibnitsa();
                    break;
                case "Ряд Валлиса":
                    calculator = new DjonVallis();
                    break;
                case "Ряд Виета":
                    calculator = new FraunsViete();
                    break;
            }

            if (calculator != null)
            {
                
                ProgressReportingCalculator adapter = new ProgressReportingCalculator(
                    calculator,
                    worker,
                    accuracy,
                    pauseEvent);

               
                result = adapter.CalculateWithProgress();
                iterations = adapter.Iterations;
            }

            
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            
            e.Result = new CalculationResult
            {
                PiValue = result,
                Method = method,
                Iterations = iterations,
                Accuracy = accuracy
            };
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            progressBar1.Value = e.ProgressPercentage;

            if (e.UserState != null)
            {
                string status = e.UserState.ToString();

                if (status.Contains("|"))
                {
                    
                    string[] parts = status.Split('|');
                    if (parts.Length >= 2)
                    {
                        labelSstatus.Text = parts[0]; 
                        labelResult.Text = parts[1]; 
                    }
                }
                else
                {
                    labelSstatus.Text = status;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                labelSstatus.Text = "Вычисление отменено";
                labelResult.Text = "Отменено пользователем";
            }
            else if (e.Error != null)
            {
                labelSstatus.Text = "Ошибка при вычислении";
                labelResult.Text = $"Ошибка: {e.Error.Message}";
            }
            else
            {
                CalculationResult result = (CalculationResult)e.Result;

                labelSstatus.Text = $"Вычисление завершено! Итераций: {result.Iterations}";
                labelResult.Text = $"π ≈ {result.PiValue:F15}\n" +
                                 $"Метод: {result.Method}\n" +
                                 $"Заданная точность: {result.Accuracy:E2}\n" +
                                 $"Фактическая погрешность: {Math.Abs(Math.PI - result.PiValue):E2}";

                
                MessageBox.Show(
                    $"Вычисление завершено!\n\n" +
                    $"Результат: {result.PiValue:F15}\n" +
                    $"Итераций: {result.Iterations}\n" +
                    $"Фактическая погрешность: {Math.Abs(Math.PI - result.PiValue):E2}",
                    "Готово",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

           
            Startbtn.Enabled = true;
            Pausebtn.Enabled = false;
            Resetbtn.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (!backgroundWorker.IsBusy)
            {
                Resetbtn_Click(sender, e);
            }
        }

        private void Startbtn_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                
                if (TryParseAccuracy(out double accuracy))
                {
                    curretMethod = comboBox1.SelectedItem.ToString();

                    
                    isPause = false;
                    pauseEvent.Set();

                    
                    progressBar1.Value = 0;
                    labelSstatus.Text = "Вычисление запущено...";
                    labelResult.Text = "Вычисляется...";

                  
                    backgroundWorker.RunWorkerAsync(new CalculationParams
                    {
                        Method = curretMethod,
                        Accuracy = accuracy
                    });

                    
                    Startbtn.Enabled = false;
                    Pausebtn.Enabled = true;
                    Resetbtn.Enabled = false;
                }
                else
                {
                    MessageBox.Show(
                        "Пожалуйста, введите корректное значение точности.\n" +
                        "Например: 0,001 или 1E-3",
                        "Ошибка ввода",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    textBoxAccuracy.Focus();
                    textBoxAccuracy.SelectAll();
                }
            }
        }

        private bool TryParseAccuracy(out double accuracy)
        {
            accuracy = 0;
            string input = textBoxAccuracy.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            
            input = input.Replace('.', ',');

            if (double.TryParse(input, out accuracy))
            {
                
                return accuracy > 0 && accuracy <= 1;
            }

            if (input.Contains("E") || input.Contains("e"))
            {
                input = input.Replace('e', 'E');
                if (double.TryParse(input, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out accuracy))
                {
                    return accuracy > 0 && accuracy <= 1;
                }
            }

            return false;
        }

        private void Pausebtn_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                if (!isPause)
                {
                   
                    isPause = true;
                    pauseEvent.Reset();
                    Pausebtn.Text = "Продолжить";
                    labelSstatus.Text = "Пауза...";
                }
                else
                {
                
                    isPause = false;
                    pauseEvent.Set();
                    Pausebtn.Text = "Пауза";
                    labelSstatus.Text = "Вычисление продолжено...";
                }
            }
        }

        private void Resetbtn_Click(object sender, EventArgs e)
        {
           
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }

            progressBar1.Value = 0;
            labelSstatus.Text = "Готово к вычислению";
            labelResult.Text = "Результат появится здесь";
            Pausebtn.Text = "Пауза";
            isPause = false;
            pauseEvent.Set();

            Startbtn.Enabled = true;
            Pausebtn.Enabled = false;
            Resetbtn.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBoxAccuracy_TextChanged(object sender, EventArgs e)
        {
           
        }

      
        private class CalculationParams
        {
            public string Method { get; set; }
            public double Accuracy { get; set; }
        }

        private class CalculationResult
        {
            public double PiValue { get; set; }
            public string Method { get; set; }
            public int Iterations { get; set; }
            public double Accuracy { get; set; }
        }

        
        private class ProgressReportingCalculator
        {
            private readonly ICalculate _calculator;
            private readonly BackgroundWorker _worker;
            private readonly double _accuracy;
            private readonly ManualResetEvent _pauseEvent;
            private int _iterations = 0;

            public int Iterations => _iterations;

            public ProgressReportingCalculator(ICalculate calculator, BackgroundWorker worker,
                                             double accuracy, ManualResetEvent pauseEvent)
            {
                _calculator = calculator;
                _worker = worker;
                _accuracy = accuracy;
                _pauseEvent = pauseEvent;
            }

            public double CalculateWithProgress()
            {
                if (_calculator is GotfridaLaibnitsa)
                {
                    return CalculateLeibnizWithProgress();
                }
                else if (_calculator is DjonVallis)
                {
                    return CalculateWallisWithProgress();
                }
                else if (_calculator is FraunsViete)
                {
                    return CalculateVieteWithProgress();
                }

                return _calculator.Calculate(_accuracy);
            }

            private double CalculateLeibnizWithProgress()
            {
                double sum = 0;
                double piCurr = 4 * sum;
                double piPrev = 0;
                _iterations = 0;

                while (Math.Abs(piCurr - piPrev) > _accuracy && !_worker.CancellationPending)
                {
                   
                    _pauseEvent.WaitOne();

                    piPrev = piCurr;
                    double term = Math.Pow(-1, _iterations) / (2.0 * _iterations + 1);
                    sum += term;
                    piCurr = 4 * sum;
                    _iterations++;

                  
                    ReportProgress(piCurr);
                }

                return piCurr;
            }

            private double CalculateWallisWithProgress()
            {
                double product = 1;
                double piCurr = 2 * product;
                double piPrev = 0;
                _iterations = 0;

                while (Math.Abs(piCurr - piPrev) > _accuracy && !_worker.CancellationPending)
                {
                    _pauseEvent.WaitOne();

                    piPrev = piCurr;
                    _iterations++;

                    double fraction1 = (2.0 * _iterations) / (2.0 * _iterations - 1);
                    double fraction2 = (2.0 * _iterations) / (2.0 * _iterations + 1);

                    product *= fraction1 * fraction2;
                    piCurr = 2 * product;

                    ReportProgress(piCurr);
                }

                return piCurr;
            }

            private double CalculateVieteWithProgress()
            {
                double a = Math.Sqrt(2);
                double product = a / 2;
                double piCurr = 2 / product;
                double piPrev = 0;
                _iterations = 0;

                while (Math.Abs(piCurr - piPrev) > _accuracy && !_worker.CancellationPending)
                {
                    _pauseEvent.WaitOne();

                    piPrev = piCurr;
                    a = Math.Sqrt(2 + a);
                    product *= (a / 2);
                    piCurr = 2 / product;
                    _iterations++;

                    ReportProgress(piCurr);
                }

                return piCurr;
            }

            private void ReportProgress(double currentPi)
            {
                
                if (_iterations % 100 == 0 || Math.Abs(currentPi - Math.PI) <= _accuracy)
                {
                   
                    double currentError = Math.Abs(currentPi - Math.PI);
                    int progress = CalculateProgressPercentage(currentError);

                    string status = $"Итерация: {_iterations} | Текущее π: {currentPi:F10}";
                    _worker.ReportProgress(progress, status);
                }
            }

            private int CalculateProgressPercentage(double currentError)
            {
               
                double initialError = 3.14;

                
                double logCurrent = Math.Log10(currentError);
                double logTarget = Math.Log10(_accuracy);
                double logInitial = Math.Log10(initialError);

               
                double progress = (logInitial - logCurrent) / (logInitial - logTarget) * 100;

                return (int)Math.Max(0, Math.Min(100, progress));
            }
        }
    }

    
    public interface ICalculate
    {
        string Name { get; set; }
        double Calculate(double accuracy);
    }

    public class FraunsViete : ICalculate
    {
        public string Name { get; set; }

        public FraunsViete()
        {
            Name = "Ряд Виета";
        }

        public FraunsViete(string Name)
        {
            this.Name = Name;
        }

        public double Calculate(double accuracy)
        {
            double a = Math.Sqrt(2);
            double product = a / 2;
            double piCurr = 2 / product;
            double piPrev = 0;

            while (Math.Abs(piCurr - piPrev) > accuracy)
            {
                piPrev = piCurr;
                a = Math.Sqrt(2 + a);
                product *= (a / 2);
                piCurr = 2 / product;
            }
            return piCurr;
        }
    }

    public class DjonVallis : ICalculate
    {
        public string Name { get; set; }

        public DjonVallis()
        {
            Name = "Ряд Валлиса";
        }

        public double Calculate(double accuracy)
        {
            double product = 1;
            double piCurr = 2 * product;
            double piPrev = 0;
            int n = 1;

            while (Math.Abs(piCurr - piPrev) > accuracy)
            {
                piPrev = piCurr;
                double fraction1 = (2.0 * n) / (2.0 * n - 1);
                double fraction2 = (2.0 * n) / (2.0 * n + 1);

                product *= fraction1 * fraction2;
                piCurr = 2 * product;
                n++;
            }
            return piCurr;
        }
    }

    public class GotfridaLaibnitsa : ICalculate
    {
        public string Name { get; set; }

        public GotfridaLaibnitsa()
        {
            Name = "Ряд Лейбница";
        }

        public double Calculate(double accuracy)
        {
            double sum = 0;
            double piCurr = 4 * sum;
            double piPrev = 0;
            int n = 0;

            while (Math.Abs(piCurr - piPrev) > accuracy)
            {
                piPrev = piCurr;
                double term = Math.Pow(-1, n) / (2.0 * n + 1);
                sum += term;
                piCurr = 4 * sum;
                n++;
            }
            return piCurr;
        }
    }
}

