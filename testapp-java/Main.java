import java.io.IOException;
import java.io.InputStreamReader;
import java.io.BufferedReader;
import java.io.PrintWriter;
import java.net.*;
import java.net.SocketException;
import javax.jmdns.JmDNS;
import javax.jmdns.ServiceInfo;


public class Main {

   public static String mServiceName = "JmDNS Server";
   public static final String SERVICE_TYPE = "_IAmTheBirdman._tcp.local";

   public static void main(String[] args) throws UnknownHostException, SocketException, IOException {
   
      ServerSocket serverSocket = new ServerSocket(0);
   
      int mPort = serverSocket.getLocalPort();
   
      JmDNS jmdns = JmDNS.create();
      ServiceInfo info = ServiceInfo.create(SERVICE_TYPE, mServiceName, mPort, "App service");
      jmdns.unregisterAllServices();
      jmdns.registerService(info);
      System.out.println("REGISTERED");
   
      try (
             Socket clientSocket = serverSocket.accept();
             PrintWriter out =
                     new PrintWriter(clientSocket.getOutputStream(), true);
             BufferedReader in = new BufferedReader(
                     new InputStreamReader(clientSocket.getInputStream()));
      ) {
         String inputLine, outputLine;
      
         // Initiate conversation with client
         outputLine = "test";
         out.println(outputLine);
      
         while ((inputLine = in.readLine()) != null) {
            outputLine = inputLine;
            out.println(outputLine);
            if (outputLine.equals("Bye."))
               break;
         }
      
      /*
      while (true) {
         if (clientSocket.isConnected()) break;
         socket.receive(packet);
         String data = new String(packet.getData());
         String[] dataParsed = data.split(",");
         float timestamp = Float.parseFloat(dataParsed[0]);
         float sensortype = Float.parseFloat(dataParsed[1]);
         float x = Float.parseFloat(dataParsed[2]);
         float y = Float.parseFloat(dataParsed[3]);
         float z = Float.parseFloat(dataParsed[4]);
         String sensorname = new String();
         if (sensortype == 1) {
             sensorname = "GPS";
         }
         else if (sensortype == 2) {
             sensorname = "Magnetometer";
         }
         else if (sensortype == 3) {
             sensorname = "Accelerometer";
         }
         else if (sensortype == 4) {
             sensorname = "Gyroscope";
         }
         else if (true) {
             sensorname = "Unknown";
         }
         Date date = new Date();String line = ("Timestamp " + timestamp + ", local date " + date.toString() + ", sensor " + sensorname + ", x " + x + ", y " + y + ", z " + z);
         System.out.println(line);
         System.out.println("not connected");
         */
      }
   
   
      System.out.print("Enter any character to stop server: ");
      // Read the char
      char ch = (char) System.in.read();
      jmdns.unregisterAllServices();
   }
}


