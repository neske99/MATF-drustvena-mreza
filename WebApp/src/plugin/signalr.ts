import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

let connection: HubConnection | null = null;

export async function startSignalRConnection(){
        if(!connection) {
          connection = new HubConnectionBuilder()
            .withUrl('http://localhost:8095/chathub')
            .withAutomaticReconnect()
            .build();
          await connection.start();
        }

        return connection;
}

export function getSignalRConnection(): HubConnection | null {
    if (!connection) {
      return null;
   }
    return connection;
}

export function stopSignalRConnection() {
  if(connection) {
    connection.stop();
    connection = null;
  }
}