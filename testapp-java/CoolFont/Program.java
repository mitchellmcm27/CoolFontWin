package CoolFont;

import java.io.IOException;
import java.io.PrintWriter;
import java.io.File;
import java.util.Scanner;

public class Program {
    public static void main (String[] args) throws IOException {

        int lastPort;
        DnsService dnsService;
        File PORT_FILE = new File("../last-port.txt");

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
            int port = dnsService.registeredPort;
            PrintWriter out = new PrintWriter(PORT_FILE);
            out.println("Last registered on port:");
            out.println(Integer.toString(port));
            out.close();
        }

        /* CALL C++ PROGRAM */

        System.in.read();
        dnsService.unregister();
    }
}
