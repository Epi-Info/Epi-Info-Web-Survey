using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.calitha.goldparser;
namespace Epi.Core.EnterInterpreter
{
    public class cAST
    {
        Token token; // node is derived from which token?
        List<cAST> children; // operands

        public cAST(Token token) { this.token = token; }
        public void addChild(cAST t)
        {
            if (children == null) children = new List<cAST>();
            children.Add(t);
        }
    }
}
