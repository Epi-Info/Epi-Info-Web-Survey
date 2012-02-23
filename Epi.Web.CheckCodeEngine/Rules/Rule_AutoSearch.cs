using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_AutoSearch: EnterRule 
    {
        string[] IdentifierList = null;

        public Rule_AutoSearch(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            //<Auto_Search_Statement> ::= AUTOSEARCH <IdentifierList> 
            this.IdentifierList = this.GetCommandElement(pToken.Tokens, 1).Split(' ');
        }


        /// <summary>
        /// uses the EnterCheckCodeInterface to perform an AutoSearch
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            this.Context.EnterCheckCodeInterface.AutoSearch(this.IdentifierList);
            return null;
        }
    }
}
