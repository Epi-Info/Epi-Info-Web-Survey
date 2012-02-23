using System;
using System.Collections.Generic;
using System.Text;
using com.calitha.goldparser;
using System.Data;
/*
using Epi;
using Epi.Data;
using Epi.Data.Services;
using Epi.DataSets;*/

namespace Epi.Core.EnterInterpreter.Rules
{
    class Rule_Undefine : EnterRule
    {
        private string identifier = string.Empty;

        public Rule_Undefine(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            this.identifier = this.GetCommandElement(pToken.Tokens, 1);
        }


        /// <summary>
        /// performs execution of the UNDEFINE command
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object results = null;
            this.Context.CurrentScope.undefine(identifier);
            /*
            if (this.Context.MemoryRegion.IsVariableInScope(identifier))
            {
                this.Context.MemoryRegion.UndefineVariable(identifier);
            }*/

            return results;
        }
    }
}
