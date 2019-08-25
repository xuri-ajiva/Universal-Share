package app;

import java.util.*;
import java.io.File;
//import java.io.*;
import java.net.*;

public class Client extends SharedData {

    /**
     *
     */
    
    private static final int ERRORCODE = -1;

    public void Start() throws Exception {
        while (true) {
            try {
                
                System.out.println("SendFile Returned: " + SendFile("..\\..\\test.txt"));

                
                Thread.sleep(1000);
            } catch (Exception e) {
                System.out.println(e.getMessage());
                Thread.sleep(1000);
            }
        }
    }

    public int SendFile(String FileName) throws Exception {
        Socket MainSocket = new Socket("localhost", FILE_PORT);

        Socket MainSocket2 = new Socket("localhost", 9999);

        System.out.println("Connected!");

        int id = new Random().nextInt(99999999) + 10000000;

        File f = new File(FileName);
        if (!f.exists())
            return ERRORCODE;

        String F_Name = f.getName();
        String F_Absolute = f.getAbsolutePath();

        SendRegister(MainSocket2, F_Name, id);
        Thread.sleep(100);
        SendFile(MainSocket, F_Absolute, id);

        MainSocket.close();
        MainSocket2.close();

        System.out.println("Finished!");
        return 0;
    }

}