package app;
//import java.io.*;
//import java.net.*;

public class App {
    public static void main(String[] args) throws Exception {
        
        System.out.println("Creating Client ...");
        Client sv =new Client();

        
        System.out.println("Strrting Client ...");
        sv.Start();
        
        System.out.println("Finished ...");
    }
}