using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Epi.Web.SurveyManager_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Epi.Web.SurveyManager.Service  client = new Epi.Web.SurveyManager.Service();

            Epi.Web.SurveyManager.cSurveyRequest Request = new Epi.Web.SurveyManager.cSurveyRequest();
            if (this.ClosingDateCalendar.SelectedDate == null)
            {
                Request.ClosingDate = (DateTime)this.ClosingDateCalendar.DisplayDate;
            }
            else
            {
                Request.ClosingDate = (DateTime)this.ClosingDateCalendar.SelectedDate;
            }

            Request.DepartmentName = this.DepartmentTextBox.Text;
            Request.IntroductionText = this.IntroductionTextBox.Selection.Text;
            Request.IsSingleResponse = (bool)this.IsSingleResponseCheckBox.IsChecked;

            Request.OrganizationName = this.OrganizationTextBox.Text;
            Request.SurveyName = this.SurveyNameTextBox.Text;
            Request.SurveyNumber = this.SurveyNumberTextBox.Text;
            Request.TemplateXML = this.TemplateXMLTextBox.Selection.Text;

            try
            {
                Epi.Web.SurveyManager.cSurveyRequestResult Result = client.PublishSurvey(Request);

                ServiceResponseTextBox.AppendText("is published: ");
                ServiceResponseTextBox.AppendText(Result.IsPulished.ToString());
                ServiceResponseTextBox.AppendText("\nURL: ");
                ServiceResponseTextBox.AppendText(Result.URL);
                ServiceResponseTextBox.AppendText("\nStatus Text: ");
                ServiceResponseTextBox.AppendText(Result.StatusText);
            }
            catch(Exception ex)
            {
                ServiceResponseTextBox.AppendText("error:\n");
                ServiceResponseTextBox.AppendText(ex.ToString());
            }
        }
    }
}
