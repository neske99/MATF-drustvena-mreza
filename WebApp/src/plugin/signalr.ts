import { authStore } from '@/stores/auth';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

let connection: HubConnection | null = null;
let callbackEventNames:string[]=[];
let callbacks:((...args:any[])=>void)[] =[];

export async function startSignalRConnection(): Promise<HubConnection | null> {
  try {
    if (connection && connection.state === HubConnectionState.Connected) {
      return connection;
    }

    if (connection && connection.state !== HubConnectionState.Disconnected) {
      await connection.stop();
    }

    connection = new HubConnectionBuilder()
      .withUrl('http://localhost:8095/chathub', {
        accessTokenFactory: () => {
          return authStore().accessToken;
        }
      })
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .build();

    await connection.start();
    for(let i=0;i<callbacks.length;i++){
      connection.on(callbackEventNames[i],callbacks[i]);
    }
    return connection;
  } catch (error) {
    connection = null;
    return null;
  }
}

export async function getSignalRConnection(): Promise<HubConnection | null> {
  if (!connection || connection.state !== HubConnectionState.Connected) {
    return await startSignalRConnection();
  }
  return connection;
}

export async function stopSignalRConnection(): Promise<void> {
  if (connection) {
    try {
      await connection.stop();
    } catch (error) {
      // Ignore errors when stopping
    } finally {
      connection = null;
    }
  }
}
export function addCallback(name:string,callback:(...args:any[])=>void): void {
  callbacks.push(callback);
  callbackEventNames.push(name);
  if(connection!==null){
    connection.on(name,callback);
  }


}