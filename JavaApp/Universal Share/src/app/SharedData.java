package app;

import java.io.*;
import java.net.*;
import java.nio.ByteBuffer;

public class SharedData {

    public final int FILE_PORT = 4333;
    public final String DEFAULT_SAVE_LOCATION = ".\\saved\\";

    public final int BUFFER_SIZE = 32768;
    public final int HEATHER_SIZE = 8;

    public byte[] toByteArray(int value) {
        return ByteBuffer.allocate(4).putInt(value).array();
    }

    public int fromByteArray(byte[] bytes) {
        return ByteBuffer.wrap(bytes).getInt();
    }
}
