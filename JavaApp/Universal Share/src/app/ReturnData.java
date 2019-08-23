package app;

import java.util.*;
import java.io.*;
import java.net.*;

public class ReturnData extends SharedData {

    public int blockCtr = -1;
    public int totalReadBytes = -1;
    public String fileName = "";

    public ReturnData(int blockCtr_L, int totalReadBytes_L, String fileName_L) {
        blockCtr = blockCtr_L;
        totalReadBytes = totalReadBytes_L;
        fileName = fileName_L;
    }
}