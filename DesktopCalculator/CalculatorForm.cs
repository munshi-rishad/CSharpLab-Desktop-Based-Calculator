﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DesktopCalculator
{
    public partial class CalculatorForm : Form
    {
        // Operator symbols (the same text is shown on the buttons)
        private const string Plus = "+";
        private const string Minus = "−";    // −
        private const string Times = "×";    // ×
        private const string Divide = "÷";   // ÷

        private const int MaxDigits = 16;

        private double firstNumber = 0;          // number typed before the operator
        private string currentOperator = "";     // "", +, −, × or ÷
        private bool startNewNumber = true;      // true: the next digit starts a new number
        private bool showingError = false;       // true: the screen shows an error message

        private Font bigFont;
        private Font smallFont;

        public CalculatorForm()
        {
            InitializeComponent();

            bigFont = lblDisplay.Font;
            smallFont = new Font(lblDisplay.Font.FontFamily, 20F, FontStyle.Bold);

            ClearAll();
        }

        // ================= Button clicks =================

        // All number buttons (0 - 9) use this one method
        private void Digit_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            AddDigit(button.Text);
        }

        // All operator buttons (+ − × ÷) use this one method
        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ChooseOperator(button.Text);
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            ShowResult();
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            AddDot();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            Backspace();
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            Percent();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            ChangeSign();
        }

        // ================= Keyboard =================

        // Numbers, . and operators from the keyboard
        private void CalculatorForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;

            if (key >= '0' && key <= '9')
                AddDigit(key.ToString());
            else if (key == '.')
                AddDot();
            else if (key == '+')
                ChooseOperator(Plus);
            else if (key == '-')
                ChooseOperator(Minus);
            else if (key == '*' || key == 'x' || key == 'X')
                ChooseOperator(Times);
            else if (key == '/')
                ChooseOperator(Divide);
            else if (key == '%')
                Percent();
            else if (key == '=')
                ShowResult();
            else
                return;

            e.Handled = true;
        }

        // Enter, Backspace and Esc are handled here, before any button can use them
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    ShowResult();
                    return true;
                case Keys.Back:
                    Backspace();
                    return true;
                case Keys.Escape:
                case Keys.Delete:
                    ClearAll();
                    return true;
                case Keys.Space:
                    return true;   // don't press the focused button with Space
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ================= Calculator logic =================

        private void AddDigit(string digit)
        {
            if (showingError)
                ClearAll();

            if (startNewNumber || lblDisplay.Text == "0")
            {
                SetDisplay(digit);
                startNewNumber = false;
            }
            else if (CountDigits(lblDisplay.Text) < MaxDigits)
            {
                SetDisplay(lblDisplay.Text + digit);
            }
        }

        private void AddDot()
        {
            if (showingError)
                ClearAll();

            if (startNewNumber)
            {
                SetDisplay("0.");
                startNewNumber = false;
            }
            else if (!lblDisplay.Text.Contains("."))
            {
                SetDisplay(lblDisplay.Text + ".");
            }
        }

        private void ChooseOperator(string newOperator)
        {
            if (showingError)
                return;

            if (currentOperator != "" && !startNewNumber)
            {
                // Example: 2 + 3 and then ×  ->  first work out 2 + 3 = 5
                if (!Calculate())
                    return;
            }
            else if (currentOperator == "")
            {
                firstNumber = GetDisplayNumber();
            }

            // If an operator was already chosen, pressing another one just replaces it
            currentOperator = newOperator;
            lblHistory.Text = FormatNumber(firstNumber) + " " + currentOperator;
            startNewNumber = true;
        }

        private void ShowResult()
        {
            if (showingError || currentOperator == "")
                return;

            double secondNumber = GetDisplayNumber();
            string history = FormatNumber(firstNumber) + " " + currentOperator + " " + FormatNumber(secondNumber) + " =";

            if (!Calculate())
                return;

            lblHistory.Text = history;
            currentOperator = "";
            startNewNumber = true;
        }

        // Does firstNumber (operator) displayed number, and shows the answer.
        // Returns false if there was an error.
        private bool Calculate()
        {
            double secondNumber = GetDisplayNumber();
            double result = 0;

            switch (currentOperator)
            {
                case Plus:
                    result = firstNumber + secondNumber;
                    break;
                case Minus:
                    result = firstNumber - secondNumber;
                    break;
                case Times:
                    result = firstNumber * secondNumber;
                    break;
                case Divide:
                    if (secondNumber == 0)
                    {
                        ShowError("Cannot divide by zero");
                        return false;
                    }
                    result = firstNumber / secondNumber;
                    break;
            }

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                ShowError("Number too large");
                return false;
            }

            firstNumber = result;
            SetDisplay(FormatNumber(result));
            return true;
        }

        private void Percent()
        {
            if (showingError)
                return;

            SetDisplay(FormatNumber(GetDisplayNumber() / 100));
        }

        private void ChangeSign()
        {
            if (showingError || lblDisplay.Text == "0")
                return;

            if (lblDisplay.Text.StartsWith("-"))
                SetDisplay(lblDisplay.Text.Substring(1));
            else
                SetDisplay("-" + lblDisplay.Text);
        }

        private void Backspace()
        {
            if (showingError)
            {
                ClearAll();
                return;
            }
            if (startNewNumber)
                return;   // don't change an answer

            string text = lblDisplay.Text.Substring(0, lblDisplay.Text.Length - 1);
            if (text == "" || text == "-")
                text = "0";
            SetDisplay(text);
        }

        private void ClearAll()
        {
            firstNumber = 0;
            currentOperator = "";
            startNewNumber = true;
            showingError = false;
            lblHistory.Text = "";
            SetDisplay("0");
        }

        private void ShowError(string message)
        {
            firstNumber = 0;
            currentOperator = "";
            startNewNumber = true;
            showingError = true;
            lblHistory.Text = "";
            SetDisplay(message);
        }

        // ================= Small helpers =================

        // Shows text on the screen; long text gets a smaller font so it fits
        private void SetDisplay(string text)
        {
            lblDisplay.Font = text.Length > 11 ? smallFont : bigFont;
            lblDisplay.Text = text;
        }

        private double GetDisplayNumber()
        {
            return double.Parse(lblDisplay.Text, CultureInfo.InvariantCulture);
        }

        // 15 significant digits removes tiny errors like 0.1 + 0.2 = 0.30000000000000004
        private string FormatNumber(double number)
        {
            return number.ToString("G15", CultureInfo.InvariantCulture);
        }

        private int CountDigits(string text)
        {
            int count = 0;
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                    count++;
            }
            return count;
        }
    }
}
