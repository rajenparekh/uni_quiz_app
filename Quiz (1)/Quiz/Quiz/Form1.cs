using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
 
            InitializeComponent();

        }

        private void Form_Load(object sender, EventArgs e)
        {
            StartQuiz sq = new StartQuiz();
            sq.start();
        }
    }

    public class question_and_answer
    {
        public string Module { get; set; }
        public int Difficulty { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string[] Possibilities { get; set; }
    }
    public class LoadJSON
    {

        List<question_and_answer> easy_questions = new List<question_and_answer>();
        List<question_and_answer> medium_questions = new List<question_and_answer>();
        List<question_and_answer> hard_questions = new List<question_and_answer>();

        public List<question_and_answer> returnEasy()
        {
            return easy_questions;
        }

        public List<question_and_answer> returnMedium()
        {
            return medium_questions;
        }

        public List<question_and_answer> returnHard()
        {
            return hard_questions;
        }

        public LoadJSON()
        {
            List<question_and_answer> values = null;
            using (StreamReader r = new StreamReader("file.json"))
            {
                string json = r.ReadToEnd();
                values = JsonConvert.DeserializeObject<List<question_and_answer>>(json);
            }

            try
            {
                foreach (question_and_answer value in values)

                {
                    switch (value.Difficulty)
                    {
                        case 1:
                            easy_questions.Add(value);
                            break;
                        case 2:
                            medium_questions.Add(value);
                            break;
                        case 3:
                            hard_questions.Add(value);
                            break;
                    }
                }
            }

            catch
            {
                Label EmptyLabel = new Label();
                EmptyLabel.Text = "json file empty";
                Application.OpenForms["Form1"].Controls.Add(EmptyLabel);
            }
        }
    }

    

    public class GenerateLabels
    {
        static int i = 0;
        static int level = 2;
        Form myform = Application.OpenForms["Form1"];
        private RadioButton selectedrb = new RadioButton(); 
        string correct_answer = "";
        bool buttonselected = false;

        public Form returnform()
        {
            return myform;
        }

        public void generate_labels(question_and_answer q_and_a)
        {
            
            GroupBox gb = new GroupBox();
            Label question = new Label();
            Label difficulty = new Label();
            Button btn_submit = new Button();
            question.Text = q_and_a.Question;
            question.Location = new Point(10, 10);
            question.Size = new Size(question.Size.Width + 10, question.Size.Height);

            Point radiobutton_point = new Point(10, 10);
            Size gb_size = new Size(200, 200);
            foreach (string possibility in q_and_a.Possibilities)
            {
                RadioButton rb = new RadioButton();
                rb.Name = possibility;
                rb.Text = possibility;
                rb.Location = new Point(radiobutton_point.X, radiobutton_point.Y + 20);
                radiobutton_point = new Point(radiobutton_point.X, radiobutton_point.Y + 20);
                gb.Controls.Add(rb);
                rb.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            }

            correct_answer = q_and_a.Answer;

            btn_submit.Text = "Submit";
            btn_submit.Location = new Point(125, 100);
            btn_submit.Click += new EventHandler(this.btn_submit_click);
            gb.Controls.Add(btn_submit);

            gb.Size = gb_size;
            gb.Controls.Add(question);
            myform.Controls.Add(gb);
            

        }

        private void btn_submit_click(object sender, EventArgs e)
        {            
            StartQuiz sq = new StartQuiz();

            if (selectedrb.Text == "")
            {
                MessageBox.Show("Please Select a Result");
            }
            else if (selectedrb.Text == correct_answer)
            {            
                switch (level)
                {
                    case 1:
                        level++;
                        break;
                    case 2:
                        level++;
                        break;
                    case 3:
                        break;
                }

                sq.nextquestion(level);
            }
            else
            {
                MessageBox.Show("Incorrect");
                switch (level)
                {
                    case 1:
                        
                        break;
                    case 2:
                        level--;
                        break;
                    case 3:
                        level--;
                        break;
                }
                sq.nextquestion(level);
            }            
        }

        public void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            else if (rb.Checked)
            {
                selectedrb = rb;
            }
        }

        public bool getbuttonselected()
        {
            return buttonselected;

        }

        public void setbutton(bool i)
        {

            buttonselected = i;

            

        }

        
    }

    public class StartQuiz
    {

        Form myform = Application.OpenForms["Form1"];
        LoadJSON lj = new LoadJSON();
        GenerateLabels gl = new GenerateLabels();
        List<question_and_answer> easyList = null;
        List<question_and_answer> mediumList = null;
        List<question_and_answer> hardList = null;
        private static int easyi = -1;
        private static int mediumi = 0;
        private static int hardi = -1;

        public void start()
        {

            
              mediumList = lj.returnMedium();
              


            gl.generate_labels(mediumList[0]);
 

        }

        public void nextquestion( int level)
        {

            easyList = lj.returnEasy();
            mediumList = lj.returnMedium();
            hardList = lj.returnHard();

            myform.Controls.Clear();


            switch (level)
            {
                case 1:
                    easyi++;
                    gl.generate_labels(easyList[easyi]);
                    break;
                case 2:
                    mediumi++;
                    gl.generate_labels(mediumList[mediumi]);
                    break;
                case 3:
                    hardi++;
                    gl.generate_labels(hardList[hardi]);
                    break;
            }

            

            



        }
    }


}

