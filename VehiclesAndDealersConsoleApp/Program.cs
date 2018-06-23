using System;

namespace Sammak.VnD
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var app = new VnAMain();
                app.Run();
            }
            catch (Exception ex)
            {
                ShowException(ex, string.Empty);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Press any Key to exit...");
                Console.ReadLine();
            }
        }

        const string INDENT = "   ";
        static void ShowException(Exception ex, string indent)
        {
            Console.WriteLine();
            Console.WriteLine($"{indent}Exception Message: {ex.Message}");
            Console.WriteLine($"{indent}Exception Type: {ex.GetType().Name.ToString()}");
            Console.WriteLine($"{indent}Exception Source: {ex.Source}");

            // format the stack trace string items and indent them accoring to the exception level
            var stackTrace = ex.StackTrace;
            var separators = new string[] { "\n", "\r\n" };
            var myList = stackTrace.Split(separators, StringSplitOptions.None);
            Console.WriteLine($"{indent}Stack Trace:");
            foreach (var item in myList)
            {
                Console.WriteLine($"{indent}{item}");
            }

            // recursively display the inner exceptions, if any
            if (ex.InnerException != null)
            {
                Console.Write($"{indent}Inner Exception:");
                ShowException(ex.InnerException, indent + INDENT);
            }
        }

        private static void ExceptionTest()
        {
            try
            {
                ExceptionThrowingMethod_1();
            }
            catch (Exception ex)
            {
                throw (new Exception("Throwing from ExceptionTest", ex));
            }
        }

        private static void ExceptionThrowingMethod_1()
        {
            try
            {
                ExceptionThrowingMethod_2();
            }
            catch (Exception ex)
            {
                throw (new Exception("Throwing from ExceptionThrowingMethod_1", ex));
            }
        }

        private static void ExceptionThrowingMethod_2()
        {
            try
            {
                ExceptionThrowingMethod_3();
            }
            catch (Exception ex)
            {
                throw (new Exception("Throwing from ExceptionThrowingMethod_2", ex));
            }
        }

        private static void ExceptionThrowingMethod_3()
        {
            throw new NotImplementedException();
        }

    }
}
