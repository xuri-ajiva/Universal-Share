package app;

import java.util.*;

import java.io.*;
import java.net.*;

public class Client extends SharedData {


    public void Start() throws Exception {
        while ( true ) {
            try {
                Socket MainSocket = new Socket("localhost", FILE_PORT);
                
                System.out.println("Connected!");

                SendFile( MainSocket, "test.txt" );
                System.out.println( "Finished!" );
                
                Thread.sleep( 1000 );

            } catch(Exception e) {
                Thread.sleep( 1000 );
            }
        }
    }

    void SendFile(Socket cl, String filename) throws Exception{
        int         readBytes = -1;

        byte[] buffer = new byte[BUFFER_SIZE];

        FileInputStream strm = new FileInputStream( filename);

        int blockCtr       = 0;
        int totalReadBytes = 0;

        
        OutputStream OutStream = cl.getOutputStream();
        while ( readBytes != 0 ) {
            readBytes = strm.read( buffer, 0, BUFFER_SIZE );
            if(readBytes == -1)
            break;
            blockCtr++;
            totalReadBytes += readBytes;
            OutStream.write( buffer, 0, readBytes);
        }

        OutStream.close();
        strm.close();
        cl.close();
        return;
    }
}