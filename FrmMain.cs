using System;
using System.Globalization;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class FrmMain : Form
    {
        private Calculator data;

        private void DisplayCallback(decimal result)
        {
            //TODO (HOMEWORK) trailing zeros after the decimal point
            lblDisplay.Text = result.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        public FrmMain()
        {
            InitializeComponent();
            data = new Calculator(DisplayCallback);
            lblDisplay.Text = data.Clear();
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(0);
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(1);
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(2);
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(3);
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(4);
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(5);
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(6);
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(7);
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(8);
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Digit(9);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Clear();
        }

        private void BtnDif_Click(object sender, EventArgs e)
        {
            data.Operator("-");
        }

        private void BtnDiv_Click(object sender, EventArgs e)
        {
            data.Operator("/");
        }

        private void BtnEquals_Click(object sender, EventArgs e)
        {
            data.Calculate();
        }

        private void BtnMul_Click(object sender, EventArgs e)
        {
            data.Operator("*");
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            data.Operator("+");
        }

        private void BtnDot_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = data.Special(".");
        }
    }
}