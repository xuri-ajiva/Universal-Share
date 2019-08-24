package app;

import java.util.*;

import java.io.*;
import java.net.*;
import java.nio.charset.StandardCharsets;

public class Client extends SharedData {

    public void Start() throws Exception {
        while (true) {
            try {
                Socket MainSocket = new Socket("localhost", FILE_PORT);

                Socket MainSocket2 = new Socket("localhost", 9999);

                System.out.println("Connected!");

                int id = new Random().nextInt(99999999) + 10000000;

                SendRegister(MainSocket2, "TestFile", id);

                SendFile(MainSocket, "..\\..\\test.txt", id);

                MainSocket.close();
                MainSocket2.close();

                System.out.println("Finished!");

                Thread.sleep(1000);

            } catch (Exception e) {
                System.out.println(e.getMessage());
                Thread.sleep(1000);
            }
        }
    }

    void SendRegister(Socket CommunicationSocket, String SaveFileName, int id) throws Exception {
        OutputStream OutStream = CommunicationSocket.getOutputStream();
        InputStream InStream = CommunicationSocket.getInputStream();

        byte[] id_B = formInt(id);
        byte[] Filaneme_B = formString(SaveFileName);

        int saveFileLangth = Filaneme_B.length;
        int bufflength = HEATHER_SIZE + saveFileLangth;

        byte[] buffer = new byte[bufflength];

        System.arraycopy(id_B, 0, buffer, 0, HEATHER_SIZE);
        System.arraycopy(Filaneme_B, 0, buffer, HEATHER_SIZE,saveFileLangth);

        OutStream.write(buffer);
        OutStream.flush();

        OutStream.close();
        InStream.close();
    }

    void SendFile(Socket FileSocket, String filename, int id) throws Exception {
        int readBytes = -1;
        byte[] buffer = new byte[BUFFER_SIZE];

        // id = id == -1 ? new Random().nextInt(99999999) + 10000000 : id;
        byte[] id_B = formInt(id);

        FileInputStream strm = new FileInputStream(filename);

        int blockCtr = 0;
        int totalReadBytes = 0;

        OutputStream OutStream = FileSocket.getOutputStream();
        while (readBytes != 0) {
            readBytes = strm.read(buffer, HEATHER_SIZE, BUFFER_SIZE - HEATHER_SIZE);
            if (readBytes == -1)
                break;
            blockCtr++;
            totalReadBytes += readBytes;

            System.arraycopy(id_B, 0, buffer, 0, id_B.length);

            OutStream.write(buffer, 0, readBytes + HEATHER_SIZE);
            System.out.println("Paket: id = " + id + "    | " + ToInt(Arrays.copyOfRange(buffer, 0, HEATHER_SIZE))
                    + "  :  " + Arrays.toString(Arrays.copyOfRange(buffer, 0, HEATHER_SIZE)));
        }

        OutStream.close();
        strm.close();
        return;
    }

    int ToInt(byte[] b) {
        return Integer.valueOf(ToString(b));
    }

    byte[] formInt(int i) {
        return formString(String.valueOf(i));
    }

    String ToString(byte[] b) {
        return new String(b, StandardCharsets.UTF_8);
    }

    byte[] formString(String S) {
        return S.getBytes(StandardCharsets.UTF_8);
    }
}