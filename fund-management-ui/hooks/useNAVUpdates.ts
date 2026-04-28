import { useEffect, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";

export interface NAVUpdate {
  fundId: number;
  nav: number;
  updatedAt: string;
}

export type ConnectionStatus = "disconnected" | "connecting" | "connected";

export function useNAVUpdates(fundId: number | null) {
  const [updates, setUpdates] = useState<NAVUpdate[]>([]);
  const [status, setStatus] = useState<ConnectionStatus>("disconnected");
  const connectionRef = useRef<signalR.HubConnection | null>(null);

  useEffect(() => {
    if (fundId === null) {
      setUpdates([]);
      setStatus("disconnected");
      return;
    }

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7038/hubs/nav", { withCredentials: true })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    connection.on("ReceiveNAVUpdate", (data: NAVUpdate) =>
      setUpdates((prev) => [data, ...prev].slice(0, 20))
    );

    connection.onreconnecting(() => setStatus("connecting"));
    connection.onreconnected(() => {
      setStatus("connected");
      connection.invoke("SubscribeToFund", fundId).catch(console.error);
    });
    connection.onclose(() => setStatus("disconnected"));

    setStatus("connecting");
    connection
      .start()
      .then(() => {
        setStatus("connected");
        return connection.invoke("SubscribeToFund", fundId);
      })
      .catch((err) => {
        console.error("SignalR connection failed:", err);
        setStatus("disconnected");
      });

    connectionRef.current = connection;

    return () => {
      connection
        .invoke("UnsubscribeFromFund", fundId)
        .catch(() => {})
        .finally(() => connection.stop());
    };
  }, [fundId]);

  return { updates, status };
}
