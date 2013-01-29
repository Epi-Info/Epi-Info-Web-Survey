using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    public class Rule_SetNOTRequired : EnterRule
    {
        private List<EnterRule> ParameterList = new List<EnterRule>();

        string[] IdentifierList = null;

        public Rule_SetNOTRequired(Rule_Context pContext, NonterminalToken pToken)
        {
            this.IdentifierList = this.IdentifierList = this.GetCommandElement(pToken.Tokens, 1).ToString().Split(' '); 
        }

        /// <summary>
        /// Executes the reduction
        /// </summary>
        /// <returns>null</returns>
        public override object Execute()
        {
            this.Context.EnterCheckCodeInterface.SetNotRequired(this.IdentifierList);
            return null;
        }
    }
}
