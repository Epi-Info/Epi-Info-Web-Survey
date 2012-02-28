

function CCE_Hide(pNameList, pIsExceptionList)
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
             AddToHiddenFieldsList(pCheckCodeList[i]);
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
            AddToHiddenFieldsList(pCheckCodeList[i]);
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

function CCE_Symbol(pName, pType) 
{
      this.Name = pName;
      this.Type = pType;
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
function AddToHiddenFieldsList(FieldName)
 {
    if (document.getElementById("HiddenFieldsList").value !="") 
    {

        document.getElementById("HiddenFieldsList").value += ",";
    
    }
    document.getElementById("HiddenFieldsList").value += FieldName;
    
 }
