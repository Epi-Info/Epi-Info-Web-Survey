

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
        $(".formError").remove();
        for (var i = 0; i < pCheckCodeList.length; i++) 
        {
//             var symbol = cce_Context.resolve(pCheckCodeList[i]);
//             var query = null;
//             query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";
//               if (!eval(document.getElementById("IsMobile")))
//               {
//                  $(query).css("display","none");
//               }else{
//                 $(query).css("visibility","collapse");
//               }
//             
//             if (!eval(document.getElementById("IsMobile"))){
//                 if (symbol.Type == "radiobutton")
//                 { 
//                 query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_groupbox_fieldWrapper";
//                 $(query).css("display","none");
//                 }
//             }
//             CCE_AddToFieldsList(pCheckCodeList[i], 'HiddenFieldsList')
  var cce_Symbol = cce_Context.resolve(pCheckCodeList[i]);
            var query = null;
            var fieldName = null;
            if (cce_Symbol != null) 
            {
                if (cce_Symbol.Source == "datasource") 
                {
                    switch (cce_Symbol.Type) 
                    {
                        case "label":
                            fieldName = 'mvcdynamicfield_' + pCheckCodeList[i]+ "_fieldWrapper";
                            if (eval(document.getElementById(fieldName))) 
                            {
                    
                                query = '#mvcdynamicfield_'  + pCheckCodeList[i]+ "_fieldWrapper" ;
                                if (!eval(document.getElementById("IsMobile"))){
                                  $(query).css("display","none");
                                  }else{
                                  $(query).css("visibility","collapse");
                                  
                                  }
                              
                            }
                            break;
                            case "groupbox":
                            //mvcdynamicfield_groupebox1_groupbox_fieldWrapper
                             fieldName = 'mvcdynamicfield_' + pCheckCodeList[i]+ "_groupbox_fieldWrapper";
                            if (eval(document.getElementById(fieldName))) 
                            {
                    
                                query = '#mvcdynamicfield_'  + pCheckCodeList[i]+ "_groupbox_fieldWrapper" ;
                                if (!eval(document.getElementById("IsMobile"))){
                                 $(query).css("display","none");
                                  }else{
                                  $(query).css("visibility","collapse");
                                  
                                  }
                              
                            }
                            break;
                        default:
                            fieldName = 'mvcdynamicfield_' + pCheckCodeList[i];
                            if (eval(document.getElementById(fieldName))) 
                            {
                                query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";
                                $(query).css("display","none");
                                if (!eval(document.getElementById("IsMobile"))){
                                    if (cce_Symbol.Type == "radiobutton")
                                    {
                                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_groupbox_fieldWrapper";
                                    $(query).css("display","none");
                                    }
                                } 
                            }
                        break;
                    }
                    
                }
            }


              
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
            var cce_Symbol = cce_Context.resolve(pCheckCodeList[i]);
            var query = null;
            var fieldName = null;
            if (cce_Symbol != null) 
            {
                if (cce_Symbol.Source == "datasource") 
                {
                    switch (cce_Symbol.Type) 
                    {
                        case "label":
                            fieldName = 'mvcdynamicfield_' + pCheckCodeList[i]+ "_fieldWrapper";
                            if (eval(document.getElementById(fieldName))) 
                            {
                    
                                query = '#mvcdynamicfield_'  + pCheckCodeList[i]+ "_fieldWrapper" ;
                                if (!eval(document.getElementById("IsMobile"))){
                                  $(query).css("display","block");
                                  }else{
                                  $(query).css("visibility","visible");
                                  
                                  }
                              
                            }
                            break;
                            case "groupbox":
                            //mvcdynamicfield_groupebox1_groupbox_fieldWrapper
                             fieldName = 'mvcdynamicfield_' + pCheckCodeList[i]+ "_groupbox_fieldWrapper";
                            if (eval(document.getElementById(fieldName))) 
                            {
                    
                                query = '#mvcdynamicfield_'  + pCheckCodeList[i]+ "_groupbox_fieldWrapper" ;
                              if (!eval(document.getElementById("IsMobile"))){
                                  $(query).css("display","block");
                                  }else{
                                  $(query).css("visibility","visible");
                                    $(query).css("display","");
                                  }
                              
                            }
                            break;
                        default:
                            fieldName = 'mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";//'mvcdynamicfield_' + pCheckCodeList[i];
                            if (eval(document.getElementById(fieldName))) 
                            {
                                query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";
                                  
                                 
                                  if (!eval(document.getElementById("IsMobile")))
                                  { 
                                      //Div
                                         query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";
                                         $(query).removeAttr("style");
                                       // Control
                                         query = '#mvcdynamicfield_'  + pCheckCodeList[i] ;
                                         $(query).css("display","block");
                                     
                                          //Label
                                         query = '#labelmvcdynamicfield_'  + pCheckCodeList[i] ;
                                         $(query).css("display","block");
                                     
                                   }
                                   else
                                   {   
                                      if (cce_Symbol.Type == "checkbox"){
                                       
                                        query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";
                                        $(query).css("display","block");
                                         $(query).find('.ui-checkbox').addClass('u');
                                          $(query).find('.ui-checkbox').removeClass('u');
                                           
                                        $(query).find('.ui-checkbox').removeClass('ui-disabled');

                                       $("input[type='checkbox']").checkboxradio("refresh");
                                        }
                                        else
                                        {
                                         query = '#mvcdynamicfield_'  + pCheckCodeList[i] + "_fieldWrapper";
                                        $(query).css("display","block"); 
                                        }
                                    }
                               } 


                                if (!eval(document.getElementById("IsMobile"))){
                                    if (cce_Symbol.Type == "radiobutton")
                                    {
                                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_groupbox_fieldWrapper";
                                     $(query).css("display","block");
                                    }
                                   
                                
                                } 
                            }
                       // break;
                    }
                    

                }

                 CCE_RemoveFromFieldsList(pCheckCodeList[i], 'HiddenFieldsList');
            }


              
           // CCE_RemoveFromFieldsList(pCheckCodeList[i], 'HiddenFieldsList');
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
                 value = field.val();
                switch (cce_Symbol.Type) 
                {
                    case "yesno":
                        if(field.val() == "") 
                        {
                           return null; 
                        }
                        else if (field.val() == "1") 
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
                        if (value == "" || isNaN(value))
                        {
                            return null;
                        }
                        else
                        {
                            return value;
                        }
                        return value;
                    case "timepicker":
                        var refDate = "01/01/1970 ";//It is a reference date 
                        var dateTime = refDate + field.val();
                        value = new Date(dateTime).valueOf();
                        if (value == "" || isNaN(value))
                        {
                            return null;
                        }
                        else
                        {
                            return value;
                        }
                        return value;
                    case "numeric": //string has been converted to number to compare with another number
                        value = new Number(field.val()).valueOf();
//                        if(value == "")
//                        {
//                            return null;
//                        }
//                        else
//                        {
//                            return value;
//                        }
                        return value;
                    case "radiobutton":
                       var RadiofieldName = "." + fieldName;
                         
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
                  /*  if(value == "")
                        {
                            return null;
                        }
                        else
                        {
                            return value;
                        }*/
                        
                        return value;
                        
                }
            }
            else 
            {
                switch (cce_Symbol.Type) {

                    case "checkbox":
                        if (cce_Symbol.Value == "Yes") {
                            return true;
                        }
                        else {
                            return false;
                        }
                    case "datepicker": //string has been converted to date for comparison with another date
                        value = new Date(cce_Symbol.Value).valueOf();
                        if (value == "" || isNaN(value)) {
                            return null;
                        }
                        else {
                            return value;
                        }
                        return value;
                    case "timepicker":
                        var refDate = "01/01/1970 ";//It is a reference date 
                        var dateTime = refDate + cce_Symbol.Value;
                        value = new Date(dateTime).valueOf();
                        if (value == "" || isNaN(value)) {
                            return null;
                        }
                        else {
                            return value;
                        }
                        return value;

                    default:

                        return cce_Symbol.Value;

                }
                
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


CCE_Context.prototype.setValue = function (pName, pValue) {
    var cce_Symbol = this.resolve(pName);

    if (cce_Symbol != null) {
        cce_Symbol.Value = pValue;

        var Jquery = '#mvcdynamicfield_' + pName;
        var FieldName = 'mvcdynamicfield_' + pName;
        var IsHidden = false;

        if (!eval(document.getElementById(FieldName))) {
            CreateHiddenField(FieldName, cce_Symbol.Type);
        }
        var ControlType = $(Jquery).attr('type');

        if (ControlType == 'hidden') {
            IsHidden = true;
        }
        // setTimeout(function () { 
        if (cce_Symbol.Source == "datasource") {
            switch (cce_Symbol.Type) {
                case "datepicker": //string has been converted to date for comparison with another date

                    //  if (eval(document.getElementById("IsMobile"))) {
                    var FormatedDate;
                    if (cce_Symbol.Value != null && cce_Symbol.Value !="")
                    {
                   var date = new Date(cce_Symbol.Value);
                    FormatedDate = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
                    cce_Symbol.Value = FormatedDate;
                       } 
                        $(Jquery).val(FormatedDate);
                    //}
                    //else {
                    
                            
                    //        $(Jquery).datepicker("setDate", new Date(pValue));
                    
                    //    cce_Symbol.Value = pValue;
                    //}

                    break;

                case "timepicker":

                    if (eval(document.getElementById("IsMobile"))) {
                        var FormatedTime;
                        var date = new Date(cce_Symbol.Value);
                        FormatedTime = FormatTime(date);
                        $(Jquery).val(FormatedTime);
                        cce_Symbol.Value = FormatedTime;
                    }
                    else {
                        $(Jquery).timepicker("setTime", new Date(pValue));
                        cce_Symbol.Value = pValue;
                    }
                    break;

                case "yesno":

                    //                    if (pValue != " ") {
                    //                        if (eval(document.getElementById("IsMobile"))) {
                    //                            if (!IsHidden) {
                    //                                $(Jquery).val(null).attr("selected", null);
                    //                                $(Jquery).selectmenu('refresh', null);
                    //                                cce_Symbol.Value = null;
                    //                            }
                    //                        }
                    //                        else {
                    //                            $(Jquery).val(null);
                    //                            cce_Symbol.Value = null;
                    //                        }
                    //                    }
                    //                    else 
                  //  setTimeout(function () { 
                    if (pValue) 
                    {
                        if (eval(document.getElementById("IsMobile"))) 
                        {
                            if (!IsHidden)
                             {
                                $(Jquery).val('1').attr("selected", "Yes");
                                $(Jquery).selectmenu('refresh', true);
                                cce_Symbol.Value = true;
                            }
                        }
                        else 
                        {
                            $(Jquery).val("1");
                            cce_Symbol.Value = true;
                        }
                    }
                    else if (!pValue && pValue != null) 
                    {
                        if (eval(document.getElementById("IsMobile"))) 
                        {
                            if (!IsHidden) 
                            {
                                $(Jquery).val('0').attr("selected", "No");
                                $(Jquery).selectmenu('refresh', true);
                                cce_Symbol.Value = false;
                            }
                        }
                        else 
                        {
                            $(Jquery).val("0");
                            cce_Symbol.Value = false;
                        }
                    } 
                    else 
                    { 
                     
                        if (eval(document.getElementById("IsMobile"))) 
                        {
                            if (!IsHidden)
                             {
                                $(Jquery).val(null).attr("selected", null);
                                $(Jquery).selectmenu('refresh', null);
                                cce_Symbol.Value = null;
                            }
                        }
                        else 
                        {
                            $(Jquery).val(null);
                            cce_Symbol.Value = null;
                        }
                  }
                    
                   // }, 250);
                    break;

                case "checkbox":

                    if (pValue) {
                        if (eval(document.getElementById("IsMobile"))) {
                            if (!IsHidden) {
                                $(Jquery).prop('checked', true).checkboxradio('refresh');
                            }
                            cce_Symbol.Value = true;
                        }
                        else {
                            $(Jquery).prop('checked', true);
                            cce_Symbol.Value = true;
                        }
                    }
                    else {
                        if (eval(document.getElementById("IsMobile"))) {
                            if (!IsHidden) {
                                $(Jquery).prop('checked', false).checkboxradio('refresh');
                            }
                            cce_Symbol.Value = false;
                        }
                        else {
                            $(Jquery).prop('checked', false);
                            cce_Symbol.Value = false;
                        }
                    }

                    if ($(Jquery + "[value]")) {
                        $(Jquery).prop('value', cce_Symbol.Value);
                    }

                    break;

                case "radiobutton":
                    if (eval(document.getElementById("IsMobile"))) {
                       // var selector = "input[id='" + FieldName + pValue + "'";
                        var selector = "#" + FieldName + pValue + "";


                        $(selector).prop("checked", true).checkboxradio("refresh");
                        $("input[type='radio']").checkboxradio("refresh");
                      
                    } else {
                        var RadiofieldName = "." + FieldName;
                        $(RadiofieldName).each(function (i, obj) {
                            if ($(this).val() == pValue) {
                                $(this).prop('checked', true);
                            }
                        });
                        $(Jquery).val(pValue);
                    }
                    cce_Symbol.Value = pValue;
                    break;

                case "commentlegal":
                case "legalvalues":
                   // setTimeout(function () {
                    if (eval(document.getElementById("IsMobile"))) {
                        if (!IsHidden) {
                            $(Jquery).val(pValue).attr("selected", true);
                            $(Jquery).selectmenu('refresh', true);
                            cce_Symbol.Value = pValue;
                        } else {
                            cce_Symbol.Value = pValue;
                        }
                    } else {
                        $(Jquery).val(pValue);
                        cce_Symbol.Value = pValue;
                    }
                   /// }, 250);
                    break;
                case "label":
                    Jquery = Jquery + '_fieldWrapper';
                    $(Jquery).html(pValue);
                    cce_Symbol.Value = pValue;
                    break;
                default:
                    $(Jquery).val(pValue);
                    cce_Symbol.Value = pValue;
                    break;
            }
        }
     //   }, 500);
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
           if (eval(document.getElementById("IsMobile"))){
            switch (symbol.Type) 
            {
                case "radiobutton":
                        
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').css("background-color","yellow");
                          
                    break;
                case "checkbox":
                        
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').css("background-color","yellow");
                          
                    break;
                case "legalvalues":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').css("background-color","yellow");
                    break;
                case "yesno":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').css("background-color","yellow");
                    break;
                case "commentlegal":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').css("background-color","yellow");
                    break;
                case "datepicker":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('div').css("background-color","yellow");
                        
                    break;
                case "timepicker":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('div').css("background-color","yellow");
                    break;

                default:
                    $(query).css("background-color","yellow");
                    query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                    break;
                    }
   }else{
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
            
            
             }
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

if (eval(document.getElementById("IsMobile"))){
            switch (symbol.Type) 
            {
                case "radiobutton":
                        
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').removeAttr('Style');   
                    break;
                case "checkbox":
                        
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').removeAttr('Style');     
                    break;
                case "legalvalues":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('span').removeAttr('Style'); 
                    break;
                 case "yesno":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                      $(query).find('span').removeAttr('Style'); 
                    break;
                case "commentlegal":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                      $(query).find('span').removeAttr('Style'); 
                    break;
               case "datepicker":
                      query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                      $(query).find('div').css("background-color","white");
                      break;
                case "timepicker":
                    query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(query).find('div').css("background-color","white");
                    break;

                default:
                    $(query).css("background-color","white");
                    query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                    break;
                    }
}else{
           if(symbol.Type == "radiobutton")
            {
                query = '.labelmvcdynamicfield_' + pCheckCodeList[i];
                $(query).each(function(i, obj) 
                {
                  $(query).css("background-color", "");
                });
            }
            else
            {
                $(query).css("background-color", "");
               
                 query = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                 //CCE_AddToHilightedFieldsList(pCheckCodeList[i]);
             }
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
      throw ex;
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
 

if (eval(document.getElementById("IsMobile"))){
  query = '#mvcdynamicfield_' + pCheckCodeList[i];
  Labelquery = '#labelmvcdynamicfield_' + pCheckCodeList[i];
   switch (symbol.Type) 
            {
                case "radiobutton":
                    Query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(Query).find('.ui-radio').addClass('ui-disabled'); 
                    break;
                case "checkbox":
                       
                    var checkboxcontrolId = '#mvcdynamicfield_' + pCheckCodeList[i] ;
                     $(checkboxcontrolId).attr("disabled", true);
                     $(Labelquery).css("color", "LightGray")
                    break;
                case "legalvalues":
                    $(query).selectmenu('disable');
                    break;
                case "yesno":
                    $(query).selectmenu('disable');
                    break;
                case "commentlegal":
                    $(query).selectmenu('disable');
                    break;
               case "datepicker":
                     $(query).datebox('disable');
                      break;
                case "timepicker":
                    $(query).datebox('disable');
                    break;
                //case "groupbox":
                  // break;

                default:
                    $(query).textinput('disable');
                    break;

            }

}else{
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
 if (eval(document.getElementById("IsMobile"))){
query = '#mvcdynamicfield_' + pCheckCodeList[i];
 Labelquery = '#labelmvcdynamicfield_' + pCheckCodeList[i];
   switch (symbol.Type) 
            {
                case "radiobutton":
                    Query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                    $(Query).find('.ui-radio').removeClass('ui-disabled'); 
                    break;
                case "checkbox":
                        
                    $(query).checkboxradio('enable');
                      $(Labelquery).css("color","Black")
                       

                    break;
                case "legalvalues":
                    $(query).selectmenu('enable');
                    break;
               case "yesno":
                    $(query).selectmenu('enable');
                    break;
                case "commentlegal":
                    $(query).selectmenu('enable');
                    break;
               case "datepicker":
                     $(query).datebox('enable');
                      break;
                case "timepicker":
                    $(query).datebox('enable');
                    break;
               case "groupbox":
                   
                    break;
                default:
                    $(query).textinput('enable');
                    break;

            }
  }else{
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
/* function CCE_ClearControlValue(pCheckCodeList) 
 {
     if (pCheckCodeList != null) 
     {
         for (var i = 0; i < pCheckCodeList.length; i++) 
         {
                cce_Context.setValue(pCheckCodeList[i], null);
         }
    }
}*/

 
 //Clear the control value
 function CCE_ClearControlValue(pCheckCodeList) 
 {
     if (pCheckCodeList != null) 
     {
          // var ControlList = new Array();
             var ControlList = "";
         for (var i = 0; i < pCheckCodeList.length; i++) 
         {
                //if control is a check box uncheck it otherwise clear the control value
                var cce_Symbol = cce_Context.resolve(pCheckCodeList[i]);
                if (cce_Symbol != null) 
                {
                   // cce_Symbol.Value = pValue;
                    cce_Symbol.Value = null;
                    var controlId = '#mvcdynamicfield_' + pCheckCodeList[i];
                    var FieldName = 'mvcdynamicfield_' + pCheckCodeList[i];

                     
                      var IsHidden = false;
                        if (!eval(document.getElementById(FieldName)) && cce_Symbol.Type != "groupbox")
                        {
                             
                            CreateHiddenField(FieldName, cce_Symbol.Type);
                        }
                         var ControlType = $(controlId).attr('type');
                       if (ControlType== 'hidden')
                       {
                           IsHidden =true;
                       }
                   // if (eval(document.getElementById(FieldName)))
                   // {
                        if(cce_Symbol.Source == "datasource")
                        {
                            switch (cce_Symbol.Type) 
                            {
                                case "checkbox":
		                            if (eval(document.getElementById("IsMobile")))
                                    {        
                                          var checkboxfield = controlId + "_fieldWrapper";
                                          // var IsDisabled = $(checkboxfield).find('ui-disabled');
                                      var IsDisabled = $(checkboxfield).hasClass('ui-disabled');
                                           if (! IsDisabled)
                                           { 
                                                 if  (!IsHidden)
                                                  {
                                                  $(controlId).attr('checked', false).checkboxradio("refresh");
                                                  }
                                           }
                                           else
                                           {
                                                 if  (!IsHidden)
                                                 {
                                                 $(controlId).attr('checked', false).checkboxradio("refresh");//1
                                                 $(checkboxfield).find('.ui-checkbox').removeClass('ui-disabled');//2
                                                 $(checkboxfield).find('.ui-checkbox').addClass('ui-disabled');//3 Keep them in this order
                                                 }
                                           }

                                             //     $('.ui-checkbox').removeClass('ui-disabled');
			                          }
                                      else
                                      {
                                                   $(controlId).attr('checked', false);
			                          }
                                    //  $(controlId).val('');
                                    break;
                                 case "radiobutton":
                                  if (eval(document.getElementById("IsMobile")))
                                    {
                                      var Radiofield = controlId + "_fieldWrapper";
                                      var label  =".label" + FieldName ;

                                      $(Radiofield).find(label).removeClass('ui-radio-on'); //1
                                      $(Radiofield).find(label).addClass('ui-radio-off'); //2

                                    $(Radiofield).find('.ui-icon-radio-on').addClass('RadioTemp'); //1
                                    $(Radiofield).find('.ui-icon-radio-on').removeClass('ui-icon-radio-on'); //2
                                    $(Radiofield).find('.RadioTemp').addClass('ui-icon-radio-off'); //3
                                    $(Radiofield).find('.ui-icon-radio-off').removeClass('RadioTemp'); //4 keep them in this order
                                    }
                                    else
                                    {
                                           var RadiofieldName = "." + FieldName;
                                            value = -1; 
                                            $(RadiofieldName).each(function(i, obj) 
                                            {
                                                if ($(this).is(':checked'))
                                                {
                                                    $(this).attr('checked', false);
                                                }
                                            });
                                            $(controlId).val('');
                                    }
                                    break;
                                  case "legalvalues":
                                   if (eval(document.getElementById("IsMobile")))
                                         {  
                                           $(controlId).val('');
                                           if  (!IsHidden)
                                                 {
                                                 $(controlId).selectmenu('refresh');
                                                 }
                                         }
                                         else
                                         {$(controlId).val('');}
                                        break;
                                  case "yesno":
                                     if (eval(document.getElementById("IsMobile")))
                                         {  
                                         
                                          $(controlId).val('');
                                          if  (!IsHidden)
                                                 {
                                                 $(controlId).selectmenu('refresh');
                                                 }

                                         
                                         }
                                         else
                                         {$(controlId).val('');}
                                        break;
                                  case "commentlegal":
                                    if (eval(document.getElementById("IsMobile")))
                                         {  
                                           $(controlId).val('');
                                         if  (!IsHidden)
                                                 {
                                                 $(controlId).selectmenu('refresh');
                                                 }
                                         }
                                         else
                                         {$(controlId).val('');}
                                        break;
                                     case "groupbox":
                                     break;

                                  default:
                                    $(controlId).val('');
                                    break;

                            }
                        }
                    //}
                    else
                    {
                    
                      if(ControlList.length == 0)
                      {
                       ControlList = pCheckCodeList[i];
                      }
                      else
                      {
                      ControlList =  ControlList +","+ pCheckCodeList[i];
                    
                      }
                     //updateXml(pCheckCodeList[i], '') ;
                    
                    }
                }
             }
        }
        if(ControlList.length > 0){
        //updateXml(ControlList, '') ;
       //CreateHiddenField(ControlList , '')
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
         $("#myform")[0].is_goto_action.value = 'true';
         //detach the validation engine as we don't want to validate data to go to another page as directed by check code
       //  $('#myform').validationEngine('detach');
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
    if ( Object.prototype.toString.call(pValue) === "[object Date]" ) 
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
        return new Date(pValue).getMonth() + 1;//I added 1 because getMonth() returns the index  of the month.
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

//function CCE_Round(pValue1, pValue2) 
//{
//    var RoundModifier = 10 * pValue2;

//    var result = Math.round(pValue1 * RoundModifier) / RoundModifier;
//    return result;
//}
function CCE_Round(pValue1 ) 
{
    var result = Math.round(pValue1)  ;
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

  //  var result = Math.round(Math.abs((date2.getTime() - date1.getTime())/(oneDay)));
    var result = Math.round((date2.getTime() - date1.getTime()) / (oneDay));
    return result;
}
/////////////////Simple  Dialogbox //////////////////////
function CCE_ContextOpenMobileSimpleDialogBox(Title,Prompt,id) 
{
        var passcode1 = '@Model.PassCode';
   
     
                         $('#'+ id).simpledialog({
                'mode' : 'blank',
                'headerText' : Title,
                'headerClose' : true,
                'dialogAllow' : true,
                'useDialogForceTrue': true, 
                'useDialogForceFalse': false,

                'prompt': Title,
                'forceInput': false,
                'useModal':true,
                'buttons' : {
                              'OK': {
                                click: function () {
                                  $('#dialogoutput').text('OK');
                                }
                              }
                       
                            },
                       'fullHTML': "<div id='SimpleDialogBox' title='"+ Title +"'><p><label id='SimpleDialogBoxPrempt'>"+ Prompt +"</label></p><p style='text-align:right;'><button  id='SimpleDialogBoxButton' type='button' style='width:100%;'onclick='CCE_CloseMobileSimpleDialogBox("+ id.toString() +");'>Ok</button></p></div>"
                  })
 
 
}

function CCE_CloseMobileSimpleDialogBox(id) 
{
        
        $(id).simpledialog('close');
       
}

function CCE_Substring(pValue1, pValue2, pValue3) 
{
        var result = null;
        var fullString = null;
        var startIndex = 0;
        var length = 0;

        fullString = new String(pValue1);
        startIndex = new Number(pValue2);

        if (!(fullString == null && fullString !=""))
        {
            if(pValue1 == null)
            {
                length = fullString.length;
            }
            else
            {
               length = new Number(pValue3);
            }

            if (startIndex + length > fullString.length)
            {
                length = fullString.length - startIndex + 1;
            }

            if (startIndex <= fullString.length)
            {
                result = fullString.substring(startIndex - 1, length);
            }
            else
            {
                result = "";
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

    //var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneHour)));
    var result = Math.round((date2.getTime() - date1.getTime()) / (oneHour));
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

   // var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneMinute)));
    var result = Math.round((date2.getTime() - date1.getTime()) / (oneMinute));
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

    //var result = Math.round(Math.abs((date1.getTime() - date2.getTime())/(oneSecond)));
    var result = Math.round((date2.getTime() - date1.getTime()) / (oneSecond));
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




function FormatTime(currentTime)
{
var PMAM ="";
var FormatedTime = "";
var hours = currentTime.getHours();
var minutes = currentTime.getMinutes();
var seconds = currentTime.getSeconds();
    if (minutes < 10)
    {
    minutes = "0" + minutes
    }
    if (seconds < 10)
    {
    seconds = "0" + seconds
    }
     
    
    if ( hours < 10 )  
    {
       hours = "0" + hours
    }
    if(hours > 11)
    {
       PMAM = "PM";
    } else {
       PMAM = "AM" ;
    }
    if (hours > 12) 
    {
        hours = hours - 12; 
        if ( hours < 10 )  
        {
           hours = "0" + hours
        } 
    }
    FormatedTime = hours + ":" + minutes + ":" + seconds + " " + PMAM;
    return FormatedTime;

}


function OpenVideoDialog()
{
    $("#VideoDialog").dialog("open");
}



/////////////////Simple  Dialogbox //////////////////////
function CCE_ContextOpenSimpleDialogBox(Title,Prompt,id) 
{
 if (!eval(document.getElementById("IsMobile")))
 {

        $('#ui-dialog-title-SimpledialogBox').text(Title.toString());
        $('.ui-dialog-title').text(Title.toString());
        $('#SimpleDialogBoxPrempt').text(Prompt.toString());
        $('#SimpleDialogBoxButton').text('Ok');
     setTimeout(function () {
        $("#SimpleDialogBox").dialog("open");
      }, 100);
     }else{
     
     CCE_ContextOpenMobileSimpleDialogBox(Title,Prompt,id) 
     
     }
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
         
        setTimeout(function () {
            $("#DialogBox").dialog("open");
        }, 100);
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

function CCE_FindText(pValue1, pValue2)
{
    if(pValue1 != null && pValue2 != null)
    {
        return pValue1.to().indexOf(pValue2);
    }
    else
    {
        return -1;
    }
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

     function OpenVideoDialogMobile(){
//       $("#VideoDialog").popup('open', options);
 //document.getElementById("VideoDialog").style.display ="true";
 
    }



function CCE_Set_Required(fNameList)
{
  try 
         {
             if (fNameList != null) 
             {
         
                for (var i = 0; i < fNameList.length; i++) 
                {
                    var query = null;  
                   
                     query = '#mvcdynamicfield_' + fNameList[i];
                     var symbol = cce_Context.resolve(fNameList[i]);
                     switch (symbol.Type) 
                            {
                                case "textbox":
                                    
                                    $(query).addClass('validate[required] text-input');
                                     CCE_AddToFieldsList(fNameList[i], 'RequiredFieldsList');
                                    break;

                                default:
                                 if (symbol.Type != "checkbox" && symbol.Type != "radiobutton")
                                    {

                                     $(query).addClass('validate[required]');
                                     CCE_AddToFieldsList(fNameList[i], 'RequiredFieldsList');
                                     
                                    }
                                    break;

                            }

                    
                   
                }
             }
         }
         catch (ex) 
         {

         }
}
  ///////Update Controls State Start//////

  function CCE_Set_Update_HighlightedControls_State(Items)
{

            var HighlightedFieldsArray = new Array();
            
            HighlightedFieldsArray = Items.split(',');
            HighlightedFieldsArray.splice(0, 1);
        
            if (HighlightedFieldsArray.length >0)
            {
              CCE_Highlight(HighlightedFieldsArray ,false);
            }
              
}

  
 
  function CCE_Set_Update_DisabledControls_State(Items)
{

            var DisabledFieldsListArray = new Array();
            
            DisabledFieldsListArray = Items.split(',');
            DisabledFieldsListArray.splice(0, 1);
        
            if (DisabledFieldsListArray.length >0)
            {
              CCE_Disable(DisabledFieldsListArray ,false);
            }
              
}
       ///////Update Controls State end//////
       //////Enable all controls before leaving the page start ////////

function CCE_ProcessEnableAllControls(List) 
 {

  var pCheckCodeList = new Array();
    pCheckCodeList = List.split(',');
    pCheckCodeList.splice(0, 1);

     if (pCheckCodeList != null) 
     {
         for (var i = 0; i < pCheckCodeList.length; i++) 
         {
             var query = null;

             
             var symbol = cce_Context.resolve(pCheckCodeList[i]);
             if(symbol != null)
             {
                 if (eval(document.getElementById("IsMobile")))
                 {
                   query = '#mvcdynamicfield_' + pCheckCodeList[i];
                   Labelquery = '#labelmvcdynamicfield_' + pCheckCodeList[i];
                   switch (symbol.Type) 
                   {
                                case "radiobutton":
                                    Query = '#mvcdynamicfield_' + pCheckCodeList[i] + "_fieldWrapper";
                                    $(Query).find('.ui-radio').removeClass('ui-disabled'); 
                                    break;
                                case "checkbox":
                        
                                    $(query).checkboxradio('enable');
                                      $(Labelquery).css("color","Black")
                                    break;
                                case "legalvalues":
                                    $(query).selectmenu('enable');
                                    break;
                               case "yesno":
                                    $(query).selectmenu('enable');
                                    break;
                                case "commentlegal":
                                    $(query).selectmenu('enable');
                                    break;
                               case "datepicker":
                                     $(query).datebox('enable');
                                      break;
                                case "timepicker":
                                    $(query).datebox('enable');
                                    break;

                                default:
                                    $(query).textinput('enable');
                                    break;

                    }
                  }
                  else
                  {
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
                }
            }
         }
     }
 }
  //////Enable all controls before leaving the page start ////////

function CCE_Set_NOT_Required(fNameList)
{
try 
         {
             if (fNameList != null) 
             {

             for (var i = 0; i < fNameList.length; i++) 
                {
          
                 var query = null;  
                  
                     query = '#mvcdynamicfield_' + fNameList[i];
                     var symbol = cce_Context.resolve(fNameList[i]);
                     switch (symbol.Type) 
                            {
                                case "textbox":
                                    
                                    $(query).removeClass('validate[required] text-input');
                                     CCE_RemoveFromFieldsList(fNameList[i], 'RequiredFieldsList');
                                     $(query).validationEngine('hidePrompt');
                                    break;

                                default:
                                    if (symbol.Type != "checkbox" && symbol.Type != "radiobutton")
                                    {
                                    $(query).removeClass('validate[required]');
                                     CCE_RemoveFromFieldsList(fNameList[i], 'RequiredFieldsList');
                                     $(query+"formError");
                                     $(query).validationEngine('hidePrompt');
                                    }
                                    break;

                            }

                  
                }
             }
         }
         catch (ex) 
         {

         }
}

///Create Hidden filed 

function CreateHiddenField(pFieldName, pFieldType)
{
    if (!eval(document.getElementById(pFieldName)))
    {
       /* var input = document.createElement("input");
        input.setAttribute("type","hidden")
        input.setAttribute("id","hidden_"+FiledName)
        input.setAttribute("value",FieldValue)
        document.getElementById("myform").appendChild(input);*/
        $('<input>').attr({
        type: 'hidden',
        id: pFieldName,
        name: pFieldName,
        value: null,
        fieldtype: pFieldType 
          }).appendTo('form');

       
    }
 
}

cce_Context = new CCE_Context();
