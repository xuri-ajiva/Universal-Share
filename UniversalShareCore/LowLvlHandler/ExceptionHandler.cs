using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml;

namespace UniversalShareCore.LowLvlHandler {
    public struct ExceptionHandler {
        public static readonly ExceptionHandler Empty = default;

        public void EscalateException(Exception e) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine( e.Message );

            switch (e) {
                case null:                                                                break;
                case ArgumentNullException argumentNullException:                         break;
                case ArgumentOutOfRangeException argumentOutOfRangeException:             break;
                case ArrayTypeMismatchException arrayTypeMismatchException:               break;
                case KeyNotFoundException keyNotFoundException:                           break;
                case DivideByZeroException divideByZeroException:                         break;
                case FieldAccessException fieldAccessException:                           break;
                case IndexOutOfRangeException indexOutOfRangeException:                   break;
                case DirectoryNotFoundException directoryNotFoundException:               break;
                case DriveNotFoundException driveNotFoundException:                       break;
                case EndOfStreamException endOfStreamException:                           break;
                case FileLoadException fileLoadException:                                 break;
                case FileNotFoundException fileNotFoundException:                         break;
                case InternalBufferOverflowException internalBufferOverflowException:     break;
                case PathTooLongException pathTooLongException:                           break;
                case SocketException socketException:                                     break;
                case WebException webException:                                           break;
                case NullReferenceException nullReferenceException:                       break;
                case OverflowException overflowException:                                 break;
                case SerializationException serializationException:                       break;
                case StackOverflowException stackOverflowException:                       break;
                case LockRecursionException lockRecursionException:                       break;
                case ThreadAbortException threadAbortException:                           break;
                case WaitHandleCannotBeOpenedException waitHandleCannotBeOpenedException: break;
                case TypeAccessException typeAccessException:                             break;
                case XmlException xmlException:                                           break;
                case ApplicationException applicationException:                           break;
                case ArgumentException argumentException:                                 break;
                case InvalidOperationException invalidOperationException:                 break;
                case IOException ioException:                                             break;
                case NotSupportedException notSupportedException:                         break;
                case OperationCanceledException operationCanceledException:               break;
                case OutOfMemoryException outOfMemoryException:                           break;
                case TimeoutException timeoutException:                                   break;
                case SystemException systemException:                                     break;
                default:                                                                  throw new ArgumentOutOfRangeException( nameof(e) );
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}