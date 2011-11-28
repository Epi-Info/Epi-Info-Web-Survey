using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using MvcDynamicForms;

namespace MvcDynamicForms.Demo.Models
{
    public static class FormProvider
    {
        public static Form GetForm()
        {
            /*
             * This method sets up the Form and Field objects that 
             * are needed to dynamically generate html forms at runtime.
             * 
             * Of course, there are other ways of going about defining your forms and their fields.
             * I used a static class in this demo application for simplicity.
             * In the real world, you could store your field definitions anywhere.
             * 
             * For example, you could create a database table to store all 
             * of the data needed to create the form fields below.
             * Some of your end users could have access to some kind of interface to create, update,
             * or delete the form field definitions in the database.
             * This described scenario was actually the inspiration for this project.
             * 
             * There are 7 different Field types that can be used to construct the form:
             *  - TextBox (single line text input)
             *  - Textarea (multi line text input)
             *  - Checkbox
             *  - CheckboxList
             *  - RadioList
             *  - Select (Drop down lists and List boxes)
             *  - Literal (Any custom html at all. For display purposes only (no user input))
             *  
             * Each Field type have a few things in common:
             *  - Title property: Used when storing end user's responses.
             *  - Prompt property: Question asked to the user for each field.
             *  - DisplayOrder property: The order that the field is displayed to the user.
             *  - Required property: Is the user required to complete the field?
             *  - InputHtmlAttributes: Allows the developer to set the input elements html attributes
             *  
             * There are other properties and behaviors that some Field types do not share with each other.
             * Take a look through the members of each Type to see what you can do.
             * Much of each type's unique functionality is demonstrated below.
             * Feel free to tinker around in this file, changing and adding fields.
             * Don't forget to add newly created fields to the Form.
             * 
             * The Form object is the object that contains all of your Field objects, 
             * triggers validation and rendering, and lets the developer access user responses.
             * When constructing your form, you can use Form.AddFields() to get your Fields
             * into the form (imagine that!).
             * 
             * Check out
             *    /Controllers/HomeController.cs
             *    /Views/Home/Demo.aspx
             *    /Views/Home/Responses.aspx
             * to learn how to use the Form object in your web application.
             */



            var description = new Literal
            {
                FieldWrapper = "p",
                Wrap = true,
                DisplayOrder = 10,
                Html = "asasa" // "This is a dynamically generated form. All of the input fields on this form are generated at runtime."

            };



            //var name = new TextBox
            //{
            //    Title = "Name",
            //    Prompt = "Enter your full name:",
            //    DisplayOrder = 20,
            //    Required = true,
            //    RequiredMessage = "Your full name is required",
            //    Key = "1233"
            //};
            //var Age = new TextBox
            //{
            //    Title = "Age",
            //    Prompt = "Enter your full Age:",
            //    DisplayOrder = 21,
            //    Required = true,
            //    RequiredMessage = "Your Age is required",
            //    Key = "1883"
            //};

            //var gender = new RadioList
            //{
            //    DisplayOrder = 30,
            //    Title = "Gender",
            //    Prompt = "Select your gender:",
            //    Required = true,
            //    Orientation = Orientation.Vertical
            //};
            //gender.AddChoices("Male,Female", ",");

            //var email = new TextBox
            //{
            //    DisplayOrder = 25,
            //    Title = "Email Address",
            //    Prompt = "Enter your email address:",
            //    Required = true,
            //    RegexMessage = "Must be a valid email address",
            //    RegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            //};

            //var sports = new CheckBoxList
            //{
            //    DisplayOrder = 40,
            //    Title = "Favorite Sports",
            //    Prompt = "What are your favorite sports?",
            //    Orientation = Orientation.Horizontal
            //};
            //sports.AddChoices("Baseball,Football,Soccer,Basketball,Tennis,Boxing,Golf", ",");

            //var states = new Select
            //{
            //    DisplayOrder = 50,
            //    Title = "Visited States",
            //    MultipleSelection = true,
            //    Size = 10,
            //    Prompt = "What US states have you visited? (Use the ctrl key to select multiple states.)"
            //};
            //states.AddChoices("Alabama,Alaska,Arizona,Arkansas,California,Colorado,Connecticut,Delaware,Florida,Georgia,Hawaii,Idaho,Illinois,Indiana,Iowa,Kansas,Kentucky,Louisiana,Maine,Maryland,Massachusetts,Michigan,Minnesota,Mississippi,Missouri,Montana,Nebraska,Nevada,New Hampshire,New Jersey,New Mexico,New York,North Carolina,North Dakota,Ohio,Oklahoma,Oregon,Pennsylvania,Rhode Island,South Carolina,South Dakota,Tennessee,Texas,Utah,Vermont,Virginia,Washington,West Virginia,Wisconsin,Wyoming", ",");

            //var bio = new TextArea
            //{
            //    DisplayOrder = 60,
            //    Title = "Bio",
            //    Prompt = "Describe yourself:"
            //};
            //bio.InputHtmlAttributes.Add("cols", "40");
            //bio.InputHtmlAttributes.Add("rows", "6");

            //var month = new Select
            //{
            //    DisplayOrder = 70,
            //    Title = "Month Born",
            //    Prompt = "What month were you born in?",
            //    ShowEmptyOption = true,
            //    EmptyOption = "- Select One - "
            //};
            //month.AddChoices("January,February,March,April,May,June,July,August,September,October,November,December", ",");

            //var agree = new CheckBox
            //{
            //    DisplayOrder = 80,
            //    Title = "Agrees To Terms",
            //    Prompt = "I agree to all of the terms in the EULA.",
            //    Required = true,
            //    RequiredMessage = "You must agree to the EULA!"
            //};

        

            // create form and add fields to it
            var form = new Form();
            //form.AddFields(description, name, Age, gender, email, sports, states, bio, month, agree );
            form.AddFields(description);

            return form;
        }

       
    }
}
