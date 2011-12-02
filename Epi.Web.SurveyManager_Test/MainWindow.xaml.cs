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
            Epi.Web.WCF.SurveyService.Service client = new Epi.Web.WCF.SurveyService.Service();

            Epi.Web.Common.Message.SurveyRequest Request = new Epi.Web.Common.Message.SurveyRequest();
            if (this.ClosingDateCalendar.SelectedDate == null)
            {
                Request.ClosingDate = (DateTime)this.ClosingDateCalendar.DisplayDate;
            }
            else
            {
                Request.ClosingDate = (DateTime)this.ClosingDateCalendar.SelectedDate;
            }

            Request.DepartmentName = this.DepartmentTextBox.Text;
            Request.IntroductionText = new TextRange(this.IntroductionTextBox.Document.ContentStart, this.IntroductionTextBox.Document.ContentEnd).Text; 
            Request.IsSingleResponse = (bool)this.IsSingleResponseCheckBox.IsChecked;

            Request.OrganizationName = this.OrganizationTextBox.Text;
            Request.SurveyName = this.SurveyNameTextBox.Text;
            Request.SurveyNumber = this.SurveyNumberTextBox.Text;
            Request.TemplateXML = new TextRange(this.TemplateXMLTextBox.Document.ContentStart, this.TemplateXMLTextBox.Document.ContentEnd).Text; 

            try
            {
                Epi.Web.Common.DTO.SurveyRequestResponse Result = client.PublishSurvey(Request);

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
