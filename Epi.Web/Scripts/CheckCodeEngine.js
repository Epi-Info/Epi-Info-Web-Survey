

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

function CCE_GetAssociatedControls()
{
    // will return a list of controls associated with a field name
}


function CCE_Context() 
{

}

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







function CCE_SymbolTable
{
        this._name;
        this._parent;
        this._SymbolList = new Array();

}
