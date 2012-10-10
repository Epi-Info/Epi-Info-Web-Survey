

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
            var symbol = cce_Context.resolve(pCheckCodeList[i]);
            var query = null;
            if(symbol.Type == "radiobutton")
            {
                query = '.mvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                    $(query).hide();    
                });

                query = '.labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                    $(query).hide();
                });

                 query = '#mvcdynamicfield_' + pCheckCodeList[i]+"_groupbox";
                $(query).each(function(i, obj) 
                {
                    $(query).hide();
                });
            }
            else
            {
                query = '#mvcdynamicfield_' + pCheckCodeList[i];
                //clear the control value before hiding
                CCE_ClearControlValue(query);

                $(query).hide();
                query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).hide();
            }
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
        var RadioControlList = new Array();

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
            var symbol = cce_Context.resolve(pCheckCodeList[i]);
            var query = null;
           
            if(symbol.Type == "radiobutton")
            {
             
                query = '.mvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                  // $(query).css("visibility", "	visible");
                   $(query).css("dispaly", "	inline"); 
                   $(query).show();
                });

                query = '.labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                  // $(query).css("visibility", "	visible");
                  $(query).css("dispaly", "	inline");
                   $(query).show();
                });
                 query = '#mvcdynamicfield_' + pCheckCodeList[i]+"_groupbox";
                $(query).each(function(i, obj) 
                {
                    $(query).show();
                });
            }
            else
            {
                query = '#mvcdynamicfield_' + pCheckCodeList[i];
                $(query).show();
                query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).show();
            }
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

CCE_Context.prototype.getValue = function (pName) 
{
    var value = null;
    var cce_Symbol = this.resolve(pName);
    if (cce_Symbol != null) 
    {
        if (cce_Symbol.Source == "datasource") 
        {
            var query = '#mvcdynamicfield_' + pName;
            var fieldName = 'mvcdynamicfield_' + pName;
            if (eval(document.getElementById(fieldName))) 
            {
                var field = $(query);
                switch (cce_Symbol.Type) 
                {
                    case "yesno":
                        if (field.val() == "1") 
                        {
                            return true; //"Yes";
                        }
                        else 
                        {
                            return false; // "No";
                        }
                    case "checkbox":
                        if (field.is(':checked')) 
                        {
                            return true;
                        }
                        else 
                        {
                            return false;
                        }

                    case "datepicker": //string has been converted to date for comparison with another date
                        value = new Date(field.val()).valueOf();
                        return value;
                    case "timepicker":
                        var refDate = "01/01/1970 ";//It is a reference date 
                        var dateTime = refDate + field.val();
                        value = new Date(dateTime).valueOf();
                        return value;
                    case "numeric": //string has been converted to number to compare with another number
                        value = new Number(field.val()).valueOf();
                        return value;
                    case "radiobutton":
                        var RadiofieldName = '.' + fieldName;
                        value = -1; 
                        $(RadiofieldName).each(function(i, obj) 
                        {
                            if ($(this).is(':checked'))
                            {
                              value = new Number($(this).val()).valueOf();
                            }
                        });
                        return value;
                    default:
                        return field.val();
                }
            }
            else 
            {
                return cce_Symbol.Value;
            }
        }
        else 
        {
            return cce_Symbol.Value;
        }
    }
    else 
    {
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

                switch (cce_Symbol.Type) 
                {
                   case "datepicker": //string has been converted to date for comparison with another date
                        $(Jquery).datepicker("setDate", new Date(pValue));
                        cce_Symbol.Value = pValue;
                        break;
                    case "timepicker":
                        //$(Jquery).timepicker("setTime", new Date(pValue));
                        //cce_Symbol.Value = pValue;
                        //break;
                   default:
                        $(Jquery).val(pValue);
                        cce_Symbol.Value = pValue;
                        break;
                }
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
           var symbol = cce_Context.resolve(pCheckCodeList[i]);
           if(symbol.Type == "radiobutton")
            {
                query = '.labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                    $(query).css("background-color","yellow");
                });
            }
            else
            {

             $(query).css("background-color","yellow");
             query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
            // $(query).hide();// no need to highlight the label
             //CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
            
             }
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
          var symbol = cce_Context.resolve(pCheckCodeList[i]);
           if(symbol.Type == "radiobutton")
            {
                query = '.labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                    $(query).css("background-color","white");
                });
            }
            else
            {
                 $(query).css("background-color", "white");
                 query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                 //CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
             }
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
             var query = null;  
             var symbol = cce_Context.resolve(pCheckCodeList[i]);
            if(symbol.Type == "radiobutton")
            {
                query = '.mvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                    $(query).attr('disabled', 'disabled');
                });
            }
            else
            {
                query = '#mvcdynamicfield_' + pCheckCodeList[i];
                $(query).attr('disabled', 'disabled');
                query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
            }
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
             var query = null;

             var symbol = cce_Context.resolve(pCheckCodeList[i]);
            if(symbol.Type == "radiobutton")
            {
                query = '.mvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                     $(query).removeAttr('disabled');
                });
            }
            else
            {
                query = '#mvcdynamicfield_' + pCheckCodeList[i];
                $(query).removeAttr('disabled');
                query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
            }
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
     var control = "#mvcdynamicfield_"+ controlId
     if ($(control).attr('type') == 'checkbox') 
     {
         $(control).attr('checked', false);
     }
     else 
     {
         $(control).val('');
     }

     CCE_Context.setValue(controlId,'');
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
         $("#myform")[0].is_goto_action.value = 'true';
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

function isValidDate(pValue)
{
    var result = false;
    if ( Object.prototype.toString.call(d) === "[object Date]" ) 
    {
        result = true;
    }
    return result;
}

function CCE_Year(pValue) 
{

    if(isValidDate(pValue))
    {
        return pValue.getFullYear();
    }
    else
    {
        return new Date(pValue).getFullYear();
    }
}

function CCE_Years(pValue1, pValue2)
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

    var result = date2.getFullYear() - date1.getFullYear();
    if
    (
        date2.getMonth() < date1.getMonth() ||
        (date2.getMonth() == date1.getMonth() && date2.getDate() < date1.getDate())
    )
    {
        result--;
    }
    return result;
}


function CCE_Month(pValue) 
{

    if(isValidDate(pValue))
    {
        return pValue.getMonth();
    }
    else
    {
        return new Date(pValue).getMonth();
    }
    
}

function CCE_Months(pValue1, pValue2) 
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

    var result = 12 * (date2.getFullYear() - date1.getFullYear()) + date2.getMonth() - date1.getMonth();

    if (date2.getDate() < date1.getDate())
    {
        result--;
    }
    return result;
}

function CCE_Round(pValue1, pValue2) 
{
    var RoundModifier = 10 * pValue2;

    var result = Math.round(pValue1 * RoundModifier) / RoundModifier;
    return result;
}

function CCE_Day(pValue) 
{

    if(isValidDate(pValue))
    {
        return pValue.getDate();
    }
    else
    {
        return new Date(pValue).getDate();
    }
    
}

function CCE_Days(pValue1, pValue2) 
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

    var oneDay = 24*60*60*1000; // hours*minutes*seconds*milliseconds

    var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneDay)));

    return result;
}

function CCE_Substring(pValue1, pValue2, pValue3) 
{
        var result = null;
        var fullString = null;
        var startIndex = 0;
        var length = 0;

        fullString = new String(pValue1);
        startIndex = new Number(pValue2);
        length = new Number(pValue3);

        if (!(fullString == null && fullString !=""))
        {
            if (fullString.ToString().Length >= startIndex - 1 + length)
            {
                result = fullString.substring(startIndex - 1, length);
            }
        }

        return result;
}


function CCE_Truncate(pValue)
{
    return pValue | 0; // bitwise operators convert operands to 32-bit integers
}

function CCE_SystemDate()
{
    return new Date();
}


function CCE_SystemTime()
{
    return new Date().getTime();
}



function CCE_Hour(pValue) 
{

    if(isValidDate(pValue))
    {
        return pValue.getHours();
    }
    else
    {
        return new Date(pValue).getHours();
    }
    
}

function CCE_Hours(pValue1, pValue2) 
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

    var oneHour = 60*60*1000; // minutes * seconds*milliseconds

    var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneHour)));

    return result;
}


function CCE_Minute(pValue) 
{

    if(isValidDate(pValue))
    {
        return pValue.getMinutes();
    }
    else
    {
        return new Date(pValue).getMinutes();
    }
    
}

function CCE_Minutes(pValue1, pValue2) 
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

    var oneMinute = 60*1000; // seconds*milliseconds

    var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneMinute)));

    return result;
}


function CCE_Second(pValue) 
{

    if(isValidDate(pValue))
    {
        return pValue.getSeconds();
    }
    else
    {
        return new Date(pValue).getSeconds();
    }
    
}

function CCE_Seconds(pValue1, pValue2) 
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

    var oneSecond = 1000; // milliseconds

    var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneSecond)));

    return result;
}


function CCE_DateDiff(pValue1, pValue2) 
{
    var date1 = new Date(pValue1);
    var date2 = new Date(pValue2);

     var result = date1.getTime() - date2.getTime();

    return result;
}


function CCE_DatePart(pValue1) 
{

    return null;
}


function OpenVideoDialog()
{
    $("#VideoDialog").dialog("open");
}



/////////////////Simple  Dialogbox //////////////////////
function CCE_ContextOpenSimpleDialogBox(Title,Prompt) 
{
        $('#ui-dialog-title-SimpledialogBox').text(Title.toString());
        $('#SimpleDialogBoxPrempt').text(Prompt.toString());
        $('#SimpleDialogBoxButton').text('Ok');
        $("#SimpleDialogBox").dialog("open");
}

function CCE_CloseSimpleDialogBox() 
{
        $('#SimpleDialogBox').dialog("close");
}

/////////////////  Dialogbox ///////////////////////////////////
function CCE_ContextOpenDialogBox(Title,MaskOpt,Identifier,Prompt) 
{
   
        $('#ui-dialog-title-DialogBox').text(Title.toString());
        $('#DialogBoxPrempt').text(Prompt.toString());
        $('#DialogBoxOkButton').text('Ok');
        $('#DialogBoxInput').datepicker( "hide" );
         
        $("#DialogBox").dialog("open");
        $('#ui-timepicker-div').hide();

        if(CCE_GetMaskedPattern(MaskOpt.toLocaleString()).toString() != "")
        {
            $('#DialogBoxInput').mask( CCE_GetMaskedPattern(MaskOpt.toLocaleString()).toString()); 
        }

        if(MaskOpt.toString() == "YN")
        {
            $('#DialogBoxOkButton').hide();
            $('#DialogBoxCancelButton').hide();
            $('#DialogBoxInput').hide();
            $('#YesButton').show();
            $('#NoButton').show();
        }
        else
        {
            $('#DialogBoxOkButton').show();
            $('#DialogBoxCancelButton').show();
            $('#YesButton').hide();
            $('#NoButton').hide();
        }
        $('#DialogBoxHiddenField').val(Identifier); 
        $('#DialogBoxType').val(CCE_GetDialogType(MaskOpt)); 
        $('#DialogBoxInput').datepicker( "hide" );
        $('#ui-timepicker-div').hide();
          
}

function  CCE_DialogBoxOkButton_Click()
{
        var FieldName = '#mvcdynamicfield_'+ $('#DialogBoxHiddenField').val().toString();
        var value =  $('#DialogBoxInput').val();
        $(FieldName).val(value.toString());
        $('#DialogBoxInput').val('');
        $('#DialogBox').dialog("close");
        $('#DialogBoxInput').datepicker( "hide" );
        $('#ui-timepicker-div').hide();
}

function CCE_CloseDialogBox() 
{
        $('#DialogBox').dialog("close");
}

function CCE_GetDateTimePicker()
{
    if($('#DialogBoxType').val() =="Time")
    {
        $('#DialogBoxInput').timepicker({showSecond:true,timeFormat: 'hh:mm:ss'});
    }

    if($('#DialogBoxType').val() =="Date")
    {
        $('#DialogBoxInput').datepicker({changeMonth: true,changeYear: true});
    }
}

function CCE_GetDialogType(Mask)
{

    var dialogType= "";
    switch (Mask)
    {
                
        case "MM-DD-YYYY":
            dialogType = "Date";
            break;
        case "DD-MM-YYYY":
            dialogType = "Date";
            break;
        case "YYYY-MM-DD":
            dialogType = "Date";
            break;
        case "HH:MM AMPM":
            dialogType = "Time";
            break;
                    
                   
    }
    return dialogType;

}

 function  CCE_GetMaskedPattern(pattern)
{
    var maskedPattern = "";
    switch (pattern)
    {
        case "#":
            maskedPattern = "9";
            break;
        case "##":
            maskedPattern = "99";
            break;
        case "###":
            maskedPattern = "999";
            break;
        case "####":
            maskedPattern = "9999" ;
            break;
        case "##.##":
            maskedPattern = "99.99";
            break;
        case "##.###":
            maskedPattern = "99.999";
            break;
        case "###-###-###-####":
            maskedPattern = "999-999-999-9999";
            break;
        case "###-####":
            maskedPattern = "999-9999";
            break;
        case "###-###-####":
            maskedPattern = "999-999-9999";
            break;
            case "#-###-###-###-####":
            maskedPattern = "9-999-999-999-9999";
            break;

//                case "DD-MM-YYYY":
//                    maskedPattern = "99/99/9999";
//                    break;
//                case "YYYY-MM-DD":
//                    maskedPattern = "9999/99/99";
//                    break;
//               case "HH:MM AMPM":
//                    maskedPattern = "hh:mm:ss";
//                    break;
//                    
                   
    }
    return maskedPattern;
}

function  CCE_YesNoClick(Val)
{
    var FieldName = '#mvcdynamicfield_'+ $('#DialogBoxHiddenField').val().toString();
           
    if (Val=="Yes")
    {
        $(FieldName).val('True');
    }
    else
    {
        $(FieldName).val('False');
    }
    $('#DialogBox').dialog("close");
        
}


function CCE_StrLEN(pValue)
{
    if(pValue == null)
    {
        return 0;
    }
    else if(typeof(pValue) != "string")
    {
        return pValue.toString().length;
    }
    else
    {
        return pValue.length;
    }
}

cce_Context = new CCE_Context();
