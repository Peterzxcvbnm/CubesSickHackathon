## MQTTClientSample
Connecting an MQTT client to an MQTT broker and subscribing and publishing to topics.
### Description
This sample shows how to connect to a MQTT broker, register a topic and publish
data on this topic. It also shows how to subscribe to a topic and receive data.
The client subscribes to topic "test/topic1". Any messages that are received under
this topic are then published to topic "test/topic2".
### How to Run
For this sample to run a MQTT server / broker has to be set up.
The sample can be run on the Emulator with a FullFeatured Device Model or any
device which has the MQTTClient functionality and can connect to the MQTT broker.
### Implementation
TLS can be used by setting USE_TLS to true.
Important when using TLS:
- Certificate verification will fail if the current time is not set on the device.
  Time can be set using DateTime.setDateTime or NTPClient API.
- Resolving of host-names only works when a DNS name server is configured for the device.
  This can be done using DHCP.
### More Information
This sample App includes a self-signed server certificate / private key pair, as well as
a client certificate / private key pair.
It also contains sample configuration file for two different brokers:
- HiveMQ (tested with version 3.2.5)
- mosquitto (tested with version 1.4.14)  There is an additional mosquitto/readme.txt that describes 
how to start mosquitto with the provided configuration and how to use the mosquitto client tools to test this sample

### Topics
System, Communication, Sample, SICK-AppSpace