namespace лаба_2._2upd
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button Startbtn;
        private Button Pausebtn;
        private Button Resetbtn;
        private ComboBox comboBox1;
        private Label labelResult;
        private Label labelSstatus;
        private ProgressBar progressBar1;
        private TextBox textBoxAccuracy;
        private Label labelAccuracy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            Startbtn = new Button();
            Pausebtn = new Button();
            Resetbtn = new Button();
            comboBox1 = new ComboBox();
            labelResult = new Label();
            labelSstatus = new Label();
            progressBar1 = new ProgressBar();
            textBoxAccuracy = new TextBox();
            labelAccuracy = new Label();
            SuspendLayout();

            // backgroundWorker1
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            // Startbtn
            Startbtn.Location = new Point(74, 243);
            Startbtn.Name = "Startbtn";
            Startbtn.Size = new Size(130, 56);
            Startbtn.TabIndex = 3;
            Startbtn.Text = "Старт";
            Startbtn.UseVisualStyleBackColor = true;
            Startbtn.Click += Startbtn_Click;

            // Pausebtn
            Pausebtn.Location = new Point(314, 243);
            Pausebtn.Name = "Pausebtn";
            Pausebtn.Size = new Size(138, 56);
            Pausebtn.TabIndex = 4;
            Pausebtn.Text = "Пауза";
            Pausebtn.UseVisualStyleBackColor = true;
            Pausebtn.Click += Pausebtn_Click;

            // Resetbtn
            Resetbtn.Location = new Point(555, 243);
            Resetbtn.Name = "Resetbtn";
            Resetbtn.Size = new Size(138, 56);
            Resetbtn.TabIndex = 5;
            Resetbtn.Text = "Сброс";
            Resetbtn.UseVisualStyleBackColor = true;
            Resetbtn.Click += Resetbtn_Click;

            // comboBox1
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Ряд Лейбница", "Ряд Валлиса", "Ряд Виета" });
            comboBox1.Location = new Point(572, 44);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            // labelResult
            labelResult.AutoSize = true;
            labelResult.Location = new Point(68, 61);
            labelResult.MaximumSize = new Size(400, 0);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(38, 15);
            labelResult.TabIndex = 6;
            labelResult.Text = "label1";

            // labelSstatus
            labelSstatus.AutoSize = true;
            labelSstatus.Location = new Point(396, 159);
            labelSstatus.Name = "labelSstatus";
            labelSstatus.Size = new Size(0, 18);
            labelSstatus.TabIndex = 5;
            labelSstatus.UseCompatibleTextRendering = true;
            labelSstatus.Click += label1_Click;

            // progressBar1
            progressBar1.Location = new Point(280, 159);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 23);
            progressBar1.TabIndex = 2;

            // textBoxAccuracy
            textBoxAccuracy.Location = new Point(572, 100);
            textBoxAccuracy.Name = "textBoxAccuracy";
            textBoxAccuracy.Size = new Size(121, 23);
            textBoxAccuracy.TabIndex = 0;
            textBoxAccuracy.TextChanged += textBoxAccuracy_TextChanged;

            // labelAccuracy
            labelAccuracy.AutoSize = true;
            labelAccuracy.Location = new Point(572, 82);
            labelAccuracy.Name = "labelAccuracy";
            labelAccuracy.Size = new Size(113, 15);
            labelAccuracy.TabIndex = 7;
            labelAccuracy.Text = "Погрешность (ε > 0):";

            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelAccuracy);
            Controls.Add(textBoxAccuracy);
            Controls.Add(progressBar1);
            Controls.Add(labelSstatus);
            Controls.Add(labelResult);
            Controls.Add(comboBox1);
            Controls.Add(Resetbtn);
            Controls.Add(Pausebtn);
            Controls.Add(Startbtn);
            Name = "Form1";
            Text = "Вычисление числа π";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}