using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace DihtomiaMethod
{
    public partial class Form1 : Form
    {
        NumberFormatInfo doubleNumbers = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
        };
        public Form1()
        {
            InitializeComponent();
            //Configure X Axis
            this.chart1.ChartAreas[0].AxisX.Crossing = 0;
            //Configure Y Axis
            this.chart1.ChartAreas[0].AxisY.Crossing = 0;
            comboBox1.Items.Add("f(x)=4*x+e^x");
            comboBox1.Items.Add("f(x)=x+8/x^4");
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text!= "Ручной ввод функции" && textBox1.Text!="")
            {
                string f, points;
                double E, a=0, b=0, EforDraw;
                for (int i=0; i<5;i++) {
                    chart1.Series[i].Points.Clear();
                }
                try {
                    a = double.Parse(textBox2.Text);
                    b= double.Parse(textBox1.Text);
                }
                catch {
                    MessageBox.Show("Введен некорректный промежуток, повторите попытку!\n*Дробные числа в программу необходимо вводить через символ ,","Ошибка парсинга.",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = "";
                    textBox1.Text = "";
                    return;
                }
                f = comboBox1.Text;
                E = double.Parse(textBox3.Text, doubleNumbers);
                EforDraw = CheckGapForE(a, b);
                if (EforDraw == -1) return;
                if (CheckIfNull(E, a, b, f))
                {
                    DihtomiaMethodRealisation realisation = new DihtomiaMethodRealisation(f);
                    DrawChart(a, b, EforDraw, realisation);
                    realisation.CountExpression(E, a, b);
                    points = realisation.GetPoints();
                    if (points == "Timeout" || points == "BadGap")
                    {
                        if (points == "BadGap")
                        {
                            MessageBox.Show("Указан отрезок [a,b], в котором f(a)*f(b)>0, метод Дихотомии не применим.", "Введен некорректный отрезок", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox2.Text = "";
                            textBox1.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Расчет занимает слишком много времени, пожалуйста, повторите попытку или укажите другой промежуток", "Тайм-аут", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            comboBox1.Text = "";
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox5.Text = "";
                        }
                        return;
                    }
                    textBox5.Text = points;
                }
                else
                {
                    MessageBox.Show("Введены некорректные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox1.Text = "";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox5.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Введены пустые поля, повторите попытку!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool CheckIfNull(double E, double a, double b, string f)
        {
            if (E < 0 || a > b || f == "")
            {
                return false;
            }
            else return true;
        }

        public double maxHeight(double a, double b, DihtomiaMethodRealisation realisation) {
            double x = (Math.Abs(a) + Math.Abs(b)) / 10;
            double max=-1;
            for(double i=a; i<b; i += x)
            {
                if (Math.Abs(realisation.ReturnFx(i)) > Math.Abs(max)) max = realisation.ReturnFx(i);
            }
            return max;
        }
        public void DrawChart(double a, double b, double EforDraw, DihtomiaMethodRealisation realisation) {
            double temp = (b-a) / 100;
            for (int i = 0; i < 100; i++)
            {
                chart1.Series[0].Points.AddXY(a, realisation.ReturnFx(a));
                a +=temp;
            }

        }
        public double CheckGapForE(double a, double b)
        {
            if (a==b)
            {
                MessageBox.Show("Указан промежуток длинной 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = "";
                textBox1.Text = "";
                return -1;
            }
            else
            {
                return (Math.Abs(a) + Math.Abs(b)) / 10;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();
            chart1.Series[4].Points.Clear();
            comboBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
        }

        public double[] returnGap(string gapNumbers) {
            double[] parsingError = new double [3] { 0, 0, 0 };
            double[] numbersToReturn=new double[2] { 0, 0};
            string[] numbersArray = gapNumbers.Split(';');
            for(int i = 0; i < 2; i++)
            {

                try {
                    numbersToReturn[i] = double.Parse(numbersArray[i]);
                }
                catch
                {
                    MessageBox.Show("Возможно вы ввели отрезок в некорректном формате","Ошибка парсинга", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = "";
                    return parsingError;
                }
            }
            return numbersToReturn;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Метод половинного деления или дихотомии (дихотомия - сопоставленность или противопоставленность двух частей целого) при нахождении корня уравнения f(x)=0 состоит в делении пополам отрезка" +
                " [a; b], где находится корень.\n\nЗатем анализируется изменение знака функции на половинных отрезках, и одна из границ отрезка [a; b] переносится в его середину. \n\nПереносится та граница, со стороны которой" +
                " функция на половине отрезка знака не меняет. Далее процесс повторяется до заданной точности.", "Описание метода", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void авторПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа была написана студентом ВУЗа ТУСУР Штабом Егором в качестве представления результатов проделанных работ во время прохождения курса Ознакомительная практика", "Автор программы", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void инструкцияПоИспользованиюПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Ввести функцию для нахождения нулей \n\n2. Ввести отрезок, на котором необходимо проверять наличие нулей функции (значение функции на концах отрезка должно отличаться- это условие нахождения нулей методом Дихотомии)\n\n3." +
                " Ввести точность E, она отвечает за то, насколько сильно найденная точка будет удалена от нулей функции\n\n4. Нажать на кнопку 'Расчитать' и дождаться результатов работы программы\n\n\n\nКнопка в нижнем правом углу используется при" +
                " необходимости очистить поля ввода и график функции.", "Инструкция", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
