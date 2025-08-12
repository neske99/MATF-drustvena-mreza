import { authStore } from '@/stores/auth';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

let connection: HubConnection | null = null;

export async function startSignalRConnection(){
  if(!connection) {
    connection = new HubConnectionBuilder()
      .withUrl('http://localhost:8095/chathub',{
          accessTokenFactory: () => {
              return authStore().accessToken;
          }})
      .withAutomaticReconnect()
      .build();
      await connection.start();
      console.log("started connection");
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
    console.log("ended connection");
    connection.stop();
    connection = null;
  }
}