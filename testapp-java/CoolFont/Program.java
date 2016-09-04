package CoolFont;

import java.io.*;
import java.util.Scanner;
import java.lang.Process;
import java.lang.ProcessBuilder;

public class Program {
    public static void main (String[] args) throws IOException {

        int lastPort;
        DnsService dnsService;
        File PORT_FILE = new File("../last-port.txt");
        int port = 0;

        try {
            Scanner in = new Scanner(PORT_FILE);
            String hdr = in.nextLine();
            lastPort = in.nextInt();
            in.close();
            System.out.println(hdr);
            System.out.println(lastPort);
        } catch (Exception e) {
            lastPort = -1;
        }

        if (lastPort < 1) {
            dnsService = new DnsService();
        } else {
            dnsService = new DnsService(lastPort);
        }

        boolean registered = dnsService.registerService();

        if (registered) {
            port = dnsService.registeredPort;
            PrintWriter out = new PrintWriter(PORT_FILE);
            out.println("Last registered on port:");
            out.println(Integer.toString(port));
            out.close();
        }

        assert(port>0);

        /* Should I call c# program to listen on the socket? */
        boolean call_cs = true;

        if (call_cs) {
            //TODO: see http://www.javaworld.com/article/2071275/core-java/when-runtime-exec---won-t.html
            ProcessBuilder pb = new ProcessBuilder("../CoolFontWin/bin/Debug/CoolFontWin.exe", Integer.toString(port)); // determined by VS
            pb.directory(new File("../CoolFontWin/bin/Debug"));
            Process proc = pb.start();
            BufferedReader stdInput = new BufferedReader(new
                    InputStreamReader(proc.getInputStream()));

            BufferedReader stdError = new BufferedReader(new
                    InputStreamReader(proc.getErrorStream()));
            String s = null;

            while (proc.isAlive()) {
                // read the output from the command
                System.out.println("Here is the standard output of the command:\n");
                while ((s = stdInput.readLine()) != null) {
                    System.out.println(s);
                }

                // read any errors from the attempted command
                System.out.println("Here is the standard error of the command (if any):\n");
                while ((s = stdError.readLine()) != null) {
                    System.out.println(s);
                }
            }
        }


        System.in.read();

        dnsService.unregister();
    }
}
