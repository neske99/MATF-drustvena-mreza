import { HubConnectionBuilder } from '@microsoft/signalr';
import type { App } from 'vue';

export default {
  install:(app:App, options?: any) => {
    const connection = new HubConnectionBuilder()
    .withUrl("http://localhost:8095/chathub")
    .withAutomaticReconnect()
    .build();

    connection.on("ReceiveMessage", (user, message) => {
      console.log(`${user}: ${message}`);
    });

    connection.start().then(() => {
      console.log("Connected to SignalR hub.");
      connection.invoke("SendMessage", "TerminalUser", "Hello from terminal!");
    });
  }
}