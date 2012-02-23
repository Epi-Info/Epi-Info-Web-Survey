using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using com.calitha.goldparser;

namespace Epi.Core.EnterInterpreter.Rules
{
    /*
		| <Simple_Execute_Statement>
		| <Execute_File_Statement>
		| <Execute_Url_Statement>
		| <Execute_Wait_For_Exit_File_Statement>
		| <Execute_Wait_For_Exit_String_Statement>
		| <Execute_Wait_For_Exit_Url_Statement>
		| <Execute_No_Wait_For_Exit_File_Statement>
		| <Execute_No_Wait_For_Exit_String_Statement>
		| <Execute_No_Wait_For_Exit_Url_Statement>
    */
    public class Rule_Execute : EnterRule
    {
        string commandName = String.Empty;
        string commandlineString = String.Empty;
        string executeOption = String.Empty;
        string firstParam = String.Empty;

        public Rule_Execute(Rule_Context pContext, NonterminalToken pToken) : base(pContext)
        {
            //<Simple_Execute_Statement> ::= EXECUTE String
            //<Execute_File_Statement> ::= EXECUTE File
            //<Execute_Url_Statement> ::= EXECUTE Url

            firstParam = this.GetCommandElement(pToken.Tokens, 1);
            if (firstParam.ToUpper() == "NOWAITFOREXIT" || firstParam.ToUpper() == "WAITFOREXIT")
            {
                executeOption = firstParam;
                commandlineString = this.GetCommandElement(pToken.Tokens, 2);
            }
            else
            {
                commandlineString = firstParam;
            }

            commandlineString = commandlineString.Trim(new char[] { '\"', '\'' });
        }


        /// <summary>
        /// performs execute command
        /// </summary>
        /// <returns>object</returns>
        public override object Execute()
        {
            object result = null;
            Process process = null;
            STARTUPINFO startupInfo = new STARTUPINFO();
            PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();

            //Could possibly use the below code instead of CreateProcess for Linux use
            //process = new Process();
            //process.StartInfo.UseShellExecute = false;
            //process.StartInfo.FileName = firstParam;

            


            //bool created = CreateProcess(null, commandlineString, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref startupInfo, out processInfo);
            //if (created)
            //{
            //    process = Process.GetProcessById((int)processInfo.dwProcessId);
            //}
            //else
            //{
            //    throw new GeneralException("Could not execute the command: '" + commandlineString + "'");
            //}

            process = new Process();
            process.StartInfo.FileName = commandlineString;
            process.Start();

            if (executeOption.ToUpper() != "NOWAITFOREXIT" && process!=null)
            {
                process.WaitForExit();
            }

            return result;
        }

        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        public struct SECURITY_ATTRIBUTES
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        // Using interop call here since this will allow us to execute the entire command line string.
        // The System.Diagnostics.Process.Start() method will not allow this.
        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
            string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

    }
}
