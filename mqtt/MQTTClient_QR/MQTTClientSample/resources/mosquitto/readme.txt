How to test run mosquitto MQTT server and test sample with mosquitto clients tools

1. Install mosquitto and mosquitto client tools. On Debian-based systems:
sudo apt-get install mosquitto mosquitto-clients

2. Run mosquitto server in this directory and pass the configuration file mosquitto.conf as argument:
~/[...]/MQTTClient/resources/mosquitto$ sudo mosquitto -c mosquitto.conf

3. Subscribe with mosquitto_sub:
mosquitto_sub -t test/#

4. Publish something with mosquitto_pub:
mosquitto_pub -t test/topic1 -m hello

You should see the following output from mosquitto_sub:
hello

5. Run MQTTClient API sample. It should connect to the server

6. Run mosquitto_pub command again. If everything works, you will see the following output from mosquitto_sub:
hello
MQTTClient API works. Received 'hello'
