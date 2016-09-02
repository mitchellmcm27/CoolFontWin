package CoolFont;

import javax.jmdns.JmDNS;
import javax.jmdns.ServiceEvent;
import javax.jmdns.ServiceInfo;
import javax.jmdns.ServiceListener;
import java.io.IOException;
import java.net.*;
import java.util.Collections;
import java.util.Enumeration;
import java.util.concurrent.ThreadLocalRandom;

public class DnsService {
    static String[] serviceUrls;
    static String DEFAULT_SERVICE_NAME;
    static String SERVICE_TYPE;
    static int serverUptime; // packets
    static boolean client;
    static JmDNS jmdns;
    static ServiceInfo info;

    static int tryPort;
    public int registeredPort;

    public DnsService(int port) {
        DEFAULT_SERVICE_NAME = "JmDNS Server";
        SERVICE_TYPE = "_IAmTheBirdman._udp.loacl";
        serverUptime = 600;
        client = false;
        tryPort = port;
    }

    public DnsService() {
        DEFAULT_SERVICE_NAME = "JmDNS Server";
        SERVICE_TYPE = "_IAmTheBirdman._udp.local";
        serverUptime = 600;
        client = false;
        tryPort = 5555;
    }

    public boolean registerService() {
        String hostname = DEFAULT_SERVICE_NAME;

        try {
            InetAddress my_addr;
            my_addr = InetAddress.getLocalHost();
            hostname = my_addr.getHostName();
        }
        catch (UnknownHostException uhe) {
            System.out.println("Hostname can not be resolved");
            hostname = DEFAULT_SERVICE_NAME;
        }
        int nAttempts = 5;
        int tries = 0;
        while (tries < nAttempts) {
            try {
                tries++;
                jmdns = JmDNS.create();
                jmdns.unregisterAllServices();
                info = ServiceInfo.create(SERVICE_TYPE, hostname, tryPort, "App service");
                jmdns.registerService(info);
                registeredPort = info.getPort();
                System.out.println(registeredPort);
                break;
            } catch (IOException ioe) {
                tryPort = ThreadLocalRandom.current().nextInt(49152, 65535 + 1);
                System.out.println(tries);
                if (tries == nAttempts) {
                    System.out.println("Unable to register service");
                    return false;
                }
            }

        }

        System.out.println("REGISTERED as " + info);
        return true;
    }
    static int hostService () {
        jmdns.unregisterAllServices();
        jmdns.addServiceListener("_IAmPhone._udp.local.", new SampleListener());
        return 1;
    }
    static void unregister () {
        jmdns.unregisterAllServices();
    }
    static void displayInterfaceInformation(NetworkInterface netint) throws SocketException {
        System.out.printf("Display name: %s\n", netint.getDisplayName());
        System.out.printf("Name: %s\n", netint.getName());
        Enumeration<InetAddress> inetAddresses = netint.getInetAddresses();
        for (InetAddress inetAddress : Collections.list(inetAddresses)) {
            System.out.printf("InetAddress: %s\n", inetAddress);
        }
        System.out.printf("\n");
    }
    static class SampleListener implements ServiceListener {
        @Override
        public void serviceAdded(ServiceEvent event) {
            System.out.println("Service added   : " + event.getName() + "." + event.getType());
        }

        @Override
        public void serviceRemoved(ServiceEvent event) {
            System.out.println("Service removed : " + event.getName() + "." + event.getType());
        }

        @Override
        public void serviceResolved(ServiceEvent event) {
            System.out.println("Service resolved: " + event.getInfo());
            serviceUrls = event.getInfo().getURLs();
        }
    }
}