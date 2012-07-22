

function CCE_Hide(pNameList, pIsExceptionList)
{
        try
        {
            if (pIsExceptionList)
            {
             CCE_ProcessHideExceptCommand(pNameList);
                
            }
            else
            {
                CCE_ProcessHideCommand(pNameList);
                
            }
        }
        catch (ex)
        {

        }
}


/// <summary>
/// Processes the Hide command for check code execution
/// </summary>
/// <param name="checkCodeList">A list of fields to hide</param>
function CCE_ProcessHideCommand(pCheckCodeList)
{
    if (pCheckCodeList != null)
    {
        //var controlsList = GetAssociatedControls(pCheckCodeList);
        //this.canvas.HideCheckCodeItems(controlsList);
        for (var i = 0; i < pCheckCodeList.length; i++) 
        {
            
            var query = '#mvcdynamicfield_' + pCheckCodeList[i];
             //clear the control value before hiding
             CCE_ClearControlValue(query);
            $(query).hide();
            query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
            $(query).hide();
            //CCE_AddToHiddenFieldsList(pCheckCodeList[i]);
            CCE_AddToFieldsList(pCheckCodeList[i], 'HiddenFieldsList')
        }
    }
}


/// <summary>
/// Processed the Hide Except command for check code execution
/// </summary>
/// <param name="checkCodeList">A list of fields</param>
function CCE_ProcessHideExceptCommand(pCheckCodeList)
{
    if (pCheckCodeList != null)
    {

        var ControlList = new Array();

        for(var i in cce_Context.symbolTable)
        {
            var symbol = cce_Context.symbolTable[i];
            var symbol_name = symbol.Name.toLowerCase();
            if(symbol.Source == "datasource")
            {
                var isFound = false;
                for(var j = 0; j < pCheckCodeList.length; j++)
                {
                    if(pCheckCodeList[j].toLowerCase() == symbol_name)
                    {
                        isFound = true;
                        break;
                    }
                }

                if(! isFound)
                {
                    ControlList.push(symbol_name);
                }
            }
        }

        CCE_ProcessHideCommand(ControlList);
    }
}


//****** UnHide Start


function CCE_UnHide(pNameList, pIsExceptionList) 
{
    /*
    var List = new Array();
    for (var i = 0; i < pNameList.Length; i++)
    {
    List.Add(pControlList[i]);
    }*/

    try 
    {
        if (pIsExceptionList) 
        {
            CCE_ProcessUnHideExceptCommand(pNameList);

        }
        else 
        {
            CCE_ProcessUnHideCommand(pNameList);
        }
    }
    catch (ex) 
    {

    }
}


/// <summary>
/// Processes the Hide command for check code execution
/// </summary>
/// <param name="checkCodeList">A list of fields to hide</param>
function CCE_ProcessUnHideCommand(pCheckCodeList) 
{
    if (pCheckCodeList != null) 
    {
        //var controlsList = GetAssociatedControls(pCheckCodeList);
        //this.canvas.HideCheckCodeItems(controlsList);
        for (var i = 0; i < pCheckCodeList.length; i++) 
        {

            var query = '#mvcdynamicfield_' + pCheckCodeList[i];
            $(query).show();
            query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
            $(query).show();

            CCE_RemoveFromFieldsList(pCheckCodeList[i], 'HiddenFieldsList');
        }
    }
}


/// <summary>
/// Processed the Hide Except command for check code execution
/// </summary>
/// <param name="checkCodeList">A list of fields</param>
function CCE_ProcessUnHideExceptCommand(pCheckCodeList)
{
    if (pCheckCodeList != null)
    {

        var ControlList = new Array();

        for(var i in cce_Context.symbolTable)
        {
            var symbol = cce_Context.symbolTable[i];
            var symbol_name = symbol.Name.toLowerCase();
            if(symbol.Source == "datasource")
            {
                var isFound = false;
                for(var j = 0; j < pCheckCodeList.length; j++)
                {
                    if(pCheckCodeList[j].toLowerCase() == symbol_name)
                    {
                        isFound = true;
                        break;
                    }
                }

                if(! isFound)
                {
                    ControlList.push(symbol_name);
                }
            }
        }

        CCE_ProcessUnHideCommand(ControlList);
    }
}
//****** UnHide End


function CCE_Like(pLHS, pRHS)
{
 
var result = false;
var testValue = "^" + (pRHS.toString()).replace("*", "\\w*") + "$";
var re = new RegExp(testValue,"i");
if (pLHS.match(re))
 {
  result = true;
 }
else
 {
  result = false;
 }
 return result;
}


function CCE_Context() 
{
    this.symbolTable = new Array();
}

 
CCE_Context.prototype.define = function (pName, pType, pSource, pPage, pValue) 
{
    this.symbolTable[pName.toLowerCase()] = new CCE_Symbol(pName.toLowerCase(), pType.toLowerCase(), pSource.toLowerCase(), pPage.toLowerCase(), pValue);
}

CCE_Context.prototype.resolve = function (pName) 
{
    var cce_Symbol = this.symbolTable[pName.toLowerCase()];

    return cce_Symbol;
}

CCE_Context.prototype.getValue = function (pName) {
    var cce_Symbol = this.resolve(pName);
    if (cce_Symbol != null) {
        if (cce_Symbol.Source == "datasource") {
            var query = '#mvcdynamicfield_' + pName;
            var fielName = 'mvcdynamicfield_' + pName;
            if (eval(document.getElementById(fielName))) {
                var field = $(query);
                switch (cce_Symbol.Type) {
                    case "yesno":
                        if (field.val() == "1") {
                            return true; //"Yes";
                        }
                        else {
                            return false; // "No";
                        }
                    case "checkbox":
                        if (field.is(':checked')) {
                            return true;
                        }
                        else {
                            return false;
                        }

                    case "datepicker": //string has been converted to date for comparison with another date
                        var datefield = new Date(field.val()).valueOf();
                        return datefield;
                    case "timepicker":
                        var refDate = "01/01/1970 ";//It is a reference date 
                        var dateTime = refDate + field.val();
                        var timefield = new Date(dateTime).valueOf();
                        return timefield;
                    case "numeric": //string has been converted to number to compare with another number
                        var numericField = new Number(field.val()).valueOf();
                        return numericField;
                    default:
                        return field.val();
                }
            }
            else {
                return cce_Symbol.Value;
            }
        }
        else {
            return cce_Symbol.Value;
        }
    }
    else {
        return null;
    }
}


CCE_Context.prototype.setValue = function (pName, pValue) 
{
    var cce_Symbol = this.resolve(pName);
    if (cce_Symbol != null) 
    {
        cce_Symbol.Value = pValue;

        var Jquery = '#mvcdynamicfield_' + pName;
        var FieldName = 'mvcdynamicfield_' + pName;
        if (eval(document.getElementById(FieldName)))
        {
            if(cce_Symbol.Source == "datasource")
                {
                   
                    $(Jquery).val(pValue);
                    cce_Symbol.Value = pValue;
                }
        }
        else
        {
        
        updateXml(pName, pValue) ;
        
        }
    }
}

function CCE_Symbol(pName, pType, pSource, pPageNumber, pValue) 
{
      this.Name = pName;
      this.Type = pType;
      this.Source = pSource;
      this.PageNumber = pPageNumber;
      this.Value = pValue;
}


function CCE_SymbolTable()
{
        this._name;
        this._parent;
        this._SymbolList = new Array();

}

function Rule_Begin_After_Statement() 
{
    this.Statements = null;
}

Rule_Begin_After_Statement.prototype.Execute = function ()
{
    return this.Statements.Execute();
}

function Rule_Hide() 
{
    this.IdentifierList, this.IsExceptList
}

Rule_Hide.prototype.Execute = function ()
{
    CCE_Hide(this.IdentifierList, this.IsExceptList)
}






 /******************Highlight and unhighlight controls********************************/

 /*-----------Highlight--------------*/
 function CCE_Highlight(pNameList, pIsExceptionList) 
 {
      try 
      {
         if (pIsExceptionList) 
         {
             CCE_ProcessHighlightExceptCommand(pNameList);
         }
         else 
         {
             CCE_ProcessHighlightCommand(pNameList);
         }
     }
     catch (ex) 
     {

     }
 }


 /// <summary>
 /// Processes the Highlight command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields to highlight</param>
 function CCE_ProcessHighlightCommand(pCheckCodeList)
 {
     if (pCheckCodeList != null) 
     {
         for (var i = 0; i < pCheckCodeList.length; i++) {

             var query = '#mvcdynamicfield_' + pCheckCodeList[i];
             $(query).css("background-color","yellow");
             query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
            // $(query).hide();// no need to highlight the label
             //CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
             CCE_AddToFieldsList(pCheckCodeList[i], 'HighlightedFieldsList');
         }
     }
 }


 /// <summary>
 /// Processed the Highlight Except command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields</param>
 function CCE_ProcessHighlightExceptCommand(pCheckCodeList) 
 {
    if (pCheckCodeList != null)
    {

        var ControlList = new Array();

        for(var i in cce_Context.symbolTable)
        {
            var symbol = cce_Context.symbolTable[i];
            var symbol_name = symbol.Name.toLowerCase();
            if(symbol.Source == "datasource")
            {
                var isFound = false;
                for(var j = 0; j < pCheckCodeList.length; j++)
                {
                    if(pCheckCodeList[j].toLowerCase() == symbol_name)
                    {
                        isFound = true;
                        break;
                    }
                }

                if(! isFound)
                {
                    ControlList.push(symbol_name);
                }
            }
        }

        CCE_ProcessHighlightCommand(ControlList);
    }
 }

 
 /*-----------------Un Highlight-----------------------------------*/

 function CCE_UnHighlight(pNameList, pIsExceptionList) 
 {
     try 
     {
         if (pIsExceptionList) 
         {
             CCE_ProcessUnHighlightExceptCommand(pNameList);
         }
         else 
         {
             CCE_ProcessUnHighlightCommand(pNameList);
         }
     }
     catch (ex) 
     {

     }
 }


 /// <summary>
 /// Processes the Un-highlight command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields to un-highlight</param>
 function CCE_ProcessUnHighlightCommand(pCheckCodeList) 
 {
     if (pCheckCodeList != null) 
     {
         for (var i = 0; i < pCheckCodeList.length; i++) 
         {

             var query = '#mvcdynamicfield_' + pCheckCodeList[i];
             $(query).css("background-color", "white");
             query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
             //CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
             CCE_RemoveFromFieldsList(pCheckCodeList[i], 'HighlightedFieldsList');
         }
     }
 }


 /// <summary>
 /// Processed the un-highlight Except command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields</param>
 function CCE_ProcessUnHighlightExceptCommand(pCheckCodeList) 
 {
    if (pCheckCodeList != null)
    {

        var ControlList = new Array();

        for(var i in cce_Context.symbolTable)
        {
            var symbol = cce_Context.symbolTable[i];
            var symbol_name = symbol.Name.toLowerCase();
            if(symbol.Source == "datasource")
            {
                var isFound = false;
                for(var j = 0; j < pCheckCodeList.length; j++)
                {
                    if(pCheckCodeList[j].toLowerCase() == symbol_name)
                    {
                        isFound = true;
                        break;
                    }
                }

                if(! isFound)
                {
                    ControlList.push(symbol_name);
                }
            }
        }

        CCE_ProcessUnHighlightCommand(ControlList);
    }
 }


 /****Add to highlighted control list********************/

 /////////////////////////////////////////////////////////////////////////////////////

 /******************Enable and disable controls********************************/

 /*-----------Disable--------------*/
 function CCE_Disable(pNameList, pIsExceptionList) 
 {
     try 
     {
         if (pIsExceptionList) 
         {
             CCE_ProcessDisableExceptCommand(pNameList);
         }
         else
         {
             CCE_ProcessDisableCommand(pNameList);
         }
     }
     catch (ex) 
     {

     }
 }


 /// <summary>
 /// Processes the Disable command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields to disable</param>
 function CCE_ProcessDisableCommand(pCheckCodeList) 
 {
     if (pCheckCodeList != null) 
     {
         //var controlsList = GetAssociatedControls(pCheckCodeList);
         //this.canvas.HideCheckCodeItems(controlsList);
         for (var i = 0; i < pCheckCodeList.length; i++) 
         {
             var query = '#mvcdynamicfield_' + pCheckCodeList[i];
             $(query).attr('disabled', 'disabled');
             query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
             // $(query).hide();// no need to disable the label
             // CCE_AddToDisabledFieldsList(pCheckCodeList[i]);
              CCE_AddToFieldsList(pCheckCodeList[i], 'DisabledFieldsList');
         }
     }
 }


 /// <summary>
 /// Processed the Disable Except command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields</param>
 function CCE_ProcessDisableExceptCommand(pCheckCodeList) 
 {
    if (pCheckCodeList != null)
    {

        var ControlList = new Array();

        for(var i in cce_Context.symbolTable)
        {
            var symbol = cce_Context.symbolTable[i];
            var symbol_name = symbol.Name.toLowerCase();
            if(symbol.Source == "datasource")
            {
                var isFound = false;
                for(var j = 0; j < pCheckCodeList.length; j++)
                {
                    if(pCheckCodeList[j].toLowerCase() == symbol_name)
                    {
                        isFound = true;
                        break;
                    }
                }

                if(! isFound)
                {
                    ControlList.push(symbol_name);
                }
            }
        }

        CCE_ProcessDisableCommand(ControlList);
    }
 }


 /*-----------------Enable-----------------------------------*/

 function CCE_Enable(pNameList, pIsExceptionList) 
 {
     try 
     {
         if (pIsExceptionList) 
         {
             CCE_ProcessEnableExceptCommand(pNameList);
         }
         else 
         {
             CCE_ProcessEnableCommand(pNameList);
         }
     }
     catch (ex) 
     {

     }
 }


 /// <summary>
 /// Processes the Enable command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields to enable</param>
 function CCE_ProcessEnableCommand(pCheckCodeList) 
 {
     if (pCheckCodeList != null) 
     {
         for (var i = 0; i < pCheckCodeList.length; i++) 
         {
             var query = '#mvcdynamicfield_' + pCheckCodeList[i];
             $(query).removeAttr('disabled');
             query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
             //$(query).show();// don't do anything with label
             // CCE_AddToDisabledFieldsList(pCheckCodeList[i]);
             CCE_RemoveFromFieldsList(pCheckCodeList[i], 'DisabledFieldsList');
         }
     }
 }


 /// <summary>
 /// Processed the enable Except command for check code execution
 /// </summary>
 /// <param name="checkCodeList">A list of fields</param>
 function CCE_ProcessEnableExceptCommand(pCheckCodeList) 
 {
    if (pCheckCodeList != null)
    {

        var ControlList = new Array();

        for(var i in cce_Context.symbolTable)
        {
            var symbol = cce_Context.symbolTable[i];
            var symbol_name = symbol.Name.toLowerCase();
            if(symbol.Source == "datasource")
            {
                var isFound = false;
                for(var j = 0; j < pCheckCodeList.length; j++)
                {
                    if(pCheckCodeList[j].toLowerCase() == symbol_name)
                    {
                        isFound = true;
                        break;
                    }
                }

                if(! isFound)
                {
                    ControlList.push(symbol_name);
                }
            }
        }

        CCE_ProcessEnableCommand(ControlList);
    }
 }


 /****Add to disabled control list********************/

// function CCE_AddToDisabledFieldsList(FieldName) 
// {
//     if (document.getElementById("DisabledFieldsList").value != "") 
//     {
//         document.getElementById("DisabledFieldsList").value += ",";
//     }
//     document.getElementById("DisabledFieldsList").value += FieldName;
// }


//  
// function CCE_AddToHilightedFieldsList(FieldName) 
// {
//     if (document.getElementById("HighlightedFieldsList").value != "") 
//     {
//         document.getElementById("HighlightedFieldsList").value += ",";
//     }
//     document.getElementById("HighlightedFieldsList").value += FieldName;

// }


//function CCE_AddToHiddenFieldsList(FieldName) 
//{
//    debugger;
//var HiddenFieldsList =document.getElementById("HiddenFieldsList").value.toString()
//if (HiddenFieldsList != "" && HiddenFieldsList.indexOf(FieldName.toString())== -1)
//    
//    {
//        document.getElementById("HiddenFieldsList").value += ",";
//    }
//    if (  HiddenFieldsList.indexOf(FieldName.toString())== -1) {
//        document.getElementById("HiddenFieldsList").value += FieldName;
//    }
//}
 
function CCE_AddToFieldsList(FieldName,ListName)
{
   
    var List = document.getElementById(ListName).value.toString()
    var ListArray = List.split(',');
    var Counter = 0;
    for (var i = 0; i < ListArray.length; i++)
     {

        if (ListArray[i] == FieldName) 
        {
            Counter ++;
        }

       
    }

    if (Counter == 0) {
        ListArray.push(FieldName);
    }
    document.getElementById(ListName).value = ListArray.join(",");
}


function CCE_RemoveFromFieldsList(FieldName,ListName) {

    
    var list = document.getElementById(ListName).value;
    var ListArray = list.split(',');
    var newList = new Array();

    for (var i = 0; i < ListArray.length; i++) 
    {
        if (ListArray[i] != FieldName && ListArray[i] != "")
        {
            newList.push(ListArray[i]);
        }
    }

    document.getElementById(ListName).value  = newList.join(",");

 }

 //Clear the control value
 function CCE_ClearControlValue(controlId) 
 {
    
    //if control is a check box uncheck it otherwise clear the control value
     if ($(controlId).attr('type') == 'checkbox') 
     {
         $(controlId).attr('checked', false);
     }
     else 
     {
         $(controlId).val('');
     }
 }

 //Go to a page or focus on a control on the same page

 function CCE_GoToControlOrPage(controlOrPage) 
 {
     if (parseInt(controlOrPage) == controlOrPage) 
     {
         var currentUrl = window.location.href;
         //get the url in the format of http://localhost/<Server>/survey/<ResponseID>
         currentUrl = processUrl(currentUrl, 'RedirectionUrl', "");
         $("#myform")[0].action = currentUrl + "/" + controlOrPage;
         //detach the validation engine as we don't want to validate data to go to another page as directed by check code
         $('#myform').validationEngine('detach');
         $("#myform").submit();
     }
     else 
     {
         var controlId = '#mvcdynamicfield_' + controlOrPage.toLowerCase();
         $(controlId).focus();
          
     }
 }


function CCE_Year(pValue) 
{
    // assumes pValue is a Javascript Date object
    return pValue.getFullYear();
}

function CCE_Years(pValue1, pValue2)
{
    var result = pValue2.getFullYear() - pValue1.getFullYear();
    if
    (
        param2.Month < param1.Month ||
        (pValue2.getMonth() == pValue1.getMonth() && pValue2.getDate() < pValue1.getDate())
    )
    {
        result--;
    }
    return result;
}



cce_Context = new CCE_Context();
