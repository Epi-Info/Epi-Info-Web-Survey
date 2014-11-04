using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Epi.Core.EnterInterpreter
{
    public class cSymbolTable : EpiInfo.Plugin.IScope 
    {
        private string _name;
        private EpiInfo.Plugin.IScope _parent;
        private Dictionary<string, EpiInfo.Plugin.IVariable> _SymbolList;

        public cSymbolTable(EpiInfo.Plugin.IScope pParent)
        {
            this._name = null;
            this._parent = pParent;
            this._SymbolList = new Dictionary<string, EpiInfo.Plugin.IVariable>(StringComparer.OrdinalIgnoreCase);
        }

        public cSymbolTable(string pName, EpiInfo.Plugin.IScope pParent)
        {
            this._name = pName;
            this._parent = pParent;
            this._SymbolList = new Dictionary<string, EpiInfo.Plugin.IVariable>(StringComparer.OrdinalIgnoreCase);
        }

        public string Name
        {
            get { return this._name; }
            set 
            {
                if (string.IsNullOrEmpty(this._name))
                {
                    this._name = value;
                }
            }

        }

        public EpiInfo.Plugin.IScope getEnclosingScope()
        {
            return this._parent;
        }

        public void define(EpiInfo.Plugin.IVariable pSymbol)
        {
            // ensure that Permanent and Global variables are placed in global scope
            if 
            (
                (pSymbol.VariableScope == EpiInfo.Plugin.VariableScope.Permanent | pSymbol.VariableScope == EpiInfo.Plugin.VariableScope.Global) && !this._name.Equals("global", StringComparison.OrdinalIgnoreCase) 
                || !string.IsNullOrEmpty(pSymbol.Namespace) && (!string.IsNullOrEmpty(this._name) && !this._name.Equals(pSymbol.Namespace, StringComparison.OrdinalIgnoreCase))
            )
            {
                if (this._parent != null)
                {
                    this._parent.define(pSymbol);
                }
            }
            else if (!string.IsNullOrEmpty(pSymbol.Namespace) && !this._name.Equals(pSymbol.Namespace))
            {
                if (this._parent != null)
                {
                    this._parent.define(pSymbol);
                }
            }
            else
            {
                if (this._SymbolList.ContainsKey(pSymbol.Name))
                {
                    // maybe throw error duplicate symbol in scope?
                    // instead of error redefining
                    this._SymbolList[pSymbol.Name] = pSymbol;
                }
                else
                {

                    this._SymbolList.Add(pSymbol.Name, pSymbol);
                    if (pSymbol.VariableScope == EpiInfo.Plugin.VariableScope.Permanent)
                    {
                      //PermanentVariable pv = new PermanentVariable(pSymbol.Name, (Epi.DataType) pSymbol.DataType);
                      //pv.Expression = pSymbol.Expression;
                       //Epi.MemoryRegion.UpdatePermanentVariable(pv);
                    }
                }
            }
        }

        public void undefine(string pName, string pNamespace = null)
        {
            if(!string.IsNullOrEmpty(pNamespace) && (!string.IsNullOrEmpty(this._name) && !this._name.Equals(pNamespace, StringComparison.OrdinalIgnoreCase)))
            {
                if (this._parent != null)
                {
                    this._parent.undefine(pName, pNamespace);
                }
            }
            else if (this._SymbolList.ContainsKey(pName))
            {
                this._SymbolList.Remove(pName);
            }
            else if (this._parent == null)
            {
                // maybe throw error  symbol not in scope
            }
            else
            {
                this._parent.undefine(pName, pNamespace);
            }
        }

        public EpiInfo.Plugin.IVariable resolve(string pName, string pNamespace = null)
        {
            EpiInfo.Plugin.IVariable result = null;

            if(!string.IsNullOrEmpty(pNamespace) && (!string.IsNullOrEmpty(this._name) && !this._name.Equals(pNamespace, StringComparison.OrdinalIgnoreCase)))
            {
                if (this._parent != null)
                {
                    result = this._parent.resolve(pName, pNamespace);
                }
            }
            else if (this._SymbolList.ContainsKey(pName))
            {
                result = this._SymbolList[pName];
            }
            else
            {
                if (this._parent != null)
                {
                    result = this._parent.resolve(pName, pNamespace);
                }
            }

            return result;
        }

        public bool SymbolIsInScope(string pName)
        {
            return this._SymbolList.ContainsKey(pName);
        }

        public Dictionary<string, EpiInfo.Plugin.IVariable> SymbolList { get { return this._SymbolList; } }

        public List<EpiInfo.Plugin.IVariable> FindVariables(EpiInfo.Plugin.VariableScope pScopeCombination, string pNamespace = null)
        {
            List<EpiInfo.Plugin.IVariable> result = new List<EpiInfo.Plugin.IVariable>();

            if (!string.IsNullOrEmpty(pNamespace) && (!string.IsNullOrEmpty(this._name) && !this._name.Equals(pNamespace, StringComparison.OrdinalIgnoreCase)))
            {
                if (this._parent != null)
                {
                    result.AddRange(this._parent.FindVariables(pScopeCombination, pNamespace));
                }
            }
            else
            {
                foreach (KeyValuePair<string, EpiInfo.Plugin.IVariable> kvp in _SymbolList)
                {
                    if ((kvp.Value.VariableScope & pScopeCombination) > 0)
                    {
                        result.Add(kvp.Value);
                    }
                }
            

                if (this._parent != null)
                {
                    result.AddRange(this._parent.FindVariables(pScopeCombination, pNamespace));
                }
            }
            return result;
        }


        public void RemoveVariablesInScope(EpiInfo.Plugin.VariableScope pScopeCombination, string pNamespace = null)
        {
            if (!string.IsNullOrEmpty(pNamespace) && (!string.IsNullOrEmpty(this._name) && !this._name.Equals(pNamespace, StringComparison.OrdinalIgnoreCase)))
            {
                if (this._parent != null)
                {
                    this._parent.RemoveVariablesInScope(pScopeCombination);
                }
            }
            else
            {
                for (int i = 0; i < _SymbolList.Count; i++)
                {
                    KeyValuePair<string, EpiInfo.Plugin.IVariable> kvp = _SymbolList.ElementAt(i);
                    if ((kvp.Value.VariableScope & pScopeCombination) > 0)
                    {
                        this.undefine(kvp.Key, pNamespace);
                    }
                }

                if (this._parent != null)
                {
                    this._parent.RemoveVariablesInScope(pScopeCombination, pNamespace);
                }
            }

        }
    }
}
