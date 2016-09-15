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

class DnsService {
    static String[] serviceUrls;
    static String DEFAULT_SERVICE_NAME;
    static String SERVICE_TYPE;
    static boolean isClient;
    static JmDNS jmdns;
    static ServiceInfo info;

    static int tryPort;
    public int registeredPort;

    public DnsService(int port) throws IOException {
        DEFAULT_SERVICE_NAME = "JmDNS Server";
        SERVICE_TYPE = "_IAmTheBirdman._udp.local";
        isClient = false;
        tryPort = port;
        try {
            jmdns = JmDNS.create();
        } catch (IOException ioe) {
            System.out.println("Unable to create service");
            throw ioe;
        }
    }

    public DnsService() throws IOException {
        this(ThreadLocalRandom.current().nextInt(49152, 65535 + 1));
    }

    public boolean registerService() {

        String hostname;

        try {
            InetAddress my_addr = InetAddress.getLocalHost();
            hostname = my_addr.getHostName();
        }
        catch (UnknownHostException uhe) {
            System.out.println("Hostname can not be resolved");
            hostname = DEFAULT_SERVICE_NAME;
        }

        try {
            info = ServiceInfo.create(SERVICE_TYPE, hostname, tryPort, "App service");
            jmdns.registerService(info);
            registeredPort = info.getPort();
            System.out.println(registeredPort);
            System.out.println("REGISTERED as " + info);
            return true;
        } catch (IOException ioe) {
            System.out.println("Unable to register service");
            return false;
        }
    }

    static int hostService () {
        jmdns.unregisterAllServices();
        jmdns.addServiceListener("_IAmPhone._udp.local.", new SampleListener());
        return 1;
    }

    public void unregister () {
        jmdns.unregisterAllServices();
        System.out.println("Unregistered all services");
    }
    public void unregister (int port) {
        jmdns.unregisterAllServices();
        ServiceInfo info = ServiceInfo.create(SERVICE_TYPE, DEFAULT_SERVICE_NAME, port, "App service");
        jmdns.unregisterService(info);
        System.out.println("Unregistered" + info);
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