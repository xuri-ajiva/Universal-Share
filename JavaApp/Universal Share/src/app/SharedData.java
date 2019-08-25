package app;

import java.util.*;
import java.io.*;
import java.net.*;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;

public class SharedData {
    public final int DEFAULT_FILE_PORT = 4333;
    public final int DEFAULT_TEXT_PORT = 9999;
    public final String DEFAULT_SAVE_LOCATION = ".\\saved\\";
    public final int DEFAULT_BUFFER_SIZE = 32768;
    public final int DEFAULT_HEATHER_SIZE = 8;

    public int FILE_PORT;
    public int TEXT_PORT;
    public String SAVE_LOCATION;
    public int BUFFER_SIZE;
    public int HEATHER_SIZE;

    /// CTOR ///
    private void Init(int filePort, int textPort, int bufferSize, String saveLocation, int heatherSize) {
        this.FILE_PORT = filePort;
        this.TEXT_PORT = textPort;
        this.BUFFER_SIZE = bufferSize;
        this.SAVE_LOCATION = saveLocation;
        this.HEATHER_SIZE = heatherSize;
    }

    public SharedData(int filePort, int textPort, int bufferSize, String saveLocation, int heatherSize) {
        int a = filePort;
        int b = textPort;
        int c = bufferSize;
        String d = saveLocation;
        int e = heatherSize;
        this.Init(a, b, c, d, e);
    }

    public SharedData(int filePort, int textPort, int bufferSize, String saveLocation) {
        int a = filePort;
        int b = textPort;
        int c = bufferSize;
        String d = saveLocation;
        int e = DEFAULT_HEATHER_SIZE;
        this.Init(a, b, c, d, e);
    }

    public SharedData(int filePort, int textPort, int bufferSize) {
        int a = filePort;
        int b = textPort;
        int c = bufferSize;
        String d = DEFAULT_SAVE_LOCATION;
        int e = DEFAULT_HEATHER_SIZE;
        this.Init(a, b, c, d, e);
    }

    public SharedData(int filePort, int textPort) {
        int a = filePort;
        int b = textPort;
        int c = DEFAULT_BUFFER_SIZE;
        String d = DEFAULT_SAVE_LOCATION;
        int e = DEFAULT_HEATHER_SIZE;
        this.Init(a, b, c, d, e);
    }

    public SharedData(int filePort) {
        int a = filePort;
        int b = DEFAULT_FILE_PORT;
        int c = DEFAULT_BUFFER_SIZE;
        String d = DEFAULT_SAVE_LOCATION;
        int e = DEFAULT_HEATHER_SIZE;
        this.Init(a, b, c, d, e);
    }

    public SharedData() {
        int a = DEFAULT_FILE_PORT;
        int b = DEFAULT_TEXT_PORT;
        int c = DEFAULT_BUFFER_SIZE;
        String d = DEFAULT_SAVE_LOCATION;
        int e = DEFAULT_HEATHER_SIZE;
        this.Init(a, b, c, d, e);
    }

    /// Client ///
    public void SendRegister(Socket CommunicationSocket, String SaveFileName, int id) throws Exception {
        OutputStream OutStream = CommunicationSocket.getOutputStream();
        InputStream InStream = CommunicationSocket.getInputStream();

        byte[] id_B = formInt(id);
        byte[] Filaneme_B = formString(SaveFileName);

        int saveFileLangth = Filaneme_B.length;
        int bufflength = HEATHER_SIZE + saveFileLangth;

        byte[] buffer = new byte[bufflength];

        System.arraycopy(id_B, 0, buffer, 0, HEATHER_SIZE);
        System.arraycopy(Filaneme_B, 0, buffer, HEATHER_SIZE, saveFileLangth);

        OutStream.write(buffer);
        OutStream.flush();

        OutStream.close();
        InStream.close();
    }

    public void SendFile(Socket FileSocket, String filename, int id) throws Exception {
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

    /// Server ///

    /// Utils ///
    public byte[] toByteArray(int value) {
        return ByteBuffer.allocate(4).putInt(value).array();
    }

    public int fromByteArray(byte[] bytes) {
        return ByteBuffer.wrap(bytes).getInt();
    }

    public int ToInt(byte[] b) {
        return Integer.valueOf(ToString(b));
    }

    public byte[] formInt(int i) {
        return formString(String.valueOf(i));
    }

    public String ToString(byte[] b) {
        return new String(b, StandardCharsets.UTF_8);
    }

    public byte[] formString(String S) {
        return S.getBytes(StandardCharsets.UTF_8);
    }

}
