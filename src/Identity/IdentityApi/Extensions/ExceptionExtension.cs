using System.Diagnostics;
using System;

namespace IdentityApi.Extensions
{
    public class ExceptionExtension
    {
        public static string ExceptionInfo(Exception exception)
        {
            var stackFrame = new StackTrace(exception, true);
            // Get the bottom stack frame
            StackFrame stackFrameForGetFrame;
            if (stackFrame.FrameCount > 1)
            {
                stackFrameForGetFrame = stackFrame.GetFrame(stackFrame.FrameCount - 2);
            }
            else
            {
                stackFrameForGetFrame = stackFrame.GetFrame(stackFrame.FrameCount - 1);
            }
            string sExceptionInfo = string.Format("Error : At line {0} column {1} in {2}: {3} {4}{3} Exception : {5}",
            stackFrameForGetFrame.GetFileLineNumber(), stackFrameForGetFrame.GetFileColumnNumber(),
            stackFrameForGetFrame.GetMethod(), Environment.NewLine, stackFrameForGetFrame.GetFileName(), exception);

            return sExceptionInfo;
        }
    }
}
