using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_SetRequired : EnterRule
    {
        string[] IdentifierList = null;

        public Rule_SetRequired(Rule_Context pContext, NonterminalToken pToken)
        {
            this.IdentifierList = this.IdentifierList = this.GetCommandElement(pToken.Tokens, 1).ToString().Split(' '); 
        }

        /// <summary>
        /// Executes the reduction
        /// </summary>
        /// <returns>null</returns>
        public override object Execute()
        {
            this.Context.EnterCheckCodeInterface.SetRequired(this.IdentifierList);
            return null;
        }
    }
}
