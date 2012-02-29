

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
            
            var query = '#MvcDynamicField_' + pCheckCodeList[i];
            $(query).hide();
            query = '#LabelMvcDynamicField_' + pCheckCodeList[i];
            $(query).hide();
             CCE_AddToHiddenFieldsList(pCheckCodeList[i]);
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
        var controlsList = GetAssociatedControls(pCheckCodeList);
        //this.canvas.HideExceptCheckCodeItems(controlsList);
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

            var query = '#MvcDynamicField_' + pCheckCodeList[i];
            $(query).show();
            query = '#LabelMvcDynamicField_' + pCheckCodeList[i];
            $(query).show();
            CCE_AddToHiddenFieldsList(pCheckCodeList[i]);
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
        var controlsList = GetAssociatedControls(pCheckCodeList);
        //this.canvas.HideExceptCheckCodeItems(controlsList);
    }
}





//****** UnHide End

function CCE_Context() 
{
    this.symbolTable = new Array();
}

CCE_Context.prototype.resolve = function (pName) 
{
    var query = '#MvcDynamicField_' + pName;
    return $(query);
}


CCE_Context.prototype.getValue = function (pName) 
{
    var field = this.resolve(pName);
    if (field != null) 
    {
        return field.val();
    }
    else 
    {
        return null;
    }
}


CCE_Context.prototype.setValue = function (pName, pValue) 
{
    var field = this.resolve(pName);
    if (field != null) 
    {
        field.val(pValue);
    }
}



cce_Context = new CCE_Context();

function CCE_Symbol() 
{
      this.Name = new String();
      this.Type = new String();
      this.Rule = null;
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

             var query = '#MvcDynamicField_' + pCheckCodeList[i];
             $(query).css("background-color","yellow");
             query = '#LabelMvcDynamicField_' + pCheckCodeList[i];
            // $(query).hide();// no need to highlight the label
            CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
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
         var controlsList = GetAssociatedControls(pCheckCodeList);
         //this.canvas.HideExceptCheckCodeItems(controlsList);
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

             var query = '#MvcDynamicField_' + pCheckCodeList[i];
             $(query).css("background-color", "white");
             query = '#LabelMvcDynamicField_' + pCheckCodeList[i];
             CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
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
         var controlsList = GetAssociatedControls(pCheckCodeList);
         //this.canvas.HideExceptCheckCodeItems(controlsList);
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
             var query = '#MvcDynamicField_' + pCheckCodeList[i];
             $(query).attr('disabled', 'disabled');
             query = '#LabelMvcDynamicField_' + pCheckCodeList[i];
             // $(query).hide();// no need to disable the label
             CCE_AddToDisabledFieldsList(pCheckCodeList[i]);
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
         var controlsList = GetAssociatedControls(pCheckCodeList);
         //this.canvas.HideExceptCheckCodeItems(controlsList);
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
             var query = '#MvcDynamicField_' + pCheckCodeList[i];
             $(query).removeAttr('disabled');
             query = '#LabelMvcDynamicField_' + pCheckCodeList[i];
             //$(query).show();// don't do anything with label
             CCE_AddToDisabledFieldsList(pCheckCodeList[i]);
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
         var controlsList = GetAssociatedControls(pCheckCodeList);
         //this.canvas.HideExceptCheckCodeItems(controlsList);
     }
 }


 /****Add to disabled control list********************/

 function CCE_AddToDisabledFieldsList(FieldName) 
 {
     if (document.getElementById("DisabledFieldsList").value != "") 
     {
         document.getElementById("DisabledFieldsList").value += ",";
     }
     document.getElementById("DisabledFieldsList").value += FieldName;
 }


  
 function CCE_AddToHilightedFieldsList(FieldName) 
 {
     if (document.getElementById("HighlightedFieldsList").value != "") 
     {
         document.getElementById("HighlightedFieldsList").value += ",";
     }
     document.getElementById("HighlightedFieldsList").value += FieldName;

 }


function CCE_AddToHiddenFieldsList(FieldName) {
    debugger;
var HiddenFieldsList =document.getElementById("HiddenFieldsList").value.toString()
if (HiddenFieldsList != "" && HiddenFieldsList.indexOf(FieldName.tostring())!= -1)
    if (HiddenFieldsList != "" && HiddenFieldsList.indexOf(FieldName.tostring())) 
    {
        document.getElementById("HiddenFieldsList").value += ",";
    }
    if (  HiddenFieldsList.indexOf(FieldName.tostring())!= -1) {
        document.getElementById("HiddenFieldsList").value += FieldName;
    }
}





