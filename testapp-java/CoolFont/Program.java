package CoolFont;

import java.io.*;
import java.util.Scanner;
import java.lang.Process;
import java.lang.ProcessBuilder;

public class Program {
    public static void main (String[] args) throws IOException {

        DnsService dnsService;
        int port = 0;
        boolean shouldRegister = true;
        boolean shouldUnregister = false;

        if (args.length > 0) {
            try {
                port = Integer.parseInt(args[0]);
            } catch (NumberFormatException e) {
                System.err.println("First argument" + args[0] + " must be an integer.");
                System.exit(1);
            }
        }
        if(args.length>1) {
            switch (args[1]) {
                case "-r": // register service
                    shouldRegister = true;
                    shouldUnregister = false;
                    break;
                case "-u": // unregister all services
                    shouldRegister = false;
                    shouldUnregister = true;
                    break;
                case "-b": // both: register then unregister (not useful?)
                    shouldRegister = true;
                    shouldUnregister = true;
                    break;
                default:
                    shouldRegister = true;
                    shouldUnregister = false;
            }
        }

        if (port < 1) {
            dnsService = new DnsService();
        } else {
            dnsService = new DnsService(port);
        }

        if (shouldRegister) {

            /*
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
            */

            boolean registered = dnsService.registerService();

            if (registered) {
                int registeredPort = dnsService.registeredPort;
                //File PORT_FILE = new File("../last-port.txt");
                //PrintWriter out = new PrintWriter(PORT_FILE);
                //out.println("Last registered on port:");
                //out.println(Integer.toString(registeredPort));
                //out.close();
                if (port>0) {
                    assert registeredPort == port;
                }
            }
        }

        /*
        boolean CALL_CS = false;
        if (CALL_CS) {
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
        */

        if (shouldUnregister)
        {
            dnsService.unregister(); // can also pass in port to unregister a specific service
        }

        System.in.read();
    }
}
