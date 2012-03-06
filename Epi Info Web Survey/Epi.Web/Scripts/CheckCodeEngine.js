

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
        var controlsList = GetAssociatedControls(pCheckCodeList);
        //this.canvas.HideExceptCheckCodeItems(controlsList);
    }
}





//****** UnHide End

function CCE_Context() 
{
    this.symbolTable = new Array();
}


CCE_Context.prototype.define = function (pName, pType, pSource) 
{
    this.symbolTable[pName.toLowerCase()] = new CCE_Symbol(pName.toLowerCase(), pType.toLowerCase(), pSource.toLowerCase());
}

CCE_Context.prototype.resolve = function (pName) 
{
    var cce_Symbol = this.symbolTable[pName.toLowerCase()];

    return cce_Symbol;
}

CCE_Context.prototype.getValue = function (pName) 
{
    var cce_Symbol = this.resolve(pName);
    if (cce_Symbol != null) 
    {
        if(cce_Symbol.Source == "datasource")
        {
            var query = '#mvcdynamicfield_' + pName;
            var field = $(query);
            if(field != null)
            {
                if(cce_Symbol.Type == "yesno")
                {
                    if(field.val() == "1")
                    {
                        return "Yes";
                    }
                    else
                    {
                        return "No";
                    }
                }
                else
                {
                    return field.val();
                }
            }
            else
            {
                return null;
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
        if(cce_Symbol.Source == "datasource")
        {
            var query = '#mvcdynamicfield_' + pName;
            $(query).val(pValue);
            cce_Symbol.Value = pValue;
        }
        else
        {
            cce_Symbol.Value = pValue;
        }

        
    }
}

function CCE_Symbol(pName, pType, pSource, pValue) 
{
      this.Name = pName;
      this.Type = pType;
      this.Source = pSource;
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
         var controlsList = GetAssociatedControls(pCheckCodeList);
         //this.canvas.HideExceptCheckCodeItems(controlsList);
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



cce_Context = new CCE_Context();