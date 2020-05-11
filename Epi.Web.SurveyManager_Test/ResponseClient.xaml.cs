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
using System.Data;
using System.Configuration;
namespace Epi.Web.SurveyManager.Client
    {
    /// <summary>
    /// Interaction logic for ResponseClient.xaml
    /// </summary>
    public partial class ResponseClient : Page
        {
        DataTable KeyValuesTable = new DataTable();
        DataColumn column;
        DataRow row;
        DataView view;
        private string Url;
        public ResponseClient()
            {
            InitializeComponent();
            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");//System.Type.GetType("System.Int32");
            column.ColumnName = "Key";
            KeyValuesTable.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Value";
            KeyValuesTable.Columns.Add(column);

            }



        private void ManageSurveyButton_Click(object sender, RoutedEventArgs e)
            {
            Page_ManageSurvey page_ManageSurvey = new Page_ManageSurvey();
            this.NavigationService.Navigate(page_ManageSurvey);
            }

        private void AddOrganization_Click(object sender, RoutedEventArgs e)
            {
            Page_AddOrganization Page_AddOrganization = new Page_AddOrganization();
            this.NavigationService.Navigate(Page_AddOrganization);
            }

        private void button1_Click(object sender, RoutedEventArgs e)
            {
            DownLoad page_Download = new DownLoad();
            this.NavigationService.Navigate(page_Download);
            }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
            {
            string Key = textBox1.Text;
            string Value = textBox2.Text;

            row = KeyValuesTable.NewRow();
            row["Key"] = Key;
            row["Value"] = Value;
            KeyValuesTable.Rows.Add(row);
            ShowData();
            ClearData();
            }

        private void btnClear_Click(object sender, RoutedEventArgs e)
            {
            textBox1.Text = "";
            textBox2.Text = "";
            }
        public void ShowData()
            {

            listView1.DataContext = KeyValuesTable.DefaultView;
            }
        public void ClearData()
            {

            textBox1.Text = "";
            textBox2.Text = "";
            }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
            {

            var current = this.listView1.Items.CurrentItem;
            if (current != null)
                {
            ListViewItem container = listView1.ItemContainerGenerator.ContainerFromItem(current) as ListViewItem;
            int currentItemIndex = listView1.ItemContainerGenerator.IndexFromContainer(container);
            KeyValuesTable.Rows.RemoveAt(currentItemIndex);
            ShowData();
                }
            }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
            {
              var current = this.listView1.Items.CurrentItem;
              if (current != null)
              {
              string Key = textBox1.Text;
              string Value = textBox2.Text;
              ((System.Data.DataRowView)(listView1.SelectedItem)).Row[0] = Key;
              ((System.Data.DataRowView)(listView1.SelectedItem)).Row[1] = Value;
            
              ShowData();
               }
            }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            if (listView1.SelectedItem != null)
                {
                textBox1.Text = ((System.Data.DataRowView)(listView1.SelectedItem)).Row[0].ToString();
                textBox2.Text = ((System.Data.DataRowView)(listView1.SelectedItem)).Row[1].ToString();

                }

            }

        private void button3_Click(object sender, RoutedEventArgs e)
            {

            try
                {

                SurveyManagerServiceV2.ManagerServiceV2Client Client = ServiceClient.GetClientV2();
                Epi.Web.Common.Message.PreFilledAnswerRequest Request = new Epi.Web.Common.Message.PreFilledAnswerRequest();
                Guid OrganizationGuid = new Guid(passwordBox1.Password);
                Guid SurveyGuid = new Guid(SurveyId.Text);
                Guid UserPublishKey = new Guid(this.UserPublishKey.Text);

                Dictionary<string, string> Values = new Dictionary<string, string>();

                foreach (var item in listView1.Items)
                    {

                    Values.Add(((System.Data.DataRowView)(item)).Row[0].ToString(), ((System.Data.DataRowView)(item)).Row[1].ToString());

                    }

                Request.AnswerInfo.OrganizationKey = OrganizationGuid;
                Request.AnswerInfo.SurveyId = SurveyGuid;
                Request.AnswerInfo.UserPublishKey = UserPublishKey;
                Request.AnswerInfo.SurveyQuestionAnswerList = Values;
                var Result = Client.SetSurveyAnswer(Request);

                  this.Url = Result.SurveyResponseUrl + "/" + Result.SurveyResponsePassCode;

                if (Result.Status == "Success")
                      {
                      
                          this.Result.AppendText("\nResponse Id: " + Result.SurveyResponseID);
                          this.Result.AppendText("\nResponse URL: " + Result.SurveyResponseUrl);
                          this.Result.AppendText("\nPass Code: " + Result.SurveyResponsePassCode);
                          
                      }
                  else {
                  if (Result.ErrorMessageList.Count() > 0)
                      {
                      foreach (var item in Result.ErrorMessageList)
                          {
                          this.Result.AppendText("\n" + item.Key + " : " + item.Value);
                          }
                      }
                      }
                }
            catch (Exception ex)
          
                {
                this.Result.AppendText( "An Error occurred  while trying to insert a response");
                
                }


            }

        private void button2_Click(object sender, RoutedEventArgs e)
            {
            this.Result.Document.Blocks.Clear();
            }

        private void OpenResponse_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(this.Url);
        }
        }
      
        }
    
