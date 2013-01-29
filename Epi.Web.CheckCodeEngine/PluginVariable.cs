using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Core.EnterInterpreter
{
    public class PluginVariable : EpiInfo.Plugin.IVariable
    {
        private string name;
        private EpiInfo.Plugin.DataType dataType;
        private EpiInfo.Plugin.VariableScope variableScope;
        private string expression;
        private string _Namespace;
        private string _ControlType;
        private string _PageNumber;

        public PluginVariable() { }

        public PluginVariable(string pName, EpiInfo.Plugin.DataType pDataType, EpiInfo.Plugin.VariableScope pVariableScope, string pExpression, string pNamespace = null)
        {
            this.name = pName;
            this.dataType = pDataType;
            this.variableScope = pVariableScope;
            this.expression = pExpression;
            this._Namespace = pNamespace;
        }

        public string Name { get { return this.name; } set { this.name = value; } }
        public EpiInfo.Plugin.DataType DataType { get { return this.dataType; } set { this.dataType = value; } }
        public EpiInfo.Plugin.VariableScope VariableScope { get { return this.variableScope; } set { this.variableScope = value; } }
        public string Expression { get { return this.expression; } set { this.expression = value; } }
        public string Namespace { get { return this._Namespace; } set { this._Namespace = value; } }
        public string ControlType { get { return this._ControlType; } set { this._ControlType = value; } }
        public string PageNumber { get { return this._PageNumber; } set { this._PageNumber = value; } }
        public string Prompt { get; set; }
    }
}
