using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UniversalShare_2.Operation;
using UniversalShare_2.Oprions;

namespace UniversalShare_2.Handlers {
    public class SettingsHandler : HandlerBase {
        public const string SAVE_BASE        = ".xml";
        public const string SAVE_IDENTIFIER  = "IDENTIFIER";
        public const string SAVE_CREDENTIALS = "CREDENTIALS";

        private DataHandler _dataHandler;

        private string name;
        private Thread _saveThread;

        private string _explicitSaveName = "";

        /// <inheritdoc />
        public SettingsHandler(ExceptionHandler exceptionHandler, UiHandler uiHandler, DataHandler dataHandler, string explicitSaveName = "") : base( exceptionHandler, uiHandler ) {
            this.name         = Path.GetFileNameWithoutExtension( System.Reflection.Assembly.GetExecutingAssembly().Location );
            this._dataHandler = dataHandler;
            this._saveThread  = new Thread( () => SaveLoop( 100 ) );

            SetexplicitSaveName( explicitSaveName );
        }

        public void SetexplicitSaveName(string explicitSaveName = "") { this._explicitSaveName = string.IsNullOrEmpty( explicitSaveName ) ? "" : "__" + explicitSaveName + "__"; }

        public bool SavingLoop {
            get => this._saveThread.IsAlive;
            set {
                if ( value ) {
                    if ( !this._saveThread.IsAlive ) { this._saveThread.Start(); }
                }
                else {
                    this._saveThread.Abort();
                    this._saveThread = new Thread( () => SaveLoop( 100 ) );
                }
            }
        }

        public void LoadSettings() {
            try {
                var lO = Load<SerializableDictionary<string, OperationInfo>>( this._explicitSaveName+this.name + SAVE_IDENTIFIER + SAVE_BASE );
                var lT = Load<SerializableDictionary<string, TokenItem>>( this._explicitSaveName+ this.name + SAVE_CREDENTIALS    + SAVE_BASE );

                if ( lO != null ) this._dataHandler.OperationIdMap = lO;
                if ( lT != null ) this._dataHandler.TokenList      = lT;
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }
        }

        public void SaveSettings() {
            try {
                if ( this._dataHandler.OperationIdMap != null ) Save(  this._dataHandler.OperationIdMap,this._explicitSaveName+ this.name + SAVE_IDENTIFIER  + SAVE_BASE );
                if ( this._dataHandler.TokenList      != null ) Save( this._dataHandler.TokenList,this._explicitSaveName+      this.name + SAVE_CREDENTIALS + SAVE_BASE );
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }
        }

        private void SaveLoop(int interval = 1000) {
            while ( true ) {
                SaveSettings();
                Thread.Sleep( interval );
            }
        }

        private static T Load <T>(string path) {
            FileStream fileStream = null;
            try {
                if ( !File.Exists( path ) ) throw new IOException( "File Dose Not Exists" );
                var xmlSerializer = new XmlSerializer( typeof(T) );
                fileStream = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
                var obj = (T) xmlSerializer.Deserialize( fileStream );
                fileStream.Close();
                return obj;
            } catch (Exception ex) {
                fileStream?.Close();
                fileStream?.Dispose();
                Console.WriteLine( ex.Message );
            }

            return default;
        }

        private static void Save <T>(T dataLayout, string path) {
            TextWriter textWriter = null;
            try {
                new FileInfo( path ).Directory?.Create();
                var xmlSerializer = new XmlSerializer( dataLayout.GetType() );
                textWriter = new StreamWriter( path );
                xmlSerializer.Serialize( textWriter, dataLayout );
                textWriter.Close();
            } catch (Exception ex) {
                textWriter?.Close();
                textWriter?.Dispose();
                Console.WriteLine( ex.Message );
            }
        }
    }
}