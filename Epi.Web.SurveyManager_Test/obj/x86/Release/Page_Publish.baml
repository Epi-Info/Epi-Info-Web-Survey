   M S B A M L   `   `   `  ����    Epi.Web.SurveyManager.Client/    )Epi.Web.SurveyManager.Client.Page_Publish   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       N� Title$  Page_Publish��.���� $
��Auto=�.��� $	��807��$	��642�� � RowDefinitions �� $��6* ��� $
��284* ��� $
��330* ��� $��2* � � ColumnDefinitions �� $	��20* ��� $	��68* ��� $	��82* ��� $
��114* ��� $
��347* �.+��� -   $��SubmitRequestButton��$��Submit Request��$��29��$
��Left=�$��204.398,119,0,0q�$	��Top=�$	��120��$��4��
 J� Uid$  ��$��2�� K� ZIndex$ 1���� -   $��label1��$��Service Response:��$��30��$
��Left=�$��
19,180,0,0q�$	��Top=�$	��124��$��1��$��2��$
��Bold"�$��2���� -   $��ServiceResponseTextBox��$��1��$
��Left=�$��19,207,0,131q�$	��588��$ ServiceResponse��$��4��$��2��" �� VerticalScrollBarVisibility$ Visible=��� -   $��button1��$��View Download Client��$��1��$��2��$��1��$��30��$
��Left=�$��17,0,0,0q�$	��Top=�$	��134���� -   $��	groupBox1��$��2��$	��508��$
��Left=�$	��Top=�$	��592��$��4��$��1��$��	18,34,0,0q�$��1�����	#FFD8D6D5���� $��Survey Information��$��13.333(�$
��Bold"�.��� $	��484��$	��569�� �� $
��372* ��� $��0* ��� $	��25* ��� $	��24* ��� $	��63* �.+��� -   $��TemplateXMLTextBox��$��32��$
��Left=�$��
28,170,0,0q�$	��Top=�$	��525��$ 
TemplatXML��$ Visible=��� -   $��SurveyNumberTextBox��$��27��$
��Left=�$��
309,25,0,0q�$	��Top=�$	��240��$ SurveyNumber��$��AE01-IOU���� -   $��SurveyNameTextBox��$��25��$��Right=�$��
0,25,289,0q�$	��Top=�$	��252��$ 
SurveyName��$��Survey1���� -	   $��DepartmentTextBox��$��23��$��309,79,48,0q�$	��Top=�$ DepartmentName��$��Department1���� -
   $��OrganizationTextBox��$��22��$��28,79,284,0q�$	��Top=�$ OrganizationName��$��Organization1���� -   $��label2��$��Department Name��$��23��$
��Left=�$��
308,55,0,0q�$	��Top=�$	��112���� -   $��label5��$��Organization Name��$��24��$
��Left=�$��	26,54,0,0q�$	��Top=�$��126.993���� -   $��label6��$��Survey Number��$��24��$
��Left=�$��	309,0,0,0q�$	��Top=�$	��106���� -   $��label8��$��Template XML��$��23��$
��Left=�$��
25,141,0,0q�$	��Top=�$��92.427���� -   $��label15��$��Survey Name��$��28��$
��Left=�$��27,2,0,0q�$	��Top=��� -   $��IsSingleResponseCheckBox��$��Is Single Response?��$��16��$
��Left=�$��
30,114,0,0q�$	��Top=�$ IsSingleResponse���� -   $��IntroductionTextBox��$��47��$
��Left=�$��
28,237,0,0q�$	��Top=�$	��525��$ IntroductionText��$ Visible=��� -   $��label3��$��Introduction Text��$��28��$
��Left=�$��
25,212,0,0q�$	��Top=��� -   $��label4��$��Closing Date:��$��28��$
��Left=�$��309,109,0,0q�$	��Top=�(  "System.Windows.Controls.DatePicker  -   $��datePicker1��$��23��$
��Left=�$��394,110.331,0,0q�$	��Top=�$	��156���� -   $��FindFileButton��$��Find File...��$��23��$
��Left=�$��129,142,0,0q�$	��Top=�$��75���� -   $��TemplateFileLabel��$*��$No Survey Template file is selected.��$��28��$
��Left=�$��210,140,0,0q�$	��Top=�$	��343�����	#FF787878�� -   $��FindIntroTextButton��$��Find File...��$��23��$
��Left=�$��130,210,0,0q�$	��Top=�$��75���� -   $��IntroFileLabel��$%��No Intro Text File is selected.��$��28��$
��Left=�$��212,0,0,136q�$��Bottom=�$	��338�����	#FF787878�� -   $��ClearTemplateButton��$��.��$
��Left=�$��558,171,0,170q�$��11���� -   $��ClearIntroButton��$��.��$
��46.5��$
��Left=�$��557,237.5,0,0q�$	��Top=�$��12���� -   $��label7��$��Exit Survey Text��$��28��$
��Left=�$��
25,294,0,0q�$	��Top=��� -   $��FindExtiTextButton��$��Find File...��$��23��$
��Left=�$��130,294,0,0q�$	��Top=�$��75���� -   $��ExitTextLabel��$$��No Exit Text file is selected.��$��28��$
��Left=�$��215,292,0,0q�$	��Top=�$	��330�����	#FF787878�� -   $��ExitTextTextBox��$��41��$��28,319,16,0q�$	��Top=�$ Visible=��� -   $��ClearExitTextButton��$��.��$��40��$
��Left=�$��557,320,0,0q�$	��Top=�$��12���� -    $��label10��$��Label��$��4��$��28��$
��Left=�$��
110,69,0,0q�$	��Top=��� -!   $��lblPublishKey��$��Publish Key��$��2��$��28��$
��Left=�$��27,0,0,0q�$	��Top=�$��75���� -"   $��txtPublishKey��$��2��$��23��$
��Left=�$��	103,0,0,0q�$	��Top=�$	��368���� -#   $��label9��$��Organization Key��$��4��$��28��$
��Left=�$��25.5,-8,0,0q�$	��Top=�$	��107��G� -$   $��passOrganizationKey��$��4��$��23��$
��Left=�$��	28,17,0,0q�$	��Top=�$	��366���� -%   $��btnGeneratePublishkey��$��Generate��$��2��$��23��$
��Left=�$��484,0.331,0,0q�$	��Top=�$��65���� -&   $��OpenURLButton��$��Open URL��$��4��$��2��$��30��$��0,303,31.049,0q�$	��Top=�{�.  $��Right=�$	��125���� -'   $��ManageSurveyButton��$��Manage Existing Surveys��$��3��$��1��$��30��$
��Left=�$��4,1,0,0q�$	��Top=�$	��140��$��2���� -(   $��AddOrganization��$��Add Organization��$��4��$��1��$��30��$
��Left=�$��40,0,0,0q�$	��Top=�$	��162��