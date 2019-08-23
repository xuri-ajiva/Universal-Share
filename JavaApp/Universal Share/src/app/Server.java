package app;

import java.util.*;
import java.io.*;
import java.net.*;

public class Server extends SharedData {

    public void Start() throws Exception {
        ServerSocket MainSocket = new ServerSocket(FILE_PORT);

        System.out.println("Waiting for connections ...");
        while (true) {
            Socket cl = MainSocket.accept();
           ReturnData returnData=  HandleClient(cl, "testfile");
           System.out.println("  -   " + returnData.fileName);
           System.out.println("  -   " + returnData.blockCtr);
           System.out.println("  -   " + returnData.totalReadBytes);
        }   
    }

    ReturnData HandleClient(Socket cl, String filename) throws Exception {
        int readBytes = -1;

        byte[] buffer = new byte[BUFFER_SIZE];

        Date d = new Date();

        var finalFileName = DEFAULT_SAVE_LOCATION + (filename + d.toString()).replaceAll("[\\\\/:*?\"<>|]", "");
        System.out.println("Saving as: " + finalFileName);

        File file = new File(finalFileName);
        file.getParentFile().mkdirs();
        FileOutputStream writer = new FileOutputStream(file);

        int blockCtr = 0;
        int totalReadBytes = 0;
        InputStream InStream = cl.getInputStream();

        while (readBytes != 0) {
            readBytes = InStream.read(buffer, 0, BUFFER_SIZE);
            if (readBytes == -1)
                break;
            blockCtr++;
            totalReadBytes += readBytes;
            writer.write(buffer, 0, readBytes);
        }
        cl.close();
        writer.close();

        return new ReturnData(blockCtr, totalReadBytes, finalFileName);
    }
}
