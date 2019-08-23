using System;
using System.Collections.Specialized;
using System.Diagnostics;

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace Universal_Share {
    static class MainPoint {
        const bool DEBUG  = true;
        const bool SERVER = true;

        static void Main(string[] args) {
            if ( DEBUG ) {
                if ( args.Length == 0 ) {
                    if ( SERVER ) {
                        Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, "C" );
                        new Server().Start();
                    }
                    else {
                        Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, "S" );
                        new Client().Start();
                    }
                }
                else {
                    StartNormal( args );
                }
            }
            else
                StartNormal( args );
        }

        private static void StartNormal(string[] args) {
            if ( args.Length > 0 ) {
                Console.WriteLine( "Normal Start" );
                Console.WriteLine( args[0] );
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (args[0].ToLower()) {
                    case "s":
                        Console.WriteLine( "starting Server..." );
                        new Server().Start();
                        break;
                    case "c":
                        new Client().Start();
                        break;
                }
            }
            else {
                Console.WriteLine( "C / S" );
            }

            Console.WriteLine( "Pause ..." );
            Console.ReadKey();
        }
    }
}